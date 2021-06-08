using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chip;

public class BoardParser : MonoBehaviour {
    private const string NEW_GAME_PATH = "New.json";
    private const string LOAD_GAME_PATH = "Load.json";

    public string SerializeChips(ChipData[] chipDatas) {
        return JsonUtility.ToJson(chipDatas);
    }

    public ChipData[] DeserializeChips(string json) {
        return JsonUtility.FromJson<ChipData[]>(json);
    }

    public void SaveToJson(string path) {

    }

    public void LoadFromJson(string path) {

    }

    public void LoadGame() {

    }

    public void SaveGame() {

    }

    public void StartNewGame() {

    }
}

