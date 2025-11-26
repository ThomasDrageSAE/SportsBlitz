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
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.startGame += ShowInstructions;
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.roundStart += HideInstructions;
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.gameEnd += HideInstructions;
        }

        void OnDisable()
        {
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.startGame -= ShowInstructions;
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.roundStart -= HideInstructions;
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.gameEnd -= HideInstructions;
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