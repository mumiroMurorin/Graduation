using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    [Header("リザルトオブジェクト")]
    [SerializeField] private GameObject result_obj;
    [SerializeField] private ComboCtrl comboCtrl;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //スタート、リスタート時の初期化
    public void Init_Start()
    {
        result_obj.SetActive(false);//かり
        comboCtrl.Init_Start();
    }

    //楽曲開始
    public void GameStart()
    {
        comboCtrl.GameStart();
    }

    //コンボ変更
    public void ChangeCombo(int combo)
    {
        comboCtrl.ChangeCombo(combo);
    }

    //リザルト画面の出現
    public void AdventResultUI()
    {
        result_obj.SetActive(true);//仮
    }
}