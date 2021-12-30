using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;
    private HeroInputActions _inputActions;    

    private void Awake()
    {
        _inputActions = new HeroInputActions();
        _inputActions.Hero.HerolMovement.performed += OnHeroMovement;
        _inputActions.Hero.HerolMovement.canceled += OnHeroMovement;
        _inputActions.Hero.Interact.performed += OnInteract;      
        _inputActions.Hero.Attack.performed += OnAttackAction; 
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void OnHeroMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();        
        _hero.SetDirection(direction);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        _hero.Interact();            
    }
    
    public void OnAttackAction(InputAction.CallbackContext context)
    {
        _hero.Attack ();            
    }
}
