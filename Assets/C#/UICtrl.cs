using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    [Header("���U���g�I�u�W�F�N�g")]
    [SerializeField] private GameObject result_obj;
    [SerializeField] private ComboCtrl comboCtrl;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //�X�^�[�g�A���X�^�[�g���̏�����
    public void Init_Start()
    {
        result_obj.SetActive(false);//����
        comboCtrl.Init_Start();
    }

    //�y�ȊJ�n
    public void GameStart()
    {
        comboCtrl.GameStart();
    }

    //�R���{�ύX
    public void ChangeCombo(int combo)
    {
        comboCtrl.ChangeCombo(combo);
    }

    //���U���g��ʂ̏o��
    public void AdventResultUI()
    {
        result_obj.SetActive(true);//��
    }
}