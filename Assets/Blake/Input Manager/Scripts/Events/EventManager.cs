using System;
using Helper.Blake;
using UnityEngine;

namespace SportsBlitz.Events
{
    public class EventManager : Singleton<EventManager>
    {
        public delegate void DelegateNoArgs();
        public delegate void DelegateOneArg<T>(T firstArg);
        public delegate void DelegateTwoArgs<T1, T2>(T1 firstArg, T2 secondArg);

        #region User Input Events
        public DelegateNoArgs incorrectKeyInput;
        public DelegateOneArg<string> correctKeyInput;
        public DelegateNoArgs correctKeySequence;
        #endregion

    }
}