using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RubiksCube : MonoBehaviour
{
    [SerializeField] private bool m_PhysicsOnStart = true;

    private Vector2 m_ForceMultiplierMinMax = new Vector2(20, 50);
    private Camera m_Camera;
    private List<Comp_CubeSlot> m_CubeSlots = new List<Comp_CubeSlot>();
    private bool m_ShouldFragment = true;

    private void Start() {
        m_Camera = Camera.main;
        m_Camera.transform.LookAt(transform);

        Transform[] ts = transform.GetComponentsInChildren<Transform>().Where(c => c.parent == transform && c.childCount == 0).ToArray();

        foreach (Transform t in ts) {
            Rigidbody goRb = t.GetComponent<Rigidbody>();
            Collider col = t.GetComponent<BoxCollider>();

            if (col)
                col.enabled = true;

            if (!goRb)
                continue;

            goRb.isKinematic = !m_PhysicsOnStart;
            if (m_PhysicsOnStart) {
                t.transform.parent = null;

                Vector3 dir = (transform.position - t.transform.position).normalized;

                Debug.DrawLine(transform.position, transform.position + (dir * 100), Color.red, 5);
                goRb.AddForce(-dir * 200 * Random.Range(m_ForceMultiplierMinMax.x, m_ForceMultiplierMinMax.y) * Time.deltaTime);
            }
        }

        m_CubeSlots = transform.GetComponentsInChildren<Comp_CubeSlot>().ToList();
        foreach (Comp_CubeSlot slot in m_CubeSlots) {
            slot.OnOccupy += OnOccupy;
        }

        StartCoroutine(DelayedRelease(10f));
    }

    private void OnOccupy() {
        if (m_CubeSlots.TrueForAll((s) => s.IsOccupied())) {
            print("DONE");
        }
    }

    private IEnumerator DelayedRelease(float seconds) {
        while (m_ShouldFragment) {
            Comp_CubeSlot[] cubes = m_CubeSlots.Where(c => c.IsOccupied()).ToArray();

            if (cubes.Length > 0) {
                Comp_CubeSlot randomSlot = cubes[Random.Range(0, cubes.Length - 1)];
                Comp_CubePiece piece = randomSlot.GetCurrentCubePiece();

                if (piece) {
                    randomSlot.Unoccupy();
                    piece.transform.parent = null;

                    Rigidbody rb = piece.GetComponent<Rigidbody>();
                    if (rb)
                        rb.isKinematic = false;


                    Vector3 dir = (piece.transform.position - transform.position).normalized;
                    rb.AddForce(dir * 300 * Random.Range(20, 50) * Time.deltaTime);
                }
            }
            yield return new WaitForSeconds(seconds);
        }
    }
}
