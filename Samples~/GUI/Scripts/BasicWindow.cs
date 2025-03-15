using UnityEngine;

public class BasicWindow : WindowBase
{
    public override void Start()
    {
        base.Start();
        // Additional initialization logic
        Debug.Log("BasicWindow initialized.");
    }

    public override void Dock(DockingZone dockingZone)
    {
        // Call the base docking logic
        base.Dock(dockingZone);
    }
}
