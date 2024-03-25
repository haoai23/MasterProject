using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Climing_Timer : MonoBehaviour
{
    public static float ClimingTimer = 90;
    public Text ClimingTimer_Text, GameOverShowTimer;
    public GameObject PlayerLegTip, Coin_Image, CountDown, GameOver_Panel, Start_Panel;
    // Update is called once per frame.
    private void Start()
    {
        InvokeRepeating("CountDowns", 0f, 1f);
    }

    void CountDowns()
    {
        if (Climing_PlayerMove.Climing_StartTimer)
        {
            ClimingTimer -= 1;
            ClimingTimer_Text.text = ClimingTimer.ToString();
            GameOverShowTimer.text = ClimingTimer.ToString();

        }
        else if (Climing_PlayerMove.isGameOver)//需要改成如果遊戲結束，設定回初始狀態
        {
            PlayerLegTip.SetActive(false);
            Coin_Image.SetActive(false);
            CountDown.SetActive(false);
            GameOver_Panel.SetActive(true);
            Start_Panel.SetActive(false);

            Climing_PlayerMove.isGameOver = true;
            Climing_PlayerMove.Climing_StartTimer = false;
            Climing_PlayerMove.isReady = false;
        }
        
    }



}
