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

    //ノートの斬撃判定
    public void GetNoteJudgeFlag()
    {
        //音声の再生(SEオブジェクトの複製)
        Instantiate(se_obj, this.gameObject.gameObject.transform.position, Quaternion.identity);
        //このオブジェクトを抹消
        Destroy(this.gameObject);
    }
}
