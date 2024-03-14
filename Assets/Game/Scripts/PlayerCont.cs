//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Game/Scripts/PlayerCont.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerCont: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerCont()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerCont"",
    ""maps"": [
        {
            ""name"": ""PlayerControlz"",
            ""id"": ""de8067c2-59cd-4592-ad1d-abf524c2105f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""daac8fd7-50cf-4ab6-b739-7b91ded67544"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""bd246f39-f293-4bc0-ac82-b03c1e718217"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c2c749db-ecf1-4fdf-8b26-94a74bd5fe9b"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e3c9dda0-e37c-4bbf-9e86-9996b50bf24a"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fc49db42-703d-493a-ab01-622bc5733ece"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e55529aa-1a7d-4c55-b924-0597bf614284"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""3527a9cd-3e14-4271-b8ab-3000aa969085"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""70d0a436-b626-4eac-a53d-8b08793dd22c"",
                    ""path"": ""<HID::Logitech G920 Driving Force Racing Wheel for Xbox One>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2903fcc9-a43d-451e-913d-0b5246e60926"",
                    ""path"": ""<HID::Logitech G920 Driving Force Racing Wheel for Xbox One>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fabed666-47fb-46f8-ba62-1c594020aefd"",
                    ""path"": ""<HID::Logitech G920 Driving Force Racing Wheel for Xbox One>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1b564367-c303-4dad-af1b-962c58cdb41f"",
                    ""path"": ""<HID::Logitech G920 Driving Force Racing Wheel for Xbox One>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""CameraControll"",
            ""id"": ""63262a01-44bb-476d-9b35-65fba0f5791c"",
            ""actions"": [
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""bea46227-55da-4bef-bbae-9e01a0a7cc81"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""326d2122-256f-4194-86db-eabd1ac6dd3c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""59eb0400-8f61-4398-9afc-08375e05a4ad"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""aeec595b-ccb7-4ff7-a646-2d3e38ede9e8"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c804d39a-189a-41d5-8dba-09aca4f4c1c9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""35a05e3f-ed7d-4b17-ad69-f308765b4d4e"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerControlz
        m_PlayerControlz = asset.FindActionMap("PlayerControlz", throwIfNotFound: true);
        m_PlayerControlz_Move = m_PlayerControlz.FindAction("Move", throwIfNotFound: true);
        // CameraControll
        m_CameraControll = asset.FindActionMap("CameraControll", throwIfNotFound: true);
        m_CameraControll_Rotate = m_CameraControll.FindAction("Rotate", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerControlz
    private readonly InputActionMap m_PlayerControlz;
    private List<IPlayerControlzActions> m_PlayerControlzActionsCallbackInterfaces = new List<IPlayerControlzActions>();
    private readonly InputAction m_PlayerControlz_Move;
    public struct PlayerControlzActions
    {
        private @PlayerCont m_Wrapper;
        public PlayerControlzActions(@PlayerCont wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControlz_Move;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControlz; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlzActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerControlzActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerControlzActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerControlzActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
        }

        private void UnregisterCallbacks(IPlayerControlzActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
        }

        public void RemoveCallbacks(IPlayerControlzActions instance)
        {
            if (m_Wrapper.m_PlayerControlzActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerControlzActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerControlzActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerControlzActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerControlzActions @PlayerControlz => new PlayerControlzActions(this);

    // CameraControll
    private readonly InputActionMap m_CameraControll;
    private List<ICameraControllActions> m_CameraControllActionsCallbackInterfaces = new List<ICameraControllActions>();
    private readonly InputAction m_CameraControll_Rotate;
    public struct CameraControllActions
    {
        private @PlayerCont m_Wrapper;
        public CameraControllActions(@PlayerCont wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotate => m_Wrapper.m_CameraControll_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_CameraControll; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControllActions set) { return set.Get(); }
        public void AddCallbacks(ICameraControllActions instance)
        {
            if (instance == null || m_Wrapper.m_CameraControllActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CameraControllActionsCallbackInterfaces.Add(instance);
            @Rotate.started += instance.OnRotate;
            @Rotate.performed += instance.OnRotate;
            @Rotate.canceled += instance.OnRotate;
        }

        private void UnregisterCallbacks(ICameraControllActions instance)
        {
            @Rotate.started -= instance.OnRotate;
            @Rotate.performed -= instance.OnRotate;
            @Rotate.canceled -= instance.OnRotate;
        }

        public void RemoveCallbacks(ICameraControllActions instance)
        {
            if (m_Wrapper.m_CameraControllActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICameraControllActions instance)
        {
            foreach (var item in m_Wrapper.m_CameraControllActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CameraControllActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CameraControllActions @CameraControll => new CameraControllActions(this);
    public interface IPlayerControlzActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
    public interface ICameraControllActions
    {
        void OnRotate(InputAction.CallbackContext context);
    }
}
