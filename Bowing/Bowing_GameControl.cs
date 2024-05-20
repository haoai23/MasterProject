using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bowing_GameControl : MonoBehaviour
{
    public GameObject Spline, StartPanel, GameOverPanel, Timer_Panel, Reiming_Image;
    public static bool isStart = false;//紀錄按鈕的狀態，要記錄感測器的數據
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(Bowing_PlayerMove.isGameOver)
        {
            WhenGameOver();
        }
    }
    public void WhenGameStartOnclick()
    {
        isStart = true;
        Spline.SetActive(true);
        StartPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        Timer_Panel.SetActive(true);
        Reiming_Image.SetActive(true);
        Bowing_Timer.BowingTimer = 0;
        Bowing_PlayerMove.isGameOver = false;
    }
    public void WhenGameOver()
    {
        AudioManager.PlayWhichBGM("GameOver");
        isStart = false;
        StartPanel.SetActive(false);
        GameOverPanel.SetActive(true);
        Timer_Panel.SetActive(false);
        Reiming_Image.SetActive(false); 
        Timer_Panel.SetActive(false);
    }
    public void RestartGame()
    {
        isStart = false;
        Spline.SetActive(false);
        StartPanel.SetActive(true);
        GameOverPanel.SetActive(false);
        Timer_Panel.SetActive(false);
        Reiming_Image.SetActive(false);
        Bowing_Timer.BowingTimer = 0;
        Bowing_PlayerMove.isGameOver = false;
    }
    public void GameList()
    {
        SceneManager.LoadScene("UserInterface");
    }
    public void ApplicationQuit()
    {
        Application.Quit();

    }
}
