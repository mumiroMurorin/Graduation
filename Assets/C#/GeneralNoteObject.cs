using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralNoteObject : MonoBehaviour
{
    [SerializeField] Note note;
    private bool isSlashStart;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Œ•‚ªG‚ê‚½‚Æ‚«A”»’èŠJn
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Sword"))
        {
            isSlashStart = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Sword") && isSlashStart)
        {
            note.GetNoteJudgeFlag(other.gameObject.transform.eulerAngles);
        }
    }
}
