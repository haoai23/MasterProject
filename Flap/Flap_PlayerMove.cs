using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEditor;
using System.Text;

public class Flap_PlayerMove : MonoBehaviour
{
    public GameObject HipsCube;
    public static bool isReady, isGameOver;
    List<float> Stability = new List<float>();
    List<float> Strength = new List<float>();
    List<float> LastingValue = new List<float>();
    
    List<float> YValue4Range = new List<float>();
    List<float> YValue5Range = new List<float>();
    List<float> YValue6Range = new List<float>();
    List<float> YValue7Range = new List<float>();
    List<float> YValue8Range = new List<float>();

    List<float> ChestPX = new List<float>();
    List<float> ChestPY = new List<float>();
    List<float> ChestPZ = new List<float>();
    List<float> ChestRX = new List<float>();
    List<float> ChestRY = new List<float>();
    List<float> ChestRZ = new List<float>();

    List<float> FlapScore = new List<float>();
    bool Record = true;
    float RecordXRotation, RecordZRotation;
    List<float> YValue = new List<float>();//用來記錄過牆時的Y軸數據
    // Update is called once per frame
    void Update()
    {
        IfPlayerDontMove();
        ReadViveTracker();
        HipsCube = GameObject.FindWithTag("Chest");
        if (Record)
        {
            RecordXRotation = HipsCube.transform.eulerAngles.x;
            RecordZRotation = HipsCube.transform.eulerAngles.z;
            Record = false;
            Debug.Log("RecordXRotation: " + RecordXRotation);
            Debug.Log("RecordZRotation: " + RecordZRotation);
        }
        if (isReady & !isGameOver)
        {
            Strength.Add(HipsCube.transform.position.y);//強度
            LastingValue.Add(RecordXRotation - HipsCube.transform.eulerAngles.x);//持久度
            Stability.Add(RecordZRotation - HipsCube.transform.eulerAngles.z);//穩定度
            ChestPX.Add(HipsCube.transform.position.x);
            ChestPY.Add(HipsCube.transform.position.y);
            ChestPZ.Add(HipsCube.transform.position.z);
            ChestRX.Add(HipsCube.transform.eulerAngles.x);
            ChestRY.Add(HipsCube.transform.eulerAngles.y);
            ChestRZ.Add(HipsCube.transform.eulerAngles.z);
            FlapScore.Add(Flap_Score.Score);

            if (Flap_Score2.IsTouch)
            {
                YValue.Add(HipsCube.transform.position.y);
                switch (ObstaclesManerger.Range) 
                { 
                    case 4:
                        YValue4Range.Add(HipsCube.transform.position.y);
                        break;
                    case 5:
                        YValue5Range.Add(HipsCube.transform.position.y);
                        break;
                    case 6:
                        YValue6Range.Add(HipsCube.transform.position.y);
                        break;
                    case 7:
                        YValue7Range.Add(HipsCube.transform.position.y);
                        break;
                    case 8:
                        YValue8Range.Add(HipsCube.transform.position.y);
                        break;
                    default:
                        break;

                }
                    

            }
        }
        if (isGameOver)
        {
            Flap_DataAnalysis();
        }      
    }
    void IfPlayerDontMove()
    {
        if (this.transform.position.x > 12f)
        {
            this.transform.position = new Vector3(9f, 8f, 0f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("撞到了");


        if (other != null)
        {
            this.transform.position = new Vector3(other.gameObject.transform.position.x, HipsCube.transform.position.y * 15 - 4, 0);
        }
        else
        {
            this.transform.position = new Vector3(0, HipsCube.transform.position.y * 15 - 4, 0);
        }
    }
    void ReadViveTracker()
    {
        this.transform.position = new Vector3(9f, HipsCube.transform.position.y * 15 - 4, 0);

    }
    public Text StabilitySD, StrengthSD, StrengthSD2, LastingValueSD;
    void Flap_DataAnalysis()
    {
        float ZRotationValueAverage = Stability.Average();
        float ZRotationSumOfSquares = Stability.Sum(x => Mathf.Pow(x - ZRotationValueAverage, 2));
        float ZRotationvariance = ZRotationSumOfSquares / Stability.Count;
        float ZRotationStandardValue = Mathf.Sqrt(ZRotationvariance);
        StabilitySD.text = ZRotationStandardValue.ToString("F2");

        float XRotationValueAverage = LastingValue.Average();
        float XRotationSumOfSquares = LastingValue.Sum(x => Mathf.Pow(x - XRotationValueAverage, 2));
        float XRotationvariance = XRotationSumOfSquares / LastingValue.Count;
        float XRotationStandardValue = Mathf.Sqrt(XRotationvariance);
        LastingValueSD.text = XRotationStandardValue.ToString("F2");

        float YPositionValueAverage = Strength.Average();
        float YPositionSumOfSquares = Strength.Sum(x => Mathf.Pow(x - YPositionValueAverage, 2));
        float YPositionvariance = YPositionSumOfSquares / Strength.Count;
        float YPositionStandardValue = Mathf.Sqrt(YPositionvariance);
        //StrengthSD.text = YPositionStandardValue.ToString("F2");

        float YValueAverage = YValue.Average();//過牆時所有的Y值，用來評估穩定度
        float YValueSumOfSquares = YValue.Sum(x => Mathf.Pow(x - YValueAverage, 2));
        float YValuevariance = YValueSumOfSquares / YValue.Count;
        float YValueStandardValue = Mathf.Sqrt(YValuevariance);
        StrengthSD.text = YValueStandardValue.ToString("F2");

      /*  float YValue4RAverage = YValue4Range.Average();//過牆時不同高度的Y值，用來評估不同高度的穩定度
        float YValue4RSumOfSquares = YValue4Range.Sum(x => Mathf.Pow(x - YValue4RAverage, 2));
        float YValue4Rvariance = YValue4RSumOfSquares / YValue4Range.Count;
        float YValue4RStandardValue = Mathf.Sqrt(YValue4Rvariance);
        
        float YValue5RAverage = YValue5Range.Average();//過牆時不同高度的Y值，用來評估不同高度的穩定度
        float YValue5RSumOfSquares = YValue5Range.Sum(x => Mathf.Pow(x - YValue5RAverage, 2));
        float YValue5Rvariance = YValue5RSumOfSquares / YValue5Range.Count;
        float YValue5RStandardValue = Mathf.Sqrt(YValue5Rvariance);
        
        float YValue6RAverage = YValue6Range.Average();//過牆時不同高度的Y值，用來評估不同高度的穩定度
        float YValue6RSumOfSquares = YValue6Range.Sum(x => Mathf.Pow(x - YValue6RAverage, 2));
        float YValue6Rvariance = YValue6RSumOfSquares / YValue6Range.Count;
        float YValue6RStandardValue = Mathf.Sqrt(YValue6Rvariance);
        
        float YValue7RAverage = YValue7Range.Average();//過牆時不同高度的Y值，用來評估不同高度的穩定度
        float YValue7RSumOfSquares = YValue7Range.Sum(x => Mathf.Pow(x - YValue7RAverage, 2));
        float YValue7Rvariance = YValue7RSumOfSquares / YValue7Range.Count;
        float YValue7RStandardValue = Mathf.Sqrt(YValue7Rvariance);
        
        float YValue8RAverage = YValue8Range.Average();//過牆時不同高度的Y值，用來評估不同高度的穩定度
        float YValue8RSumOfSquares = YValue8Range.Sum(x => Mathf.Pow(x - YValue8RAverage, 2));
        float YValue8Rvariance = YValue8RSumOfSquares / YValue8Range.Count;
        float YValue8RStandardValue = Mathf.Sqrt(YValue8Rvariance);*/

        FlapSaveCSV(ZRotationValueAverage, ZRotationStandardValue, XRotationValueAverage, 
            XRotationStandardValue, YPositionValueAverage, YPositionStandardValue,Flap_Score.Score);


    }
    public void FlapSaveCSV(float ZRA, float ZRSD, float XRA, float XRSD, float YPA, float YPSD, float score)
    {
        string fileName = "Flap.csv";
        string timePath = Path.Combine(PlayerPrefs.GetString("timePath"), fileName);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("ChestPX,ChestPY,ChestPZ,ChestRX,ChestRY,ChestRZ,RangeNuber,FlapScore,ZRA,ZRSD,XRA,XRSD,YPA,YPSD");//YValue4Range,YValue5Range,YValue6Range,YValue7Range,YValue8Range,

        // 確定最大長度
        int maxLength = new int[] { ChestPX.Count, ChestPY.Count, ChestPZ.Count, ChestRX.Count, ChestRY.Count, ChestRZ.Count }.Max();

        // 根據最大長度遍歷
        for (int i = 0; i < maxLength; i++)
        {///*{GetValueOrDefault(YValue4Range, i)},{GetValueOrDefault(YValue5Range, i)},{GetValueOrDefault(YValue6Range, i)},{GetValueOrDefault(YValue7Range, i)},{GetValueOrDefault(YValue8Range, i)},
            string line = $"{GetValueOrDefault(ChestPX, i)},{GetValueOrDefault(ChestPY, i)},{GetValueOrDefault(ChestPZ, i)},{GetValueOrDefault(ChestRX, i)},{GetValueOrDefault(ChestRY, i)},{GetValueOrDefault(ChestRZ, i)},{GetValueOrDefault(ObstaclesManerger.RandomValue2, i)},{GetValueOrDefault(FlapScore, i)}";
            // 在每一行的末尾添加統計數據
            if (i == 0) // 假設統計數據只需添加一次
            {
                line += $",{ZRA},{ZRSD},{XRA},{XRSD},{YPA},{YPSD},{score}";
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
