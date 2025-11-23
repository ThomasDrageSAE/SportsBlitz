using System;
using Helper.Blake;
using UnityEngine;

namespace SportsBlitz.Blake
{
    public class EventManager : Singleton<EventManager>
    {

        public delegate void DelegateNoArgs();
        public delegate void DelegateOneArg<T>(T arg);
        public delegate void DelegateTwoArgs<T1, T2>(T1 arg1, T2 arg2);

        #region User Input Events
        public DelegateNoArgs gameEnd; // INFO: Game has ended
        public DelegateNoArgs gameLose; // INFO: Player lost the game
        public DelegateNoArgs gameWon; // INFO: Player won the game
        public DelegateNoArgs Wongame; // INFO: Player won the game
        public DelegateNoArgs startGame; // INFO: Scene loaded game starting
        public DelegateNoArgs roundStart; // INFO: Game loaded and  round starting

        #region Timer Events
        public DelegateOneArg<float> startTimer; // INFO: Start round timer
        public DelegateNoArgs timeOver; // INFO: Timer has run out
        #endregion

        #region UI Events
        public DelegateOneArg<float> OnUpdateTimerText; // INFO: Show instruction UI
        #endregion

        #endregion

    }
}