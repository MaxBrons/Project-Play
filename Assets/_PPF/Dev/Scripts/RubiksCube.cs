using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RubiksCube : MonoBehaviour
{
    [SerializeField] private bool PhysicsOnStart = true;
    [SerializeField] private float RotationSpeed = 10f;

    private List<GameObject> _Children = new List<GameObject>();
    private Vector2 _ForceMultiplierMinMax = new Vector2(20,50);
    private float _StartX;
    private Vector3 _StartRot;
    private Camera _MainCamera;
    private Cube_Slot_Manager _Slot_Manager;

    private void Start() {
        _MainCamera = Camera.main;
        _MainCamera.transform.LookAt(transform);

        Transform[] ts = transform.GetComponentsInChildren<Transform>().Where(c=>c.parent == transform && c.childCount == 0).ToArray();
        foreach (Transform t in ts) {
            _Children.Add(t.gameObject);
        }

        foreach (GameObject go in _Children) {
            Rigidbody goRb = go.GetComponent<Rigidbody>();
            Collider col = go.GetComponent<BoxCollider>();

            if (col) 
                col.enabled = true;

            if (!goRb) continue;

            goRb.isKinematic = !PhysicsOnStart;
            if (PhysicsOnStart) {
                go.transform.parent = null;

                Vector3 dir = (transform.position - go.transform.position).normalized;

                Debug.DrawLine(transform.position, transform.position + (dir * 100), Color.red, 5);
                goRb.AddForce(-dir * 200 * Random.Range(_ForceMultiplierMinMax.x,_ForceMultiplierMinMax.y) * Time.deltaTime);
            }
        }

        _Slot_Manager = FindObjectOfType<Cube_Slot_Manager>();
        _Slot_Manager.OnCubeFinnished += _Slot_Manager_OnCubeFinnished;
    }

    private void _Slot_Manager_OnCubeFinnished() {
        _Slot_Manager.OnCubeFinnished -= _Slot_Manager_OnCubeFinnished;
        foreach (GameObject go in _Children) {
            Collider col = go.GetComponent<Collider>();
            if (col)
                col.enabled = false;
        }
    }

    private void OnMouseDown() {
        _StartX = Input.mousePosition.x;
        _StartRot = gameObject.transform.eulerAngles;
    }
    private void OnMouseDrag() {
        Vector3 newRot = new Vector3(0, _StartRot.y + (_StartX - Input.mousePosition.x), 0);
        transform.eulerAngles = new Vector3(newRot.x, newRot.y, newRot.z);
    }

    private void Update() {
        float horInput = Input.GetAxis("Horizontal");

        if (horInput != 0) {
            _MainCamera.transform.LookAt(transform);
            _MainCamera.transform.Translate(new Vector3(Mathf.Clamp(horInput, -1, 1) * RotationSpeed * Time.deltaTime, 0, 0));
        }
    }
}
