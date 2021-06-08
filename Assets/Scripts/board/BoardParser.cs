using UnityEngine;
using System.IO;

namespace board {
    public class BoardParser : MonoBehaviour {

        public string SerializeChips(BoardData boardData) {
            return JsonUtility.ToJson(boardData);
        }

        public BoardData DeserializeChips(string json) {
            return JsonUtility.FromJson<BoardData>(json);
        }

        public void SaveToJson(string path, string json) {
            using (StreamWriter streamWriter = new StreamWriter(path)) {
                streamWriter.Write(json);
            }
        }

        public string LoadFromJson(string path) {
            using (StreamReader reader = new StreamReader(path)) {
                string json = reader.ReadToEnd();
                return json;
            }
        }
    }
}

