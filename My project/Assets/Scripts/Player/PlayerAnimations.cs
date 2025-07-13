using DG.Tweening;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    //Hashes
    private static readonly int IS_MOVING_HASH = Animator.StringToHash("isMoving");

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Transform _luggageBase;
    [SerializeField] private Animator _animator;

    private float _direction = 1f;

    private void Update()
    {
        _animator.SetBool(IS_MOVING_HASH, _playerMovement.IsMoving);

        /*var direction = Mathf.Sign(_playerMovement.MoveDir.x);
        Debug.LogWarning(direction);
        if (direction != 0f && _direction != direction)
        {
            _animator.transform.GetChild(0).transform.DOScaleX(direction, 0.2f);
            _luggageBase.transform.DOScaleX(direction, 0.2f);
            _direction = direction;
        }*/
    }
}
