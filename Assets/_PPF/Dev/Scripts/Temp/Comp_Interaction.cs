using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_Interaction : MonoBehaviour
{
    [SerializeField] private float m_UpdateFrequency = 5;
    [SerializeField] private float m_DragScrollSpeed = .5f;

    private Camera m_Camera;
    private GameObject m_CurrentObject;
    private GameObject m_DragHandle;

    private Vector3 m_ScreenPoint;
    private Vector3 m_ObjectOffset;
    private bool m_ShouldUpdate = true;
    private bool m_IsDragging = false;

    const string MOUSESCROLLAXIS = "Mouse ScrollWheel";

    void Start() {
        Drag_Init();
        if (m_Camera == null)
            return;

        StartCoroutine(Interval_Update_Drag());
    }

    private void Drag_Init() {
        m_Camera = gameObject.GetComponent<Camera>();
        if (m_Camera != null) {
            m_DragHandle = Instantiate(new GameObject());
            m_DragHandle.transform.parent = m_Camera.transform;
            m_DragHandle.transform.position = m_Camera.transform.position;
            m_DragHandle.transform.rotation = m_Camera.transform.rotation;
            //m_DragHandle.transform.localScale = new Vector3(10, 10, 10);
        }
    }

    private void Update() {
        if (m_IsDragging) {
            m_ObjectOffset += m_Camera.transform.forward * Input.GetAxis(MOUSESCROLLAXIS) * m_DragScrollSpeed;
            Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z);
            Vector3 cursorWP = m_Camera.ScreenToWorldPoint(cursorPos);
            Vector3 newPosition = new Vector3(cursorWP.x, cursorWP.y >= 0f ? cursorWP.y : 0f, cursorWP.z) + m_ObjectOffset;
            m_DragHandle.transform.position = Vector3.Slerp(m_DragHandle.transform.position, newPosition, 10f * Time.deltaTime);
        }

    }

    private IEnumerator Interval_Update_Drag() {
        while (m_ShouldUpdate) {
            if (!m_IsDragging) {

                Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity)) {
                    if (m_CurrentObject != raycastHit.transform.gameObject) {

                        Comp_Drag newDragComp = raycastHit.transform.GetComponent<Comp_Drag>();
                        if (newDragComp) {
                            if (m_CurrentObject != null) {
                                Comp_Drag curDragComp = m_CurrentObject.GetComponent<Comp_Drag>();
                                curDragComp.OnDrag -= OnDrag;
                                curDragComp.OnRelease -= OnRelease;
                            }

                            m_CurrentObject = raycastHit.transform.gameObject;
                            newDragComp.OnDrag += OnDrag;
                            newDragComp.OnRelease += OnRelease;

                            m_DragHandle.transform.position = raycastHit.point;
                        }
                    }
                }
                else {
                    m_CurrentObject = null;
                }
            }

            yield return new WaitForSeconds(1 / m_UpdateFrequency);
        }
        yield return null;
    }

    private void OnDrag(GameObject obj) {
        m_IsDragging = true;

        if (obj != null && m_DragHandle != null) {
            obj.transform.parent = m_DragHandle.transform;
            m_ScreenPoint = m_Camera.WorldToScreenPoint(m_DragHandle.transform.position);
            m_ObjectOffset = m_DragHandle.transform.position - m_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z));
        }
    }

    private void OnRelease(GameObject obj) {
        m_IsDragging = false;
        if (obj != null && obj.transform.parent == m_DragHandle.transform) {
            obj.transform.parent = null;
        }
    }
}
