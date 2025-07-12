using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    //Hashes
    private static readonly int IS_MOVING_HASH = Animator.StringToHash("isMoving");

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Animator _animator;


    private void Update()
    {
        _animator.SetBool(IS_MOVING_HASH, _playerMovement.IsMoving);
    }
}
