using TMPro;

using PiggyFence.UI;

using UnityEngine;
using UnityEngine.UI;

namespace PiggyFence.Managers
{
    // Class controls UI elements in project such as info panels and button. Class uses information from uiData to fill important information.
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
        [SerializeField] private UIData uiData;
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

        private void OnDestroy()
        {
            uiData = null;
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
                gridText.text = $"{uiData.gridSize} x {uiData.gridSize}";
                fenceText.text = $"{uiData.fenceLength}m ({uiData.fencePieceCount} piecies)";
                firebaseText.text = $"{uiData.dbDataLoadingTime.ToString("HH:mm:ss.f")}";
            }
        }
    }
}
