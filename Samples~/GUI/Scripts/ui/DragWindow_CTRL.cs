using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWindow_CTRL : MonoBehaviour
{
    public Transform dragParent;
    [HideInInspector] public bool dragging = false;
    [HideInInspector] public Vector2 offsetPT;
    
}
