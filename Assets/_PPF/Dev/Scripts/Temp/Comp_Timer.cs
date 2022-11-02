using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Comp_Timer : MonoBehaviour
{
    [SerializeField] private bool m_RunOnStart = false;

    private TextMeshProUGUI text;
    private bool m_Run = false;
    private float m_Time = 0f;

    private void Start() {
        text = GetComponent<TextMeshProUGUI>();

        if (m_RunOnStart)
            StartTimer();
    }

    public void StartTimer() => m_Run = true;
    public void StopTimer() => m_Run = false;
    public void ResetTimer() => m_Time = 0f;
    public float GetTime() => m_Time;

    private void Update() {
        if (!m_Run) return;

        m_Time += Time.deltaTime;

        int minutes = (int)m_Time / 60;
        int seconds = (int)m_Time % 60;
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
