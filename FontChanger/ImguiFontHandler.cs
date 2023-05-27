using UnityEngine;

namespace FontChanger;

/// <summary>
/// A MonoBehavior class that uses OnGUI to handle font of IMGUI.
/// </summary>
public class ImguiFontHandler : MonoBehaviour
{
    private Font? _originalFont;

    public bool applied = false;
    public bool restoreNeeded = false;
    
    private void OnGUI()
    {
        if (!applied)
        {
            _originalFont ??= GUI.skin.font;
            GUI.skin.font = FontChanger.Font;
            applied = true;
        }

        if (restoreNeeded)
        {
            GUI.skin.font = _originalFont;
            Destroy(gameObject);
            Destroy(this);
        }
    }
}