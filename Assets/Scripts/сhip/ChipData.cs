using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace chip {

    [System.Serializable]
    public struct ChipData {
        public float x;
        public float z;
        public Size size;
        public bool isBlue;
        public bool isUsed;
    }

    public enum Size {
        Small,
        Medium,
        Big
    }

}
