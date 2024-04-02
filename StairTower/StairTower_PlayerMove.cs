using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
//using UnityEngine.Windows;



public class StairTower_PlayerMove : MonoBehaviour
{
    GameObject CurrentFloor;
    public GameObject StairTower_Player;
    bool IsJumping = false;
    public Text Score;
    public GameObject Tracker1, Tracker2, Tracker3;
    GameObject Chest, RightLeg, LeftLeg;
    bool isReady = false;
    public static int StairTower_Score;
    public Text StairTowerScore_Text;
    public static bool StairTowerStartTimer = false;

    public GameObject _Start_Panel,_GameOver_Panel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            StairTower_Player.GetComponent<Rigidbody2D>().velocity = Vector2.up * 2f;
            IsJumping = true;
        }
        if (isReady == false)
        { 
            StairTower_DefinedViveTracker(Tracker1, Tracker2, Tracker3);
        }
        else if (isReady == true)
        {
            RecordPlayerPosture();
            if (RecordSuceesful)
            {
                PlayerMove();
                AnalyzePlayerData();
                StairTowerStartTimer = true;
            }
            if(isGameOver)
            {
                
                _GameOver_Panel.SetActive(true);
            }
        }
    }
    private void PlayerMove() 
    {
        //StairTower_Player.transform.Translate(new Vector2(2.3f * Time.deltaTime * direction , 0));
        if (RightLeg.transform.position.y > LeftLeg.transform.position.y + 0.1 && PersonalStandardHeight != 0)//右腳比左腳高Xf
        {
            StairTower_Player.transform.Translate(new Vector2(0.03f, 0));
        }

        else if (RightLeg.transform.position.y + 0.1 < LeftLeg.transform.position.y && PersonalStandardHeight != 0)//左腳比左腳高Xf
        {
            StairTower_Player.transform.Translate(new Vector2(-0.03f, 0));
        }
        else if (Chest.transform.position.y - ChestNoTiptoes > PersonalStandardHeight * 0.8)//墊腳的高度判斷是否有墊腳尖，高於最大值的九成
        {
            IsJumping = true;
            StairTower_Player.GetComponent<Rigidbody2D>().velocity = Vector2.up *2f;
        }
        else
        {
            IsJumping = false;
        }
        Debug.Log("Isjump:"+IsJumping);
    }
    float RightLegTiptoes, LeftLegTiptoes, ChestTiptoes;
    float RightLegNoTiptoes, LeftLegNoTiptoes, ChestNoTiptoes;

    int i = 0;
    float PersonalStandardHeight = 0;
    bool RecordSuceesful = false;
    void RecordPlayerPosture()
    {
 
        if (Input.GetKeyDown("r"))
        {
            i += 1;
            if (i == 1 && RightLeg != null && LeftLeg != null && Chest != null)//沒有點腳尖
            {
                RightLegNoTiptoes = RightLeg.transform.position.y;
                LeftLegNoTiptoes = LeftLeg.transform.position.y;
                ChestNoTiptoes = Chest.transform.position.y;
                Debug.Log(i + ": OK");                
            }
            if (i == 2 && RightLeg != null && LeftLeg != null && Chest != null)//有點腳尖
            {
                RightLegTiptoes = RightLeg.transform.position.y;
                LeftLegTiptoes = LeftLeg.transform.position.y;
                ChestTiptoes = Chest.transform.position.y;
                RecordSuceesful = true;
                _Start_Panel.SetActive(false);
                Debug.Log(i + ": OK");
            }   
        }
        if (RecordSuceesful)
        {
            PersonalStandardHeight = ((RightLegTiptoes - RightLegNoTiptoes) + (LeftLegTiptoes - LeftLegNoTiptoes) + (ChestTiptoes - ChestNoTiptoes)) / 3;///平均高度
            
            Debug.Log("個人標準高度: "+PersonalStandardHeight);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "StairTower_NormalFloor")
        {                 
            CurrentFloor = collision.gameObject;
            if (IsJumping && collision.contacts[0].normal == new Vector2(0f, -1f))
            {                   
                CurrentFloor.GetComponent<BoxCollider2D>().enabled = false;
                Debug.Log("目前階梯: "+CurrentFloor.GetComponent<BoxCollider2D>().enabled);

                /*if ( CurrentFloor.GetComponent<BoxCollider2D>().enabled == false)
                 {
                     CurrentFloor.GetComponent<BoxCollider2D>().enabled = true;
                     IsJumping = false;
                 }*/
                StartCoroutine(EnableColliderAfterDelay(CurrentFloor.GetComponent<BoxCollider2D>(), 1f));

            }
            else if (collision.contacts[0].normal == new Vector2(0f, -1f))
            {
                //ModifyHp(1);
            }
        }
        else if (collision.gameObject.tag == "StairTower_NailsFloor")
        {


            if (IsJumping && collision.contacts[0].normal == new Vector2(-1f, 0f))
            {
                CurrentFloor.GetComponent<BoxCollider2D>().enabled = true;
                IsJumping = false; // 玩家落地，重置跳躍狀態
            }
            else if (collision.contacts[0].normal == new Vector2(-1f, 0f))
            {
                //ModifyHp(-1);
            }

        }
        /*else if (collision.gameObject.name == "Wall_Left" || collision.gameObject.name == "Wall_Right")
        {
            direction *= -1;
        }*/
        else if (collision.gameObject.tag == "StairTower_Diamond")
        {
            StairTower_Score += 1;
            StairTowerScore_Text.text = StairTower_Score.ToString();
            Destroy(collision.gameObject);
        }
    }
    public static bool isGameOver = false;//判斷是否遊戲結束了以方便分析
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "StairTower_Stairline")
        {
            StairTower_PrefabMove.MoveSpeed = 0.3f + Time.time/100;

        }
        else if (collision.gameObject.name == "Deathline")
        {
            isGameOver = true;
            StairTower_PrefabMove.MoveSpeed = 0f;
            Debug.Log("遊戲結束");
        }
        Debug.Log("開始下降");
        
    }

    private IEnumerator EnableColliderAfterDelay(BoxCollider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }

    void StairTower_DefinedViveTracker(GameObject Tracker, GameObject Tracker1, GameObject Tracker2)
    {
        RightLeg = GameObject.FindWithTag("RightLeg");
        LeftLeg = GameObject.FindWithTag("LeftHLeg");
        Chest = GameObject.FindWithTag("Chest");
        /*if (Tracker.transform.position.y > Tracker1.transform.position.y &&
            Tracker.transform.position.y > Tracker2.transform.position.y)
        {
            Chest = Tracker;
            Debug.Log("1");
            if (Tracker2.transform.position.x > Tracker1.transform.position.x &&
                Tracker.transform.position.x > Tracker1.transform.position.x)
            {
                RightLeg = Tracker2;
                LeftLeg = Tracker1;
            }
            else
            {
                RightLeg = Tracker1;
                LeftLeg = Tracker2;
            }
            isReady = true;
        }
        if (Tracker1.transform.position.y > Tracker.transform.position.y &&
            Tracker1.transform.position.y > Tracker2.transform.position.y)
        {
            Chest = Tracker1;
            Debug.Log("2");
            if (Tracker2.transform.position.x > Tracker.transform.position.x &&
                Tracker1.transform.position.x > Tracker.transform.position.x)
            {
                RightLeg = Tracker2;
                LeftLeg = Tracker;
            }
            else
            {
                RightLeg = Tracker;
                LeftLeg = Tracker2;
            }
            isReady = true;
        }
        if (Tracker2.transform.position.y > Tracker.transform.position.y &&
            Tracker2.transform.position.y > Tracker1.transform.position.y)
        {
            Chest = Tracker2;
            Debug.Log("3");
            if (Tracker.transform.position.x > Tracker1.transform.position.x &&
                Tracker2.transform.position.x > Tracker1.transform.position.x)
            {
                RightLeg = Tracker;
                LeftLeg = Tracker1;
            }
            else
            {
                RightLeg = Tracker1;
                LeftLeg = Tracker;
            }
            isReady = true;
        }*/


    }
    List<float> AverageRightLeg = new List<float>();
    List<float> AverageLeftLeg = new List<float>();
    List<float> AbdominalXrotation = new List<float>();
    public Text _LeftLegCalfStability, _RightLegCalfStability, _ChestStability, _Score,_Time;
    void AnalyzePlayerData()
    {
        float ChestRightDifference = Chest.transform.position.x - RightLeg.transform.position.x;
        float ChestLefttDifference = Chest.transform.position.x - LeftLeg.transform.position.x;
        if(!isGameOver)
        { 
            AbdominalXrotation.Add(Chest.transform.position.x);
            _Score.text = StairTower_Score.ToString();
        }
       
        float averageRightLeg, averageLeftLeg;
        if (isReady && RecordSuceesful && LeftLeg.transform.position.y > RightLeg.transform.position.y + 0.1f  && !isGameOver)
        {
            AverageRightLeg.Add(ChestRightDifference);     
        }
        else if (isReady && RecordSuceesful && RightLeg.transform.position.y > LeftLeg.transform.position.y + 0.1f && !isGameOver)
        {
            AverageLeftLeg.Add(ChestLefttDifference);    
        }
        if (isGameOver)
        {
            float RightVariance, LeftVariance, RightStandardDeviation, LeftStandardDeviation;
            averageRightLeg = AverageRightLeg.Average();
            averageLeftLeg = AverageLeftLeg.Average();

            float AbdominalXrotationMean = AbdominalXrotation.Average();
            // 計算每個數據與平均值的差值的平方相加
            float sumOfSquares = AbdominalXrotation.Sum(x => Mathf.Pow(x - AbdominalXrotationMean, 2));
            float variance = sumOfSquares / AbdominalXrotation.Count;
            float StandardValue = Mathf.Sqrt(variance);
            _ChestStability.text = StandardValue.ToString();
            Debug.Log("Chest標準差" + StandardValue);

            float RightsumOfSquares = AverageRightLeg.Sum(x => Mathf.Pow(x - averageRightLeg, 2));
            RightVariance = RightsumOfSquares / AverageRightLeg.Count;//右腳的變異數
            RightStandardDeviation = (float)Mathf.Sqrt(RightVariance);
            _RightLegCalfStability.text= RightStandardDeviation.ToString();
            Debug.Log("RightVariance: " + RightVariance);
            Debug.Log("RightStandardDeviation: " + RightStandardDeviation);

            float LeftsumOfSquares = AverageLeftLeg.Sum(x => Mathf.Pow(x - averageLeftLeg, 2));
            LeftVariance = LeftsumOfSquares / AverageLeftLeg.Count;//左腳的變異數
            LeftStandardDeviation = (float)Mathf.Sqrt(LeftVariance);
            _LeftLegCalfStability.text = LeftStandardDeviation.ToString();
            Debug.Log("LeftVariance: " + LeftVariance);
            Debug.Log("LeftStandardDeviation: " + LeftStandardDeviation);            
        }
    }
}
