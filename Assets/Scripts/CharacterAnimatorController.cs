using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    [SerializeField]private Animator animator;
    private string currentAnimation;

    // Activa animaciones continuas (como caminar)
    public void SetAnimationState(string animationName, bool state)
    {
        // Evita cambiar el estado innecesariamente si ya está configurado
        if (animator.GetBool(animationName) == state) return;

        // Establece el estado de la animación
        animator.SetBool(animationName, state);

        // Actualiza la animación actual si está activa
        if (state) currentAnimation = animationName;
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

    public string ReturnCurrentAnimation()
    {
        return currentAnimation;
    }
}
