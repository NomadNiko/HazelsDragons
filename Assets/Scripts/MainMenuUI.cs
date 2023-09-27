using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] TextMeshProUGUI personalHighscoreText;
    [SerializeField] TextMeshProUGUI globalHighscoreText;


    private void Awake() {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(Application.Quit);
        
    }
    private void Start() {
        SoundManager.PlaySound(SoundManager.Sound.IntroMusic, .13f);
        personalHighscoreText.text = Score.GetHighscore().ToString();
    }

    private void StartGame() {
        Loader.Load(Loader.Scene.GameScene);
    }
}
