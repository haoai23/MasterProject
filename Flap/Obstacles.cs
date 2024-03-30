using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    float ObstaclesMoveSpeeds = 3.5f;
    public GameObject RightObj, LeftObj;
    public GameObject player;

    private void Update()
    {
        transform.Translate(ObstaclesMoveSpeeds * Time.deltaTime, 0f, 0f, Space.World);
        if (transform.localPosition.x >= 20f)
        {
            Destroy(this.gameObject);
            transform.parent.GetComponent<ObstaclesManerger>().SpawnObstacles();
        }
        ObjGap();
        SettingObstaclesMoveSpeeds();
        
    }
    
    void ObjGap()
    {
        float RightObjXPosition = RightObj.transform.localPosition.x;//5
        float LeftObjXPosition = LeftObj.transform.localPosition.x;//-5
        float Gap = Mathf.Abs(RightObjXPosition) - Mathf.Abs(LeftObjXPosition);
        // Debug.Log(Gap);

        if (Gap < 1)
        {
            float AddGap = Random.Range(0.5f, 1.5f);
            RightObj.transform.localPosition = new Vector3(RightObjXPosition + AddGap, 0f, 0f);


        }
    }
    void SettingObstaclesMoveSpeeds()
    {
        if (Flap_Score.Score > 3)
        {
            ObstaclesMoveSpeeds = 4f;                
     
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {       
        Flap_Score.Score -= 1 ;
    }

}
