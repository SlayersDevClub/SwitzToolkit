using UnityEngine.UI;
public class GenericWindowContext : BaseContextMenu
{
    // Define your actions here for context menu population
    public void MinimizeWindow() 
    { 
        Toggle tog = transform.Find("Minimize_Button").GetComponent<Toggle>();
        tog.isOn = !tog.isOn;
        transform.Find("Minimize_Button").GetComponent<Toggle>().onValueChanged.Invoke(tog.isOn);
    }
    public void CloseWindow() 
    { 
        transform.Find("Close_Button").GetComponent<Button>().onClick.Invoke();
    }


}
