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

    private void Start()
    {
        DontDestroyOnLoad(this);
        GameObject[] Objs = GameObject.FindGameObjectsWithTag("TrackerManeger");
        if (Objs.Length > 1)
        {
            Destroy(this);
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReadHTCViveTracker();
            Debug.Log("是在哈瞜");
        }
    }
    private void FixedUpdate()
    {
        
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
            Debug.Log("tagindex" + tagIndex);
        }

    }
    bool IsTrackerLightBase(Vector3 position)
    {
        // 不希望添加到Tracker列表的位置
        Vector3[] undesiredPositions =
        {

            new Vector3(-2.471596f, 2.297002f, 1.154322f),
            new Vector3(1.097765f, 2.327811f, 0.9900124f),
            new Vector3(-1.740437f, 1.887648f, -5.103213f),
            new Vector3(0.9536016f, 1.885166f, -7.26f),
            new Vector3(0f,0f,0f)
        };

        foreach (Vector3 undesiredPos in undesiredPositions)
        {
            if (Vector3.Distance(position, undesiredPos) < 1f)
            {
                // 如果感測器位置在不希望的位置附近，返回true
                return true;
            }
        }

        // 如果感測器位置不在不希望的位置附近，返回false
        return false;
    }

   
}
