using UnityEngine;
using System.IO;

namespace board {
    // не так уж много парсинга тут, я бы сказал около 0
    public class BoardParser : MonoBehaviour {

        public string SerializeBoardData(BoardData boardData) {
            // кидает ли JSONUtility эксепшны? почему нет обработки ошибок?
            return JsonUtility.ToJson(boardData);
        }

        public BoardData DeserializeBoardData(string json) {
            return JsonUtility.FromJson<BoardData>(json);
        }

        // больше похоже на сохранение текста в файл, причём тут джсон?
        public void SaveToJson(string path, string json) {
            // нет проверки на ошибки
            using (StreamWriter streamWriter = new StreamWriter(path)) {
                streamWriter.Write(json);
            }
        }

        // а тут причём джсон? вроде читаем просто файл по указанному пути
        public string LoadFromJson(string path) {
            // нет проверки на ошибки
            using (StreamReader reader = new StreamReader(path)) {
                string json = reader.ReadToEnd();
                return json;
            }
        }
    }
}

