using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Cube_Slot_Manager : MonoBehaviour
{
    public delegate void CubeFinnishedEvent();
    public event CubeFinnishedEvent OnCubeFinnished;

    private List<Cube_Slot> cubeSlots = new List<Cube_Slot>();
    private GameObject MainCube;
    private bool _ShouldDestruct = true;

    private void Start() {
        Draggable_Piece[] pieces = FindObjectsOfType<Draggable_Piece>();
        foreach (var piece in pieces) {
            piece.OnDrag += OnDrag;
            piece.OnRelease += OnRelease;
        }

        MainCube = FindObjectOfType<RubiksCube>().gameObject;

        cubeSlots = FindObjectsOfType<Cube_Slot>().ToList();
        foreach (Cube_Slot slot in cubeSlots) {
            slot.OnOccupy += Slot_OnOccupy;
        }

        SetSlotsActive(PieceType.Edge, false);
        SetSlotsActive(PieceType.Corner, false);

        _ShouldDestruct = true;
        StartCoroutine(DelayedRelease(10));
    }

    private void Slot_OnOccupy(GameObject obj) {
        if (MainCube) {
            obj.transform.parent = MainCube.transform;
        }

        if (cubeSlots.All(c => c.CurrentObject)) {
            OnCubeFinnished?.Invoke();
            _ShouldDestruct = false;
        }
    }

    private void OnDrag(PieceType pieceType) {
        SetSlotsActive(pieceType, true);
    }

    private void OnRelease(PieceType pieceType) {
        SetSlotsActive(pieceType, false);
    }

    private void SetSlotsActive(PieceType type, bool active) {
        foreach (Cube_Slot slot in cubeSlots) {
            if (slot.CubePieceType != type || slot.CurrentObject)
                continue;

            slot.gameObject.SetActive(active);
        }
    }

    private IEnumerator DelayedRelease(float seconds) {
        while (_ShouldDestruct) {
            yield return new WaitForSeconds(seconds);
            if (_ShouldDestruct) {
                Cube_Slot[] cubes = cubeSlots.Where(c => c.CurrentObject != null).ToArray();

                if (cubes.Length > 0) {
                    Cube_Slot randomSlot = cubes[Random.Range(0, cubes.Length - 1)];
                    Draggable_Piece piece = randomSlot.CurrentObject.GetComponent<Draggable_Piece>();

                    if (piece) {
                        randomSlot.DeOccupy();
                        piece.transform.parent = null;

                        if (piece.Rb)
                            piece.Rb.isKinematic = false;


                        Vector3 dir = (piece.transform.position - MainCube.transform.position).normalized;
                        piece.Rb.AddForce(dir * 300 * Random.Range(20,50) * Time.deltaTime);
                        piece.GetComponent<Collider>().enabled = true;
                    }
                }
            }
        }
    }
}

