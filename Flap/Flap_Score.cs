using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flap_Score : MonoBehaviour
{
    public GameObject ScoreCube;
    
    public static float Score;
    public Text ScoreText,GameOverScore;
  
    private void OnTriggerEnter(Collider other)
    {
        Score += 0.5f;
        ScoreText.text = Score.ToString();
        GameOverScore.text = Score.ToString();
    }
}
