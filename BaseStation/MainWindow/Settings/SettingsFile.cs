using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Settings along with a file and auto-save functionality.
    /// </summary>
    public class SettingsFile
    {
        public string FileName { get; private set; }
        public Settings Settings { get; private set; }

        public SettingsFile(string fileName)
        {
            FileName = fileName;

            if (File.Exists(FileName))
            {
                try {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    TextReader reader = new StreamReader(FileName);
                    Settings = (Settings)serializer.Deserialize(reader);
                } catch (Exception) {

				}
            }
            Settings = Settings ?? new Settings();

            Settings.ValueChanged += (sender, e) => Save();
        }

        public void Save()
        {
            Console.WriteLine("Saving..");
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (TextWriter writer = new StreamWriter(FileName))
            {
                serializer.Serialize(writer, Settings);
            }
            Console.WriteLine("Done");
        }
    }
}
