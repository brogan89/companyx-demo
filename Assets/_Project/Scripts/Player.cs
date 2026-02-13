using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private const float SPEED = 10f;
    private NetworkObject _networkObject;

    private void Awake()
    {
        _networkObject = GetComponent<NetworkObject>();
    }

    private void Update()
    {
        if (!_networkObject.IsLocalPlayer)
            return;

        MovePlayer(Time.deltaTime);
    }

    private void MovePlayer(float delta)
    {
        var keyboard = Keyboard.current;
        var speedMultiplier = delta * SPEED;

        if (keyboard.wKey.IsPressed())
            transform.Translate(Vector3.forward * speedMultiplier);
        if (keyboard.sKey.IsPressed())
            transform.Translate(Vector3.back * speedMultiplier);
        if (keyboard.aKey.IsPressed())
            transform.Translate(Vector3.left * speedMultiplier);
        if (keyboard.dKey.IsPressed())
            transform.Translate(Vector3.right * speedMultiplier);
    }
}
