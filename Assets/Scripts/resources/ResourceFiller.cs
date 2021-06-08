using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using combination;

namespace resources {
    public class ResourceFiller : MonoBehaviour {
        [SerializeField]
        private Resource resource;
        [SerializeField]
        private List<WinCombination> winCombinations;

        private void Awake() {
            resource.winCombinations = winCombinations;
        }
    }
}
