using UnityEngine;
using SportsBlitz.Events;
using Helper.Blake;

namespace SportsBlitz.Blake
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject _instructionUI;

        #region Events
        void OnEnable()
        {
            if (Blake.EventManager.Instance != null) Blake.EventManager.Instance.startGame += ShowInstructions;
            if (Blake.EventManager.Instance != null) Blake.EventManager.Instance.roundStart += HideInstructions;
            if (Blake.EventManager.Instance != null) Blake.EventManager.Instance.gameEnd += HideInstructions;
        }

        void OnDisable()
        {
            if (Blake.EventManager.Instance != null) Blake.EventManager.Instance.startGame -= ShowInstructions;
            if (Blake.EventManager.Instance != null) Blake.EventManager.Instance.roundStart -= HideInstructions;
            if (Blake.EventManager.Instance != null) Blake.EventManager.Instance.gameEnd -= HideInstructions;
        }
        #endregion


        public void ShowInstructions()
        {
            _instructionUI.SetActive(true);

        }

        public void HideInstructions()
        {
            _instructionUI.SetActive(false);
        }


    }
}