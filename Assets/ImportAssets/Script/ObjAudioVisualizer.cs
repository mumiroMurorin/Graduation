using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objAudioVisualizer : MonoBehaviour
{
    [SerializeField] private AudioSpectrum spectrum;
    [SerializeField] private Transform[] cubes;
    [SerializeField] private float scale;
    [SerializeField, Range(0.0f, 1.0f)] private float saturation;
    [SerializeField, Range(0.0f, 1.0f)] private float brightness;
    [SerializeField, Range(0.0f, 1.0f)] private float hue_dif;
    [SerializeField, Range(0.0f, 1.0f)] private float hue_add;

    private MeshRenderer[] meshR;
    private float hue;

    private void Start()
    {
        meshR = new MeshRenderer[cubes.Length];
        hue = 0;
        for(int i = 0; i < cubes.Length; i++)
        {
            meshR[i] = cubes[i].gameObject.GetComponent<MeshRenderer>();
        }  
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            var cube = cubes[i];
            var localScale = cube.localScale;
            localScale.y = spectrum.Levels[i] * scale;
            cube.localScale = localScale;
            cube.localPosition = new Vector3(cube.localPosition.x, cube.localScale.y / 2f, cube.localPosition.z);

            Color col = Color.HSVToRGB((hue + hue_dif * i) % 1.0f, saturation, brightness);
            //mr.material.SetColor("_RimColor", col);
            meshR[i].material.SetColor("_Color", col);
        }
        hue = (hue + hue_add) % 1.0f;
    }
}