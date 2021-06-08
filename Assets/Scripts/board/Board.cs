using chip;
using System.Collections.Generic;
using UnityEngine;
using vjp;
using resources;

namespace board {
    public class Board : MonoBehaviour {

        [SerializeField]
        private Resource resource;

        private const int SIZE = 3;
        private const int LOWER_BOUND = 0;

        private bool isBlueTurn;

        public Option<ChipComponent>[,] cells = new Option<ChipComponent>[SIZE, SIZE];
        public bool isGameEnded;

        private void Awake() {
            isBlueTurn = true;
            isGameEnded = false;

            for (int i = 0; i < SIZE; i++) {
                for (int j = 0; j < SIZE; j++) {
                    cells[i, j] = Option<ChipComponent>.None();
                }
            }
        }

        public bool IsCorrectMove(ChipData chipData, int x, int z) {
            if (x < LOWER_BOUND || z < LOWER_BOUND || x >= SIZE || z >= SIZE) {
                return false;
            }

            if (cells[x, z].IsSome()) {

                var cellChipData = cells[x, z].Peel().chipData;
                if (chipData.isBlue == cellChipData.isBlue) {
                    return false;
                }

                if (chipData.size <= cellChipData.size) {
                    return false;
                }
            }

            return true;
        }

        public bool IsCorrectSelect(ChipData chipData) {
            if (chipData.isBlue != isBlueTurn) {
                return false;
            }

            if (chipData.isUsed) {
                return false;
            }

            return true;
        }

        public void MakeMove(ChipComponent currentChip, int x, int z) {

            if (cells[x, z].IsSome()) {
                Destroy(cells[x, z].Peel().gameObject);
            }
            var data = new ChipData() {
                x = x,
                z = z,
                isBlue = currentChip.chipData.isBlue,
                isUsed = currentChip.chipData.isUsed,
                size = currentChip.chipData.size
            };
            currentChip.chipData = data;
            cells[x, z] = Option<ChipComponent>.Some(currentChip);
            cells[x, z].Peel().chipData.isUsed = true;

            if (IsSideWin(cells,isBlueTurn)) {
                if (isBlueTurn) {
                    Debug.Log("Blue wins");
                } else {
                    Debug.Log("Red Wins");
                }
                isGameEnded = true;
                enabled = false;
                return;
            }

            isBlueTurn = !isBlueTurn;
        }

        public bool IsSideWin(Option<ChipComponent>[,] cells, bool isBlueTurn) {
            bool isWinCombination = false;
            foreach (var combination in resource.winCombinations) {
                bool color = isBlueTurn;

                foreach (var position in combination.itemsPosition) {
                    var combinationX = position.x;
                    var combinationZ = position.y;
                    if (cells[combinationX, combinationZ].IsNone()) {
                        isWinCombination = false;
                        break;
                    }
                    var chipColor = cells[combinationX, combinationZ].Peel().chipData.isBlue;
                    if(color != chipColor) {
                        isWinCombination = false;
                        break;
                    }
                    isWinCombination = true;
                }

                if (isWinCombination) {
                    break;
                }

            }
            return isWinCombination;
        }

    }
}

