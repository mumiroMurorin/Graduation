using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private float magni;        //力の大きさ(距離依存)
    private Vector3 pos_past;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        //剣の現在位置を取得
        Vector3 pos_now = this.gameObject.transform.position;
        
        //力(距離)を計算して代入
        magni = ReturnMagnitude(pos_past, pos_now) / Time.fixedDeltaTime;
        Debug.Log("剣力: " + magni);

        //剣の現在位置を過去のポジションとして登録
        pos_past = this.gameObject.transform.position;
    }

    //剣の動きの大きさ(距離)を返す
    private float ReturnMagnitude(Vector3 f, Vector3 l)
    {
        return  Mathf.Sqrt(Mathf.Pow(f.x - l.x, 2) + Mathf.Pow(f.y - l.y, 2) + Mathf.Pow(f.z - l.z, 2));
    }

    //力の大きさを返す(ゲッター)
    public float ReturnMagni()
    {
        return magni;
    }
}
