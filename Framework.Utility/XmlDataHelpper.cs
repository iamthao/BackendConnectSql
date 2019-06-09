using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Framework.Utility
{
    public class XmlDataHelpper
    {
        private static readonly string PathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileUpload", "ConfigData",
            "SystemData.xml");

        private static readonly string MessageRegexReplaceItem = @"\[([^]]*)\]";

        private XmlDataHelpper()
        {
            _listAllData = new Dictionary<string, Dictionary<string, string>>();
            var xmlDocument = XDocument.Load(PathFile);
            var root = xmlDocument.Root;
            if (root == null)
            {
                return;
            }
            foreach (var child in root.Elements())
            {
                var objAdd = new Dictionary<string, string>();
                foreach (var item in child.Elements())
                {
                    if (item.Attribute("value") != null)
                    {
                        objAdd.Add(item.Attribute("value").Value, item.Value);
                    }
                }
                _listAllData.Add(child.Name.ToString(), objAdd);
            }
        }

        public static XmlDataHelpper Instance
        {
            get { return Nested._instance; }
        }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly XmlDataHelpper _instance = new XmlDataHelpper();
        }

        private static Dictionary<string, Dictionary<string, string>> _listAllData;

        public Dictionary<string, string> GetData(string type)
        {
            if (_listAllData == null || _listAllData.Count == 0 || string.IsNullOrEmpty(type))
            {
                return new Dictionary<string, string>();
            }
            return !_listAllData.ContainsKey(type) ? new Dictionary<string, string>() : _listAllData[type];
        }

        public string GetValue(string type, string key)
        {
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(key) || !_listAllData.ContainsKey(type))
            {
                return "";
            }
            var objListItem = _listAllData[type];
            if (objListItem != null && objListItem.ContainsKey(key))
            {
                return objListItem[key];
            }
            return "";
        }

        public string GetMessageValue(string type, string key, Dictionary<string, string> customParam, string title = "courier")
        {
            string returnMessage = GetValue(type, key);
            var match = Regex.Match(returnMessage, MessageRegexReplaceItem);
            while (match.Value.Length > 0)
            {
                string value;
                if (!(customParam != null && match.Groups.Count > 1 &&
                    customParam.TryGetValue(match.Groups[1].Value, out value)))
                {
                    value = string.Empty;
                }

                if (match.Value == "[Courier]" || match.Value == "[FromCourier]" || match.Value == "[ToCourier]")
                {
                    var startingIndex = returnMessage.IndexOf(match.Value, System.StringComparison.OrdinalIgnoreCase);
                    var startMessage = returnMessage.Substring(0, startingIndex);
                    var endMessage = returnMessage.Substring(startingIndex + match.Value.Length);
                    startMessage = startMessage.Replace("courier", title.ToLower());
                    startMessage = startMessage.Replace("Courier", title);
                    endMessage = endMessage.Replace("courier", title.ToLower());
                    endMessage = endMessage.Replace("Courier", title);
                    returnMessage = string.Format("{0} {1} {2}", startMessage.Trim(), value.Trim(), endMessage.Trim());
                }
                returnMessage = returnMessage.Replace(match.Value, value);
                match = Regex.Match(returnMessage, MessageRegexReplaceItem);
            }
            
            //returnMessage = returnMessage.Replace("courier", title.ToLower());
            //returnMessage = returnMessage.Replace("Courier", title);

            return returnMessage;
        }
    }
}
