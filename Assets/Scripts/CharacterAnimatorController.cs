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

        // Evita cambiar el estado innecesariamente si ya está configurado
        if (animator.GetBool(animationName) == state) return;

        // Establece el estado de la animación
        animator.SetBool(animationName, state);

        // Actualiza la animación actual si está activa
        if (state) currentAnimation = animationName;

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

    public string ReturnCurrentAnimation()
    {
        return currentAnimation;
    }
}
