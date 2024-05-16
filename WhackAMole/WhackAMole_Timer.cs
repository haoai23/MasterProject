using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WhackAMole_Timer : MonoBehaviour
{
    public  static int WhackAMoleTimer_i = 20;
    public Text WhackAMoleTimer_Text;
    public GameObject MainCamera, ReactionTime_Image,Time_Image, GameOver_Panel, WhackAMolePrefabs, Score, OriginalPoint;
  
    void Start()
    {
        InvokeRepeating("CountDown90s", 0f, 1f);
        Debug.Log("WhackAMoleTimer_i: " + WhackAMoleTimer_i);
    }
    void CountDown90s()
    {
        if (WhackAMole_PlayerMove.WhackAMole_StartTimer && WhackAMoleTimer_i > 0)
        {
            WhackAMoleTimer_i--;
            WhackAMoleTimer_Text.text = WhackAMoleTimer_i.ToString();
        }
        else if (WhackAMoleTimer_i == 0)
        {
            //WhackAMoleTimer_i = 0;
            WhackAMole_PlayerMove.WhackAMole_StartTimer = false;
            MainCamera.SetActive(false);
            ReactionTime_Image.SetActive(false);
            Time_Image.SetActive(false);
            GameOver_Panel.SetActive(true);
            WhackAMolePrefabs.SetActive(false);
            Score.SetActive(false);
            OriginalPoint.SetActive(false);
        }
      
    }
}
