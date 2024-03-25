using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Climing_GameControl : MonoBehaviour
{
    public GameObject StepTimer, GameOverPanel, PlayerTips, StartPanel, Coin_Image;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            Climing_GameReload();
        }
        if(Climing_PlayerMove.isGameOver) { Climing_GameOver(); }
    }

    public void Climing_WhenGameStart()
    {
        StartPanel.SetActive(false);
        StepTimer.SetActive(true);
        GameOverPanel.SetActive(false); 
        PlayerTips.SetActive(true);
        Coin_Image.SetActive(true);
    }
    public void Climing_Quit()
    {
        Application.Quit();
    }
    public void Climing_GameReload()
    {
        StartPanel.SetActive(true);
        StepTimer.SetActive(false);
        GameOverPanel.SetActive(false);
        PlayerTips.SetActive(true);
        Coin_Image.SetActive(true);
        Climing_Timer.ClimingTimer = 90;
        Climing_PlayerMove.Climing_StartTimer = false;
        Climing_PlayerMove.isReady = false;
    }
    public void Climing_GameOver()
    {
        StartPanel.SetActive(false);
        StepTimer.SetActive(false);
        GameOverPanel.SetActive(true);
        PlayerTips.SetActive(false);
        Coin_Image.SetActive(false);
        Climing_Timer.ClimingTimer = 90;
        Climing_PlayerMove.Climing_StartTimer = false;
        Climing_PlayerMove.isReady = false;
    }
    public void GameList()
    {
        SceneManager.LoadScene("UserInterface");
    }
    
}
