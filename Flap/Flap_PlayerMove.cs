using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Flap_PlayerMove : MonoBehaviour
{
    public GameObject HipsCube;
    public static bool isReady, isGameOver;
    List<float> Stability = new List<float>();
    List<float> Strength = new List<float>();
    List<float> LastingValue = new List<float>();
    public Text Score;

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
        }
        if (isReady)
        {
            Strength.Add(HipsCube.transform.position.y);//強度
            LastingValue.Add(RecordXRotation - HipsCube.transform.eulerAngles.x);//持久度
            Stability.Add(RecordZRotation - HipsCube.transform.eulerAngles.z);//穩定度            
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
        StabilitySD.text = ZRotationStandardValue.ToString();

        float XRotationValueAverage = LastingValue.Average();
        float XRotationSumOfSquares = LastingValue.Sum(x => Mathf.Pow(x - XRotationValueAverage, 2));
        float XRotationvariance = XRotationSumOfSquares / LastingValue.Count;
        float XRotationStandardValue = Mathf.Sqrt(XRotationvariance);
        LastingValueSD.text = XRotationStandardValue.ToString();

        float YPositionValueAverage = Strength.Average();
        float YPositionSumOfSquares = Strength.Sum(x => Mathf.Pow(x - YPositionValueAverage, 2));
        float YPositionvariance = YPositionSumOfSquares / Strength.Count;
        float YPositionStandardValue = Mathf.Sqrt(YPositionvariance);
        StrengthSD.text = YPositionStandardValue.ToString();

    }


}
