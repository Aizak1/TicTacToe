using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using combination;
using chip;

namespace resources {
    public class Resource : MonoBehaviour {
        // очень умно забивать руками условия победы, особенно учитывая их зависимость от размера доски
        public List<WinCombination> winCombinations = new List<WinCombination>();

        // ещё раз храним фишки
        public Dictionary<Size, GameObject> redChips = new Dictionary<Size, GameObject>();
        public Dictionary<Size, GameObject> blueChips = new Dictionary<Size, GameObject>();
    }
}

