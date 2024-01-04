using UnityEngine;

public class objAudioVisualizer : MonoBehaviour
{
    public AudioSpectrum spectrum;
    public Transform[] cubes;
    public float scale;

    private void Update()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            var cube = cubes[i];
            var localScale = cube.localScale;
            localScale.y = spectrum.Levels[i] * scale;
            cube.localScale = localScale;
            cube.localPosition = new Vector3(cube.localPosition.x, cube.localScale.y / 2f, cube.localPosition.z);
        }
    }
}