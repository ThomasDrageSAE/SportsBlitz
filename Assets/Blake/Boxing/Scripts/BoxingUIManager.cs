using UnityEngine;
using SportsBlitz.Events;
using Helper.Blake;

namespace SportsBlitz.Blake.Boxing
{
    public class BoxingUIManager : Singleton<BoxingUIManager>
    {
        [SerializeField] private GameObject _instructionUI;

        #region Events
        void OnEnable()
        {
            if (BoxingEventManager.Instance != null) BoxingEventManager.Instance.startGame += ShowInstructions;
            if (BoxingEventManager.Instance != null) BoxingEventManager.Instance.roundStart += HideInstructions;
            if (BoxingEventManager.Instance != null) BoxingEventManager.Instance.gameEnd += HideInstructions;
        }

        void OnDisable()
        {
            if (BoxingEventManager.Instance != null) BoxingEventManager.Instance.startGame -= ShowInstructions;
            if (BoxingEventManager.Instance != null) BoxingEventManager.Instance.roundStart -= HideInstructions;
            if (BoxingEventManager.Instance != null) BoxingEventManager.Instance.gameEnd -= HideInstructions;
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