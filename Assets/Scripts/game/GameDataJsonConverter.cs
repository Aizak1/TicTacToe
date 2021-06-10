using UnityEngine;
using System.IO;
using vjp;
//GameDataJsonConverter
namespace game {
    public class GameDataJsonConverter : MonoBehaviour {

        public string SerializeGameData(GameData gameData) {

            if(gameData.chipDatas == null) {
                Debug.LogError("Wrong data to serialize");
                return null;
            }

            try {
                return JsonUtility.ToJson(gameData);
            } catch (System.Exception ex) {
                Debug.LogError($"Serialization Error -  {ex.Message}");
                return null;
            }

        }

        public Option<GameData> DeserializeGameData(string json) {

            if (string.IsNullOrWhiteSpace(json)) {
                Debug.LogError("No data to deserialize");
                return Option<GameData>.None();
            }

            try {

                var gameData = JsonUtility.FromJson<GameData>(json);
                return Option<GameData>.Some(gameData);

            } catch (System.Exception ex) {

                Debug.LogError($"Deserialization Error -  {ex.Message}");
                return Option<GameData>.None();

            }
        }
    }
}

