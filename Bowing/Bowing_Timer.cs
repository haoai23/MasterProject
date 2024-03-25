using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bowing_Timer : MonoBehaviour
{
    public static float BowingTimer = 0;
    public Text BowingTimer_Text, GameOverShowTimer;

    // Update is called once per frame.
    private void Start()
    {
        InvokeRepeating("CountDowns", 0f, 1f);
    }

    void CountDowns()
    {
        if (Bowing_GameControl.isStart && !Bowing_PlayerMove.isGameOver)
        {
            BowingTimer += 1;
            BowingTimer_Text.text = BowingTimer.ToString();
            GameOverShowTimer.text = BowingTimer.ToString();

        }
        else if (Bowing_PlayerMove.isGameOver)//需要改成如果遊戲結束，設定回初始狀態
        {
        }

    }
}
