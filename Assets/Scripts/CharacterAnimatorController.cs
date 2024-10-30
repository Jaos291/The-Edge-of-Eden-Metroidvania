using UnityEngine;
using System;

public class CharacterAnimatorController : MonoBehaviour
{
    [SerializeField]private Animator animator;
    private string currentAnimation;

    public event Action OnFinish;

    // Activa animaciones continuas (como caminar)
    public void SetAnimationState(string animationName, bool state)
    {

        // Evita cambiar el estado innecesariamente si ya est� configurado
        if (animator.GetBool(animationName) == state) return;

        // Establece el estado de la animaci�n
        animator.SetBool(animationName, state);

        // Actualiza la animaci�n actual si est� activa
        if (state) currentAnimation = animationName;

        CheckAndSetIdle();

    }

    // Cambia de una animaci�n puntual (como saltar o atacar)
    public void PlayAnimationOnce(string animationName)
    {
        // Resetea la animaci�n anterior
        if (!string.IsNullOrEmpty(currentAnimation))
        {
            animator.ResetTrigger(currentAnimation);
        }

        // Activa la nueva animaci�n puntual
        animator.SetTrigger(animationName);
        currentAnimation = animationName;
    }

    void CheckAndSetIdle()
    {
        bool enableIdle = true;

        // Recorre todos los par�metros del Animator
        for (int i = 0; i < animator.parameterCount; i++)
        {
            AnimatorControllerParameter parameter = animator.GetParameter(i);

            // Solo revisa los par�metros de tipo bool
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                // Si alguno est� en true, no establece Idle
                if (animator.GetBool(parameter.name))
                {
                    enableIdle = false;
                    break;
                }
            }
        }

        // Si todos los par�metros est�n en false, establece Idle en true
        animator.SetBool("Idle", enableIdle);
    }

    public string ReturnCurrentAnimation()
    {
        return currentAnimation;
    }
}
