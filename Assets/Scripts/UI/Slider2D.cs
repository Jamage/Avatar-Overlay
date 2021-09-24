using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    public UnityAction OnValueChanged;

    private void Awake()
    {
        width = BackgroundRenderer.size.x;
        height = BackgroundRenderer.size.y;
        
        float range = maxValue - minValue;
        float diff = value - minValue;
        float normalizedValue = diff / range;
        knob.transform.position =  new Vector3(Mathf.Lerp(-width / 2, width / 2,  normalizedValue), 0, transform.localPosition.z);
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
