using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Debug.Log("今: " + Time.time);
                OVRInput.SetControllerVibration(0f, 1f, OVRInput.Controller.RTouch);
                yield return new WaitForSeconds(GrovalConst.SWORD_VIBRATION_TIME);
                OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
                Debug.Log("あと: " + Time.time);
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
    public static GameManager instance = null;
    public float speed;

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