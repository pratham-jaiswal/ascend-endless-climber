using UnityEngine;
using TMPro;

public class DisplayResults : MonoBehaviour
{
    public TMP_Text currentScoreText;
    public TMP_Text highScoreText;

    private int highScore = 0;
    private int currentScore = 0;

    void Start()
    {
        currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        currentScoreText.text = "Score: " + currentScore;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }
}