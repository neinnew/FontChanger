using ColossalFramework.UI;
using UnityEngine;

namespace FontChanger;

/// <summary>
/// Main mod class.
/// </summary>
public static class FontChanger
{
    private static GameObject? _imguiFontHandlerObject;
    private static ImguiFontHandler? _imguiFontHandler;
    
    public static string FontFamily = string.Empty;
    public static string FontFamilyForOpenSansRegular = string.Empty;
    public static string FontFamilyForOpenSansSemibold = string.Empty;
    public static string FontFamilyForArchitectsDaughter = string.Empty;

    public static bool ApplyOnImgui = true;
    public static bool AdvancedFontFamily = false;

    public static int FontSize = 16;

    public static Font? Font;

    public static void Change()
    {
        // Creates a string array of font names.
        var fontNames = string.IsNullOrEmpty(FontFamily) ? null
            : FontFamily.Split(',').Select(name => name.Trim().Trim('"').Trim('\'')).ToArray();
        var fontNamesOpenSansRegular = string.IsNullOrEmpty(FontFamilyForOpenSansRegular) ? null
            : FontFamilyForOpenSansRegular.Split(',').Select(name => name.Trim().Trim('"').Trim('\'')).ToArray();
        var fontNamesOpenSansSemibold = string.IsNullOrEmpty(FontFamilyForOpenSansSemibold) ? null
            : FontFamilyForOpenSansSemibold.Split(',').Select(name => name.Trim().Trim('"').Trim('\'')).ToArray();
        var fontNamesArchitectsDaughter = string.IsNullOrEmpty(FontFamilyForArchitectsDaughter) ? null 
            : FontFamilyForArchitectsDaughter.Split(',').Select(name => name.Trim().Trim('"').Trim('\'')).ToArray();

        bool appliedOpenSansRegular = false;
        bool appliedOpenSansSemibold = false;
        bool appliedArchitectsDaughter = false;
        
        // Applies to all text CO.UI.
        foreach (var component in UnityEngine.Object.FindObjectsOfType<UITextComponent>())
        {
            if (fontNames is null) break;
            if (appliedOpenSansRegular && appliedOpenSansSemibold && appliedArchitectsDaughter) break;
            
            UIDynamicFont font = (UIDynamicFont)component.font;
            
            switch (component.font.name)
            {
                case "OpenSans-Regular" :
                    if (appliedOpenSansRegular) continue;
                    appliedOpenSansRegular = true;
                    ApplyAFont(font, fontNamesOpenSansRegular);
                    break;
                case "OpenSans-Semibold" :
                    if (appliedOpenSansSemibold) continue;
                    appliedOpenSansSemibold = true;
                    ApplyAFont(font, fontNamesOpenSansSemibold);
                    break;
                case "ArchitectsDaughter" :
                    if (appliedArchitectsDaughter) continue;
                    appliedArchitectsDaughter = true;
                    ApplyAFont(font, fontNamesArchitectsDaughter);
                    break;
                default:
                    ApplyAFont(font, fontNames);
                    break;
            }
        }

        void ApplyAFont(UIDynamicFont font, string[]? advancedFontNames)
        {
            font.baseFont.fontNames = AdvancedFontFamily && advancedFontNames is not null 
                ? advancedFontNames : fontNames;
            
            // If font size not changed from the default, don't do pointless application.
            if (FontSize == font.size) return;
            font.size = FontSize;
            font.baseline = FontSize + 2;
        }

        // Applies to IMGUI.
        if (ApplyOnImgui && fontNames is not null)
        {
            _imguiFontHandlerObject ??= new GameObject(Mod.Info.Name);
            _imguiFontHandler ??= _imguiFontHandlerObject.AddComponent<ImguiFontHandler>();

            // Create a font to change.
            Font = Font.CreateDynamicFontFromOSFont(fontNames.Concat(new[] {"Arial"}).ToArray(), 13);
            // Note:
            // 'Arial' and '13' are from the default GUISkin. Not sure if that's the case in all environments.
            // specified Arial as the final fallback, otherwise there would be an issue where the text would overflowing the actual space of the UI.
            
            // Let ImguiFontHandler.OnGUI() do the application.
            _imguiFontHandler.applied = false;
        }
        else
        {
            // If not applied, do nothing.
            if (_imguiFontHandlerObject == null || _imguiFontHandler == null) return;
            
            // Let ImguiFontHandler.OnGUI() do the restoring the font and destroying itself.
            _imguiFontHandler.restoreNeeded = true;
            _imguiFontHandlerObject = null;
            _imguiFontHandler = null;
        }
    }
}