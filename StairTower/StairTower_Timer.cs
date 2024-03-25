using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StairTower_Timer : MonoBehaviour
{
    public static int StairTower_i = 0;
    public Text StairTowerTimer_Text, WhenGOShowStairTowerTimer_Text;
    public GameObject _StairTowerTimer;

    void Start()
    {
        InvokeRepeating("CountDown90s", 1f, 1f);
        Debug.Log("WhackAMoleTimer_i: " + StairTower_i);
    }
    void CountDown90s()
    {
        if(!StairTower_PlayerMove.isGameOver)
        {
            _StairTowerTimer.SetActive(false);
            
        }
        if (StairTower_PlayerMove.StairTowerStartTimer)
        {
            StairTower_i++;
            StairTowerTimer_Text.text = StairTower_i.ToString();
        }
    } 
}
