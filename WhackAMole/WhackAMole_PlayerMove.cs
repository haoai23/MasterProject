using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using static Unity.Collections.AllocatorManager;
using System.Linq;

public class WhackAMole_PlayerMove : MonoBehaviour
{
    public GameObject Tracker1, Tracker2, RightHand, LeftHand;
    GameObject RightHandT, LeftHandT;
    public GameObject DeductBlood;
    public static bool AtOriginalPoint = false;
    bool isReady;
    //public GameObject[] Block;
    public static int i = 0;
    float XProportion = 0;
    float YProportion = 0;
    //public static float DestroyPrefabTime;
    public GameObject WhackAMolePrefab;
    public static bool WhackAMole_StartTimer = false;
    public GameObject GameOver_Panel, ReactionTime_Image, Time_Image, OrignalPoint, MainCamera;

    List<float> RightHandPX = new List<float>();
    List<float> RightHandPY = new List<float>();
    List<float> RightHandPZ = new List<float>();
    List<float> RightHandRX = new List<float>();
    List<float> RightHandRY = new List<float>();
    List<float> RightHandRZ = new List<float>();

    List<float> LeftHandPX = new List<float>();
    List<float> LeftHandPY = new List<float>();
    List<float> LeftHandPZ = new List<float>();
    List<float> LeftHandRX = new List<float>();
    List<float> LeftHandRY = new List<float>();
    List<float> LeftHandRZ = new List<float>();

    // Update is called once per frame 
    void Update()
    {
        DefineTracker(Tracker1, Tracker2);
        WhackAMolePlayerMove();
        if (isReady)
        {
            RecordPlayerPosition(Tracker1, Tracker2);
        }
        if (isReady & HAttitudeCorrection & VAttitudeCorrection)//紀錄數據
        {
            RightHandPX.Add(RightHandT.transform.position.x);
            RightHandPY.Add(RightHandT.transform.position.y);
            RightHandPZ.Add(RightHandT.transform.position.z);
            RightHandRX.Add(RightHandT.transform.eulerAngles.x);
            RightHandRY.Add(RightHandT.transform.eulerAngles.y);
            RightHandRZ.Add(RightHandT.transform.eulerAngles.z);

            LeftHandPX.Add(LeftHandT.transform.position.x);
            LeftHandPY.Add(LeftHandT.transform.position.y);
            LeftHandPZ.Add(LeftHandT.transform.position.z);
            LeftHandRX.Add(LeftHandT.transform.eulerAngles.x);
            LeftHandRY.Add(LeftHandT.transform.eulerAngles.y);
            LeftHandRZ.Add(LeftHandT.transform.eulerAngles.z);


        }
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
        LeftHandT = GameObject.FindWithTag("LeftHand");
        if (RightHandT & LeftHandT != null)
        {
            isReady = true;
            Debug.Log("isReady: " + isReady);
        }
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
            //WhackAMole_Score.Score = WhackAMole_Score.Score - 1;
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
    bool HAttitudeCorrection = false;//水平校正
    bool VAttitudeCorrection = false;//垂直校正

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
    public void WhackAMoleSaveCSV()
    {
        string fileName = "StairTower.csv";
        string timePath = Path.Combine(PlayerPrefs.GetString("timePath"), fileName);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("RightHandPX,RightHandPY,RightHandPZ,RightHandRX,RightHandRY,RightHandRZ,LeftHandPX,LeftHandPY,LeftHandPZ,LeftHandRX,LeftHandRY,LeftHandRZ,");

        // 確定最大長度
        int maxLength = new int[] { RightHandPX.Count, RightHandPY.Count, RightHandPZ.Count, RightHandRX.Count, RightHandRY.Count, RightHandRZ.Count, 
            LeftHandPX.Count, LeftHandPY.Count, LeftHandPZ.Count, LeftHandRX.Count, LeftHandRY.Count, LeftHandRZ.Count }.Max();


        // 根據最大長度遍歷
        for (int i = 0; i < maxLength; i++)
        {
            string line = $"{GetValueOrDefault(RightHandPX, i)},{GetValueOrDefault(RightHandPY, i)},{GetValueOrDefault(RightHandPZ, i)},{GetValueOrDefault(RightHandRX, i)},{GetValueOrDefault(RightHandRY, i)},{GetValueOrDefault(RightHandRZ, i)}," +
                          $"{GetValueOrDefault(LeftHandPX, i)},{GetValueOrDefault(LeftHandPY, i)},{GetValueOrDefault(LeftHandPZ, i)},{GetValueOrDefault(LeftHandRX, i)},{GetValueOrDefault(LeftHandRY, i)},{GetValueOrDefault(LeftHandRZ, i)}";
                          

            // 在每一行的末尾添加統計數據
            if (i == 0) // 假設統計數據只需添加一次
            {
                //line += $",{ChestXRA},{ChestXRSD},{RightLegCalfStability},{RightLegCalfStabilitySD},{LeftLegCalfStability},{LeftLegCalfStabilityYSD},{time},{stepcount},{score}";
            }
            sb.AppendLine(line);
        }

        // 使用 FileStream 和 StreamWriter 寫入文件
        using (FileStream fs = new FileStream(timePath, FileMode.Create, FileAccess.Write, FileShare.None))
        using (StreamWriter sw = new StreamWriter(fs))
        {
            sw.Write(sb.ToString());
        }
    }

    // 輔助方法來處理可能的索引越界問題
    private string GetValueOrDefault(List<float> list, int index)
    {
        if (index < list.Count)
        {
            return list[index].ToString();
        }
        return "N/A"; // 或者您可以選擇返回空字符串 ""
    }

}
