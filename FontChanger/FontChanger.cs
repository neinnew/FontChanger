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

    public static Font? Font;

    public static string[] Sizes =
{
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"
        };
    public static int GetSizeIndex(int Size)
    {
        var index = -1;
        var sizeString = Size.ToString();
        for (var i = 0; i < Sizes.Length; i++)
            if (string.Compare(Sizes[i], sizeString, StringComparison.OrdinalIgnoreCase) == 0)
            {
                index = i;
                break;
            }
        return index;
    }
    public static void SetSizeByIndex(int index, string type)
    {
        switch (type)
        {
            case "FontSizeForOpenSansRegular":
                FontSizeForOpenSansRegular = int.Parse(Sizes[index]);
                break;
            case "FontSizeForOpenSansSemibold":
                FontSizeForOpenSansSemibold = int.Parse(Sizes[index]);
                break;
            case "FontSizeForArchitectsDaughter":
                FontSizeForArchitectsDaughter = int.Parse(Sizes[index]);
                break;
            default:
                FontSize = int.Parse(Sizes[index]);
                break;
        }
    }
    public static void Change()
    {
        // Creates a string array of font names.
        var fontNames = string.IsNullOrEmpty(FontFamily) ? null
            : FontFamily.Split(',').Select(name => name.Trim().Trim('"').Trim('\'')).ToArray();
        var fontSize = FontSize;
        var fontNamesOpenSansRegular = string.IsNullOrEmpty(FontFamilyForOpenSansRegular) ? null
            : FontFamilyForOpenSansRegular.Split(',').Select(name => name.Trim().Trim('"').Trim('\'')).ToArray();
        var fontOpenSansRegularSize = FontSizeForOpenSansRegular;
        var fontNamesOpenSansSemibold = string.IsNullOrEmpty(FontFamilyForOpenSansSemibold) ? null
            : FontFamilyForOpenSansSemibold.Split(',').Select(name => name.Trim().Trim('"').Trim('\'')).ToArray();
        var fontOpenSansSemiboldSize = FontSizeForOpenSansSemibold;
        var fontNamesArchitectsDaughter = string.IsNullOrEmpty(FontFamilyForArchitectsDaughter) ? null 
            : FontFamilyForArchitectsDaughter.Split(',').Select(name => name.Trim().Trim('"').Trim('\'')).ToArray();
        var fontArchitectsDaughterSize = FontSizeForArchitectsDaughter;

        // Applies to all text CO.UI.
        foreach (var component in UnityEngine.Object.FindObjectsOfType<UITextComponent>())
        {
            if (AdvancedFontFamily)
            {
                switch (component.font.name)
                {
                    case "OpenSans-Regular" :
                        if (fontNamesOpenSansRegular is null) continue;
                        component.font.baseFont.fontNames = fontNamesOpenSansRegular;
                        component.font.size = fontOpenSansRegularSize;
                        break;
                    case "OpenSans-Semibold" :
                        if (fontNamesOpenSansSemibold is null) continue;
                        component.font.baseFont.fontNames = fontNamesOpenSansSemibold;
                        component.font.size = fontOpenSansSemiboldSize;
                        break;
                    case "ArchitectsDaughter" :
                        if (fontNamesArchitectsDaughter is null) continue;
                        component.font.baseFont.fontNames = fontNamesArchitectsDaughter;
                        component.font.size = fontArchitectsDaughterSize;
                        break;
                    default:
                        if (fontNames is null) continue;
                        component.font.baseFont.fontNames = fontNames;
                        component.font.size = fontSize;
                        break;
                }
            }
            else
            {
                if (fontNames is null) continue;
                component.font.baseFont.fontNames = fontNames;
                component.font.size = fontSize;
            }
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
        
        UIView.RefreshAll();
    }
}