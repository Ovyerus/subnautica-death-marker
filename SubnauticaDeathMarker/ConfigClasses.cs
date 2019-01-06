namespace SubnauticaDeathMarker.ConfigClasses
{
    public class ModJson
    {
        public string Id;
        public string DisplayName;
        public string Author;
        public string Version;
        public bool Enable;
        public string AssemblyName;
        public string EntryMethod;
        public ConfigJson Config;
    }

    public class ConfigJson
    {
        public bool AddCoords;
    }
}
