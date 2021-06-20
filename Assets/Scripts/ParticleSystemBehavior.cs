using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemBehavior : MonoBehaviour
{
    public bool active;
    public bool useBurst = false;
    public ParticleSystem particles;
    public ParticleSystem burstParticles;
    public MicrophoneInput microphoneInput;

    private void OnEnable() {
        microphoneInput = GetComponent<MicrophoneInput>();
        microphoneInput.OnAboveThreshold += OnAboveThreshold;
        microphoneInput.OnBelowThreshold += OnBelowThreshold;
    }

    private void OnAboveThreshold() {
        particles.Play();
    }

    private void OnBelowThreshold() {
        particles.Stop();
        if(useBurst)
            StartCoroutine(PlayBurstParticles());
    }

    IEnumerator PlayBurstParticles() {
        burstParticles.Play();
        yield return null;
        burstParticles.Stop();
    }
}
