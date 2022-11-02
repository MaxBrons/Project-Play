using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_ResetOnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision) {
        GameObject obj = collision.gameObject;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb && !rb.isKinematic) {
            rb.isKinematic = true;

            obj.transform.position = new Vector3(0, 3, 0);

            rb.isKinematic = false;
        }
    }
}
