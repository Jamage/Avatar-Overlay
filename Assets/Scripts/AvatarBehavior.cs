using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarBehavior : MonoBehaviour
{
    public MicrophoneInput microphoneInput;
    public GameObject IdleImage;
    public GameObject TalkingImage;

    private void OnEnable() {
        microphoneInput = GetComponent<MicrophoneInput>();
        microphoneInput.OnAboveThreshold += OnAboveThreshold;
        microphoneInput.OnBelowThreshold += OnBelowThreshold;
    }
 
    private void OnAboveThreshold() {
        TalkingImage.SetActive(true);
        IdleImage.SetActive(false);
    }

    private void OnBelowThreshold() {
        TalkingImage.SetActive(false);
        IdleImage.SetActive(true);
    }
}
