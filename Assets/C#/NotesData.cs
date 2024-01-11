using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//通常ノートのクラス(新ノーツ追加時は参考にするといいよ)
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

//各種ノートのリストをここに格納(ややこしいので分からんかったら聞いて)
public class NotesBlock
{
    public float time;
    //通常ノートのリスト
    public List<GeneralNote> general_list;
    //以下に新規ノートのリストを宣言
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
    List<NotesBlock> score;   //譜面データ

    public NotesData()
    {
        score = new List<NotesBlock>();
    }

    //譜面データに通常ノートを追加
    public bool AddGeneralNote(GeneralNote g)
    {
        //既に譜面データに同じtimeのデータがある場合、挿入
        int index = ReturnExistTimeIndex(g.time);
        if (index != -1) { score[index].general_list.Add(g); }
        else    //上記を満たさなかった場合、新たにそのtimeのNotesBlockを生成して譜面データに追加
        {
            NotesBlock notesBlock = ReturnVoidNotesBlock();
            notesBlock.time = g.time;
            notesBlock.general_list = new List<GeneralNote> { g };
            score.Add(notesBlock);
        }

        return true;
    }

    //譜面データに特殊ノートを追加
    public bool AddButtonANote(ButtonANote a)
    {
        //既に譜面データに同じtimeのデータがある場合、挿入
        int index = ReturnExistTimeIndex(a.time);
        if (index != -1) { score[index].buttonA_list.Add(a); }
        else    //上記を満たさなかった場合、新たにそのtimeのNotesBlockを生成して譜面データに追加
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
        //既に譜面データに同じtimeのデータがある場合、挿入
        int index = ReturnExistTimeIndex(b.time);
        if (index != -1) { score[index].buttonB_list.Add(b); }
        else    //上記を満たさなかった場合、新たにそのtimeのNotesBlockを生成して譜面データに追加
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
        //既に譜面データに同じtimeのデータがある場合、挿入
        int index = ReturnExistTimeIndex(x.time);
        if (index != -1) { score[index].buttonX_list.Add(x); }
        else    //上記を満たさなかった場合、新たにそのtimeのNotesBlockを生成して譜面データに追加
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
        //既に譜面データに同じtimeのデータがある場合、挿入
        int index = ReturnExistTimeIndex(y.time);
        if (index != -1) { score[index].buttonY_list.Add(y); }
        else    //上記を満たさなかった場合、新たにそのtimeのNotesBlockを生成して譜面データに追加
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
        //既に譜面データに同じtimeのデータがある場合、挿入
        int index = ReturnExistTimeIndex(l.time);
        if (index != -1) { score[index].buttonL_list.Add(l); }
        else    //上記を満たさなかった場合、新たにそのtimeのNotesBlockを生成して譜面データに追加
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
        //既に譜面データに同じtimeのデータがある場合、挿入
        int index = ReturnExistTimeIndex(r.time);
        if (index != -1) { score[index].buttonR_list.Add(r); }
        else    //上記を満たさなかった場合、新たにそのtimeのNotesBlockを生成して譜面データに追加
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
        //既に譜面データに同じtimeのデータがある場合、挿入
        int index = ReturnExistTimeIndex(e.time);
        if (index != -1) { score[index].escape_list.Add(e); }
        else    //上記を満たさなかった場合、新たにそのtimeのNotesBlockを生成して譜面データに追加
        {
            NotesBlock notesBlock = ReturnVoidNotesBlock();
            notesBlock.time = e.time;
            notesBlock.escape_list = new List<EscapeNote> { e };
            score.Add(notesBlock);
        }

        return true;
    }

    //初期化されたNotesBlockを返す
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

    //引数のtimeのNotesBlockが譜面データに存在した場合、そのIndexを返す
    private int ReturnExistTimeIndex(float time)
    {
        for (int i = 0; i < score.Count; i++)
        {
            if (score[i].time == time) { return i; }
        }

        return -1;
    }

    //譜面データを返す
    public List<NotesBlock> ReturnScoreData()
    {
        return score;
    }
}
