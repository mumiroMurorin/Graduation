using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 楽曲データ
/// </summary>
public class MusicData
{
    //楽曲情報
    public string file_name;
    public string title;
    public string composer;
    public Sprite thumbneil;
    public AudioClip preview;

    //オプション
    public float sword_effect_magni;
    public float judge_correct_effect_magni;
    public float judgeUI_magni;
}

/// <summary>
/// リザルトデータ
/// </summary>
public class ResultData
{
    public int score;
    public string rank;
    public int p_cri_num;
    public int cri_num;
    public int hit_num;
    public int miss_num;
}

namespace Common
{
    /// <summary>
    /// 共通定数
    /// </summary>
    public static class GrovalConst
    {
        public const int P_CRITICAL_NUMBER = 0;
        public const int CRITICAL_NUMBER = 1;
        public const int HIT_NUMBER = 2;
        public const int MISS_NUMBER = 3;

        public const float SWORD_VIBRATION_TIME = 0.175f;
    }

    /// <summary>
    /// コントローラ関係
    /// </summary>
    public static class OculusController
    {
        //コントローラを震わせる
        public static IEnumerator VibrationController(bool isRight)
        {
            if (isRight)
            {
                OVRInput.SetControllerVibration(0f, 1f, OVRInput.Controller.RTouch);
                yield return new WaitForSeconds(GrovalConst.SWORD_VIBRATION_TIME);
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
            }
            else
            {
                OVRInput.SetControllerVibration(0f, 1f, OVRInput.Controller.LTouch);
                yield return new WaitForSeconds(GrovalConst.SWORD_VIBRATION_TIME);
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
            }
        }
    }
}

public class GameManager : MonoBehaviour
{
    [Header("作業中？")]
    public bool isConstruction = false;

    public static GameManager instance = null;
    public float speed;
    public float sword_effect_magni;
    public float judge_correct_effect_magni;
    public float judgeUI_magni;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}