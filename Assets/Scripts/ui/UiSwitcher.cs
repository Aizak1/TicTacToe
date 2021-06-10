using UnityEngine;
using game;
using UnityEngine.UI;

namespace ui {
    public class UiSwitcher : MonoBehaviour {

        [SerializeField]
        private GameManager manager;

        [SerializeField]
        private Canvas mainMenu;
        [SerializeField]
        private Canvas gameMenu;
        [SerializeField]
        private Canvas endMenu;

        [SerializeField]
        private Text endMenuText;

        [SerializeField]
        private Color redColor;
        [SerializeField]
        private Color blueColor;

        [SerializeField]
        private Light turnLight;
        [SerializeField]
        private float lightTransitionTime;

        private const string BLUE_WIN_TEXT = "Blue wins";
        private const string RED_WIN_TEXT = "Red wins";
        private const string DRAW_TEXT = "Draw";

        private void Update() {
            if (mainMenu.enabled) {
                return;
            }

            var time = lightTransitionTime * Time.deltaTime;
            if (manager.isBlueTurn) {
                turnLight.color = Color.Lerp(turnLight.color, blueColor, time);
            } else {
                turnLight.color = Color.Lerp(turnLight.color, redColor, time);
            }

            if (manager.gameState == GameState.InProcessing) {

                gameMenu.enabled = true;
                endMenu.enabled = false;

            } else {

                endMenu.enabled = true;
                gameMenu.enabled = false;
                if(manager.gameResult == GameResult.BlueWins) {
                    endMenuText.color = blueColor;
                    endMenuText.text = BLUE_WIN_TEXT;

                } else if (manager.gameResult == GameResult.RedWins) {
                    endMenuText.color = redColor;
                    endMenuText.text = RED_WIN_TEXT;
                } else {
                    endMenuText.color = Color.white;
                    endMenuText.text = DRAW_TEXT;
                }
            }
        }

        public void DisalbeMainMenu() {
            mainMenu.enabled = false;
        }

    }
}

