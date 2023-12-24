using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] MeshRenderer m_rend;
    [SerializeField] AudioClip break_se;

    private float speed = 1.0f;

    void Start()
    {
        m_rend = GetComponentInChildren<MeshRenderer>();
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
    public void GetNoteJudgeFlag()
    {
        audioSource.PlayOneShot(break_se);
        StartCoroutine(DestroyObject());
    }

    private IEnumerator DestroyObject()
    {
        m_rend.enabled = false;
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
    }
}
