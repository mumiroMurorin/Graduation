using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform ui_rect;

    void Start()
    {
        if (cam) { canvas.worldCamera = cam; }
        else { cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); }
    }

    void Update()
    {
        // ©g‚ÌŒü‚«‚ğƒJƒƒ‰‚ÉŒü‚¯‚é
        if (cam && ui_rect) { ui_rect.LookAt(cam.transform); }
    }
}