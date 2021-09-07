using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiFousNone : MonoBehaviour, ICanvasRaycastFilter
{
    public bool IsFocus = false;
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return IsFocus;
    }
}
