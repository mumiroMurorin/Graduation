using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;
using UnityEngine;

public class ReadScoreData : MonoBehaviour
{
    //�uKIND�v��ɂ��镶���̒萔��
    const string GENERAL_NOTE = "g";

    private int NOTE_TIME_COLUMN;
    private int NOTE_KIND_COLUMN;
    private int NOTE_POSITION_COLUMN;
    private int NOTE_ANGLE_COLUMN;

    TextAsset csvFile; // CSV�t�@�C��
    List<string[]> csvDatas; // CSV�̒��g�����郊�X�g;

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
            SetDataColumn();    //���̗񂪉���\���Ă��邩�ݒ�
            CSVDataToNotesData();    //���ʃf�[�^��}��
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
        //������
        csvFile = new TextAsset();
        csvDatas = new List<string[]>();
        notesData = new NotesData();

        string csv_name = file_name + "_score";
        StartCoroutine(LoadScoreData(csv_name));
    }

    //���̗񂪉���\���Ă��邩�ݒ�
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
                    Debug.LogError("�m��Ȃ������񂪓����Ă���: " + csvDatas[0][i]);
                    break;
            }
        }
    }

    //CSV�f�[�^���m�[�c�f�[�^(�V�m�[�c�ǉ����͂�����M��)
    private void CSVDataToNotesData()
    {
        string[] str;
        note_num = 0;
        for (int i = 1; i < csvDatas.Count; i++)
        {
            str = csvDatas[i];
            if (str[0] == "" || str[0] == null) { break; }

            note_num++;
            //�V�m�[�c�ǉ����͂����ɕ����ǉ�
            switch (str[NOTE_KIND_COLUMN])
            {
                case GENERAL_NOTE:
                    GeneralNote g = ConvertGeneralNote(str);
                    notesData.AddGeneralNote(g);
                    break;
                default:
                    Debug.LogError("�m��Ȃ����:" + str[NOTE_KIND_COLUMN]);
                    break;
            }
        }
    }

    private GeneralNote ConvertGeneralNote(string[] str)
    {
        GeneralNote g = new GeneralNote();

        //time��float�ɕϊ�
        if (!float.TryParse(str[NOTE_TIME_COLUMN], out g.time))
        {
            Debug.LogError("time����float�ɕϊ��s�ȕ����񂪂���܂���: " + str[NOTE_TIME_COLUMN]);
            return null;
        }

        //angle��float�ɕϊ�
        if (!float.TryParse(str[NOTE_ANGLE_COLUMN], out g.angle))
        {
            Debug.LogError("time����float�ɕϊ��s�ȕ����񂪂���܂���: " + str[NOTE_ANGLE_COLUMN]);
            return null;
        }

        g.pos = StringToVector3(str[NOTE_POSITION_COLUMN]);

        return g;
    }

    //string�u(0:0:0)�v��Vector3�ɕϊ�
    private Vector3 StringToVector3(string input)
    {
        //�����񂩂�ǂݎ��
        var elements = input.Trim('(', ')').Split(':'); // �O��Ɋۊ��ʂ�����΍폜���A�J���}�ŕ���
        var result = Vector3.zero;
        var elementCount = Mathf.Min(elements.Length, 3); // ���[�v�񐔂�elements�̐��ȉ�����3�ȉ��ɂ���

        for (var i = 0; i < elementCount; i++)
        {
            float value;
            value = float.Parse(elements[i]);
            //float.TryParse(elements[i], out value); // �ϊ��Ɏ��s�����Ƃ��ɗ�O���o������]�܂�����΁AParse���g���̂������ł��傤
            result[i] = value;
        }
        return result;
    }

    //����CSV�̓ǂݍ���
    private IEnumerator LoadScoreData(string file_name)
    {
        //CSV�f�[�^�̓ǂݍ���
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

    //---------------------�Q�b�^�[---------------------

    //���ʃf�[�^��n��
    public List<NotesBlock> ReturnScoreData()
    {
        return notesData.ReturnScoreData();
    }

    //�ő�R���{��(�m�[�g��)��Ԃ�
    public int ReturnNoteNum()
    {
        return note_num;
    }

    //���ʂ̏������݂������������Ԃ�
    public bool IsReturnScoreCompleted()
    {
        return isComplete;
    }
}
