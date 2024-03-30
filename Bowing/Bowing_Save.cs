using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Valve.VR;

public class Bowing_Save : MonoBehaviour
{
    GameObject Chest, LeftLeg, RightLeg;
    string SaveBowingData;

    // Update is called once per frame
    void Update()
    {
        Chest = Bowing_PlayerMove.Chest;
        LeftLeg = Bowing_PlayerMove.LeftLeg;
        RightLeg = Bowing_PlayerMove.RightLeg;
        SaveBowingData = Read_Data(Chest, LeftLeg, RightLeg);

    }
    String Read_Data(GameObject TrackerValue, GameObject Tracker1Value, GameObject Tracker2Value)
    {
        String ChestZRotation = Chest.transform.eulerAngles.x.ToString();
        String LeftLegZRotation = LeftLeg.transform.eulerAngles.x.ToString();
        String RightLegZRotation = RightLeg.transform.eulerAngles.x.ToString();
        string BowingData = ChestZRotation +"\t"+LeftLegZRotation+"\t"+RightLegZRotation;
        return BowingData;
    }
    public void Save(string SaveBowingData)
    { 
        
    }
}
