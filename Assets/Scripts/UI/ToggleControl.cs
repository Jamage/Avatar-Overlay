using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomControls
{
    public class ToggleControl : MonoBehaviour
    {
        public bool IsInteractable = true;
        public bool IsChecked { get; private set; }

        public UnityAction<bool> OnValueChanged;
        public GameObject Checkmark;

        public SpriteRenderer Background;
        public Color32 NormalColor = new Color32(255, 255, 255, 255);
        public Color32 HighlightedColor = new Color32(245, 245, 245, 255);
        public Color32 PressedColor = new Color32(200, 200, 200, 255);
        public Color32 SelectedColor = new Color32(245, 245, 245, 255);
        public Color32 DisabledColor = new Color32(200, 200, 200, 128);


        private void Start()
        {
            UpdateCheckmark();
        }

        public void Setup(bool value)
        {
            IsChecked = value;
            UpdateCheckmark();
        }

        private void OnMouseDown()
        {
            if (IsInteractable == false)
                return;

            Background.color = PressedColor;
            Toggle();
            UpdateCheckmark();
        }

        private void Toggle()
        {
            IsChecked = !IsChecked;
            OnValueChanged?.Invoke(IsChecked);
        }

        private void UpdateCheckmark()
        {
            Checkmark.SetActive(IsChecked);
        }

        private void OnMouseUp()
        {
            if (IsInteractable)
                Background.color = NormalColor;
        }

        private void OnMouseOver()
        {
            if (IsInteractable)
                Background.color = HighlightedColor;
        }

        private void OnMouseExit()
        {
            if (IsInteractable && Input.GetMouseButton(0) == false)
                Background.color = NormalColor;
        }



    }
}