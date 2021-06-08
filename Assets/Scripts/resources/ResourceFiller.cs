using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using combination;
using chip;

namespace resources {
    public class ResourceFiller : MonoBehaviour {
        [SerializeField]
        private Resource resource;
        [SerializeField]
        private List<WinCombination> winCombinations;
        [SerializeField]
        private GameObject[] redChipModels;
        [SerializeField]
        private GameObject[] blueChipModels;

        private void Awake() {

            resource.winCombinations = winCombinations;

            foreach (var item in redChipModels) {
                var size = item.GetComponent<ChipComponent>().chipData.size;
                resource.redChips.Add(size, item);
            }

            foreach (var item in blueChipModels) {
                var size = item.GetComponent<ChipComponent>().chipData.size;
                resource.blueChips.Add(size, item);
            }
        }
    }
}
