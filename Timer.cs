using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public static int Timer_i = 90;
    public static bool Start_Timer = false;
    public Text TimerText;
    public GameObject StartButton;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CountDown", 0f, 1f);
    }
    private void Update()
    {       
        TimerText.text = Timer_i.ToString();       
    }
     private void CountDown()
     {
        
        if (Start_Timer)
        {
            Timer_i--;
            if (Timer_i < 0)
            {
                Start_Timer = false;
                Flap_GameStartandEnd gameController = StartButton.GetComponent<Flap_GameStartandEnd>();
                gameController.WhenGameOver();
            }

            
        }

     }
   
  
}
