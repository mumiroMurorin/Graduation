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

public class ButtonANote
{
    public float time;
    public float angle;
    public Vector3 pos;
    public GameObject obj;
}

public class ButtonBNote
{
    public float time;
    public float angle;
    public Vector3 pos;
    public GameObject obj;
}

public class ButtonXNote
{
    public float time;
    public float angle;
    public Vector3 pos;
    public GameObject obj;
}

public class ButtonYNote
{
    public float time;
    public float angle;
    public Vector3 pos;
    public GameObject obj;
}

public class ButtonRNote
{
    public float time;
    public float angle;
    public Vector3 pos;
    public GameObject obj;
}

public class ButtonLNote
{
    public float time;
    public float angle;
    public Vector3 pos;
    public GameObject obj;
}

public class EscapeNote
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
    public List<ButtonANote> buttonA_list;
    public List<ButtonBNote> buttonB_list;
    public List<ButtonXNote> buttonX_list;
    public List<ButtonYNote> buttonY_list;
    public List<ButtonLNote> buttonL_list;
    public List<ButtonRNote> buttonR_list;
    public List<EscapeNote> escape_list;
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

    //���ʃf�[�^�ɓ���m�[�g��ǉ�
    public bool AddButtonANote(ButtonANote a)
    {
        //���ɕ��ʃf�[�^�ɓ���time�̃f�[�^������ꍇ�A�}��
        int index = ReturnExistTimeIndex(a.time);
        if (index != -1) { score[index].buttonA_list.Add(a); }
        else    //��L�𖞂����Ȃ������ꍇ�A�V���ɂ���time��NotesBlock�𐶐����ĕ��ʃf�[�^�ɒǉ�
        {
            NotesBlock notesBlock = ReturnVoidNotesBlock();
            notesBlock.time = a.time;
            notesBlock.buttonA_list = new List<ButtonANote> { a };
            score.Add(notesBlock);
        }

        return true;
    }
    public bool AddButtonBNote(ButtonBNote b)
    {
        //���ɕ��ʃf�[�^�ɓ���time�̃f�[�^������ꍇ�A�}��
        int index = ReturnExistTimeIndex(b.time);
        if (index != -1) { score[index].buttonB_list.Add(b); }
        else    //��L�𖞂����Ȃ������ꍇ�A�V���ɂ���time��NotesBlock�𐶐����ĕ��ʃf�[�^�ɒǉ�
        {
            NotesBlock notesBlock = ReturnVoidNotesBlock();
            notesBlock.time = b.time;
            notesBlock.buttonB_list = new List<ButtonBNote> { b };
            score.Add(notesBlock);
        }

        return true;
    }
    public bool AddButtonXNote(ButtonXNote x)
    {
        //���ɕ��ʃf�[�^�ɓ���time�̃f�[�^������ꍇ�A�}��
        int index = ReturnExistTimeIndex(x.time);
        if (index != -1) { score[index].buttonX_list.Add(x); }
        else    //��L�𖞂����Ȃ������ꍇ�A�V���ɂ���time��NotesBlock�𐶐����ĕ��ʃf�[�^�ɒǉ�
        {
            NotesBlock notesBlock = ReturnVoidNotesBlock();
            notesBlock.time = x.time;
            notesBlock.buttonX_list = new List<ButtonXNote> { x };
            score.Add(notesBlock);
        }

        return true;
    }
    public bool AddButtonYNote(ButtonYNote y)
    {
        //���ɕ��ʃf�[�^�ɓ���time�̃f�[�^������ꍇ�A�}��
        int index = ReturnExistTimeIndex(y.time);
        if (index != -1) { score[index].buttonY_list.Add(y); }
        else    //��L�𖞂����Ȃ������ꍇ�A�V���ɂ���time��NotesBlock�𐶐����ĕ��ʃf�[�^�ɒǉ�
        {
            NotesBlock notesBlock = ReturnVoidNotesBlock();
            notesBlock.time = y.time;
            notesBlock.buttonY_list = new List<ButtonYNote> { y };
            score.Add(notesBlock);
        }

        return true;
    }
    public bool AddButtonLNote(ButtonLNote l)
    {
        //���ɕ��ʃf�[�^�ɓ���time�̃f�[�^������ꍇ�A�}��
        int index = ReturnExistTimeIndex(l.time);
        if (index != -1) { score[index].buttonL_list.Add(l); }
        else    //��L�𖞂����Ȃ������ꍇ�A�V���ɂ���time��NotesBlock�𐶐����ĕ��ʃf�[�^�ɒǉ�
        {
            NotesBlock notesBlock = ReturnVoidNotesBlock();
            notesBlock.time = l.time;
            notesBlock.buttonL_list = new List<ButtonLNote> { l };
            score.Add(notesBlock);
        }

        return true;
    }
    public bool AddButtonRNote(ButtonRNote r)
    {
        //���ɕ��ʃf�[�^�ɓ���time�̃f�[�^������ꍇ�A�}��
        int index = ReturnExistTimeIndex(r.time);
        if (index != -1) { score[index].buttonR_list.Add(r); }
        else    //��L�𖞂����Ȃ������ꍇ�A�V���ɂ���time��NotesBlock�𐶐����ĕ��ʃf�[�^�ɒǉ�
        {
            NotesBlock notesBlock = ReturnVoidNotesBlock();
            notesBlock.time = r.time;
            notesBlock.buttonR_list = new List<ButtonRNote> { r };
            score.Add(notesBlock);
        }

        return true;
    }
    public bool AddEscapeNote(EscapeNote e)
    {
        //���ɕ��ʃf�[�^�ɓ���time�̃f�[�^������ꍇ�A�}��
        int index = ReturnExistTimeIndex(e.time);
        if (index != -1) { score[index].escape_list.Add(e); }
        else    //��L�𖞂����Ȃ������ꍇ�A�V���ɂ���time��NotesBlock�𐶐����ĕ��ʃf�[�^�ɒǉ�
        {
            NotesBlock notesBlock = ReturnVoidNotesBlock();
            notesBlock.time = e.time;
            notesBlock.escape_list = new List<EscapeNote> { e };
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
            buttonA_list = new List<ButtonANote>(),
            buttonB_list = new List<ButtonBNote>(),
            buttonX_list = new List<ButtonXNote>(),
            buttonY_list = new List<ButtonYNote>(),
            buttonL_list = new List<ButtonLNote>(),
            buttonR_list = new List<ButtonRNote>(),
            escape_list = new List<EscapeNote>(),
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
