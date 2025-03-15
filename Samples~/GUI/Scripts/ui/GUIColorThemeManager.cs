using UnityEngine;
using SS.Patterns;
using SS.Enums;

public class GUIColorThemeManager : Subject
{
    public ColorThemeTemplate currentColorTheme;

    private void OnValidate()
    {
        Notify();
    }

    public Color ChangeColor(GUIElementIdentifier id)
    {
        Color _color = new Color();

        switch (id.ElementType)
        {
            case GUIElementType.None:
                break;
            case GUIElementType.Button:
                break;
            case GUIElementType.Toggle:
                break;
            case GUIElementType.Slider:
                break;
            case GUIElementType.InputField:
                break;
            case GUIElementType.Dropdown:
                break;
            case GUIElementType.ScrollView:
                break;
            case GUIElementType.Image:
                break;
            case GUIElementType.Logo:
                break;
            case GUIElementType.Text:
                break;
            case GUIElementType.TextArea:
                break;
            case GUIElementType.Panel:
                break;
            case GUIElementType.Window:
                break;
            case GUIElementType.ListView:
                break;
            case GUIElementType.ProgressBar:
                break;
            default:
                print("GUI ID not recognized!");
                return Color.magenta;
        }
        return _color;
    }

    public Color ChangeGradientColor1(GUIElementIdentifier id)
    {
        Color _color = new Color();
        switch (id.ElementType)
        {
            case GUIElementType.Gradient:
                _color = currentColorTheme.gradient.color1;
                break;
            default:
                break;
        }
        return _color;
    }
    public Color ChangeGradientColor2(GUIElementIdentifier id)
    {
        Color _color = new Color();
        switch (id.ElementType)
        {
            case GUIElementType.Gradient:
                _color = currentColorTheme.gradient.color2;
                break;
            default:
                break;
        }
        return _color;
    }

    public Sprite ChangeSprite(GUIElementIdentifier id)
    {
        Sprite _sprite;

        switch (id.ElementType)
        {
            case GUIElementType.Logo:
                _sprite = currentColorTheme.logo;
                break;
            default:
                _sprite = null;
                print("No Sprite found in the template!");
                break;
        }
        return _sprite;
    }

    public int ChangeTextSize(GUIElementIdentifier id)
    {
        int textSize;

        switch(id.ElementType)
        {
            default:
                textSize = 18;
                break;
        }

        return textSize;
    }
}
