using UnityEngine;

namespace SportsBlitz.Animation
{
    public class AnimationHandler : MonoBehaviour
    {
        [HideInInspector] public Animator animator;


        // INFO: Set Bool
        public void SetBool(string boolName, bool state)
        {
            if (animator != null) animator.SetBool(boolName, state);
        }

        // INFO: Activate Trigger
        public void ActivateTrigger(string triggerName)
        {
            if (animator != null) animator.SetTrigger(triggerName);
        }

        // INFO: Set Float
        public void SetFloat(string floatName, float value)
        {
            if (animator != null) animator.SetFloat(floatName, value);
        }

    }
}