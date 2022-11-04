using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_CubeSlot : MonoBehaviour
{
    public delegate void OccupyEvent();
    public event OccupyEvent OnOccupy;
    public event OccupyEvent OnUnoccypy;

    [SerializeField] private CubePieceTypes m_AllowedCubePiece;
    [SerializeField] private List<CubePieceColors> m_AllowedPieceColors = new List<CubePieceColors>() { CubePieceColors.White, CubePieceColors.White };

    private Comp_CubePiece m_CurrentPiece;

    public bool Compare(CubePieceTypes type, List<CubePieceColors> colors) {
        if (type != m_AllowedCubePiece)
            return false;

        if (colors.Count != m_AllowedPieceColors.Count)
            return false;

        foreach (CubePieceColors color in colors) {
            if (!m_AllowedPieceColors.Contains(color))
                return false;
        }
        return true;
    }

    public void SetActive(bool active) {
        gameObject.GetComponent<MeshRenderer>().enabled = active;
    }

    public bool IsOccupied() {
        return m_CurrentPiece;
    }

    public void Occupy(Comp_CubePiece cubePiece) {
        if (!m_CurrentPiece && cubePiece) {
            m_CurrentPiece = cubePiece;
            cubePiece.GetComponent<Rigidbody>().isKinematic = true;
            cubePiece.transform.SetPositionAndRotation(transform.position, transform.rotation);
            cubePiece.transform.parent = transform;
            OnOccupy?.Invoke();
        }
    }

    public void Unoccupy() {
        m_CurrentPiece = null;
        OnUnoccypy?.Invoke();
    }

    public Comp_CubePiece GetCurrentCubePiece() => m_CurrentPiece;
}
