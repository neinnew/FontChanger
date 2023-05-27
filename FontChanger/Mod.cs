using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace FontChanger;

public class Mod : IUserMod
{
    public string Name => Info.Name;
    public string Description => Info.Description;

    public static class Info
    {
        public static readonly string Name = "Font Changer";
        public static readonly string Description = "continued mod for game font changing";
    }

    public void OnEnabled()
    {
        ModConfig.Load();
    }

    public void OnSettingsUI(UIHelper helper)
    {
        helper.AddDropdown("Available fonts list - it is just list, selection do nothing.", new[] { " " }.Concat(Font.GetOSInstalledFontNames()).ToArray(), 0, _ => {});
        
        var textfieldFontFamily = helper.AddTextfield("Font Family", FontChanger.FontFamily, _ => { }, val =>
        {
            FontChanger.FontFamily = val;
            ModConfig.Save();
        }) as UITextField;
        textfieldFontFamily!.width = 744;
        
        var textfieldOpenSansRegular = helper.AddTextfield("Font Family (for OpenSans-Regular)", FontChanger.FontFamilyForOpenSansRegular, _ => { }, val =>
        {
            FontChanger.FontFamilyForOpenSansRegular = val;
            ModConfig.Save();
        }) as UITextField;
        textfieldOpenSansRegular!.width = 744;
        
        var textfieldOpenSansSemiBold = helper.AddTextfield("Font Family (for OpenSans-SemiBold)", FontChanger.FontFamilyForOpenSansSemibold, _ => { }, val =>
        {
            FontChanger.FontFamilyForOpenSansSemibold = val;
            ModConfig.Save();
        }) as UITextField;
        textfieldOpenSansSemiBold!.width = 744;
        
        var textfieldArchitectsDaughter = helper.AddTextfield("Font Family (for ArchitectsDaughter)", FontChanger.FontFamilyForArchitectsDaughter, _ => { }, val =>
        {
            FontChanger.FontFamilyForArchitectsDaughter = val;
            ModConfig.Save();
        }) as UITextField;
        textfieldArchitectsDaughter!.width = 744;
        
        helper.AddCheckbox("Apply on IMGUI", FontChanger.ApplyOnImgui, val =>
        {
            FontChanger.ApplyOnImgui = val;
            ModConfig.Save();
        });

        helper.AddCheckbox("Use advanced font family", FontChanger.AdvancedFontFamily, val =>
        {
            FontChanger.AdvancedFontFamily = val;
            ToggleAdvancedFontFamilyFieldVisibility();
            ModConfig.Save();
        });

        helper.AddButton("Apply", FontChanger.Change);

        FontChanger.Change();

        ToggleAdvancedFontFamilyFieldVisibility();

        void ToggleAdvancedFontFamilyFieldVisibility()
        {
            textfieldOpenSansRegular.parent.isVisible = FontChanger.AdvancedFontFamily;
            textfieldOpenSansSemiBold.parent.isVisible = FontChanger.AdvancedFontFamily;
            textfieldArchitectsDaughter.parent.isVisible = FontChanger.AdvancedFontFamily;
        }
    }
    
}