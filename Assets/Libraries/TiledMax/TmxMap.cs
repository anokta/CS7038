using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Globalization;

namespace TiledMax
{
    public class TmxMap
    {
        public List<Layer> Layers { get; set; }

        public string Version;             // The TMX format version, generally 1.0.
        public MapOrientation Orientation; // Map orientation. Tiled supports "orthogonal" and "isometric" at the moment.
        public int Width;                  // The map width in tiles.
        public int Height;                 // The map height in tiles.
        public int TileWidth;              // The width of a tile.
        public int TileHeight;             // The height of a tile
		public PropertyReader Properties {
			get; private set;
		}

		public List<TmxObject> Objects {
			get;
			private set;
		}

        public TmxMap()
        {
            Layers = new List<Layer>();
			Objects = new List<TmxObject>();
            Orientation = MapOrientation.Orthogonal;
        }

        public static TmxMap Open(TextReader reader)
		{
			var doc = new XmlDocument();
			var result = new TmxMap();

			var properties = new Dictionary<string, string>();
			result.Properties = new PropertyReader(properties);

			doc.Load(reader);

			XmlNode node = null;

			foreach (XmlNode xmlNode in doc.ChildNodes) {
				if (xmlNode.Name == "map" && xmlNode.HasChildNodes) {
					node = xmlNode;
					break;
				}
			}

			if (node == null) {
				throw new Exception("Tried to load a file that does not contain map data.");
			}

			result.Version = node.ReadTag("version");
			result.Width = node.ReadInt("width");
			result.Height = node.ReadInt("height");
			result.TileWidth = node.ReadInt("tilewidth");
			result.TileHeight = node.ReadInt("tileheight");

			switch (node.ReadTag("orientation")) {
				case "isometric":
					result.Orientation = MapOrientation.Isometric;
					break;
				default:
					result.Orientation = MapOrientation.Orthogonal;
					break;
			}

			foreach (XmlNode xNode in node.ChildNodes) {
				switch (xNode.Name) {
					case "layer":
						ReadLayer(xNode, ref result);
						break;
					case "properties":
						ReadProperties(xNode, properties);
						break;
					case "objectgroup":
						ReadObjects(xNode, result);
						break;
				}
			}

			return result;
		}

		private static void ReadObjects(XmlNode node, TmxMap result) {
			if (node.HasChildNodes) {
				foreach (XmlNode child in node.ChildNodes) {
					if (child.Name == "object") {
						Rect rect = new Rect(
							(float)(child.ReadDouble("x")) / 100,
							(float)(child.ReadDouble("y")) / 100,
							(float)(child.ReadDouble("width", 100)) / 100,
							(float)(child.ReadDouble("height", 100)) / 100);
						rect.y = result.Height - rect.height - rect.y;
						
                        child.ReadTag("type", "any");
						
						var properties = new Dictionary<string, string>();
						bool ellipse = false;
						foreach (XmlNode grandchild in child.ChildNodes) {
							if (grandchild.Name == "properties") {
								ReadProperties(grandchild, properties);
							}
							else if (grandchild.Name == "ellipse") {
								ellipse = true;
							}
						}
						TmxObject.ObjectType oType;
						int id = child.ReadInt("gid", -1);
						if (ellipse == true) {
							oType = TmxObject.ObjectType.Ellipse;
						}
						if (id != -1) {
							oType = TmxObject.ObjectType.Tile;
						}
						else {
							oType = TmxObject.ObjectType.Rect;
						}

						string type = child.ReadTag("type", "any");

						TmxObject obj = new TmxObject(rect, oType, type, properties);
						result.Objects.Add(obj);
					}
				}
			}
		}

		private static void ReadProperties(XmlNode node, Dictionary<string, string> properties) {
			if (node.HasChildNodes) {
				foreach (XmlNode child in node.ChildNodes) {
					properties.Add(child.ReadTag("name"), child.ReadTag("value"));
				}
			}
		}
		
		private static void ReadLayer(XmlNode node, ref TmxMap map)
        {
            var r = new Layer(node.ReadInt("width"), node.ReadInt("height"))
            {
                Name = node.ReadTag("name"),
                X = node.ReadInt("x"),
                Y = node.ReadInt("y"),
                Opacity = node.ReadDouble("opacity", 1),
                Visible = node.ReadInt("visible") == 1
            };

            if (node.HasChildNodes)
            {
                var data = node.FirstChild;
                var dataVal = data.InnerText.Trim('\n', ' ');
                var encoding = data.ReadTag("encoding");

                if (encoding != "csv") throw new Exception("TMX must use CSV format");

                dataVal = dataVal.Replace("\n", "");

                var indices = dataVal.Split(',');

                for (var y = 0; y < r.Height; y++)
                {
                    for (var x = 0; x < r.Width; x++)
                    {
                        var v = int.Parse(indices[x + y * r.Width]);
                        r.Tiles[x, y] = new Tile(x, y, v);
                    }
                }

                map.Layers.Add(r);
            }
        }
    }

    public enum MapOrientation
    {
        Orthogonal,
        Isometric
    }
}
