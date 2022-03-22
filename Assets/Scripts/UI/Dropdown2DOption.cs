using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

namespace CustomControls
{
    public class Dropdown2DOption : MonoBehaviour
    {
        public bool IsInteractable = true;
        public bool IsSelected = false;
        public UnityAction<Dropdown2DOption> OnSelection;
        public SpriteRenderer Checkmark;
        public TextMeshPro Text;

        public SpriteRenderer Background;
        public Color32 NormalColor = new Color32(255, 255, 255, 255);
        public Color32 HighlightedColor = new Color32(245, 245, 245, 255);
        public Color32 PressedColor = new Color32(200, 200, 200, 255);
        public Color32 SelectedColor = new Color32(245, 245, 245, 255);
        public Color32 DisabledColor = new Color32(200, 200, 200, 128);

        void Start()
        {
            Checkmark.enabled = IsSelected;
        }

        private void OnMouseDown()
        {
            OnSelection?.Invoke(this);
            IsSelected = true;
            UpdateCheckmark();
        }

        public void UpdateCheckmark()
        {
            Checkmark.enabled = IsSelected;
        }

        public void SetCheckmark(bool value)
        {
            Checkmark.enabled = value;
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