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
    
    List<float> ChestPX = new List<float>();
    List<float> ChestPY = new List<float>();
    List<float> ChestPZ = new List<float>();
    List<float> ChestRX = new List<float>();
    List<float> ChestRY = new List<float>();
    List<float> ChestRZ = new List<float>();


    bool Record = true;
    float RecordXRotation, RecordZRotation;

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
    public Text StabilitySD, StrengthSD, LastingValueSD;
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
        StrengthSD.text = YPositionStandardValue.ToString("F2");

        
        FlapSaveCSV(ZRotationValueAverage, ZRotationStandardValue, XRotationValueAverage, 
            XRotationStandardValue, YPositionValueAverage, YPositionStandardValue);


    }
    public void FlapSaveCSV(float ZRA, float ZRSD, float XRA, float XRSD, float YPA, float YPSD)
    {
        string fileName = "Flap.csv";
        string timePath = Path.Combine(PlayerPrefs.GetString("timePath"), fileName);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("ChestPX,ChestPY,ChestPZ,ChestRX,ChestRY,ChestRZ,ZRA,ZRSD,XRA,XRSD,YPA,YPSD");

        // 確定最大長度
        int maxLength = new int[] { ChestPX.Count, ChestPY.Count, ChestPZ.Count, ChestRX.Count, ChestRY.Count, ChestRZ.Count }.Max();

        // 根據最大長度遍歷
        for (int i = 0; i < maxLength; i++)
        {
            string line = $"{GetValueOrDefault(ChestPX, i)},{GetValueOrDefault(ChestPY, i)},{GetValueOrDefault(ChestPZ, i)},{GetValueOrDefault(ChestRX, i)},{GetValueOrDefault(ChestRY, i)},{GetValueOrDefault(ChestRZ, i)}";
            // 在每一行的末尾添加統計數據
            if (i == 0) // 假設統計數據只需添加一次
            {
                line += $",{ZRA},{ZRSD},{XRA},{XRSD},{YPA},{YPSD}";
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
