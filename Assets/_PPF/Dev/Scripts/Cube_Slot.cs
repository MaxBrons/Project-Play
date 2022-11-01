using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Slot : MonoBehaviour
{
    public PieceType CubePieceType = PieceType.Corner;
    public GameObject CurrentObject { get; private set; }

    public delegate void SlotEvent(GameObject obj);
    public event SlotEvent OnOccupy;
    public event SlotEvent OnDeOccupy;
    
    public bool Occupy(GameObject obj) {
        if (!CurrentObject) {
            CurrentObject = obj;
            CurrentObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
            CurrentObject.GetComponent<Draggable_Piece>().OnDrag += Cube_Slot_OnDrag;
            OnOccupy?.Invoke(CurrentObject);
            return true;
        }
        return false;
    }

    private void Cube_Slot_OnDrag(PieceType pieceType) {
        CurrentObject.GetComponent<Draggable_Piece>().OnDrag -= Cube_Slot_OnDrag;
        DeOccupy();
    }

    public void DeOccupy() {
        CurrentObject.GetComponent<Draggable_Piece>().OnDrag -= Cube_Slot_OnDrag;
        OnDeOccupy?.Invoke(CurrentObject);
        CurrentObject = null;
    }

    private IEnumerator ReleaseTimer(float TimeToWait) {
        yield return new WaitForSeconds(TimeToWait);
        DeOccupy();
    }
}
