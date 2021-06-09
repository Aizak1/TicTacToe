using UnityEngine;
using board;
using UnityEngine.UI;

namespace ui {
    public class UiSwitcher : MonoBehaviour {

        [SerializeField]
        private Board board;

        [SerializeField]
        private Canvas mainMenu;
        [SerializeField]
        private Canvas gameMenu;
        [SerializeField]
        private Canvas endMenu;

        [SerializeField]
        private Text endMenuText;

        [SerializeField]
        private Color redWinColor;
        [SerializeField]
        private Color blueWinColor;

        private const string BLUE_WIN_TEXT = "Blue wins";
        private const string RED_WIN_TEXT = "Red wins";
        private const string DRAW_TEXT = "Draw";

        private void Update() {
            if (mainMenu.enabled) {
                return;
            }

            if (board.isGameProcessing) {

                gameMenu.enabled = true;
                endMenu.enabled = false;

            } else {

                endMenu.enabled = true;
                gameMenu.enabled = false;
                if(board.gameResult == GameResult.BlueWins) {
                    endMenuText.color = blueWinColor;
                    endMenuText.text = BLUE_WIN_TEXT;

                } else if (board.gameResult == GameResult.RedWins) {
                    endMenuText.color = redWinColor;
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

