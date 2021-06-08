using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace board {
    public class BoardLoader : MonoBehaviour {
        [SerializeField]
        private BoardParser boardParser;
        [SerializeField]
        private Board board;

        private const string NEW_GAME_PATH = "New.json";
        private const string LOAD_GAME_PATH = "Load.json";

        public void LoadGame() {
            string path = GetPersistentDataPath(LOAD_GAME_PATH);
            if (!File.Exists(path)) {
                Debug.LogError("File does not exist");
                return;
            }

            string jsonData = boardParser.LoadFromJson(path);
            var boardData = boardParser.DeserializeChips(jsonData);
            board.ClearBoard();
            board.LoadBoard(boardData);

        }

        public void SaveGame() {
            string path = GetPersistentDataPath(LOAD_GAME_PATH);

            var boardData = board.SaveBoard();

            string jsonData = boardParser.SerializeChips(boardData);
            boardParser.SaveToJson(path, jsonData);
        }

        public void StartNewGame() {
            string path = GetStreamingAssetsPath(NEW_GAME_PATH);
            if (!File.Exists(path)) {
                Debug.LogError("File does not exist");
                return;
            }

            string jsonData = boardParser.LoadFromJson(path);
            var boardData = boardParser.DeserializeChips(jsonData);
            board.ClearBoard();
            board.LoadBoard(boardData);


        }
        private string GetPersistentDataPath(string path) {
            return Path.Combine(Application.persistentDataPath, path);
        }

        private string GetStreamingAssetsPath(string path) {
            return Path.Combine(Application.streamingAssetsPath, path);
        }
    }
}

