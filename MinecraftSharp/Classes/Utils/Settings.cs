using MinecraftSharp.Classes.Display;

namespace MinecraftSharp.Classes.Utils
{
    public static class Settings
    {
        private static Dictionary<string, object> values = new Dictionary<string, object>();
        public static void Initialize(string path)
        {
            int count = 0;
            foreach(string line in File.ReadAllLines(path))
            {
                if (line.StartsWith("#"))
                    continue;

                int lastIndex = 0;
                for(int i = 0; i < 2; i++) {
                    lastIndex = line.IndexOf(":",lastIndex + 1);
                }
                
                string[] lineInfo = line.Split(":");

                string type     = lineInfo[0];
                string name     = lineInfo[1];
                string value    = line.Substring(lastIndex + 1);//lineInfo[2];

                if(type == "str")
                    values.Add(name,value);
                else if(type =="int")
                    values.Add(name,int.Parse(value));
                else if(type == "float")
                    values.Add(name,float.Parse(value));

                count++;
            }
            ConsoleHelper.WriteInfo($"Read {count} options!");
        }
        public static object GetValue(string key)
        {
            if (!values.TryGetValue(key, out object? value))
                throw new Exception($"Failed to find option key: {key}");
            return value;
        }
    }
}
