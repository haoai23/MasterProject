using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Flap_GameStartandEnd : MonoBehaviour
{
    public GameObject GameInfo, Obstacles, StartScreenCanvas, ResultScreen;

   
    public void WhenStartGame()
    {
        GameInfo.SetActive(true);
        Obstacles.SetActive(true);
        StartScreenCanvas.SetActive(false);
        ResultScreen.SetActive(false);
        Timer.Start_Timer = true;
        Flap_PlayerMove.isReady = true;
        Flap_PlayerMove.isGameOver = false;
    }
    public  void WhenGameOver()
    {
        GameInfo.SetActive(false);
        Obstacles.SetActive(false);
        StartScreenCanvas.SetActive(false);
        ResultScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Flap");
        StartScreenCanvas.SetActive(true);
        Flap_PlayerMove.isGameOver = false;
        Timer.Start_Timer = false;
        Timer.Timer_i = 90;
        Flap_Score.Score = 0;
    }
}
