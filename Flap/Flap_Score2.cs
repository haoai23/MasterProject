using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Flap_Score2 : MonoBehaviour
{
    public static bool IsTouch;


    private void OnTriggerEnter(Collider other)
    {
        IsTouch = true;
        if (IsTouch & !Flap_Score.IsObstacles)
        {
            Flap_Score.Score += 0.5f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IsTouch = false;
    }
    private void Update()
    {
        Debug.Log("¸I¨ì¨S" + IsTouch);
    }
}
