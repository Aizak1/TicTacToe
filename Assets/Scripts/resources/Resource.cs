using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chip;

namespace resources {
    public class Resource : MonoBehaviour {
        public Dictionary<Size, GameObject> redModels = new Dictionary<Size, GameObject>();
        public Dictionary<Size, GameObject> blueModels = new Dictionary<Size, GameObject>();
    }
}

