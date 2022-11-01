using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision) {
        GameObject obj = collision.gameObject;

        if (obj.GetComponent<Draggable_Piece>()) {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = true;

            obj.transform.position = new Vector3(0, 3, 0);

            if (rb) rb.isKinematic = false;
        }
    }
}
