using chip;
using System.Collections.Generic;
using UnityEngine;
using vjp;
using resources;

namespace board {

    public enum GameResult {
        RedWins,
        BlueWins,
        Draw
    }

    // класс называется доской, но ведёт себя как полноценный ящик
    public class Board : MonoBehaviour {

        [SerializeField]
        private Resource resource;

        private const int BOARD_SIZE = 3;
        private const int LOWER_BOUND = 0;

        // дважды храним чипы, неужели этого никак нельзя было избежать?
        public Option<ChipComponent>[,] cells;
        public List<ChipComponent> chipsInGame;

        public bool isBlueTurn;

        // какой-то левый флаг, значение которого сложно понять без контекста использования
        // использования таких вещей нужно избегать
        public bool isGameProcessing;
        public GameResult gameResult;

        private void Awake() {
            isBlueTurn = true;
            isGameProcessing = false;

            cells = new Option<ChipComponent>[BOARD_SIZE,BOARD_SIZE];
            chipsInGame = new List<ChipComponent>();

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

            if (chipData.isUsed) {
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
                chipsInGame.Remove(cells[x, z].Peel());
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

            if (IsTeamWin(cells,isBlueTurn,resource)) {
                if (isBlueTurn) {
                    gameResult = GameResult.BlueWins;
                } else {
                    gameResult = GameResult.RedWins;
                }
                isGameProcessing = false;
                return;
            }

            isBlueTurn = !isBlueTurn;

            if (GetTeamMovesCount(chipsInGame, isBlueTurn) == 0) {
                gameResult = GameResult.Draw;
                isGameProcessing = false;
                return;
            }

        }

        public bool IsTeamWin(Option<ChipComponent>[,] cells, bool isBlueTurn, Resource res) {

            bool isWinCombination = false;
            foreach (var combination in res.winCombinations) {
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

        public int GetTeamMovesCount(List<ChipComponent> chipsInGame, bool isBlueTurn) {

            int movesCount = 0;
            foreach (var item in chipsInGame) {

                if(item.chipData.isBlue != isBlueTurn) {
                    continue;
                }

                if (item.chipData.isUsed) {
                    continue;
                }

                for (int i = 0; i < BOARD_SIZE; i++) {
                    for (int j = 0; j < BOARD_SIZE; j++) {
                        if (IsCorrectMove(item.chipData, i, j)) {
                            movesCount++;
                        }
                    }
                }

            }
            return movesCount;
        }

        // этот метод ничего не сохраняет, название врёт
        public BoardData SaveBoard() {
            ChipData[] chipDatas = new ChipData[chipsInGame.Count];
            for (int i = 0; i < chipDatas.Length; i++) {
                chipDatas[i] = chipsInGame[i].chipData;
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

                chipsInGame.Add(chip);

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
            foreach (var item in chipsInGame) {
                Destroy(item.gameObject);
            }
            chipsInGame = new List<ChipComponent>();
        }
    }
}

