using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chip;
using vjp;

public class Mover : MonoBehaviour {

    [SerializeField]
    private Board board;

    private void Update() {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            var picked = hit.transform.gameObject.GetComponent<ChipComponent>();
            if(picked == null) {
                board.currentChip = Option<ChipComponent>.None();
            }
            board.currentChip = Option<ChipComponent>.Some(picked);
        }


        if (board.currentChip.IsSome()) {
            var position = new Vector3(hit.point.x, 0, hit.point.z) + Vector3.up;
            board.currentChip.Peel().transform.position = position;
        }

        if (Input.GetMouseButtonUp(0)) {

            if(board.currentChip.IsNone()) {
                return;
            }

            var finalX = Mathf.RoundToInt(hit.point.x);
            var finalZ = Mathf.RoundToInt(hit.point.z);
            var finalPosition = new Vector3Int(finalX, 0, finalZ);

            board.currentChip.Peel().transform.position = finalPosition;
            board.currentChip = Option<ChipComponent>.None();

        }
    }
}
