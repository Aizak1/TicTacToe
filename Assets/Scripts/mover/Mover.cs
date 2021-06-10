
using UnityEngine;
using chip;
using vjp;
using board;

namespace mover {
    public class Mover : MonoBehaviour {

        [SerializeField]
        private Board board;

        private Option<ChipComponent> currentChip;
        private const float Y_ON_DRUGS = 0.5f;
        RaycastHit hit;


        private void Update() {
            if (Input.GetMouseButton(0)) {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out hit)) {
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
                    var position = new Vector3(hit.point.x, Y_ON_DRUGS, hit.point.z);
                    currentChip.Peel().transform.position = position;
                }
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

