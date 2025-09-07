
    using System;
    using System.Xml.Linq;
    using System.Collections.Generic;

namespace CT.Utils
{
    public class XmlConfigReader
    {
        private static string _filePath = "config.xml";
         

        // Đọc một giá trị từ phần tử 'appSettings' trong XML
        public static string GetAppSettingValue(string key)
        {
            try
            {
                XDocument doc = XDocument.Load(_filePath);
                var appSettings = doc.Element("configuration")?.Element("appSettings");
                var setting = appSettings?.Element("add");

                if (setting != null)
                {
                    var settingValue = appSettings.Elements("add")
                                                  .FirstOrDefault(x => x.Attribute("key")?.Value == key)?
                                                  .Attribute("value")?.Value;

                    return settingValue;
                }
                //No appSettings section found!
                return "";
            }
            catch (Exception ex)
            {
                return "";// $"Error reading XML file: {ex.Message}";
            }
        }

        // Đọc tất cả các appSettings và trả về một dictionary
        public Dictionary<string, string> GetAllAppSettings()
        {
            try
            {
                XDocument doc = XDocument.Load(_filePath);
                var appSettings = doc.Element("configuration")?.Element("appSettings");
                var settings = new Dictionary<string, string>();

                if (appSettings != null)
                {
                    foreach (var addElement in appSettings.Elements("add"))
                    {
                        string key = addElement.Attribute("key")?.Value;
                        string value = addElement.Attribute("value")?.Value;
                        if (key != null && value != null)
                        {
                            settings[key] = value;
                        }
                    }
                }

                return settings;
            }
            catch (Exception ex)
            { 
                return new Dictionary<string, string>();
            }
        }
    }

}
