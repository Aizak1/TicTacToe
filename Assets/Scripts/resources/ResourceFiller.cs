using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chip;

namespace resources {
    public class ResourceFiller : MonoBehaviour {
        [SerializeField]
        private Resource resource;
        [SerializeField]
        private GameObject[] redChipModels;
        [SerializeField]
        private GameObject[] blueChipModels;

        private void Awake() {

            foreach (var item in redChipModels) {
                var chip = item.GetComponent<ChipComponent>();
                if(chip == null) {
                    Debug.LogError("Wrong model");
                    continue;
                }

                var size = chip.chipData.size;

                if (resource.redModels.ContainsKey(size)) {
                    Debug.LogError($"This model is already exists in resources {size}");
                    continue;
                }

                resource.redModels.Add(size, item);
            }

            foreach (var item in blueChipModels) {
                var chip = item.GetComponent<ChipComponent>();

                if (chip == null) {
                    Debug.LogError("Wrong model");
                    continue;
                }

                var size = chip.chipData.size;
                if (resource.blueModels.ContainsKey(size)) {
                    Debug.LogError($"This model is already exists in resources {size}");
                    continue;
                }

                resource.blueModels.Add(size, item);
            }
        }
    }
}
