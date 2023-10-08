using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PiggyFence.UI;

namespace PiggyFence.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        private const string HIDE_DEBUG_TEXT = "Hide Debug Info";
        private const string SHOW_DEBUG_TEXT = "Show Debug Info";

        [Header("Panels")]
        [SerializeField] private GameObject gridPanel;
        [SerializeField] private GameObject fencePanel;
        [SerializeField] private GameObject firebasePanel;
        [Space]
        [Header("Button")]
        [SerializeField] private Button uiButton;
        [Space]
        [Header("UIData")]
        [SerializeField] private UIData uiInfo;
        [Space]
        [Header("Text Fields")]
        [SerializeField] private TMP_Text gridText;
        [SerializeField] private TMP_Text fenceText;
        [SerializeField] private TMP_Text firebaseText;

        private bool showPanels;

        private void Start()
        {
            showPanels = false;
            uiButton.onClick.AddListener(ShowHidePanels);
            uiButton.onClick.AddListener(ChangeButtonText);
            uiButton.onClick.AddListener(ShowData);
        }

        private void ShowHidePanels()
        {
            showPanels = !showPanels;

            gridPanel.SetActive(showPanels);
            fencePanel.SetActive(showPanels);
            firebasePanel.SetActive(showPanels);
        }

        private void ChangeButtonText()
        {
            uiButton.GetComponentInChildren<TMP_Text>().text = showPanels ? HIDE_DEBUG_TEXT : SHOW_DEBUG_TEXT;
        }

        private void ShowData()
        {
            if (showPanels)
            {
                gridText.text = $"{uiInfo.gridSize} x {uiInfo.gridSize}";
                fenceText.text = $"{uiInfo.fenceLength}m ({uiInfo.fencePieceCount} piecies)";
                firebaseText.text = $"{uiInfo.dbDataLoadingTime.ToString("HH:mm:ss.f")}";
            }
        }
    }
}
