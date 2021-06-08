using chip;
using System.Collections.Generic;
using UnityEngine;
using vjp;
using resources;

namespace board {
    public class Board : MonoBehaviour {

        [SerializeField]
        private Resource resource;

        private const int BOARD_SIZE = 3;
        private const int LOWER_BOUND = 0;

        public bool isBlueTurn;

        public Option<ChipComponent>[,] cells = new Option<ChipComponent>[BOARD_SIZE, BOARD_SIZE];
        public bool isGameProcessing;

        private void Awake() {
            isBlueTurn = true;
            isGameProcessing = false;

            for (int i = 0; i < BOARD_SIZE; i++) {
                for (int j = 0; j < BOARD_SIZE; j++) {
                    cells[i, j] = Option<ChipComponent>.None();
                }
            }
        }

        public bool IsCorrectMove(ChipData chipData, int x, int z) {
            if (x < LOWER_BOUND || z < LOWER_BOUND || x >= BOARD_SIZE || z >= BOARD_SIZE) {
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
                isGameProcessing = false;
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


        public BoardData SaveBoard() {
            var chips = FindObjectsOfType<ChipComponent>();
            ChipData[] chipDatas = new ChipData[chips.Length];
            for (int i = 0; i < chipDatas.Length; i++) {
                chipDatas[i] = chips[i].chipData;
            }

            var boardData = new BoardData() {
                chipDatas = chipDatas,
                isBlueTurn = isBlueTurn
            };

            return boardData;
        }


        public void LoadBoard(BoardData boardData) {
            isGameProcessing = true;
            isBlueTurn = boardData.isBlueTurn;
            foreach (var item in boardData.chipDatas) {
                GameObject chipModel;
                if (item.isBlue) {
                    chipModel = resource.blueChips[item.size];
                } else {
                    chipModel = resource.redChips[item.size];
                }

                var position = new Vector3(item.x, 0, item.z);
                var rotation = chipModel.transform.rotation;
                var chipObject = Instantiate(chipModel, position, rotation, transform);

                var chip = chipObject.GetComponent<ChipComponent>();
                chip.chipData = item;

                bool isLowerThanBoard = item.x < LOWER_BOUND || item.z < LOWER_BOUND;
                bool isHigherThanBoard = item.x >= BOARD_SIZE || item.z >= BOARD_SIZE;
                if (isLowerThanBoard || isHigherThanBoard) {
                    continue;
                }

                cells[(int)item.x, (int)item.z] = Option<ChipComponent>.Some(chip);
            }

        }

        public void ClearBoard() {
            cells = new Option<ChipComponent>[BOARD_SIZE, BOARD_SIZE];
            var chips = FindObjectsOfType<ChipComponent>();
            foreach (var item in chips) {
                Destroy(item.gameObject);
            }
        }
    }
}

