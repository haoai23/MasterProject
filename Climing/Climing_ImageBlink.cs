using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Climing_ImageBlink : MonoBehaviour
{
    //float BlinkSpeed = 0.5f;
    public Image[] PlayerFootTips;
   // public int i = 0, WhiteOrBlack = 255,  IsBlink = 0;
    public static Climing_ImageBlink ClimingImageBlink;
    // Start is called before the first frame update
    void Start()
    {
        PlayerFootTips = GetComponentsInChildren<Image>();
        ClimingImageBlink = this;
    }
    public void ImageBlink(int Index,int WhiteOrBlack, int IsBlink)//白色255 黑色0，透明度0看不到 透明度255看的到
    {
        PlayerFootTips[Index].color = new Color(WhiteOrBlack, WhiteOrBlack, WhiteOrBlack, IsBlink);

    }
    /*public static void ImageBlinkTest(int Index, int WhiteOrBlack, int IsBlink)
    {
        if (ClimingImageBlink != null && ClimingImageBlink.PlayerFootTips != null &&
            Index >= 0 && Index < ClimingImageBlink.PlayerFootTips.Length)
        {
            ClimingImageBlink.PlayerFootTips[Index].color = new Color(WhiteOrBlack, WhiteOrBlack, WhiteOrBlack, IsBlink);
        }
        else
        {
            Debug.LogError("无效的索引或缺少 PlayerFootTips 数组。");
        }
    }*/
}
