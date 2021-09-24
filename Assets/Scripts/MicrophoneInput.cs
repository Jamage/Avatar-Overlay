using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityValueText;
    public TMP_Dropdown microphoneDropdown;
    private string selectedMicrophone;

    private void Awake() {
        AudioLevel = 0;

        sensitivitySlider.value = sensitivity;
        sensitivityValueText.text = sensitivity.ToString("F4");
        sensitivitySlider.onValueChanged.AddListener(delegate { OnSensitivityChanged(); });

        if (Microphone.devices.Length > 0) {
            selectedMicrophone = Microphone.devices[0];
            microphoneInput = Microphone.Start(selectedMicrophone, true, 999, 44100);
            microphoneInitialized = true;
            microphoneDropdown.AddOptions(Microphone.devices.ToList());
        }

        microphoneDropdown.onValueChanged.AddListener(delegate { OnMicrophoneChanged(); });
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
            settingsCanvas.GetComponentInChildren<TMP_Dropdown>().Select();
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

    private void OnMicrophoneChanged()
    {
        StopAllCoroutines();
        Microphone.End(selectedMicrophone);
        int microphoneIndex = microphoneDropdown.value;
        selectedMicrophone = microphoneDropdown.options[microphoneIndex].text;
        microphoneInput = Microphone.Start(selectedMicrophone, true, 999, 44100);
        StartCoroutine(CheckIfAboveThreshold());
    }
}
