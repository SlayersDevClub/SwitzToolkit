using UnityEngine;

public class ItemBase : WindowBase
{
    public GameObject itemObject;

    public override void Start()
    {
        base.Start();
        elementSocketType = UIDraggableElement.Item;
        if (itemObject != null)
        {
            itemObject.SetActive(false);
        }
    }

    public void ResetItem()
    {
        if (itemObject != null)
        {
            itemObject.SetActive(false);
        }
    }



    public override void Dock(DockingZone dockingZone)
    {
        // Additional pre-docking logic
        Debug.Log("Advanced docking logic starting...");

        // Call the base docking logic
        base.Dock(dockingZone);

        // Additional post-docking logic
        Debug.Log("Advanced docking logic complete.");
        if(dockingZone.elementSocketType == UIDraggableElement.Item)
        {
            itemObject.SetActive(true);
        }
    }

    public override void Undock()
    {
        // Additional pre-undocking logic
        Debug.Log("Advanced undocking logic starting...");

        // Call the base undocking logic
        base.Undock();
        ResetItem();

        // Additional post-undocking logic
        Debug.Log("Advanced undocking logic complete.");
    }

}
