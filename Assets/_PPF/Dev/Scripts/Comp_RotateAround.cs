using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_RotateAround : MonoBehaviour
{
    [SerializeField] private GameObject m_ObjectToRotateAround;
    [SerializeField] private float m_RotationSpeed = 75f;
    [SerializeField] private float m_MinZoomDistance = 1f;
    [SerializeField] private float m_MaxZoomDistance = 2.5f;

    private Camera m_Camera;

    const string HORIZONTALAXIS = "Horizontal";
    const string VERTICALALAXIS = "Vertical";

    private void Start() {
        m_Camera = GetComponent<Camera>();

        if (m_Camera)
            m_Camera.transform.LookAt(m_ObjectToRotateAround.transform);
    }

    // Update is called once per frame
    void Update() {
        float horInput = Input.GetAxisRaw(HORIZONTALAXIS);
        float verInput = Input.GetAxisRaw(VERTICALALAXIS);
        if (m_Camera && m_ObjectToRotateAround) {
            if (horInput != 0) {
                float dir = horInput > 0 ? 1 : -1;
                m_Camera.transform.RotateAround(m_ObjectToRotateAround.transform.position, Vector3.up, m_RotationSpeed * -dir * Time.deltaTime);
                //m_Camera.transform.LookAt(m_ObjectToRotateAround.transform);
            }
            if (verInput != 0) {
                float distance = (m_Camera.transform.position - m_ObjectToRotateAround.transform.position).magnitude;
                float dir = verInput > 0 ? 1 : -1;
                if (distance > m_MinZoomDistance && verInput > 0) {
                    m_Camera.transform.Translate(new Vector3(0, 0, dir * Time.deltaTime), Space.Self);
                }
                if (distance < m_MaxZoomDistance && verInput < 0) {
                    m_Camera.transform.Translate(new Vector3(0, 0, dir * Time.deltaTime), Space.Self);
                }
            }
        }
    }
}
