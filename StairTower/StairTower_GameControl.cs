using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StairTower_GameControl : MonoBehaviour
{
    public GameObject _StartPanel,_TimerPanel,_GameOverPanel;
    public void RestartGame()
    {
        SceneManager.LoadScene("UpTheStair_StairTower");
        StairTower_PlayerMove.StairTowerStartTimer = false;
        StairTower_PlayerMove.isGameOver = false;
        StairTower_PlayerMove.StairTower_Score = 0;
        StairTower_PlayerMove.isStart=false;
        StairTower_PrefabMove.MoveSpeed = 0;
        StairTower_Timer.StairTower_i = 0;


        _StartPanel.SetActive(true);
        _TimerPanel.SetActive(false);
        _GameOverPanel.SetActive(false);
    } 
}
