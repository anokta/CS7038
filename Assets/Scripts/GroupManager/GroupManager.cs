using UnityEngine;
using System.Collections;
using Zootools;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Grouping
{
    public class GroupManager
    {
        Dictionary<string, Group> _data;
        Indexer<string, Group> _indexer;
        static GroupManager _instance;
        Group _active;

        public GroupManager()
            : this(true)
        {
        }

        public GroupManager(bool caseSensitive)
        {
            if (caseSensitive)
            {
                _data = new Dictionary<string, Group>();
            }
            else
            {
                _data = new Dictionary<string, Group>(StringComparer.InvariantCultureIgnoreCase);
            }
            _indexer = new Indexer<string, Group>(_data);
        }

        static GroupManager()
        {
            //Called only once and therefore reflection overhead is negligible
            typeof(Group).GetMethod("Init",
            BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
        }

        public static GroupManager main
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public Indexer<string, Group> group
        {
            get { return _indexer; }
        }

        public Group activeGroup
        {
            get { return _active; }
            set { Activate(value); }
        }

        void Activate(Group group)
        {
            if (group.manager != this)
            {
                throw new InvalidOperationException(
                string.Format("Group '{0}' does not belong to this GroupManager", group.name));
            }

            if (_active != group && _active != null)
            {
                var listy = _active.scripts.Except(group.scripts);
                foreach (var item in listy)
                {
                    if (item != null && item.enabled)
                    {
                        item.enabled = false;
                    }
                }
            }
            if ((_active = group) != null)
            {
                _cleanup(_active);
                foreach (var entity in _active.scripts)
                {
                    if (!entity.enabled)
                    {
                        entity.enabled = true;
                    }
                }
            }
        }

        public IEnumerable<Group> enumerate
        {
            get { return _data.Values; }
        }

        public IEnumerable<string> names
        {
            get { return _data.Keys; }
        }

        public Group Add(string name)
        {
            if (_data.ContainsKey(name))
            {
                throw new ArgumentException(
                string.Format("State '{0}' already exists in this StateManager", name));
            }
            Group s = _construct(this, name);
            _data.Add(name, s);
            return s;
        }

        public void Remove(Group s)
        {
            if (s.manager == this)
            {
                _invalidate(s);
                _data.Remove(s.name);
            }
        }

        public bool Contains(string name)
        {
            return _data.ContainsKey(name);
        }


        public class Group
        {
            List<Behaviour> _list;

            public GroupManager manager { get; private set; }

            public string name { get; private set; }

            public IEnumerable<Behaviour> scripts { get { return _list; } }

            private static void Invalidate(Group g)
            {
                //TODO: A bit expensive, find better way maybe?
                //This shouldn't be called often during gameplay anyway
                while (g._list.Count > 0)
                {
                    g.Remove(g._list[0]);
                }
                g.manager = null;
            }

            private static void Cleanup(Group g)
            {
                g._list.RemoveAll(_ => _ == null);
            }

            private static void Init()
            {
                /* Hack: Since C# doesn't support parent classes having private access to nested
             * classes (it should, even Java does that!), this is a dirty hack that achieves
             * the same effect by calling this method using Reflection and then assigning
             * delegates to the parent class, sent through here where private access is allowed.
             */
                GroupManager._construct = delegate(GroupManager gm, string name)
                {
                    return new Group(gm, name);
                };

                GroupManager._invalidate = Invalidate;
                GroupManager._cleanup = Cleanup;
            }

            private Group(GroupManager manager, string name)
            {
                _list = new List<Behaviour>();
                this.name = name;
                this.manager = manager;
            }

            public void Add(Behaviour entity)
            {
                if (manager == null)
                {
                    throw new InvalidOperationException(
                    string.Format("State '{0}' is deactivated", name));
                }
                if (!_list.Contains(entity))
                {
                    _list.Add(entity);
                }
                if (manager._active != null && manager._active._list.Contains(entity))
                {
                    if (!entity.enabled)
                    {
                        entity.enabled = true;
                    }
                }
                else if (entity.enabled)
                {
                    entity.enabled = false;
                }
            }

            public void Add(Component component, GroupItem item)
            {
                Add(component.gameObject, item);
            }

            public void Add(GameObject gameObject, GroupItem item)
            {
                var obj = gameObject.AddComponent<GroupBehaviour>();
                obj.groupItem = item;
                Add(obj);
                obj.hideFlags = HideFlags.HideInInspector;
            }

            public bool Contains(Behaviour script)
            {
                return _list.Contains(script);
            }

            public void Remove(Behaviour script)
            {
                int found = 0;
                bool thisContains = false;
                foreach (Group group in manager.enumerate)
                {
                    if (group.Contains(script))
                    {
                        if (group == this)
                        {
                            thisContains = true;
                        }
                        ++found;
                    }
                }
                if (thisContains)
                {
                    if (found == 1)
                    {
                        //The Behaviour no longer belongs to a state!
                        if (script != null && !script.enabled)
                        {
                            script.enabled = true;
                        }
                    }
                    else if (manager._active == this)
                    {
                        if (script != null && script.enabled)
                        {
                            script.enabled = false;
                        }
                    }
                    _list.Remove(script);
                }
            }

            public override string ToString()
            {
                return string.Format("[Group: {0}]", name);
            }
        }

        private delegate Group _constructorDelegate(GroupManager sm, string n);
        private delegate void _operateDelegate(Group s);

        private static _constructorDelegate _construct;
        //Used to invalidate a group (when removing it)
        private static _operateDelegate _invalidate;
        //Used to clean-up a group (removes invalidated scripts)
        private static _operateDelegate _cleanup;
    }
}