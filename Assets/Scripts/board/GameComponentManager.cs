using UnityEngine;
using mover;


namespace board {
    public class GameComponentManager : MonoBehaviour {
        [SerializeField]
        private Board board;
        [SerializeField]
        private Mover mover;

        private void Update() {
            if (board.gameState == GameState.Paused) {
                mover.enabled = false;
            } else if (board.gameState == GameState.InProcessing) {
                mover.enabled = true;
            }
        }
    }
}

