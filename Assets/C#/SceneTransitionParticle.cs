using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionParticle : MonoBehaviour
{
    [SerializeField] private bool start;
    [SerializeField] private bool stop;
    [Header("���b��Ƀp�����[�^��ς���H")]
    [SerializeField] private float change_time;
    [Header("�J�ڑO��StartLifeTime")]
    [SerializeField] private AnimationCurve f_startLifeTime_aCurve;
    [Header("�J�ڑO��RateOverTime")]
    [SerializeField] private AnimationCurve f_rateOverTime_aCurve;
    [Header("�J�ڒ���StartLifeTime")]
    [SerializeField] private AnimationCurve l_startLifeTime_aCurve;
    [Header("�J�ڒ���RateOverTime")]
    [SerializeField] private AnimationCurve l_rateOverTime_aCurve;

    private ParticleSystem particle;
    private ParticleSystem.MainModule _mainModule;
    private ParticleSystem.EmissionModule _emissionModule;

    private bool isPlaying;
    private bool isHideScene;
    private float change_time_count;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying) {
            change_time_count += Time.deltaTime;
            if(!isHideScene && change_time < change_time_count)
            {
                isHideScene = true;
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

    //�p�����[�^�̕ύX
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

    //�Đ�
    public void PlayParticle()
    {
        if (!particle)
        {
            particle = GetComponent<ParticleSystem>();
            _mainModule = particle.main;
            _emissionModule = particle.emission;
        }

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
        isHideScene = false;
    }

    //����
    public void FinishParticle()
    {
        isPlaying = false;
        isHideScene = false;
        change_time_count = 0;
        _mainModule.loop = false;
    }

    //�V�[�����B��Ă��邩�Ԃ�
    public bool IsReturnHideScene()
    {
        return isHideScene;
    }
}
