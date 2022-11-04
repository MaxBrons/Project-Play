using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_ObjectToHideOnGameFinnished;
    [SerializeField] private GameObject[] m_ObjectToShowOnGameFinnished;

    private Comp_Timer m_Timer;
    private RubiksCube m_Cube;
    private Comp_EndScreen m_EndScreen;
    private Comp_RotateAround m_RotateComp;
    private Comp_AutoRotate m_AutoRotateComp;

    private void Start() {
        m_Timer = FindObjectOfType<Comp_Timer>();
        m_Cube = FindObjectOfType<RubiksCube>();
        m_EndScreen = FindObjectOfType<Comp_EndScreen>();
        m_RotateComp = FindObjectOfType<Comp_RotateAround>();
        m_AutoRotateComp = FindObjectOfType<Comp_AutoRotate>();

        Init();
    }

    private void Init() {
        if (m_Timer) {
            m_Timer.OnTimerFinnished += OnTimerFinnished;
        }
        if (m_Cube) {
            m_Cube.OnCubeEssambled += OnCubeEssambled;
            m_Cube.OnCubeSlotOccupied += OnCubeSlotOccupied;
        }
    }

    private void OnCubeSlotOccupied() {
        m_Cube.OnCubeSlotOccupied -= OnCubeSlotOccupied;
        if (m_Timer)
            m_Timer.StartTimer();
        
        m_Cube.StartFragmenting();
    }

    private void OnCubeEssambled() {
        m_Timer.StopTimer();
        EndGame();
    }

    private void OnTimerFinnished() {
        EndGame();
    }

    private void EndGame() {
        if (m_EndScreen) {
            m_EndScreen.SetupScreen(m_Timer.GetTime());
        }

        if (m_Timer) {
            m_Timer.OnTimerFinnished -= OnTimerFinnished;
            m_Timer.SetTimerEnabled(false);
        }

        if (m_Cube) {
            m_Cube.OnCubeEssambled -= OnCubeEssambled;
            m_Cube.StopFragmenting();
        }

        if (m_RotateComp) {
            m_RotateComp.enabled = false;

            if (m_AutoRotateComp) {
                m_AutoRotateComp.enabled = true;
            }
        }

        foreach(GameObject obj in m_ObjectToHideOnGameFinnished) {
            obj.SetActive(false);
        } 
        foreach(GameObject obj in m_ObjectToShowOnGameFinnished) {
            obj.SetActive(true);
        }
    }
}
