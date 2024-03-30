using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowing_Fish : MonoBehaviour
{
    public GameObject WoodBoat;
    public GameObject Whale;
    Vector3 CurrentWoodBoatPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ReadWoodBoatPosition() 
    {
        CurrentWoodBoatPosition = WoodBoat.transform.position;

    }
    private void WhalePosition(Vector3 currentWoodBoatPosition)
    {
        Vector3 WhaleSpawnPosition = new Vector3(currentWoodBoatPosition.x, currentWoodBoatPosition.y, currentWoodBoatPosition.z + 5f);
        Instantiate(WoodBoat, WhaleSpawnPosition, Quaternion.identity);
    }
}
