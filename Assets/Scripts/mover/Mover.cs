using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chip;
using vjp;

public class Mover : MonoBehaviour
{
    private Option<ChipComponent> currentChip;

    void Update() {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            var picked = hit.transform.gameObject.GetComponent<ChipComponent>();
            if(picked == null) {
                currentChip = Option<ChipComponent>.None();
            }
            currentChip = Option<ChipComponent>.Some(picked);
        }

        if (currentChip.IsSome()) {
            var position = new Vector3(hit.point.x, 0, hit.point.z) + Vector3.up;
            currentChip.Peel().transform.position = position;
        }

        if (Input.GetMouseButtonUp(0)) {

            if(currentChip.IsNone()) {
                return;
            }

            var finalX = Mathf.RoundToInt(hit.point.x);
            var finalZ = Mathf.RoundToInt(hit.point.z);
            var finalPosition = new Vector3Int(finalX, 0, finalZ);

            currentChip.Peel().transform.position = finalPosition;
            currentChip = Option<ChipComponent>.None();

        }
    }
}
