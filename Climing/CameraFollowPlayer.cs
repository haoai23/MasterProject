using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public Transform followTarget;
    //方向向量
    private Vector3 dir;
    //射线碰撞检测器
    private RaycastHit hit;
    //摄像机移动速度
    public float moveSpeed;
    //摄像机旋转速度
    public float turnSpeed;
    //摄像机观察的档位【可选的视角位置的个数】
    public const int camera_watch_gear = 5;
    //观察玩家身体偏移量
    public const float PLAYER_WATCHBODY_OFFSET = 1f;

    private void Start()
    {
        //计算方向向量【摄像机指向玩家】
        dir = followTarget.transform.position - transform.position  ;//相機跟玩家之間的距離
        //dir = -transform.forward;
    }

    private void Update()
    {
        FollowMethod();
    }

    /// <summary>
    /// 跟随算法
    /// </summary>
    private void FollowMethod()
    {
        //时时刻刻计算摄像机的跟随的最佳位置
        Vector3 bestWatchPos = followTarget.position - dir;
        //计算跟随目标头顶的俯视位置【不好、但可以保证看到玩家】
        Vector3 badWatchPos = followTarget.position + Vector3.up * (dir.magnitude);
        Debug.Log(bestWatchPos);
        //定义所有观察点的数组【数组长度就为档位个数】
        Vector3[] watchPoints = new Vector3[camera_watch_gear];
        //设置数组的起始点
        watchPoints[0] = bestWatchPos;
        watchPoints[watchPoints.Length - 1] = badWatchPos;

        for (int i = 1; i <= watchPoints.Length - 2; i++)
        {
            //计算中间观察点的坐标
            watchPoints[i] = Vector3.Lerp(bestWatchPos, badWatchPos,
                (float)i / (camera_watch_gear - 1));
        }

        //声明最合适的观察点【初值是最初的观察点】
        Vector3 suitablePos = bestWatchPos;
        //遍历所有的观察点
        for (int i = 0; i < watchPoints.Length; i++)
        {
            //检测该点是否可以看到玩家
            if (CanSeeTarget(watchPoints[i]))
            {
                //选出最合适的点
                suitablePos = watchPoints[i];
                //跳出循环
                break;
            }
        }
        //插值移动到合适的位置
        transform.position = Vector3.Lerp(transform.position, suitablePos, Time.deltaTime * moveSpeed);

        //计算该点指向玩家的方向向量
        Vector3 crtDir = followTarget.position + Vector3.up * PLAYER_WATCHBODY_OFFSET - suitablePos;
        //将方向向量转成四元数
        Quaternion targetQua = Quaternion.LookRotation(crtDir);
        //Lerp过去
        transform.rotation = Quaternion.Lerp(transform.rotation,
            targetQua, Time.deltaTime * turnSpeed);
        //欧拉角修正
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
    }

    /// <summary>
    /// 检测该点可以看到玩家
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool CanSeeTarget(Vector3 pos)
    {
        //计算此时的方向向量
        Vector3 crtDir = followTarget.position +Vector3.up * PLAYER_WATCHBODY_OFFSET - pos;
        //发射物理射线
        if (Physics.Raycast(pos, crtDir, out hit))
        {
            //射线打到的对象是玩家，说明该点可以看到玩家
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
}

