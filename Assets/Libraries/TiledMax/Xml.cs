using System.Xml;

namespace TiledMax
{
    public static class Xml
    {
        public static string ReadTag(this XmlNode node, string tagName, string defaultValue = "")
        {
            if (node.Attributes != null)
            {
                return node.Attributes[tagName] != null ? node.Attributes[tagName].Value : defaultValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int ReadInt(this XmlNode node, string tagName, int defaultValue = 0)
        {
            if (node.Attributes != null)
            {
                return node.Attributes[tagName] != null ? int.Parse(node.Attributes[tagName].Value) : defaultValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static double ReadDouble(this XmlNode node, string tagName, double defaultValue = 1)
        {
            if (node.Attributes != null)
            {
                return node.Attributes[tagName] != null ? double.Parse(node.Attributes[tagName].Value) : defaultValue;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
