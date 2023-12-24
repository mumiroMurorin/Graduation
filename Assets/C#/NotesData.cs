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
    List<NotesBlock> score;   //譜面データ

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

    //初期化されたNotesBlockを返す
    private NotesBlock ReturnVoidNotesBlock()
    {
        return new NotesBlock
        {
            time = 0,
            general_list = new List<GeneralNote>(),
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
