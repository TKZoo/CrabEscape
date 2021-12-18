// GENERATED AUTOMATICALLY FROM 'Assets/UserInput/HeroInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @HeroInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset Asset { get; }
    public @HeroInputActions()
    {
        Asset = InputActionAsset.FromJson(@"{
    ""name"": ""HeroInputActions"",
    ""maps"": [
        {
            ""name"": ""Hero"",
            ""id"": ""39f384e9-6a39-4135-a3f3-c203120ba0e8"",
            ""actions"": [
                {
                    ""name"": ""HerolMovement"",
                    ""type"": ""Value"",
                    ""id"": ""084e7b05-0f97-40e2-9395-314b5522d9b0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""8bceb215-308a-4dc3-879f-e864d2d81440"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""39ebad8f-f383-48e5-b782-492568a79c8a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HerolMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""19956438-ad2f-406c-b203-ac08dbbbef41"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HerolMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""2bf25ba8-d364-4a44-8a5d-33cdadc06d08"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HerolMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""e4b23b0b-8b2c-4604-8a92-a491c2244668"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HerolMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""583ff2db-3f7d-400b-a548-3a1405bdb6e9"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HerolMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c49c92f3-dceb-45db-bb01-796364abbda2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Hero
        _mHero = Asset.FindActionMap("Hero", throwIfNotFound: true);
        _mHeroHerolMovement = _mHero.FindAction("HerolMovement", throwIfNotFound: true);
        _mHeroInteract = _mHero.FindAction("Interact", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(Asset);
    }

    public InputBinding? bindingMask
    {
        get => Asset.bindingMask;
        set => Asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => Asset.devices;
        set => Asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => Asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return Asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return Asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        Asset.Enable();
    }

    public void Disable()
    {
        Asset.Disable();
    }

    // Hero
    private readonly InputActionMap _mHero;
    private IHeroActions _mHeroActionsCallbackInterface;
    private readonly InputAction _mHeroHerolMovement;
    private readonly InputAction _mHeroInteract;
    public struct HeroActions
    {
        private @HeroInputActions _mWrapper;
        public HeroActions(@HeroInputActions wrapper) { _mWrapper = wrapper; }
        public InputAction @HerolMovement => _mWrapper._mHeroHerolMovement;
        public InputAction @Interact => _mWrapper._mHeroInteract;
        public InputActionMap Get() { return _mWrapper._mHero; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool Enabled => Get().enabled;
        public static implicit operator InputActionMap(HeroActions set) { return set.Get(); }
        public void SetCallbacks(IHeroActions instance)
        {
            if (_mWrapper._mHeroActionsCallbackInterface != null)
            {
                @HerolMovement.started -= _mWrapper._mHeroActionsCallbackInterface.OnHerolMovement;
                @HerolMovement.performed -= _mWrapper._mHeroActionsCallbackInterface.OnHerolMovement;
                @HerolMovement.canceled -= _mWrapper._mHeroActionsCallbackInterface.OnHerolMovement;
                @Interact.started -= _mWrapper._mHeroActionsCallbackInterface.OnInteract;
                @Interact.performed -= _mWrapper._mHeroActionsCallbackInterface.OnInteract;
                @Interact.canceled -= _mWrapper._mHeroActionsCallbackInterface.OnInteract;
            }
            _mWrapper._mHeroActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HerolMovement.started += instance.OnHerolMovement;
                @HerolMovement.performed += instance.OnHerolMovement;
                @HerolMovement.canceled += instance.OnHerolMovement;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public HeroActions @Hero => new HeroActions(this);
    public interface IHeroActions
    {
        void OnHerolMovement(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
