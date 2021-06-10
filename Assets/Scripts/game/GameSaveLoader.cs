using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace game {
    public class GameSaveLoader : MonoBehaviour {

        [SerializeField]
        private GameDataJsonConverter converter;
        [SerializeField]
        private GameManager manager;

        private const string NEW_GAME_PATH = "New.json";
        private const string LOAD_GAME_PATH = "Load.json";

        public void LoadGame() {
            string path = GetPersistentDataPath(LOAD_GAME_PATH);
            string jsonData = LoadTextFromFile(path);

            if (string.IsNullOrWhiteSpace(jsonData)) {
                Debug.LogError("No data in json file");
                return;
            }

            var optionGameData = converter.DeserializeGameData(jsonData);

            if(optionGameData.IsNone()) {
                Debug.LogError("Wrong json data in loaded file");
                return;
            }

            manager.ResetGame();
            manager.InitializeGame(optionGameData.Peel());

        }

        public void SaveGame() {
            string path = GetPersistentDataPath(LOAD_GAME_PATH);

            var gameData = manager.GetGameData();

            if (gameData.chipDatas == null) {
                Debug.LogError("Wrong data to save");
                return;
            }

            string jsonData = converter.SerializeGameData(gameData);

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

            var optionGameData = converter.DeserializeGameData(jsonData);

            if (optionGameData.IsNone()) {
                Debug.LogError("Wrong json data in loaded file");
                return;
            }

            manager.ResetGame();
            manager.InitializeGame(optionGameData.Peel());
        }

        public void Quit() {
            Application.Quit();
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

