using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    [Header("リザルトオブジェクト")]
    [SerializeField] private GameObject result_obj;
    [SerializeField] private ComboCtrl comboCtrl;

    [Header("判定親")]
    [SerializeField] private GameObject judge_par;
    [Header("上から判定4つ")]
    [SerializeField] private GameObject[] judge_obj_ori;

    private GameObject[] judge_obj;

    void Start()
    {
        //仮
        judge_obj = new GameObject[4];
        for(int i = 0;i < 4; i++)
        {
            judge_obj[i] = Instantiate(judge_obj_ori[i], judge_par.transform);
            judge_obj[i].SetActive(false);
        }
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

    //判定UIの表示
    public void AdventJudgeUI(int judgement_num, Vector3 pos)
    {
        GameObject g = Instantiate(judge_obj[judgement_num], pos, Quaternion.identity, judge_par.transform);
        g.SetActive(true);
    }

    //リザルト画面の出現
    public void AdventResultUI()
    {
        result_obj.SetActive(true);//仮
    }
}