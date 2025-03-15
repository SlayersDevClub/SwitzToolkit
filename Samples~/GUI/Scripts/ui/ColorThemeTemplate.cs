using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorThemes", menuName = "ScriptableObjects/ColorTheme", order = 1)]
public class ColorThemeTemplate : ScriptableObject
{
    [System.Serializable]

    public struct Button
    {
        public Color32 color;
        public Color icon;
    }


    [System.Serializable]
    public struct Window
    {
        public Color primaryColor;
        public Color tab;
        public Color darkSideColor;
    }

    [System.Serializable]
    public struct ListItem
    {
        public Color primaryColor;
        public Color secondaryColor;
    }

    [System.Serializable]
    public struct Slider
    {
        public Color32 trackColor;
        public Color32 trackFillColor;
        public Color32 thumbColor;
    }

    [System.Serializable]
    public struct Text
    {
        public Color 
            primaryColor, 
            secondaryColor, 
            interactiveColor, 
            disabledColor, 
            errorColor;

            public int primaryTextSize, secondaryTextSize;
    }

    [System.Serializable]
    public struct InputField
    {
        public Color primaryColor;
        public Color searchButtonColor;
    }

    [System.Serializable]
    public struct ScrollBar
    {
        public Color scrollFill, scrollHandle;
    }

    [System.Serializable]
    public struct ButtonPrimary
    {
        public Color color, text, icon;
        public int textSize;
    }
    [System.Serializable]
    public struct ButtonSecondary
    {
        public Color color, text, icon;
        public int textSize;
    }
    [System.Serializable]
    public struct ButtonFlat
    {
        public Color color, text, icon;
        public int textSize;
    }
    [System.Serializable]
    public struct ButtonSpecial
    {
        public Color color, text, icon;
        public int textSize;
    }
    [System.Serializable]
    public struct Gradient
    {
        public Color color1, color2;
    }

    [SerializeField]
    public Sprite logo;
    public Button button;
    public Slider slider;
    public Window window;
    public ListItem listItem;
    public Text text;
    public InputField inputField;
    public ScrollBar scrollBar;
    public ButtonPrimary buttonPrimary;
    public ButtonSecondary buttonSecondary;
    public ButtonFlat buttonFlat;
    public ButtonSpecial buttonSpecial;
    public Gradient gradient;

}


