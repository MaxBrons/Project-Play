using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Comp_EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject FinalTime;
    private Canvas canvas;

    private void Start() {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void SetupScreen(float endTime) {
        int minutes = (int)endTime / 60;
        int seconds = (int)endTime % 60;
        string time = string.Format("{0:00}:{1:00}", minutes, seconds);
        FinalTime.GetComponent<TextMeshProUGUI>().SetText(time);
        canvas.enabled = true;
    }
}
