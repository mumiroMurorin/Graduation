using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionParticle : MonoBehaviour
{
    [SerializeField] private bool start;
    [SerializeField] private bool stop;
    [Header("何秒後にパラメータを変える？")]
    [SerializeField] private float change_time;
    [Header("遷移前のStartLifeTime")]
    [SerializeField] private AnimationCurve f_startLifeTime_aCurve;
    [Header("遷移前のRateOverTime")]
    [SerializeField] private AnimationCurve f_rateOverTime_aCurve;
    [Header("遷移中のStartLifeTime")]
    [SerializeField] private AnimationCurve l_startLifeTime_aCurve;
    [Header("遷移中のRateOverTime")]
    [SerializeField] private AnimationCurve l_rateOverTime_aCurve;

    private ParticleSystem particle;
    private ParticleSystem.MainModule _mainModule;
    private ParticleSystem.EmissionModule _emissionModule;

    private bool isPlaying;
    private float change_time_count;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        _mainModule = particle.main;
        _emissionModule = particle.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying) {
            change_time_count += Time.deltaTime;
            if(change_time < change_time_count)
            {
                change_time_count = 0;
                SetParameter();
            }
        }

        if (start)
        {
            PlayParticle();
            start = false;
        }

        if (stop)
        {
            FinishParticle();
            stop = false;
        }
    }

    //パラメータの変更
    private void SetParameter()
    {
        //StartLifeTime
        ParticleSystem.MinMaxCurve minMaxCurve = _mainModule.startLifetime;
        minMaxCurve.curve = l_startLifeTime_aCurve;
        _mainModule.startLifetime = minMaxCurve;
        //RateOverTime
        minMaxCurve = _emissionModule.rateOverTime;
        minMaxCurve.curve = l_rateOverTime_aCurve;
        _emissionModule.rateOverTime = minMaxCurve;
    }

    //再生
    public void PlayParticle()
    {
        _mainModule.loop = true;
        //StartLifeTime
        ParticleSystem.MinMaxCurve minMaxCurve = _mainModule.startLifetime;
        minMaxCurve.curve = f_startLifeTime_aCurve;
        _mainModule.startLifetime = minMaxCurve;
        //RateOverTime
        minMaxCurve = _emissionModule.rateOverTime;
        minMaxCurve.curve = f_rateOverTime_aCurve;
        _emissionModule.rateOverTime = minMaxCurve;
        particle.Play();
        isPlaying = true;
    }

    //鎮火
    public void FinishParticle()
    {
        isPlaying = false;
        change_time_count = 0;
        _mainModule.loop = false;
    }
}
