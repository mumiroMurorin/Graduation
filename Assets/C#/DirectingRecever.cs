using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectingRecever : MonoBehaviour
{
    private DirectingCtrl directingCtrl;

    void Start()
    {
        directingCtrl = GameObject.Find("DirectingCtrl").GetComponent<DirectingCtrl>();
    }

    //��M
    public void ReceveSignal()
    {
        directingCtrl.SetDirectingFinish();
    }

}
