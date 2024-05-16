using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTower_PrefabMove : MonoBehaviour
{
    public static float MoveSpeed = 0f;
    void Update()
    {
        if (StairTower_PlayerMove.isStart)
        {
            MoveSpeed = 0.3f + StairTower_Timer.StairTower_i / 50;
            if (MoveSpeed > 1.2f)
            {
                transform.Translate(0, -1.2f * Time.deltaTime, 0);
            }
            else
            {
                transform.Translate(0, -MoveSpeed * Time.deltaTime, 0);
            }

            Debug.Log("MoveSpeed: " + MoveSpeed);
            Debug.Log("isStart: " + StairTower_PlayerMove.isStart);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
