using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CubePieceTypes
{
    Corner,
    Edge
}

public enum CubePieceColors
{
    Blue,
    Orange,
    Green,
    Red,
    White,
    Yellow
}

public class Comp_CubePiece : MonoBehaviour
{
    [SerializeField] private CubePieceTypes m_CubePiece;
    [SerializeField] private List<CubePieceColors> m_CubePieceColors = new List<CubePieceColors>() { CubePieceColors.White, CubePieceColors.White };
    [SerializeField] private float m_SearchRadius = 2f;

    private Comp_Drag m_DragComp;
    private Comp_CubeSlot m_CurrentSlot;
    private bool m_ShouldSearch = false;
    private int m_UpdateFrequency = 20;

    private void Start() {
        m_DragComp = GetComponent<Comp_Drag>();
        if (m_DragComp != null) {
            m_DragComp.OnDrag += OnDrag;
            m_DragComp.OnRelease += OnRelease;
        }
    }

    private void OnDrag(GameObject obj) {
        m_ShouldSearch = true;

        if (m_CurrentSlot) {
            m_CurrentSlot.Unoccupy();
            m_CurrentSlot = null;
        }

        StartCoroutine(Interval_Update());
    }

    private void OnRelease(GameObject obj) {
        m_ShouldSearch = false;

        if (m_CurrentSlot) {
            m_CurrentSlot.SetActive(false);
            TryOccupy();
        }
    }

    IEnumerator Interval_Update() {
        while (m_ShouldSearch) {
            Comp_CubeSlot slot = FindSlot();
            if (slot != null) {
                m_CurrentSlot = slot;
                m_CurrentSlot.SetActive(true);
            }
            else {
                if (m_CurrentSlot)
                    m_CurrentSlot.SetActive(false);

                m_CurrentSlot = null;
            }

            yield return new WaitForSeconds(1 / m_UpdateFrequency);
        }
        yield return null;
    }

    private void TryOccupy() {
        if (m_CurrentSlot) { 
            m_CurrentSlot.Occupy(this);
            return;
        }
        m_CurrentSlot = null;
    }

    private Comp_CubeSlot FindSlot() {
        RaycastHit[] results = Physics.SphereCastAll(transform.position, m_SearchRadius, Vector3.up);
        foreach (RaycastHit hit in results) {
            Comp_CubeSlot slot = hit.transform.GetComponent<Comp_CubeSlot>();
            if (slot != null) {
                if (slot.Compare(m_CubePiece, m_CubePieceColors)) {
                    return slot;
                }
            }
        }
        return null;
    }
}
