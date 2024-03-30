using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Climing_CameraFollowPlayer : MonoBehaviour
{
    public Transform _Player;
    public float CameraMoveSpeed = 1f;
    public float CameraTurnSpeed =1f;
    Vector3 RecordLastPlayer, RecordNowPlayer, LastCamera, NowCamera;
    Vector3 PlayerDistance;
    // Start is called before the first frame update
    void Start()//<用歐式距離來算>
    { 
        RecordLastPlayer = _Player.position;
        LastCamera = this.transform.position;
        //Debug.Log("RecordLastPlayer: "+ RecordLastPlayer);
        //Debug.Log("LastCamera: " + LastCamera);
    }

    // Update is called once per frame
    void Update()
    {
        RecordNowPlayer = _Player.position;
        if (RecordLastPlayer != RecordNowPlayer) 
        {
            FollowMethod();
        }
        else
        {
            //Debug.Log("NO");
        }
        
    }
    void FollowMethod()
    {
       // Debug.Log("OK");
        PlayerDistance = RecordNowPlayer - RecordLastPlayer;
        NowCamera = this.transform.position + PlayerDistance;
       // Debug.Log("RecordNowPlayer: " + RecordNowPlayer);
        //Debug.Log("NowCamera: " + NowCamera);
        //Debug.Log("PlayerDistance: " + PlayerDistance);
        
        transform.position = Vector3.Lerp(LastCamera,NowCamera, 1f);
        RecordLastPlayer = RecordNowPlayer;
        LastCamera = NowCamera;



    } 
    void IfRayHit()
    {
        // 从相机位置向玩家位置发射射线
        RaycastHit hit;
        if (Physics.Raycast(transform.position, _Player.position, out hit, Mathf.Infinity))
        {
            
            if (hit.collider != _Player)
            {
                GetComponent<MeshRenderer>().enabled = false;

                Debug.Log("相机和玩家之间遮挡物体。");
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = true;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.black);
                Debug.Log("相机和玩家之间无遮挡物体。");
            }

        }
        else
        {
            // 射线未碰撞到遮挡物体
            Debug.Log("相机和玩家之间无遮挡物体。");

            // 在此处执行打开 Mesh Renderer 的操作
            GetComponent<MeshRenderer>().enabled = true;
        }

    }
}

