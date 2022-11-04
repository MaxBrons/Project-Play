using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_InputUI : MonoBehaviour
{
    [SerializeField] private GameObject[] m_ElementsToShowOnPiecePickup;

    private void Start() {
        Comp_Drag[] pieces = FindObjectsOfType<Comp_Drag>();
        foreach(Comp_Drag piece in pieces) {
            piece.OnDrag += OnDrag;
            piece.OnRelease += OnRelease;
        }
    }

    private void OnRelease(GameObject obj) {
        foreach(var e in m_ElementsToShowOnPiecePickup) {
            e.SetActive(false);
        }
    }

    private void OnDrag(GameObject obj) {
        foreach (var e in m_ElementsToShowOnPiecePickup) {
            e.SetActive(true);
        }
    }
}
