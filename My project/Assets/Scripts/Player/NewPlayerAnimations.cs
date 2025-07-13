using DG.Tweening;
using UnityEngine;

public class NewPlayerAnimations : MonoBehaviour
{
    //Hashes
    private static readonly int IS_MOVING_HASH = Animator.StringToHash("isMoving");

    [SerializeField] private NewPlayerMovement _playerMovement;
    [SerializeField] private Transform _playerSpriteHolder;
    [SerializeField] private Transform _luggageBase;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        _playerMovement.OnFlip += Flip;
    }

    private void Flip()
    {
        _playerSpriteHolder.DORotate(_playerMovement.FacingRight ? Vector3.zero : Vector3.up * 180f, 0.2f);
    }

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
