using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using static Unity.Collections.AllocatorManager;

public class WhackAMole_PlayerMove : MonoBehaviour
{
    public GameObject Tracker1, Tracker2, RightHand, LeftHand;
    GameObject RightHandT, LeftHandT;
    public GameObject DeductBlood;
    public static bool AtOriginalPoint = false;
    //public GameObject[] Block;
    public static int i = 0;
    float XProportion = 0;
    float YProportion = 0;
    //public static float DestroyPrefabTime;
    public GameObject WhackAMolePrefab;
    public static bool WhackAMole_StartTimer = false;
    public GameObject GameOver_Panel, ReactionTime_Image, Time_Image, OrignalPoint, MainCamera;
       
    // Update is called once per frame 
    void Update()
    {
        DefineTracker(Tracker1, Tracker2);
        WhackAMolePlayerMove();
        //if (WhaxkAMole_GameController.IsGameStarted)
        //{
        RecordPlayerPosition(Tracker1, Tracker2);
        //}
    }
       
    void DefineTracker(GameObject Tracker1, GameObject Tracker2)
        {
        /*if (Tracker1.transform.position.x < Tracker2.transform.position.x)
        {
            RightHandT = Tracker1;
            LeftHandT = Tracker2;
        }
        else
        {
            RightHandT = Tracker2;
            LeftHandT = Tracker1;
        }*/
        RightHandT = GameObject.FindWithTag("RightHand");
        LeftHandT = GameObject.FindWithTag("LefttHand");
    }
    void WhackAMolePlayerMove()
    {
        RightHand.transform.position = new Vector3(-RightHandT.transform.position.x * XProportion, RightHandT.transform.position.y * YProportion, RightHandT.transform.position.z);
        LeftHand.transform.position = new Vector3(-LeftHandT.transform.position.x * XProportion, LeftHandT.transform.position.y * YProportion, LeftHandT.transform.position.z);
        
    }
    float PunchSpeed = 0, punchspeed1 = 0, punchspeed0 = 0;
    void OnCollisionEnter(Collision collision)        
    {
        
        if (collision.gameObject.tag == "WhackAMole_Mole")
        {
            Destroy(collision.gameObject);
            WhackAMole_Score.Score = WhackAMole_Score.Score + 10;
            AtOriginalPoint = false;
            if (AtOriginalPoint == false)//紀錄回來時間
            {
                punchspeed0 = Time.time;
            }
            //DestroyPrefabTime = Time.time;
            //Debug.Log(" DestroyPrefabTime: " + DestroyPrefabTime);
        }
        else if (collision.gameObject.tag == "WhackAMole_Flower")
        {
            DeductBlood.SetActive(true);
            StartCoroutine(DeactivateDeductBlood(0.3f));
            WhackAMole_Score.Score = WhackAMole_Score.Score - 1;
        }
        else if (collision.gameObject.name == "OriginalPoint")
        {
            if (RightHand.transform.position.x - LeftHand.transform.position.x <6)
            {
                AtOriginalPoint = true;
                if (AtOriginalPoint == true)
                {
                    punchspeed1 = Time.time;
                }
                PunchSpeed = punchspeed0 - punchspeed1;
                //punchspeed1 = punchspeed0;
                Debug.Log("PunchSpeed" + PunchSpeed);
            }
        }
        IEnumerator DeactivateDeductBlood(float delay)
        {
            yield return new WaitForSeconds(delay);
            DeductBlood.SetActive(false);
        }
    }
    void RecordPlayerPosition(GameObject RightHand, GameObject LeftHand)
    {

        if (Input.GetKeyDown("s"))
        {
            float RecordXPosition = Mathf.Abs(RightHand.transform.position.x - LeftHand.transform.position.x);
            XProportion = 23 / RecordXPosition;
            
            Debug.Log(XProportion);
        }
        if (Input.GetKeyDown("a"))
        {
            float RecordYPosition = Mathf.Abs(RightHand.transform.position.y - LeftHand.transform.position.y);
            YProportion = 11 / RecordYPosition;
            
            Debug.Log(YProportion);
            //i = 0;
        }
        if (XProportion > 10 && YProportion > 5 && WhackAMole_Timer.WhackAMoleTimer_i > 0)
        {
            WhackAMole_StartTimer = true;
            WhackAMolePrefab.SetActive(true);
            GameOver_Panel.SetActive(false);
            ReactionTime_Image.SetActive(true) ;
            Time_Image.SetActive(true) ;
            OrignalPoint.SetActive(true);
            MainCamera.SetActive(true);

        }
    }

    
}
