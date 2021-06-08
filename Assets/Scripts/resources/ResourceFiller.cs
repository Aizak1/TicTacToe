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
                var chip = item.GetComponent<ChipComponent>();
                if(chip == null) {
                    Debug.LogError("Wrong model");
                    continue;
                }

                var size = chip.chipData.size;

                if (resource.redChips.ContainsKey(size)) {
                    Debug.LogError($"This model is already exists in resources {size}");
                    continue;
                }

                resource.redChips.Add(size, item);
            }

            foreach (var item in blueChipModels) {
                var chip = item.GetComponent<ChipComponent>();

                if (chip == null) {
                    Debug.LogError("Wrong model");
                    continue;
                }

                var size = chip.chipData.size;
                if (resource.blueChips.ContainsKey(size)) {
                    Debug.LogError($"This model is already exists in resources {size}");
                    continue;
                }

                resource.blueChips.Add(size, item);
            }
        }
    }
}
