using chip;
using System.Collections.Generic;
using UnityEngine;
using vjp;
using resources;
using combination;

namespace game {

    public enum GameResult {
        RedWins,
        BlueWins,
        Draw
    }

    public enum GameState {
        Paused,
        InProcessing,
    }

    public class GameManager : MonoBehaviour {

        [SerializeField]
        private Resource resource;

        private const int BOARD_SIZE = 3;
        private const int LOWER_BOUND = 0;

        public Option<ChipComponent>[,] board;

        public bool isBlueTurn;

        public GameState gameState;
        public GameResult gameResult;


        private void Awake() {
            isBlueTurn = true;
            gameState = GameState.Paused;
            board = new Option<ChipComponent>[BOARD_SIZE,BOARD_SIZE];

            for (int i = 0; i < BOARD_SIZE; i++) {
                for (int j = 0; j < BOARD_SIZE; j++) {
                    board[i, j] = Option<ChipComponent>.None();
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

            if (board[x, z].IsSome()) {

                var cellChipData = board[x, z].Peel().chipData;
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

            if (board[x, z].IsSome()) {
                Destroy(board[x, z].Peel().gameObject);
            }
            var data = new ChipData() {
                x = x,
                z = z,
                isBlue = currentChip.chipData.isBlue,
                isUsed = currentChip.chipData.isUsed,
                size = currentChip.chipData.size
            };
            currentChip.chipData = data;
            board[x, z] = Option<ChipComponent>.Some(currentChip);
            board[x, z].Peel().chipData.isUsed = true;

            if (IsTeamWin(board,isBlueTurn,resource)) {
                if (isBlueTurn) {
                    gameResult = GameResult.BlueWins;
                } else {
                    gameResult = GameResult.RedWins;
                }
                gameState = GameState.Paused;
                return;
            }

            isBlueTurn = !isBlueTurn;
            var chipsInGame = FindObjectsOfType<ChipComponent>();
            if (GetTeamMovesCount(chipsInGame, isBlueTurn) == 0) {
                gameResult = GameResult.Draw;
                gameState = GameState.Paused;
                return;
            }

        }

        public bool IsTeamWin(Option<ChipComponent>[,] board, bool isBlueTurn, Resource res) {

            bool isWinCombination = false;
            foreach (var combination in res.winCombinations) {
                bool color = isBlueTurn;

                foreach (var position in combination.itemsPosition) {
                    var combinationX = position.x;
                    var combinationZ = position.y;
                    if (board[combinationX, combinationZ].IsNone()) {
                        isWinCombination = false;
                        break;
                    }
                    var chipColor = board[combinationX, combinationZ].Peel().chipData.isBlue;
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

        public int GetTeamMovesCount(ChipComponent[] chipsInGame, bool isBlueTurn) {

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

        public GameData GetGameData() {
            var chipsInGame = FindObjectsOfType<ChipComponent>();
            ChipData[] chipDatas = new ChipData[chipsInGame.Length];
            for (int i = 0; i < chipDatas.Length; i++) {
                chipDatas[i] = chipsInGame[i].chipData;
            }

            var gameData = new GameData() {
                chipDatas = chipDatas,
                isBlueTurn = isBlueTurn
            };

            return gameData;
        }


        public void InitializeGame(GameData gameData) {
            gameState = GameState.InProcessing;
            isBlueTurn = gameData.isBlueTurn;
            foreach (var item in gameData.chipDatas) {
                GameObject chipModel;

                if (!resource.blueModels.ContainsKey(item.size)) {
                    Debug.LogError($"There is no model with {item.size} in resources");
                }

                if (item.isBlue) {
                    chipModel = resource.blueModels[item.size];
                } else {
                    chipModel = resource.redModels[item.size];
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

                board[(int)item.x, (int)item.z] = Option<ChipComponent>.Some(chip);
            }

        }

        public void ResetGame() {
            var chipsInGame = FindObjectsOfType<ChipComponent>();
            board = new Option<ChipComponent>[BOARD_SIZE, BOARD_SIZE];
            foreach (var item in chipsInGame) {
                Destroy(item.gameObject);
            }
        }


    }
}

