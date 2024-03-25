using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
//using System.Diagnostics;

public class Flap_Save : MonoBehaviour
{
    public GameObject HipsTracker;
    public Text FlapScore;
    string SaveFlapData;
    // Update is called once per frame
    void Update()
    {
        if (Timer.Start_Timer)
        { 
            SaveFlapData = Read_Data(FlapScore, HipsTracker);
            //Save(SaveFlapData);

            Debug.Log("∂}©l¶s¿…");
        }
        
    }
    string Read_Data(Text FlapScore, GameObject HipsTracker)
    {
        string FlapScoreText = FlapScore.text;
        string Flap_HipsTrackerYPosition = HipsTracker.transform.position.y.ToString();
        string Flap_HipsTrackerRotationin = HipsTracker.transform.eulerAngles.ToString();

        string FlapData = FlapScoreText + "\t"  + Flap_HipsTrackerYPosition +"\t" +Flap_HipsTrackerRotationin + "\n";
        return FlapData;
    
    }
    /*public void Save(string SaveFlapData) 
    {
        DateTime localDate = DateTime.Now;
        print(localDate.ToString("yyyyMMdd-HH-mm-ss"));


        string path = Application.dataPath + "/"+ localDate.ToString("yyyyMMdd-HH-mm-ss") + "/FlapData.txt";
        string  Data = SaveFlapData;

        if (!File.Exists(path))

        {
            File.WriteAllText(path, Data);
        }
        else
        {
            File.AppendAllText(path, Data);
        }

    }*/


}
