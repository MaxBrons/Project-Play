using UnityEngine;

public class Comp_Drag : MonoBehaviour
{
    public delegate void DragEvent(GameObject obj);
    public event DragEvent OnDrag;
    public event DragEvent OnRelease;

    [SerializeField] private float _speed = 5f;

    private Rigidbody m_Rb;

    private void Start() {
        m_Rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown() {
        if (m_Rb)
            m_Rb.isKinematic = true;

        OnDrag?.Invoke(gameObject);
    }

    private void OnMouseUp() {
        if (m_Rb)
            m_Rb.isKinematic = false;

        OnRelease?.Invoke(gameObject);
    }
}
