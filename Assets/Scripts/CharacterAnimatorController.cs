using UnityEngine;
using System;

public class CharacterAnimatorController : MonoBehaviour
{
    public Animator animator;
    private string currentAnimation;
    private PlayerAnimationState currentState;

    // Activa animaciones continuas (como caminar)

    private void Awake()
    {
        currentAnimation = "Idle";
    }
    public void SetAnimationState(string animationName, bool state)
    {

        // Evita cambiar el estado innecesariamente si ya está configurado
        if (animator.GetBool(animationName) == state) return;

        // Establece el estado de la animación
        animator.SetBool(animationName, state);

        // Actualiza la animación actual si está activa
        if (state) currentAnimation = animationName;

        foreach (PlayerAnimationState playerState in Enum.GetValues(typeof(PlayerAnimationState)))
        {
            if (playerState.ToString().Equals(animationName))
            {
                currentState = playerState;
            }
        }

        CheckAndSetIdle();

    }

    // Cambia de una animación puntual (como saltar o atacar)
    public void PlayAnimationOnce(string animationName)
    {
        // Resetea la animación anterior
        if (!string.IsNullOrEmpty(currentAnimation))
        {
            animator.ResetTrigger(currentAnimation);
        }

        // Activa la nueva animación puntual
        animator.SetTrigger(animationName);
        currentAnimation = animationName;
    }

    void CheckAndSetIdle()
    {
        bool enableIdle = true;

        // Recorre todos los parámetros del Animator
        for (int i = 0; i < animator.parameterCount; i++)
        {
            AnimatorControllerParameter parameter = animator.GetParameter(i);

            // Solo revisa los parámetros de tipo bool
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                // Si alguno está en true, no establece Idle
                if (animator.GetBool(parameter.name))
                {
                    enableIdle = false;
                    break;
                }
            }
        }

        // Si todos los parámetros están en false, establece Idle en true
        animator.SetBool("Idle", enableIdle);
    }

    public void DisableAllAnimations()
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            AnimatorControllerParameter parameter = animator.GetParameter(i);

            // Solo revisa los parámetros de tipo bool
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }

    public string ReturnCurrentAnimation()
    {
        return currentAnimation;
    }

    private void ChangeAnimationState(PlayerAnimationState newState)
    {
        if (currentState == newState) return; // Evita activar el mismo estado nuevamente

        // Desactiva todas las animaciones previas antes de cambiar el estado
        animator.ResetTrigger(currentState.ToString());

        // Activa la animación del nuevo estado
        animator.SetTrigger(newState.ToString());

        // Actualiza el estado actual
        currentState = newState;
    }

    public enum PlayerAnimationState
    {
        Idle,
        Walking,
        Crouching,
        Attacking1,
        Attacking2,
        Jumping,
        Dashing
    }
}
