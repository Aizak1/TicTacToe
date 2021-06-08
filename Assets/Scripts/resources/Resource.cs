using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using combination;
using chip;

namespace resources {
    public class Resource : MonoBehaviour {
        public List<WinCombination> winCombinations = new List<WinCombination>();

        public Dictionary<Size, GameObject> redChips = new Dictionary<Size, GameObject>();
        public Dictionary<Size, GameObject> blueChips = new Dictionary<Size, GameObject>();
    }
}

