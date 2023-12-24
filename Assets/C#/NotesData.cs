using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralNote
{
    public float time;
    public float angle;
    public Vector3 pos;
    public GameObject obj;
}

public class NotesBlock
{
    public float time;
    public List<GeneralNote> general_list;
}

public class NotesData 
{
    List<NotesBlock> score;   //���ʃf�[�^

    //���ʃf�[�^�ɒʏ�m�[�g��ǉ�
    public bool AddGeneralNote(GeneralNote g)
    {
        //���ɕ��ʃf�[�^�ɓ���time�̃f�[�^������ꍇ�A�}��
        int index = ReturnExistTimeIndex(g.time);
        if (index != -1) { score[index].general_list.Add(g); }
        else    //��L�𖞂����Ȃ������ꍇ�A�V���ɂ���time��NotesBlock�𐶐����ĕ��ʃf�[�^�ɒǉ�
        {
            NotesBlock notesBlock = ReturnVoidNotesBlock();
            notesBlock.time = g.time;
            notesBlock.general_list = new List<GeneralNote> { g };
            score.Add(notesBlock);
        }

        return true;
    }

    //���������ꂽNotesBlock��Ԃ�
    private NotesBlock ReturnVoidNotesBlock()
    {
        return new NotesBlock
        {
            time = 0,
            general_list = new List<GeneralNote>(),
        };
    }

    //������time��NotesBlock�����ʃf�[�^�ɑ��݂����ꍇ�A����Index��Ԃ�
    private int ReturnExistTimeIndex(float time)
    {
        for (int i = 0; i < score.Count; i++)
        {
            if (score[i].time == time) { return i; }
        }

        return -1;
    }

    //���ʃf�[�^��Ԃ�
    public List<NotesBlock> ReturnScoreData()
    {
        return score;
    }
}
