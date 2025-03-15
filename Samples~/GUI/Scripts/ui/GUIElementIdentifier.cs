using SS.Patterns;
using UnityEngine.UI;
using TMPro;
using SS.Enums;
using UnityEngine.UI.Extensions;

public class GUIElementIdentifier : Observer
{
    public GUIElementType ElementType;
    bool initialzed = false;

    private void OnEnable()
    {
        GUIColorThemeManager _themeManager = FindFirstObjectByType<GUIColorThemeManager>();
        _themeManager.Attach(this);

        if (!initialzed)
        {
            Notify(_themeManager);
            initialzed = true;
        }

    }

    public override void Notify(Subject subject)
    {
        GUIColorThemeManager _manager = subject.GetComponent<GUIColorThemeManager>();

        if (GetComponent<Image>() && ElementType == GUIElementType.Logo)
        {
            GetComponent<Image>().sprite = _manager.ChangeSprite(this);
            return;
        }

        // if (GetComponent<Gradient>())
        // {
        //     GetComponent<Gradient>().Vertex2 = _manager.ChangeGradientColor1(this);
        //     GetComponent<Gradient>().Vertex1 = _manager.ChangeGradientColor2(this);
        //     return;
        // }

        if (GetComponent<Image>())
        {
            GetComponent<Image>().color = _manager.ChangeColor(this);
            return;
        }

        if (GetComponent<TextMeshProUGUI>())
        {
            GetComponent<TextMeshProUGUI>().color = _manager.ChangeColor(this);
            GetComponent<TextMeshProUGUI>().fontSize = _manager.ChangeTextSize(this);
            return;
        }




    }
}
