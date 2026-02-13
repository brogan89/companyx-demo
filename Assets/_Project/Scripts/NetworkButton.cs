using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.UI;

public class NetworkButton : NetworkBehaviour
{
    public UnityEvent OnPressedShared;
    
    private void Start()
    {
        GetComponent<Button>().interactable = false;
        GetComponent<Button>().onClick.AddListener(OnPressedServerRpc);
    }

    protected override void OnNetworkPostSpawn()
    {
        base.OnNetworkPostSpawn();
        GetComponent<Button>().interactable = true;
    }

    [Rpc(SendTo.Server)]
    private void OnPressedServerRpc()
    {
        OnPressedClientRpc();
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    private void OnPressedClientRpc()
    {
        OnPressedShared?.Invoke();
    }
}
