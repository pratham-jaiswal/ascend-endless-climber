using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour {
    public static HighScoreManager instance;

    public TMP_Text scoreText;

    int score = 0;

    private void Awake() {
        instance = this;
    }
    
    public void UpdateScore(int newScore) {
        score = newScore;
        scoreText.text = "High Score:\n"+score;
    }
}