using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneInput : MonoBehaviour {
    AudioClip microphoneInput;
    public bool microphoneInitialized;
    [Range(0f, 1f)]
    public float sensitivity;
    [SerializeField]
    public float AudioLevel { get; private set; }
    float coroutineTime;

    public bool AboveThreshold = false;
    public System.Action OnAboveThreshold;
    public System.Action OnBelowThreshold;

    private void Awake() {
        AudioLevel = 0;

        if (Microphone.devices.Length > 0) {
            microphoneInput = Microphone.Start(Microphone.devices[0], true, 999, 44100);
            microphoneInitialized = true;
        }
    }

    private void Start() {
        coroutineTime = Time.fixedDeltaTime * 24;
        StartCoroutine(CheckIfAboveThreshold());
    }

    IEnumerator CheckIfAboveThreshold() {

        do {
            yield return new WaitForSeconds(coroutineTime);
            SetLevelFromAudioPeak();
        }
        while (AudioLevel < sensitivity);

        if (OnAboveThreshold != null) {
            AboveThreshold = true;
            OnAboveThreshold();
        }

        StartCoroutine(CheckIfBelowThreshold());
    }

    IEnumerator CheckIfBelowThreshold() {

        do {
            yield return new WaitForSeconds(coroutineTime);
            SetLevelFromAudioPeak();
        }
        while (AudioLevel > sensitivity);

        if (OnBelowThreshold != null) {
            AboveThreshold = false;
            OnBelowThreshold();
        }

        StartCoroutine(CheckIfAboveThreshold());
    }

    private void SetLevelFromAudioPeak() {
        //get mic volume
        int dec = 128;
        float[] waveData = new float[dec];
        int micPosition = Microphone.GetPosition(null) - (dec + 1); // null means the first microphone
        microphoneInput.GetData(waveData, micPosition);

        // Getting a peak on the last 128 samples
        float levelMax = 0;
        for (int i = 0; i < dec; i++) {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak) {
                levelMax = wavePeak;
            }
        }

        AudioLevel = Mathf.Sqrt(Mathf.Sqrt(levelMax));
    }
}
