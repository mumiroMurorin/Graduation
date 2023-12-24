using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralNoteObject : MonoBehaviour
{
    [SerializeField] Note note;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //剣が触れたとき、判定開始
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Sword"))
        {
            note.GetNoteJudgeFlag();
        }
    }
}
