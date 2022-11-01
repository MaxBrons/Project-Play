using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float _time { get; private set; } = 0f;
    
    private TextMeshProUGUI text;
    private Cube_Slot_Manager _slot_Manager;
    private bool _run = false;

    private void Start() {
        text = GetComponent<TextMeshProUGUI>();
        _slot_Manager = FindObjectOfType<Cube_Slot_Manager>();
        _slot_Manager.OnCubeFinnished += _slot_Manager_OnCubeFinnished;

        SetTimerActive(true, true);
    }

    private void _slot_Manager_OnCubeFinnished() {
        SetTimerActive(false, true);
    }

    private void SetTimerActive(bool active, bool reset = false) {
        _run = active;
        if (reset)
            _time = 0f;
    }

    private void Update() {
        if (!_run) return;

        _time += Time.deltaTime;

        int minutes = (int)_time / 60;
        int seconds = (int)_time % 60;
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
