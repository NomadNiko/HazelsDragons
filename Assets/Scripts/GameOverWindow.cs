using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    private void Awake() {
        retryButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(LoadMenu);
        
    }
    private void RestartGame() {
        Loader.Load(Loader.Scene.GameScene);
    }
    private void LoadMenu() {
        Loader.Load(Loader.Scene.MainMenu);
    }

    private void Start() {
        Dragon.GetInstance().OnDied += Dragon_OnDied;
        //Debug.Log("Subscribed to Event");
        Hide();
    }

    private void Dragon_OnDied(object sender, System.EventArgs e) {
        //Debug.Log("Dragon_OnDied event triggered");
        Show();
        scoreText.text = Level.GetInstance().GetCliffsPassed().ToString();
        
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
