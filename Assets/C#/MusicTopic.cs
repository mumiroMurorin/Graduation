using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicTopic : MonoBehaviour
{
    private Image thumbneil_image;
    private TextMeshProUGUI title_tmp;
    private TextMeshProUGUI composer_tmp;
    [HideInInspector] public int musicData_index;

    public void Init(MusicData md, int num)
    {
        thumbneil_image = this.transform.Find("Thumbneil").GetComponent<Image>();
        title_tmp = this.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        composer_tmp = this.transform.Find("Composer").GetComponent<TextMeshProUGUI>();

        musicData_index = num;
        thumbneil_image.sprite = md.thumbneil;
        title_tmp.text = md.title;
        composer_tmp.text = md.composer;
    }

}
