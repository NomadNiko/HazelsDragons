using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreWindow : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;

    private void Awake() {
        scoreText.text = "0";
    }

    public void Start() {
        highscoreText.text = Score.GetHighscore().ToString();
    }

    private void Update() {
        scoreText.text = Level.GetInstance().GetCliffsPassed().ToString();
    }
}
