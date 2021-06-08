
using UnityEngine;
using chip;
using vjp;
using board;

namespace mover {
    public class Mover : MonoBehaviour {

        [SerializeField]
        private Board board;

        private Option<ChipComponent> currentChip;

        private void Update() {

            if (board.isGameEnded) {
                enabled = false;
                return;
            }

            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
                return;
            }

            if (Input.GetMouseButtonDown(0)) {
                var picked = hit.transform.gameObject.GetComponent<ChipComponent>();
                if (picked == null || !board.IsCorrectSelect(picked.chipData)) {
                    currentChip = Option<ChipComponent>.None();
                    return;
                }
                currentChip = Option<ChipComponent>.Some(picked);
            }


            if (currentChip.IsSome()) {
                var position = new Vector3(hit.point.x, 0, hit.point.z) + Vector3.up;
                currentChip.Peel().transform.position = position;
            }

            if (Input.GetMouseButtonUp(0)) {

                if (currentChip.IsNone()) {
                    return;
                }

                var finalX = Mathf.RoundToInt(hit.point.x);
                var finalZ = Mathf.RoundToInt(hit.point.z);

                if (!board.IsCorrectMove(currentChip.Peel().chipData, finalX, finalZ)) {
                    var startX = currentChip.Peel().chipData.x;
                    var startZ = currentChip.Peel().chipData.z;
                    var startPosition = new Vector3(startX, 0, startZ);
                    currentChip.Peel().transform.position = startPosition;
                    currentChip = Option<ChipComponent>.None();
                    return;
                }
                board.MakeMove(currentChip.Peel(), finalX, finalZ);

                var finalPosition = new Vector3Int(finalX, 0, finalZ);
                currentChip.Peel().transform.position = finalPosition;
                currentChip = Option<ChipComponent>.None();

            }
        }
    }
}

