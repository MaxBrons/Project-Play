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
    [SerializeField] private float m_SearchRadius = 5f;

    private Comp_Drag m_DragComp;
    private Comp_CubeSlot m_CurrentSlot;
    private Comp_CubeSlot m_CorrespondingSlot;
    private bool m_ShouldSearch = false;
    private int m_UpdateFrequency = 20;

    private void Start() {
        m_DragComp = GetComponent<Comp_Drag>();
        if (m_DragComp != null) {
            m_DragComp.OnDrag += OnDrag;
            m_DragComp.OnRelease += OnRelease;
        }

        m_CorrespondingSlot = FindCorrespondingCubeSlot();
    }

    private Comp_CubeSlot FindCorrespondingCubeSlot() {
        var slots = FindObjectsOfType<Comp_CubeSlot>();
        foreach (Comp_CubeSlot slot in slots)
            if (slot.Compare(m_CubePiece, m_CubePieceColors)) {
                return slot;
            }
        return null;
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
            m_DragComp.Lock();
            m_CurrentSlot.OnUnoccypy += OnCurrentSlotUnoccupy;
            return;
        }
        m_CurrentSlot = null;
    }

    private void OnCurrentSlotUnoccupy() {
        m_CurrentSlot.OnUnoccypy -= OnCurrentSlotUnoccupy;
        m_DragComp.UnLock();
    }
    private Comp_CubeSlot FindSlot() {
        if (Vector3.Distance(transform.position, m_CorrespondingSlot.transform.position) <= m_SearchRadius)
            return m_CorrespondingSlot;
        return null;

        //RaycastHit[] results = Physics.SphereCastAll(transform.position, m_SearchRadius, Vector3.up);
        //foreach (RaycastHit hit in results) {
        //    Comp_CubeSlot slot = hit.transform.GetComponent<Comp_CubeSlot>();
        //    if (slot != null) {
        //        if (slot.Compare(m_CubePiece, m_CubePieceColors)) {
        //            return slot;
        //        }
        //    }
        //}
        //return null;
    }
}
