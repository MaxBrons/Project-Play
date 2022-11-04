using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_ResetOnCollision : MonoBehaviour
{
    [SerializeField] private Transform m_TeleportLocation;
    private void OnTriggerEnter(Collider collision) {
        GameObject obj = collision.gameObject;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb && !rb.isKinematic) {
            rb.isKinematic = true;

            obj.transform.position = m_TeleportLocation ? m_TeleportLocation.position : new Vector3(0, 1.5f, 0);

            rb.isKinematic = false;
        }
    }
}
