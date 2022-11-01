using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
    private void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Application.Quit();
        }
    }
}
