using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_RotateWithMouse : MonoBehaviour
{
    private float m_StartX;
    private Vector3 m_StartRot;

    private void OnMouseDown() {
        m_StartX = Input.mousePosition.x;
        m_StartRot = gameObject.transform.eulerAngles;
    }
    private void OnMouseDrag() {
        Vector3 newRot = new Vector3(0, m_StartRot.y + (m_StartX - Input.mousePosition.x), 0);
        transform.eulerAngles = new Vector3(newRot.x, newRot.y, newRot.z);
    }
}
