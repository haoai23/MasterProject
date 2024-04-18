using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class Flap_Score : MonoBehaviour
{
    public GameObject ScoreCube;
    public static bool IsObstacles;
    public static float Score;
    public Text ScoreText,GameOverScore;
  
    private void OnTriggerEnter(Collider other)
    {        
        IsObstacles = true;
    }
    private void OnTriggerExit(Collider other)
    {
        IsObstacles = false;
    }
    private void Update()
    {
        Debug.Log("¸I¨ì¤F" + IsObstacles);
        ScoreText.text = Score.ToString();
        GameOverScore.text = Score.ToString();

    }
}
