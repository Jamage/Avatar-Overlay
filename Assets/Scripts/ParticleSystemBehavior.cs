using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomControls;

public class ParticleSystemBehavior : MonoBehaviour
{
    public bool active;
    public ToggleControl particleToggle;
    public bool useBurst = false;
    public ParticleSystem yipYapParticles;
    public ParticleSystem burstParticles;
    public MicrophoneInput microphoneInput;

    private void OnEnable() {
        microphoneInput = GetComponent<MicrophoneInput>();
        particleToggle.OnValueChanged += OnToggle;
        particleToggle.Setup(active);
        microphoneInput.OnAboveThreshold += OnAboveThreshold;
        microphoneInput.OnBelowThreshold += OnBelowThreshold;
    }

    private void OnToggle(bool toggleValue)
    {
        active = toggleValue;
    }

    private void OnAboveThreshold() {
        if(active)
            yipYapParticles.Play();
    }

    private void OnBelowThreshold() {
        yipYapParticles.Stop();
        if(useBurst && active)
            StartCoroutine(PlayBurstParticles());
    }

    IEnumerator PlayBurstParticles() {
        burstParticles.Play();
        yield return null;
        burstParticles.Stop();
    }
}
