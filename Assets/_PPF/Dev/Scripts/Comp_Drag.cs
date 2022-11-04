using UnityEngine;

public class Comp_Drag : MonoBehaviour
{
    public delegate void DragEvent(GameObject obj);
    public event DragEvent OnDrag;
    public event DragEvent OnRelease;

    [SerializeField] private float _speed = 5f;

    private Rigidbody m_Rb;
    private bool m_Locked = false;

    private void Start() {
        m_Rb = GetComponent<Rigidbody>();
    }

    public void Lock() => m_Locked = true;
    public void UnLock() => m_Locked = false;

    public void Drag() {
        if (m_Locked)
            return;

        if (m_Rb)
            m_Rb.isKinematic = true;

        OnDrag?.Invoke(gameObject);
    }

    public void Release() {
        if (m_Locked)
            return;

        if (m_Rb)
            m_Rb.isKinematic = false;

        OnRelease?.Invoke(gameObject);
    }
}
