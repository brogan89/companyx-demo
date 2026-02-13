using Unity.Netcode;
using UnityEngine;

public class LightSwitch : NetworkBehaviour
{
    private static readonly int IsOn = Animator.StringToHash("IsOn");
    [SerializeField] private Light _light;
    
    private Animator _animator;
    private readonly NetworkVariable<bool> _isOn = new(true);

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        GetComponent<Renderer>().material.color = Color.black;
        
        _isOn.OnValueChanged += OnIsOnValueChanged;
    }

    protected override void OnNetworkPostSpawn()
    {
        base.OnNetworkPostSpawn();
        
        // ensure sync state
        OnIsOnValueChanged(_isOn.Value, _isOn.Value);
    }

    private void OnIsOnValueChanged(bool previousValue, bool newValue)
    {
        _light.enabled = newValue;
        _animator.SetBool(IsOn, newValue);
    }

    [Rpc(SendTo.Server)]
    public void ToggleSwitchServerRpc()
    {
        if (!IsServer)
        {
            Debug.LogError("Should not be able to interact with light switch on client");
            return;
        }
        
        _isOn.Value = !_isOn.Value;
    }
}
