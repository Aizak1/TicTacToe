using UnityEngine;
using mover;


namespace game {
    public class GameComponentManager : MonoBehaviour {
        [SerializeField]
        private GameManager manager;
        [SerializeField]
        private Mover mover;

        private void Update() {
            if (manager.gameState == GameState.Paused) {
                mover.enabled = false;
            } else if (manager.gameState == GameState.InProcessing) {
                mover.enabled = true;
            }
        }
    }
}

