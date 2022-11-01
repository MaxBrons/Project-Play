using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject Timer;
    [SerializeField] private GameObject FinalTimer;
    private Canvas canvas;

    private Cube_Slot_Manager manager;

    private void Start() {
        manager = FindObjectOfType<Cube_Slot_Manager>();
        canvas = gameObject.GetComponent<Canvas>();

        if (manager) {
            manager.OnCubeFinnished += Manager_OnCubeFinnished;
            canvas.enabled = false;
        }
    }

    private void Manager_OnCubeFinnished() {
        FinalTimer.GetComponent<TextMeshProUGUI>().SetText(Timer.GetComponent<TextMeshProUGUI>().text);
        canvas.enabled = true;
        Timer.SetActive(false);
    }
}
