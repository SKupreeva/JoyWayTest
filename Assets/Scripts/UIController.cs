using UnityEngine;
using UnityEngine.UI;

namespace UIControls
{
    // class controls map ui

    public class UIController : MonoBehaviour
    {
        [SerializeField] private Button _finishTurnBtn;
        [SerializeField] private Button _skipTurnBtn;
        [SerializeField] private RectTransform _winnerPanel;
        [SerializeField] private RectTransform _looserPanel;

        public Button FinishTurnBtn => _finishTurnBtn;
        public Button SkipTurnBtn => _skipTurnBtn;

        public void Setup()
        {
            OnPlayerTurnStarted();
            _winnerPanel.gameObject.SetActive(false);
            _looserPanel.gameObject.SetActive(false);
        }

        public void OnPlayerTurnStarted()
        {
            _finishTurnBtn.interactable = true;
            _skipTurnBtn.interactable = true;
        }

        public void OnPlayerTurnEnded()
        {
            _finishTurnBtn.interactable = false;
            _skipTurnBtn.interactable = false;
        }

        public void OnPlayerWon()
        {
            _winnerPanel.gameObject.SetActive(true);
        }

        public void OnPlayerLoose()
        {
            _looserPanel.gameObject.SetActive(true);
        }

        public void OnEffectUsed()
        {
            _skipTurnBtn.interactable = false;
        }
    }
}
