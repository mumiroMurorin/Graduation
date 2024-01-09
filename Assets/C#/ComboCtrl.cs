using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCtrl : MonoBehaviour
{
    const int MAX_DIGIT = 4;
    [Header("�R���{�e")]
    [SerializeField] private GameObject combo_par;

    [Header("�uCOMBO�v")]
    [SerializeField] private GameObject combo_char_obj;

    [Header("0�`9�܂�")]
    [SerializeField] private GameObject[] number_obj;

    [Header("�����Ԋu")]
    [SerializeField] private float char_distance;

    [Header("�G�t�F�N�g")]
    [SerializeField] private ParticleSystem particle;

    private Animator animator;
    private GameObject[,] number_obj_array = new GameObject[MAX_DIGIT, 10];
    private GameObject[] activeNum_array = new GameObject[MAX_DIGIT];

    void Start()
    {
        animator = GetComponent<Animator>();
        //���u������Ă���I�u�W�F�N�g���\��
        foreach (Transform child in combo_par.transform)
        {
            child.gameObject.SetActive(false);
        }
        combo_char_obj.transform.SetParent(combo_par.transform);

        //���ꂼ��̌��̐�������
        for (int i = 0; i < MAX_DIGIT; i++) 
        {
            for (int j = 0; j < 10; j++)
            {
                number_obj_array[i, j] = Instantiate(number_obj[j], combo_par.transform);
                number_obj_array[i, j].SetActive(false);
            }
        }
        ChangeCombo(0);
    }

    void Update()
    {
        
    }

    //�����̃R���{���ɕύX
    public void ChangeCombo(int combo)
    {
        particle.Stop();

        //�R���{4�ȉ��̎��͔�\��
        if (combo < 5) {
            combo_par.SetActive(false);
            combo_char_obj.SetActive(false);
            return; 
        }
        combo_par.SetActive(true);
        combo_char_obj.SetActive(true);

        //�G���[����
        if (combo > 9999 || combo < 0) { 
            Debug.LogWarning("�R���{����0�`9999�ł͂���܂���: " + combo); 
            return; 
        }

        //�����R���{�̔�\��
        foreach(GameObject g in activeNum_array) { if (g) { g.SetActive(false); } }
        //�����擾
        int digit = (combo == 0) ? 1 : ((int)Mathf.Log10(combo) + 1);
        //����ő�4��
        for (int i = 0; i < ((digit < MAX_DIGIT + 1) ? digit : MAX_DIGIT); i++) 
        { GenerateComboObj(combo, digit, i); }

        //�A�j���[�V�����ƃG�t�F�N�g
        animator.SetTrigger("Add");
        particle.Play();
    }

    //(n+1)���ڂ̃I�u�W�F�N�g�𐶐�
    private void GenerateComboObj(int combo, int digit, int n)
    {
        int num = ReturnDigitNum(combo, n);
        //���̌��������������������A��
        if(num == -1) { return; }
        float x = (digit % 2 == 0) ? (digit / 2 - 0.5f - n) * char_distance :
           (digit / 2 - n) * char_distance;
        number_obj_array[n, num].transform.localPosition = new Vector3(x, 0, 0);
        number_obj_array[n, num].SetActive(true);
        activeNum_array[n] = number_obj_array[n, num];
    }

    //�R���{��(n + 1)���ڂ̐������o��(combo��(n + 1)�������������ꍇ-1��Ԃ�)
    private int ReturnDigitNum(int combo, int n)
    {
        if (combo % (int)Mathf.Pow(10, n) == combo && combo != 0) { return -1; }
        combo %= (int)Mathf.Pow(10, n + 1);
        combo /= (int)Mathf.Pow(10, n);
        return combo;
    }

    //�y�ȊJ�n
    public void GameStart()
    {
        combo_par.SetActive(true);
        ChangeCombo(0);
    }

    //�X�^�[�g�A���X�^�[�g���̏�����
    public void Init_Start()
    {
        combo_par.SetActive(false);//����
    }
}
