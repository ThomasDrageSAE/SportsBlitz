using UnityEngine;
using SportzBlitz.AnimationHandler;

[RequireComponent(typeof(AnimationHandler))]
public class AnimatorExample : MonoBehaviour
{
    [SerializeField] private Animator characterToAnimate;
    private AnimationHandler animationHandler;

    private void Start()
    {
        animationHandler = GetComponent<AnimationHandler>();
        if (animationHandler == null) return;
        if (characterToAnimate == null) return;

        animationHandler.animator = characterToAnimate; // INFO: Set the animator of the character we want to animate

        // INFO: Example usage
        animationHandler.SetBool("testBool", true);
    }

    // INFO: Reset the bool when disabled
    private void OnDisable()
    {
        if (animationHandler != null) return;
        animationHandler?.SetBool("testBool", false);
    }
    
}