using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ʏ�m�[�g�̃N���X(�V�m�[�c�ǉ����͎Q�l�ɂ���Ƃ�����)
public class GeneralNote
{
    public float time;
    public float angle;
    public Vector3 pos;
    public GameObject obj;
}

//�e��m�[�g�̃��X�g�������Ɋi�[(��₱�����̂ŕ�����񂩂����畷����)
public class NotesBlock
{
    public float time;
    //�ʏ�m�[�g�̃��X�g
    public List<GeneralNote> general_list;
    //�ȉ��ɐV�K�m�[�g�̃��X�g��錾

}

public class NotesData 
{
    List<NotesBlock> score;   //���ʃf�[�^

    public NotesData()
    {
        score = new List<NotesBlock>();
    }

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
