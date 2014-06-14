using UnityEngine;
using System.Collections;

namespace HandyEditor
{
	using UnityEditor;
	using UnityEngine;

	public class RenameWizard : ScriptableWizard {
		private static RenameWizard _instance;

		public static bool IsOpen { get { return _instance != null; } }
		string _message;
		string _value;
		static void Show(string title, string message, string value) {
			if (_instance == null) {
				_instance = ScriptableWizard.DisplayWizard<RenameWizard>(title);
			}
			_instance._message = message;
			_instance._value = value;
			_instance.Focus();
		}

		public delegate void AcceptCallback(string newMessage);

		public AcceptCallback Accept;

		void OnGUI() {
			GUILayout.BeginVertical();
			GUILayout.Label(_message);
			GUI.SetNextControlName("TextFieldThing");
			_value = GUILayout.TextField(_value);
			GUI.FocusControl("TextFieldThing");
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Cancel", GUILayout.ExpandWidth(false))) {
				this.Close();
			}
			if (GUILayout.Button("Accept", GUILayout.ExpandWidth(false))) {
				if (Accept != null) {
					Accept(_value);
				}
				this.Close();
			}

			GUILayout.EndHorizontal();
		}

		public static void CloseInstance() {
			if (_instance != null) {
				_instance.Close();
			}
		}

		void OnDestroy() {
			_instance = null;
		}
		void OnLostFocus() {
			this.Focus();
		}

	}
}