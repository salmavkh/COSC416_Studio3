using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }
}
