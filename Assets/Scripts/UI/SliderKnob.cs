using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CustomControls
{
    public class SliderKnob : MonoBehaviour
    {
        public UnityAction<float> OnValueDragging;
        public UnityAction<float> OnValueChanged;
        public SpriteRenderer Background;
        public float sliderWidth;
        public Vector3 startPos;
        public Vector3 endPos;

        void Awake()
        {
            sliderWidth = Background.size.x;
            startPos = new Vector3(transform.parent.position.x - (sliderWidth / 2), transform.parent.position.y, transform.position.z);
            endPos = new Vector3(transform.parent.position.x + (sliderWidth / 2), transform.parent.position.y, transform.position.z);
        }

        private void OnMouseDrag()
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.y = transform.position.y;
            worldPoint.z = transform.position.z;
            if (worldPoint.x < startPos.x)
                transform.position = startPos;
            else if (worldPoint.x > endPos.x)
                transform.position = endPos;
            else
                transform.position = worldPoint;

            float percentValue = Mathf.InverseLerp(startPos.x, endPos.x, transform.position.x);

            OnValueDragging?.Invoke(percentValue);
        }

        private void OnMouseUp()
        {
            float percentValue = Mathf.InverseLerp(startPos.x, endPos.x, transform.position.x);
            OnValueChanged?.Invoke(percentValue);
        }

    }
}