using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("GAME OVER UI")]
    [SerializeField] private Canvas joystickCanvas = null;
    [SerializeField] private GameObject gameOverUIContainer = null;
    [SerializeField] private TMP_Text gameOverText = null;
    [SerializeField] private Button tryAgainButton = null;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        gameOverUIContainer.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    public void GameOver(bool playerWin) {
        gameOverUIContainer.SetActive(true);
        tryAgainButton.gameObject.SetActive(true);
        joystickCanvas.gameObject.SetActive(false);

        if (playerWin) {
            PlayerWin();
        }
        else {
            PlayerLose();
        }
    }

    private void PlayerWin() {
        gameOverText.text = "You Win! :)";
        gameOverText.color = Color.green;
    }

    private void PlayerLose() {
        gameOverText.text = "You Lose :(";
        gameOverText.color = Color.red;
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
