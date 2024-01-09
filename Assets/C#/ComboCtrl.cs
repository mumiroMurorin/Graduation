using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCtrl : MonoBehaviour
{
    const int MAX_DIGIT = 4;
    [Header("コンボ親")]
    [SerializeField] private GameObject combo_par;

    [Header("「COMBO」")]
    [SerializeField] private GameObject combo_char_obj;

    [Header("0～9まで")]
    [SerializeField] private GameObject[] number_obj;

    [Header("文字間隔")]
    [SerializeField] private float char_distance;

    [Header("エフェクト")]
    [SerializeField] private ParticleSystem particle;

    private Animator animator;
    private GameObject[,] number_obj_array = new GameObject[MAX_DIGIT, 10];
    private GameObject[] activeNum_array = new GameObject[MAX_DIGIT];

    void Start()
    {
        animator = GetComponent<Animator>();
        //仮置きされているオブジェクトを非表示
        foreach (Transform child in combo_par.transform)
        {
            child.gameObject.SetActive(false);
        }
        combo_char_obj.transform.SetParent(combo_par.transform);

        //それぞれの桁の数字生成
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

    //引数のコンボ数に変更
    public void ChangeCombo(int combo)
    {
        particle.Stop();

        //コンボ4以下の時は非表示
        if (combo < 5) {
            combo_par.SetActive(false);
            combo_char_obj.SetActive(false);
            return; 
        }
        combo_par.SetActive(true);
        combo_char_obj.SetActive(true);

        //エラー処理
        if (combo > 9999 || combo < 0) { 
            Debug.LogWarning("コンボ数が0～9999ではありません: " + combo); 
            return; 
        }

        //既存コンボの非表示
        foreach(GameObject g in activeNum_array) { if (g) { g.SetActive(false); } }
        //桁数取得
        int digit = (combo == 0) ? 1 : ((int)Mathf.Log10(combo) + 1);
        //現状最大4桁
        for (int i = 0; i < ((digit < MAX_DIGIT + 1) ? digit : MAX_DIGIT); i++) 
        { GenerateComboObj(combo, digit, i); }

        //アニメーションとエフェクト
        animator.SetTrigger("Add");
        particle.Play();
    }

    //(n+1)桁目のオブジェクトを生成
    private void GenerateComboObj(int combo, int digit, int n)
    {
        int num = ReturnDigitNum(combo, n);
        //その桁が無い時生成せずお帰り
        if(num == -1) { return; }
        float x = (digit % 2 == 0) ? (digit / 2 - 0.5f - n) * char_distance :
           (digit / 2 - n) * char_distance;
        number_obj_array[n, num].transform.localPosition = new Vector3(x, 0, 0);
        number_obj_array[n, num].SetActive(true);
        activeNum_array[n] = number_obj_array[n, num];
    }

    //コンボの(n + 1)桁目の数字を出力(comboが(n + 1)桁未満だった場合-1を返す)
    private int ReturnDigitNum(int combo, int n)
    {
        if (combo % (int)Mathf.Pow(10, n) == combo && combo != 0) { return -1; }
        combo %= (int)Mathf.Pow(10, n + 1);
        combo /= (int)Mathf.Pow(10, n);
        return combo;
    }

    //楽曲開始
    public void GameStart()
    {
        combo_par.SetActive(true);
        ChangeCombo(0);
    }

    //スタート、リスタート時の初期化
    public void Init_Start()
    {
        combo_par.SetActive(false);//かり
    }
}
