using UnityEngine;
using System.Collections;

public class FloatingController : MonoBehaviour
{
    [SerializeField] private AnimationCurve floatingAnimation;
    [SerializeField] private float moveSpeed, rotationSpeed;
    [SerializeField, Range(0.0f, 1.0f)] private float curve_diff;
    [SerializeField] private float curve_magni = 1.0f;
    [SerializeField] private bool animatePots = true;

    private Transform potTF;
    private Vector3 wantedPosition, rotationVector = new Vector3(0,1,0), startPostion;
    private float wantedHeight, startHeight, evalAnimCurve;
    private Coroutine floatPotsCoroutine;

    // Use this for initialization
    void Start()
    {
        potTF = transform;
        wantedPosition = potTF.localPosition;
        startHeight = potTF.localPosition.y;

        if (animatePots)
            floatPotsCoroutine = StartCoroutine(FloatPotsAnimation());
        evalAnimCurve = evalAnimCurve + curve_diff;
    }

    private IEnumerator FloatPotsAnimation()
    {
        while (animatePots)
        {
            if (evalAnimCurve < 1)
                evalAnimCurve += moveSpeed * Time.deltaTime;
            else if (evalAnimCurve >= 1)
                evalAnimCurve = 0;

            wantedPosition[1] = startHeight + floatingAnimation.Evaluate(evalAnimCurve) * curve_magni;

            potTF.localPosition = wantedPosition;

            if (rotationSpeed != 0)
                potTF.Rotate(rotationVector, Time.deltaTime * rotationSpeed);

            yield return new WaitForEndOfFrame();
        }
    }
}
