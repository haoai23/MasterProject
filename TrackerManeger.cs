using OpenCvSharp.Tracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Valve.VR;
using static Valve.VR.SteamVR_TrackedObject;

public class TrackerManeger : MonoBehaviour
{
    int TrackerNO_i = 0 ;
    string[] tags = { "LeftHand", "LeftLeg","Chest", "RightLeg", "RightHand"};//由左到右
    int tagIndex = 0; // 用于在 tags 數列中循环使用tag
    public GameObject[] trackedObjects;

    private void Awake()
    {
        GameObject[] Objs = GameObject.FindGameObjectsWithTag("TrackerManerger");
        if (Objs.Length > 0)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            ReadHTCViveTracker();
        }
    }
    public static List<GameObject> Tracker = new List<GameObject>() ;

     void ReadHTCViveTracker()
    {
        foreach (EIndex index in Enum.GetValues(typeof(EIndex)))
        {
            if (index == EIndex.None || index == EIndex.Hmd)
                continue;
            
            Transform deviceTransform = trackedObjects[(int)index].transform;
            
            if (!IsTrackerLightBase(deviceTransform.position))
            {
                Tracker.Add(trackedObjects[(int)index]);
                Debug.Log(index.ToString() + trackedObjects[(int)index].transform.position);
                
            }
            // 按照Tracker的 x 座標從左到右排序
            Tracker.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));                       
        }
        // Tracker的標籤
        foreach (GameObject tracker in Tracker)
        {
            tracker.tag = tags[tagIndex];
            tagIndex++;

        }

    }
    bool IsTrackerLightBase(Vector3 position)
    {
        // 不希望添加到Tracker列表的位置
        Vector3[] undesiredPositions =
        {

            new Vector3(-0.02094269f, 1.853097f, -7.3522f),
            new Vector3(-0.5748546f, 0.2134703f, -7.274735f),
            new Vector3(-2.758149f, 0.5444908f, -4.741567f),
            new Vector3(-2.381721f, 2.190113f, 1.40946f),
            new Vector3(1.168892f, 2.284639f, 0.7743435f),
            new Vector3(-2.344299f, 1.719263f, -4.903171f),
            new Vector3(0f,0f,0f)
        };

        foreach (Vector3 undesiredPos in undesiredPositions)
        {
            if (Vector3.Distance(position, undesiredPos) < 0.5f)
            {
                // 如果感測器位置在不希望的位置附近，返回true
                return true;
            }
        }

        // 如果感測器位置不在不希望的位置附近，返回false
        return false;
    }

   
}
