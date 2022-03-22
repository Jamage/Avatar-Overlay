using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace CustomControls
{
    public class Dropdown2D : MonoBehaviour
    {
        public bool IsInteractable = true;
        public string selectedOption = "";
        public int selectedIndex = 0;
        public TextMeshPro selectedText;
        public UnityAction<string> OnValueChanged;
        [SerializeField] List<string> dropdownOptions;

        public Dropdown2DOption optionPrefab;
        public Transform prefabParent;
        public List<Dropdown2DOption> optionGameObjects;

        public SpriteRenderer Background;
        public Color32 NormalColor = new Color32(255, 255, 255, 255);
        public Color32 HighlightedColor = new Color32(245, 245, 245, 255);
        public Color32 PressedColor = new Color32(200, 200, 200, 255);
        public Color32 SelectedColor = new Color32(245, 245, 245, 255);
        public Color32 DisabledColor = new Color32(200, 200, 200, 128);

        private bool IsShowingOptions = false;

        public void SetDropdownOptions(List<string> options)
        {
            dropdownOptions = options;
            if (String.IsNullOrEmpty(selectedOption))
                selectedOption = options[0];
            else
                selectedOption = options.FirstOrDefault(x => x.Contains(selectedOption)) ?? options[0];
            selectedText.text = selectedOption;

            OnValueChanged?.Invoke(selectedOption);
        }

        void Start()
        {
            if (dropdownOptions.Count > 0)
                Initialize(dropdownOptions);
        }

        private void Initialize(List<string> options)
        {
            foreach (string option in options)
            {
                Dropdown2DOption newOption = Instantiate(optionPrefab, prefabParent).GetComponent<Dropdown2DOption>();
                newOption.Text.text = option;
                newOption.OnSelection += OnSelection;
                if (newOption.Text.text == selectedOption)
                    newOption.IsSelected = true;
                optionGameObjects.Add(newOption);
            }

            SetDropdownOptions(options);
        }

        private void OnSelection(Dropdown2DOption selected)
        {
            foreach (Dropdown2DOption option in optionGameObjects)
            {
                if (option == selected)
                    continue;
                option.IsSelected = false;
                option.SetCheckmark(false);
            }

            selectedIndex = optionGameObjects.IndexOf(selected);
            selectedOption = selected.Text.text;
            selectedText.text = selectedOption;
            ToggleDropdownShown();
            OnValueChanged?.Invoke(selectedOption);
        }

        private void OnMouseDown()
        {
            ToggleDropdownShown();
        }

        private void ToggleDropdownShown()
        {
            IsShowingOptions = !IsShowingOptions;
            SetOptionsActive(IsShowingOptions);
        }

        private void SetOptionsActive(bool isShowing)
        {
            foreach (Dropdown2DOption optionGO in optionGameObjects)
            {
                optionGO.gameObject.SetActive(isShowing);
            }
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