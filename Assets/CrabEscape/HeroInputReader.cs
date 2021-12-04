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
        var directionX = context.ReadValue<Vector2>().x;
        var directionY = context.ReadValue<Vector2>().y;
        _hero.SetDirection(directionX, directionY);
    }
}
