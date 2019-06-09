using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Framework.Utility
{
    public class XmlConfigReader
    {
        private static readonly string PathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");

        public static string GetValue(string keyValue)
        {
            var xmlDocument = XDocument.Load(PathFile);
            var root = xmlDocument.Root;
            if (root == null)
            {
                return "";
            }
            var listAllData = root.Elements().ToDictionary(child => child.Name.LocalName, child => child.Value);

            return listAllData.FirstOrDefault(p => p.Key == keyValue).Value;
        }
    }
}
