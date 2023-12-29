using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// ã§í íËêî
    /// </summary>
    public static class GrovalConst
    {
        public const int P_CRITICAL_NUMBER = 0;
        public const int CRITICAL_NUMBER = 1;
        public const int HIT_NUMBER = 2;
        public const int MISS_NUMBER = 3;
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