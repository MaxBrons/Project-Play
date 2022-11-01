using System;
using System.Collections;
using UnityEngine;

public enum PieceType
{
    Corner,
    Edge
}

public class Draggable_Piece : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    public delegate void DragEvent(PieceType pieceType);
    public event DragEvent OnDrag;
    public event DragEvent OnRelease;

    public PieceType CubePieceType = PieceType.Corner;
    public Rigidbody Rb { get; private set; }

    private Vector3 _screenPoint;
    private Vector3 _objectOffset;
    private bool _isDragging = false;
    private Camera _camera;
    private Cube_Slot _currentSlot;
    private float minHeight = 0;

    private void Start() {
        _camera = Camera.main;
        Rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown() {
        _isDragging = true;
        OnDrag?.Invoke(CubePieceType);

        transform.parent = null;
        _currentSlot = null;

        if (Rb)
            Rb.isKinematic = true;

        _screenPoint = _camera.WorldToScreenPoint(gameObject.transform.position);
        _objectOffset = gameObject.transform.position - _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
    }

    private void OnMouseUp() {
        _isDragging = false;
        OnRelease?.Invoke(CubePieceType);

        if (_currentSlot) {
            if (_currentSlot.Occupy(gameObject)) {
                return;
            }
        }

        if (Rb)
            Rb.isKinematic = false;
    }

    private void Update() {
        if (_isDragging) {
            Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            Vector3 cursorWP = _camera.ScreenToWorldPoint(cursorPos);
            Vector3 newPosition = new Vector3(cursorWP.x, cursorWP.y >= minHeight ? cursorWP.y : minHeight, cursorWP.z) + _objectOffset;

            gameObject.transform.position = Vector3.Slerp(transform.position, newPosition, _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider collision) {

        if (!_currentSlot)
            _currentSlot = collision.gameObject.GetComponent<Cube_Slot>();
    }

    private void OnTriggerExit(Collider collision) {
        if (_currentSlot)
            _currentSlot = collision.gameObject == _currentSlot.gameObject ? null : _currentSlot;
    }
}
