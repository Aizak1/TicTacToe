using UnityEngine;
using System.IO;
using vjp;
//GameDataJsonConverter
namespace board {
    public class BoardParser : MonoBehaviour {

        public string SerializeBoardData(BoardData boardData) {

            if(boardData.chipDatas == null) {
                Debug.LogError("Wrong data to serialize");
                return null;
            }

            try {
                return JsonUtility.ToJson(boardData);
            } catch (System.Exception ex) {
                Debug.LogError($"Serialization Error -  {ex.Message}");
                return null;
            }

        }

        public Option<BoardData> DeserializeBoardData(string json) {

            if (string.IsNullOrWhiteSpace(json)) {
                Debug.LogError("No data to deserialize");
                return Option<BoardData>.None();
            }

            try {

                var boardData = JsonUtility.FromJson<BoardData>(json);
                return Option<BoardData>.Some(boardData);

            } catch (System.Exception ex) {

                Debug.LogError($"Deserialization Error -  {ex.Message}");
                return Option<BoardData>.None();

            }
        }
    }
}

