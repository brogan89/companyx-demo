using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : NetworkBehaviour
{
    private LightSwitch _lightSwitch;
    private TextMeshProUGUI _inputPrompt;

    private void Awake()
    {
        _inputPrompt = GameObject.FindWithTag("InputPrompt").GetComponent<TextMeshProUGUI>();
        _lightSwitch = GameObject.FindWithTag("LightSwitch").GetComponent<LightSwitch>();
    }

    private void Update()
    {
        if (!IsOwner || !IsSpawned)
            return;
        
        var isInRange = IsInRangeOfSwitch();
        _inputPrompt.enabled = isInRange;

        if (isInRange && Keyboard.current.spaceKey.wasPressedThisFrame)
            _lightSwitch.ToggleSwitchServerRpc();
    }

    private bool IsInRangeOfSwitch()
    {
        var dist = Vector3.Distance(transform.position, _lightSwitch.transform.position);
        return dist <= 1;
    }
}
