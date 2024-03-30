using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public class WhackAMole_SpwanObstacles : MonoBehaviour
{
    public GameObject[] WhackAMolePrefabs;
    public Transform [] SpawnTransform;
    public GameObject[] SpawnObject;
    float  Timer_i = 0;
    public  bool[]PositionOccupied;
    public static float[] SpawnPrefabTime;
    public static float[] DestroyPrefabTime;

    float ReactionTime;
    private void Start()
    {
        if (SpawnTransform != null && PositionOccupied != null)
        {
            // 初始化 PositionOccupied 
            PositionOccupied = new bool[SpawnTransform.Length];
            SpawnObject = new GameObject[SpawnTransform.Length];
            SpawnPrefabTime = new float[SpawnTransform.Length];
            DestroyPrefabTime = new float[SpawnTransform.Length];
}
    }
    // Update is called once per frame
    void Update()
    {
        Timer_i +=  Time.deltaTime;
        if (Timer_i >= 2)
        {
            StartCoroutine(SpawnWhackAMolePrefabs3s());
            WhackAMole_ReactionTime();
        }
      
    }
    IEnumerator SpawnWhackAMolePrefabs3s()  
    {
        int TransformIndex = Random.Range(0, SpawnTransform.Length);
        int PrefabsIndex = Random.Range(0, WhackAMolePrefabs.Length);


        bool PositionOccupid = PositionOccupied[TransformIndex];
        
        if (!PositionOccupid)//檢查該位置是否有物件
        {
            GameObject spawnObject = Instantiate(WhackAMolePrefabs[0], SpawnTransform[TransformIndex].position, SpawnTransform[TransformIndex].rotation);
            PositionOccupied[TransformIndex] = true;
            SpawnObject[TransformIndex] = spawnObject;
            
            if (spawnObject.tag == "WhackAMole_Mole")
            {
                SpawnPrefabTime[TransformIndex] = Time.time;
                Debug.Log("SpawnPrefabTime[TransformIndex] " + SpawnPrefabTime[TransformIndex]);
            }
            else 
            {
                SpawnPrefabTime[TransformIndex] = 0;
            }
            for (int i = 0; i < SpawnTransform.Length; i++)
            {
                if (SpawnObject[i] == null)
                {
                    PositionOccupied[i] = false;
                    DestroyPrefabTime[i] = Time.time;
                }
                
            }
                /*else if (SpawnObject[i] == null)
                {
                    PositionOccupied[i] = false;
                }*/
            
           
        }
        
        Timer_i = 0;
        yield return new WaitForSeconds(2);
        
    }
    void WhackAMole_ReactionTime()
    {
        for (int i = 0; i < SpawnTransform.Length; i++)
        {
            if (SpawnPrefabTime[i] != 0 && DestroyPrefabTime[i] != 0)
            {
                ReactionTime = SpawnPrefabTime[i] - DestroyPrefabTime[i];
                Debug.Log("ReactionTime " + ReactionTime);
            }
            
        }
    }
    
}
 