using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chip;

namespace game {
    [System.Serializable]
    public struct GameData {
        public ChipData[] chipDatas;
        public bool isBlueTurn;
    }
}

