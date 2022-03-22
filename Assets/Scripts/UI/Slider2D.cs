using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace CustomControls
{
    public class Slider2D : MonoBehaviour
    {
        public SliderKnob knob;
        public GameObject Background;
        public SpriteRenderer BackgroundRenderer;
        public GameObject Fill;
        public float minValue = 0;
        public float maxValue = 1;
        public float value = 0;
        float width;
        float height;
        float xPos;
        public UnityAction OnValueChanged;

        private void Awake()
        {
            width = BackgroundRenderer.size.x;
            height = BackgroundRenderer.size.y;

            float range = maxValue - minValue; //10
            float diff = value - minValue; //2.5
            float normalizedValue = diff / range; //.25
            xPos = width / 2;
            //knob.startPos.x = -width / 2;
            //knob.endPos.x = width / 2;
            knob.transform.localPosition = new Vector3(Mathf.Lerp(-xPos, xPos, normalizedValue), 0, 0);
            knob.OnValueChanged += OnValueChange;
            knob.OnValueDragging += OnValueDragging;
        }

        private void OnValueDragging(float knobPosition)
        {
            SetValue(knobPosition);
        }

        private void OnValueChange(float percentValue)
        {
            SetValue(percentValue);
        }

        private void SetValue(float percentValue)
        {
            value = Mathf.Lerp(minValue, maxValue, percentValue);
            OnValueChanged?.Invoke();
            Debug.Log($"Value: {value}");
        }

        private void OnEnable()
        {

        }

    }
}