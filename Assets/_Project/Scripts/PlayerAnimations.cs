using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private static readonly int PlayerMoveHash = Animator.StringToHash("PlayerMove");
    private static readonly int PlayerIdleHash = Animator.StringToHash("PlayerIdle");

    private Animator _animator;
    private Vector3 _positionLastFrame;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var velocity = (transform.position - _positionLastFrame) / Time.deltaTime;
        
        var animHash = velocity.sqrMagnitude > 0.01f 
            ? PlayerMoveHash 
            : PlayerIdleHash;
        _animator.Play(animHash);
        
        _positionLastFrame = transform.position;
    }
}
