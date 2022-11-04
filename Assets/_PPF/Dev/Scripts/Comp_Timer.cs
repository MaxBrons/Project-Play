using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Comp_Timer : MonoBehaviour
{
    public delegate void TimerEvent();
    public event TimerEvent OnTimerStart;
    public event TimerEvent OnTimerStop;
    public event TimerEvent OnTimerResumed;
    public event TimerEvent OnTimerFinnished;
    public event TimerEvent OnTimerUpdate;

    [SerializeField] private bool m_RunOnStart = false;
    [SerializeField] private bool m_CountDown = true;
    [SerializeField] private float m_StartTime = 3 * 60;

    private TextMeshProUGUI text;
    private bool m_Run = false;
    private float m_Time = 0f;

    private void Start() {
        text = GetComponent<TextMeshProUGUI>();
        m_Time = m_StartTime;

        if (m_RunOnStart)
            StartTimer();
        SetTimerText();
    }

    public void StartTimer() {
        m_Run = true;
        ResetTimer();
        OnTimerStart?.Invoke();
    }

    public void StopTimer() {
        m_Run = false;
        OnTimerStop?.Invoke();
    }
    
    public void ResumeTimer() {
        m_Run = true;
        OnTimerResumed?.Invoke();
    }
    public void ResetTimer() => m_Time = m_StartTime;
    public float GetTime() => m_Time;
    public void SetTimerEnabled(bool enabled) {
        text.enabled = enabled;
    }

    private void Update() {
        if (!m_Run)
            return;

        m_Time += m_CountDown ? -Time.deltaTime : Time.deltaTime;

        if (m_CountDown) {
            if(m_Time <= 0) {
                ResetTimer();
                StopTimer();
                OnTimerFinnished?.Invoke();
            }
        }
        SetTimerText();
        OnTimerUpdate?.Invoke();
    }

    private void SetTimerText() {
        int minutes = (int)m_Time / 60;
        int seconds = (int)m_Time % 60;
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
