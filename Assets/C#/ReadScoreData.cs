using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;
using UnityEngine;

public class ReadScoreData : MonoBehaviour
{
    //「KIND」列にある文字の定数化
    const string GENERAL_NOTE = "g";

    private int NOTE_TIME_COLUMN;
    private int NOTE_KIND_COLUMN;
    private int NOTE_POSITION_COLUMN;
    private int NOTE_ANGLE_COLUMN;

    TextAsset csvFile; // CSVファイル
    List<string[]> csvDatas; // CSVの中身を入れるリスト;

    private NotesData notesData;

    private bool isReadCSV;
    private bool isComplete;
    private int note_num;

    void Start()
    {
        
    }

    void Update()
    {
        if (isReadCSV && !isComplete)
        {
            SetDataColumn();    //その列が何を表しているか設定
            CSVDataToNotesData();    //譜面データを挿入
            isComplete = true;
        }
    }

    /*public ReadScoreData(string file_name)
    {
        string csv_name = "Score_" + file_name;
        StartCoroutine(LoadScoreData(csv_name));
    }*/

    public void LoadScoreCSV(string file_name)
    {
        //初期化
        csvFile = new TextAsset();
        csvDatas = new List<string[]>();
        notesData = new NotesData();

        string csv_name = file_name + "_score";
        StartCoroutine(LoadScoreData(csv_name));
    }

    //その列が何を表しているか設定
    private void SetDataColumn()
    {
        for (int i = 0; i < csvDatas[0].Length; i++)
        {
            switch (csvDatas[0][i])
            {
                case "[TIME]":
                    NOTE_TIME_COLUMN = i;
                    break;
                case "[KIND]":
                    NOTE_KIND_COLUMN = i;
                    break;
                case "[POSITION]":
                    NOTE_POSITION_COLUMN = i;
                    break;
                case "[ANGLE]":
                    NOTE_ANGLE_COLUMN = i;
                    break;
                default:
                    Debug.LogError("知らない文字列が入ってきた: " + csvDatas[0][i]);
                    break;
            }
        }
    }

    //CSVデータ→ノーツデータ(新ノーツ追加時はここを弄る)
    private void CSVDataToNotesData()
    {
        string[] str;
        note_num = 0;
        for (int i = 1; i < csvDatas.Count; i++)
        {
            str = csvDatas[i];
            if (str[0] == "" || str[0] == null) { break; }

            note_num++;
            //新ノーツ追加時はここに分岐を追加
            switch (str[NOTE_KIND_COLUMN])
            {
                case GENERAL_NOTE:
                    GeneralNote g = ConvertGeneralNote(str);
                    notesData.AddGeneralNote(g);
                    break;
                default:
                    Debug.LogError("知らないやつだ:" + str[NOTE_KIND_COLUMN]);
                    break;
            }
        }
    }

    private GeneralNote ConvertGeneralNote(string[] str)
    {
        GeneralNote g = new GeneralNote();

        //timeをfloatに変換
        if (!float.TryParse(str[NOTE_TIME_COLUMN], out g.time))
        {
            Debug.LogError("time中にfloatに変換不可な文字列がありました: " + str[NOTE_TIME_COLUMN]);
            return null;
        }

        //angleをfloatに変換
        if (!float.TryParse(str[NOTE_ANGLE_COLUMN], out g.angle))
        {
            Debug.LogError("time中にfloatに変換不可な文字列がありました: " + str[NOTE_ANGLE_COLUMN]);
            return null;
        }

        g.pos = StringToVector3(str[NOTE_POSITION_COLUMN]);

        return g;
    }

    //string「(0:0:0)」をVector3に変換
    private Vector3 StringToVector3(string input)
    {
        //文字列から読み取る
        var elements = input.Trim('(', ')').Split(':'); // 前後に丸括弧があれば削除し、カンマで分割
        var result = Vector3.zero;
        var elementCount = Mathf.Min(elements.Length, 3); // ループ回数をelementsの数以下かつ3以下にする

        for (var i = 0; i < elementCount; i++)
        {
            float value;
            value = float.Parse(elements[i]);
            //float.TryParse(elements[i], out value); // 変換に失敗したときに例外が出る方が望ましければ、Parseを使うのがいいでしょう
            result[i] = value;
        }
        return result;
    }

    //譜面CSVの読み込み
    private IEnumerator LoadScoreData(string file_name)
    {
        //CSVデータの読み込み
        Addressables.LoadAssetAsync<TextAsset>(file_name).Completed += op =>
        {
            csvFile = op.Result;
            StringReader reader = new StringReader(csvFile.text);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                csvDatas.Add(line.Split(','));
            }
            Addressables.Release(op);
        };

        do
        {
            yield return null;
        } while (csvDatas.Count == 0);

        isReadCSV = true;
    }

    //---------------------ゲッター---------------------

    //譜面データを渡す
    public List<NotesBlock> ReturnScoreData()
    {
        return notesData.ReturnScoreData();
    }

    //最大コンボ数(ノート数)を返す
    public int ReturnNoteNum()
    {
        return note_num;
    }

    //譜面の書き込みが完了したか返す
    public bool IsReturnScoreCompleted()
    {
        return isComplete;
    }
}
