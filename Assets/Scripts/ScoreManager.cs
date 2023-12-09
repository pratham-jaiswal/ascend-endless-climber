using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {
    public static ScoreManager instance;

    public TMP_Text scoreText;

    int score = 0;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        scoreText.text = score.ToString();
    }

    public void UpdateScore(int newScore) {
        score = newScore;
        scoreText.text = score.ToString();
    }
}