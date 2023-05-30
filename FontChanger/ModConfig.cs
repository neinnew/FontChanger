using System.Xml.Serialization;

namespace FontChanger;

public class ModConfig
{
        // Settings file name.
        [XmlIgnore]
        private static readonly string SettingsFilePath = Path.Combine(ColossalFramework.IO.DataLocation.localApplicationData, "FontChanger.xml");

        // File version.
        [XmlAttribute("Version")]
        public int Version = 0;

        [XmlElement("FontFamily")]
        public string? FontFamily { get => FontChanger.FontFamily; set => FontChanger.FontFamily = value ?? string.Empty; }

        [XmlElement("FontSize")]
        public int FontSize { get => FontChanger.FontSize; set => FontChanger.FontSize = value; }

        [XmlElement("FontFamilyForOpenSansRegular")]
        public string? FontFamilyForOpenSansRegular { get => FontChanger.FontFamilyForOpenSansRegular; set => FontChanger.FontFamilyForOpenSansRegular = value ?? string.Empty; }

        [XmlElement("FontFamilyForOpenSansSemibold")]
        public string? FontFamilyForOpenSansSemibold { get => FontChanger.FontFamilyForOpenSansSemibold; set => FontChanger.FontFamilyForOpenSansSemibold = value ?? string.Empty; }

        [XmlElement("FontFamilyForArchitectsDaughter")]
        public string? FontFamilyForArchitectsDaughter { get => FontChanger.FontFamilyForArchitectsDaughter; set => FontChanger.FontFamilyForArchitectsDaughter = value ?? string.Empty; }

        [XmlElement("ApplyOnImgui")]
        public bool ApplyOnImgui { get => FontChanger.ApplyOnImgui; set => FontChanger.ApplyOnImgui = value; }
        
        [XmlElement("AdvancedFontFamily")]
        public bool AdvancedFontFamily { get => FontChanger.AdvancedFontFamily; set => FontChanger.AdvancedFontFamily = value; }
        
        internal static void Load()
        {
            try
            {
                // Check to see if configuration file exists.
                if (File.Exists(SettingsFilePath))
                {
                    // Read it.
                    using var reader = new StreamReader(SettingsFilePath);
                    var xmlSerializer = new XmlSerializer(typeof(ModConfig));
                    if (xmlSerializer.Deserialize(reader) is not ModConfig)
                    {
                        UnityEngine.Debug.Log($"{Mod.Info.Name}: couldn't deserialize settings file");
                    }
                }
                else
                {
                    UnityEngine.Debug.Log($"{Mod.Info.Name}:  no settings file found");
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        internal static void Save()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SettingsFilePath))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ModConfig));
                    xmlSerializer.Serialize(writer, new ModConfig());
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }
}