using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.Utilities
{
    public static class StateStore
    {
        private static string FileName;
        private static Dictionary<string, string> Data;
        public static bool Started { get; private set; }

        public static void Start(string SystemName)
        {
            FileName = "ScarletStore-" + SystemName + ".txt";
            if(!File.Exists(FileName)) { File.Create(FileName).Close(); }
            Data = new Dictionary<string, string>();
            foreach (string Line in File.ReadAllLines(FileName))
            {
                Data.Add(Line.Split('=')[0], string.Join("=", Line.Split('=').Skip(1).ToArray()));
            }
            Started = true;
        }

        public static void Save()
        {
            List<string> Lines = new List<string>(Data.Count);
            foreach(KeyValuePair<string, string> Item in Data)
            {
                Lines.Add(Item.Key + '=' + Item.Value);
            }
            string OldFile = FileName + "old";
            File.Move(FileName, OldFile);
            File.WriteAllLines(FileName, Lines);
            File.Delete(OldFile);
        }

        public static void Set(string Key, string Value)
        {
            if (!Data.ContainsKey(Key)) { Data.Add(Key, Value); }
            else { Data[Key] = Value; }
        }

        public static string Get(string Key) { return Data.ContainsKey(Key) ? Data[Key] : null; }

        public static string GetOrCreate(string Key, string DefaultValue)
        {
            if (!Data.ContainsKey(Key)) { Data.Add(Key, DefaultValue); }
            return Data[Key];
        }
    }
}
