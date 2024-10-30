using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    [SerializeField]private Animator animator;
    private string currentAnimation;

    // Activa animaciones continuas (como caminar)
    public void SetAnimationState(string animationName, bool state)
    {
        // Evita cambiar el estado innecesariamente si ya est� configurado
        if (animator.GetBool(animationName) == state) return;

        // Establece el estado de la animaci�n
        animator.SetBool(animationName, state);

        // Actualiza la animaci�n actual si est� activa
        if (state) currentAnimation = animationName;
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

    public string ReturnCurrentAnimation()
    {
        return currentAnimation;
    }
}
