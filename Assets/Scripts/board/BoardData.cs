using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chip;

namespace board {
    [System.Serializable]
    public struct BoardData {
        public ChipData[] chipDatas;
        public bool isBlueTurn;
    }
}

