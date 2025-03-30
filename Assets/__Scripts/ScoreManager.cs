using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager S;
    
    [Header("Dynamic")]
    [SerializeField]
    private int _score = 0;
    
    [Header("Inscribed")]
    public TextMeshProUGUI scoreText;
    
    void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("ScoreManager.Awake() - Attempted to assign second ScoreManager.S!");
        }
        
        // Initialize score display
        UpdateScoreText();
    }
    
    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            UpdateScoreText();
        }
    }
    
    public void AddScore(int points)
    {
        score += points;
    }
    
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + _score.ToString();
        }
    }
} 