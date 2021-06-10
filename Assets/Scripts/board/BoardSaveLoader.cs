using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace board {
    public class BoardSaveLoader : MonoBehaviour {
        [SerializeField]
        private BoardParser boardParser;
        [SerializeField]
        private Board board;

        private const string NEW_GAME_PATH = "New.json";
        private const string LOAD_GAME_PATH = "Load.json";

        public void LoadGame() {
            string path = GetPersistentDataPath(LOAD_GAME_PATH);
            string jsonData = LoadTextFromFile(path);

            if (string.IsNullOrWhiteSpace(jsonData)) {
                Debug.LogError("No data in json file");
                return;
            }

            var optionBoardData = boardParser.DeserializeBoardData(jsonData);

            if(optionBoardData.IsNone()) {
                Debug.LogError("Wrong json data in loaded file");
                return;
            }

            board.ClearBoard();
            board.InitializeBoard(optionBoardData.Peel());

        }

        public void SaveGame() {
            string path = GetPersistentDataPath(LOAD_GAME_PATH);

            var boardData = board.GetBoardData();

            if (boardData.chipDatas == null) {
                Debug.LogError("Wrong data to save");
                return;
            }

            string jsonData = boardParser.SerializeBoardData(boardData);

            if (string.IsNullOrWhiteSpace(jsonData)) {
                Debug.LogError("Incorrect serialization");
                return;
            }

            SaveTextToFile(path, jsonData);
        }

        public void StartNewGame() {
            string path = GetStreamingAssetsPath(NEW_GAME_PATH);
            string jsonData = LoadTextFromFile(path);

            if (string.IsNullOrWhiteSpace(jsonData)) {
                Debug.LogError("No data in json file");
                return;
            }

            var optionBoardData = boardParser.DeserializeBoardData(jsonData);

            if (optionBoardData.IsNone()) {
                Debug.LogError("Wrong json data in loaded file");
                return;
            }

            board.ClearBoard();
            board.InitializeBoard(optionBoardData.Peel());
        }


        private string GetPersistentDataPath(string path) {
            return Path.Combine(Application.persistentDataPath, path);
        }

        private string GetStreamingAssetsPath(string path) {
            return Path.Combine(Application.streamingAssetsPath, path);
        }


        private void SaveTextToFile(string path, string text) {
            using (StreamWriter streamWriter = new StreamWriter(path)) {
                try {
                    streamWriter.Write(text);
                } catch (System.Exception ex) {
                    Debug.LogError(ex.Message);
                    return;
                }
            }
        }

        private string LoadTextFromFile(string path) {
            using (StreamReader reader = new StreamReader(path)) {

                if (!File.Exists(path)) {
                    Debug.LogError("File does not exist");
                    return null;
                }

                try {
                    string json = reader.ReadToEnd();
                    return json;
                } catch (System.Exception ex) {
                    Debug.LogError(ex.Message);
                    return null;
                }
            }
        }
    }
}

