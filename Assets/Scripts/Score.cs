using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    public static void Start() {
        Dragon.GetInstance().OnDied += Dragon_OnDied;
    }

    private static void Dragon_OnDied(object sender, System.EventArgs e) {
        TrySetNewHighscore(Level.GetInstance().GetCliffsPassed());
    }

    public static int GetHighscore() {
        return PlayerPrefs.GetInt("highscore");
    }

    public static bool TrySetNewHighscore(int score) {
        int currentHighscore = GetHighscore();
        if (score > currentHighscore) {
            // New Highscore
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        } else {
            return false;
        }
    }
    public static bool TrySetNewGlobalHighscore(int score) {
        int currentLowestHighscore = GetLowestGlobalHighscore();
        if (score > currentLowestHighscore) {
            // New Highscore
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        } else {
            return false;
        }
    }

    public static void ResetHighScore() {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.Save();
    }
    private static int GetLowestGlobalHighscore() {
        string leaderboardKey = "level_1_highscore";

        LootLockerSDKManager.GetScoreList(leaderboardKey, 1, 9, (response) =>
        {
            if (response.statusCode == 200) {
                Debug.Log("Successful");
            } else {
                Debug.Log("failed: " + response.errorData.message);
            }
        });
        return 0;
    }
}
