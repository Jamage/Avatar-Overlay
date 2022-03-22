using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomControls;

public class MicrophoneInput : MonoBehaviour {
    AudioClip microphoneInput;
    public bool microphoneInitialized;
    [Range(0f, 1f)]
    public float sensitivity;
    public float AudioLevel { get; private set; }
    float coroutineTime;

    public bool AboveThreshold = false;
    public System.Action OnAboveThreshold;
    public System.Action OnBelowThreshold;

    public GameObject settingsCanvas;
    public Slider2D sensitivitySlider;
    public TextMeshPro sensitivityValueText;

    public Dropdown2D micDropdown;
    public string microphoneLookup = "AT2020USB";
    private string selectedMicrophone;

    private void Awake() {
        AudioLevel = 0;

        sensitivitySlider.value = sensitivity;
        sensitivityValueText.text = sensitivity.ToString("F4");
        sensitivitySlider.OnValueChanged += OnSensitivityChanged;

        if (Microphone.devices.Length > 0) {
            micDropdown.SetDropdownOptions(Microphone.devices.ToList());
            micDropdown.OnValueChanged += OnMicrophoneChanged;
            
            if (String.IsNullOrEmpty(microphoneLookup))
                selectedMicrophone = Microphone.devices[0];
            else
                selectedMicrophone = Microphone.devices.FirstOrDefault(x => x.Contains(microphoneLookup));

            microphoneInput = Microphone.Start(selectedMicrophone, true, 60 * 60 - 1, 44100);
            microphoneInitialized = true;
        }

    }

    private void Start() {
        coroutineTime = Time.fixedDeltaTime * 24;
        StartCoroutine(CheckIfAboveThreshold());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            settingsCanvas.SetActive(settingsCanvas.activeInHierarchy == false);
        }
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
        int micPosition = Microphone.GetPosition(selectedMicrophone) - (dec + 1); // null means the first microphone
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

    private void OnSensitivityChanged()
    {
        sensitivity = sensitivitySlider.value;
        sensitivityValueText.text = sensitivity.ToString("F4");
    }

    private void OnMicrophoneChanged(string selectedMic)
    {
        StopAllCoroutines();
        Microphone.End(selectedMicrophone);
        selectedMicrophone = selectedMic;
        microphoneInput = Microphone.Start(selectedMicrophone, true, 60*60 - 1, 44100);
        StartCoroutine(CheckIfAboveThreshold());
    }
}
