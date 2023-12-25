using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private GameObject se_obj;
    [SerializeField] private GameObject effect_obj;

    private float speed = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MoveObject();
    }

    public void Init(float s)
    {
        speed = s;
    }

    //ノートを動かす(一旦真っすぐ)
    private void MoveObject()
    {
        this.gameObject.transform.position += -Vector3.forward * Time.fixedDeltaTime * speed;
    }

    //子オブジェクトがジャッジ基準を満たしたとき
    public void GetNoteJudgeFlag(Vector3 sword_angle)
    {
        Instantiate(se_obj, this.gameObject.gameObject.transform.position, Quaternion.identity);
        //Instantiate(effect_obj, this.gameObject.gameObject.transform.position, Quaternion.Euler(sword_angle + effect_obj.transform.eulerAngles));
        Destroy(this.gameObject);
    }
}
