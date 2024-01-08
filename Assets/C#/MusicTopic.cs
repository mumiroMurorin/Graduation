using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicTopic : MonoBehaviour
{
    [Header("洗濯時背景")]
    [SerializeField] private GameObject selectBack;

    private Image thumbneil_image;
    private TextMeshProUGUI title_tmp;
    private TextMeshProUGUI composer_tmp;
    private EventTrigger trigger;
    private int musicData_index;

    public void Init(MusicData md, int num)
    {
        thumbneil_image = this.transform.Find("Thumbneil").GetComponent<Image>();
        title_tmp = this.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        composer_tmp = this.transform.Find("Composer").GetComponent<TextMeshProUGUI>();
        trigger = this.GetComponentInChildren<EventTrigger>();

        musicData_index = num;
        thumbneil_image.sprite = md.thumbneil;
        title_tmp.text = md.title;
        composer_tmp.text = md.composer;
        selectBack.SetActive(false);
        this.gameObject.SetActive(true);

        GameCtrl gameCtrl = GameObject.Find("GameCtrl").GetComponent<GameCtrl>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((eventData) => gameCtrl.SetPlayMusicData(musicData_index));
        trigger.triggers.Add(entry);
    }

    //トピック選択
    public void SelectTopic()
    {
        selectBack.SetActive(true);
    }
}
