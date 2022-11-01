using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateObject : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    private void Update() {
        gameObject.transform.eulerAngles += new Vector3(0, _rotationSpeed * Time.deltaTime, 0);
    }
}
