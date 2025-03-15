using UnityEngine;
using TMPro;

public class ToggleButtonVisualSwap : MonoBehaviour
{
    public TextMeshProUGUI onOffText;
    
    public void ToggleIt(bool b)
    {
        if(b)
        {
            onOffText.SetText("On");
        } 
        else
        {
            onOffText.SetText("Off");
        }
    }
}
