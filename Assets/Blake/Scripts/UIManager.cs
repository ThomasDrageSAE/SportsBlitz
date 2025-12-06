using UnityEngine;
using SportsBlitz.Events;
using Helper.Blake;

namespace SportsBlitz.Blake
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject _instructionUI;
        private Blake.EventManagerBlake _eventManagerBlake => Blake.EventManagerBlake.Instance;

        #region Events
        void OnEnable()
        {
            if (_eventManagerBlake != null) _eventManagerBlake.startGame += ShowInstructions;
            if (_eventManagerBlake != null) _eventManagerBlake.roundStart += HideInstructions;
            if (_eventManagerBlake != null) _eventManagerBlake.gameEnd += HideInstructions;
        }

        void OnDisable()
        {
            if (_eventManagerBlake != null) _eventManagerBlake.startGame -= ShowInstructions;
            if (_eventManagerBlake != null) _eventManagerBlake.roundStart -= HideInstructions;
            if (_eventManagerBlake != null) _eventManagerBlake.gameEnd -= HideInstructions;
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