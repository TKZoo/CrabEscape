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
        _inputActions.Hero.ThrowAttack.performed += OnThrowAttackAction;
        _inputActions.Hero.QuickSlotUse.performed += OnQuickSlotUse;
        _inputActions.Hero.NextItem.performed += OnNextItem;
        _inputActions.Hero.PerkUse.performed += OnPerkUse;
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
        _hero.Attack();            
    }
    
    public void OnQuickSlotUse(InputAction.CallbackContext context)
    {
        _hero.QuickSlotUse();            
    }
    
    private void OnNextItem(InputAction.CallbackContext obj)
    {
        _hero.NextItem();
    }
    
    private void OnPerkUse(InputAction.CallbackContext obj)
    {
        _hero.UsePerk();
    }

    public void OnThrowAttackAction(InputAction.CallbackContext context)
    {
        _hero.ThrowAttack ();
        
        // use for detect hold button for combo attack
        
        /*float val = context.ReadValue<float>();

        if (val >= InputSystem.settings.defaultHoldTime)
        {
            _hero.ThrowComboAttack();
        }
        else
        {
            _hero.ThrowAttack ();
        } */
    }
}
