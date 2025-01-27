//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/UserInput.inputactions
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

public partial class @UserInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @UserInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UserInput"",
    ""maps"": [
        {
            ""name"": ""MobileTouch"",
            ""id"": ""8140958d-cad8-438b-89e2-170993e4becd"",
            ""actions"": [
                {
                    ""name"": ""Tap"",
                    ""type"": ""Button"",
                    ""id"": ""3371ac11-f6d0-4a70-b0d6-0611e0173fd8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c5ac6588-875b-45ec-933c-1880466517fd"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99d07e27-e4da-4d97-98d1-0d2ddcdd2da8"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MobileTouch
        m_MobileTouch = asset.FindActionMap("MobileTouch", throwIfNotFound: true);
        m_MobileTouch_Tap = m_MobileTouch.FindAction("Tap", throwIfNotFound: true);
    }

    ~@UserInput()
    {
        UnityEngine.Debug.Assert(!m_MobileTouch.enabled, "This will cause a leak and performance issues, UserInput.MobileTouch.Disable() has not been called.");
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

    // MobileTouch
    private readonly InputActionMap m_MobileTouch;
    private List<IMobileTouchActions> m_MobileTouchActionsCallbackInterfaces = new List<IMobileTouchActions>();
    private readonly InputAction m_MobileTouch_Tap;
    public struct MobileTouchActions
    {
        private @UserInput m_Wrapper;
        public MobileTouchActions(@UserInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Tap => m_Wrapper.m_MobileTouch_Tap;
        public InputActionMap Get() { return m_Wrapper.m_MobileTouch; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MobileTouchActions set) { return set.Get(); }
        public void AddCallbacks(IMobileTouchActions instance)
        {
            if (instance == null || m_Wrapper.m_MobileTouchActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MobileTouchActionsCallbackInterfaces.Add(instance);
            @Tap.started += instance.OnTap;
            @Tap.performed += instance.OnTap;
            @Tap.canceled += instance.OnTap;
        }

        private void UnregisterCallbacks(IMobileTouchActions instance)
        {
            @Tap.started -= instance.OnTap;
            @Tap.performed -= instance.OnTap;
            @Tap.canceled -= instance.OnTap;
        }

        public void RemoveCallbacks(IMobileTouchActions instance)
        {
            if (m_Wrapper.m_MobileTouchActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMobileTouchActions instance)
        {
            foreach (var item in m_Wrapper.m_MobileTouchActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MobileTouchActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MobileTouchActions @MobileTouch => new MobileTouchActions(this);
    public interface IMobileTouchActions
    {
        void OnTap(InputAction.CallbackContext context);
    }
}
