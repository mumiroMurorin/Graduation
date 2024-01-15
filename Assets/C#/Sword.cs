using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private float magni;        //力の大きさ(距離依存)
    private Vector3 power_vec;  //力の向き
    private Vector3 pos_past;
    [SerializeField] private ParticleSystem particle;

    //private ParticleSystem.Trails trails;

    void Start()
    {

    }

    void FixedUpdate()
    {
        //剣の現在位置を取得
        Vector3 pos_now = this.gameObject.transform.position;
        
        //力(距離)を計算して代入
        magni = ReturnMagnitude(pos_past, pos_now) / Time.fixedDeltaTime;
        //power_vec = ReturnPowerVector(pos_past, pos_now);
        //Debug.Log("剣力: " + magni);

        //剣の現在位置を過去のポジションとして登録
        pos_past = this.gameObject.transform.position;
    }

    //剣の動きの大きさ(距離)を返す
    private float ReturnMagnitude(Vector3 f, Vector3 l)
    {
        return  Mathf.Sqrt(Mathf.Pow(f.x - l.x, 2) + Mathf.Pow(f.y - l.y, 2) + Mathf.Pow(f.z - l.z, 2));
    }

    //力の向きを返す
    private Vector3 ReturnPowerVector(Vector3 f, Vector3 l)
    {
        return l - f;
    }

    //力の大きさを返す(ゲッター)
    public float ReturnMagni()
    {
        return magni;
    }

    //力の向きを返す
    public Vector3 ReturnVector()
    {
        return power_vec;
    }

    //剣エフェクトの調整
    public void SetSwordEffect(float _ratio,float life_time)
    {
        var trails = particle.trails;
        trails.ratio = _ratio;
        trails.lifetime = new ParticleSystem.MinMaxCurve(life_time);
    }

}