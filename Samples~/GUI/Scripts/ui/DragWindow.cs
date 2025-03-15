using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(DragWindow_CTRL))]
public class DragWindow : EventTrigger
{
    //  ****************************************************************************************
    //  Revisit to remove hard-coded draggable objects. Currently set up for options panel only.
    //  ****************************************************************************************

    private DragWindow_CTRL DragVars;

    private void Start()
    {
        DragVars = GetComponent<DragWindow_CTRL>();
        if (DragVars.dragParent == null) DragVars.dragParent = transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (DragVars.dragging)
        {
            DragVars.dragParent.position = new Vector2(Input.mousePosition.x + DragVars.offsetPT.x, Input.mousePosition.y + DragVars.offsetPT.y);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        DragVars.offsetPT = new Vector2(DragVars.dragParent.position.x - Input.mousePosition.x, DragVars.dragParent.position.y - Input.mousePosition.y);
        DragVars.dragging = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        DragVars.dragging = false;
    }
}
