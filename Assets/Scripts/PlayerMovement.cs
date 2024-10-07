using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _runSpeed = 12f;
    [SerializeField] private float _gravityModifier = 3f;
    [SerializeField] private float _jumpPower = 10f;
    private Vector3 _move;
    private bool _canJump;

    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] Animator _animator;

    public static PlayerMovement Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float velocity = _move.y;

        Vector3 verticalMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontalMove = transform.right * Input.GetAxis("Horizontal");

        _move = horizontalMove + verticalMove;
        _move = Vector3.ClampMagnitude(_move, 1f);


        if (Input.GetKey(KeyCode.LeftShift))
        {
            _move *= _runSpeed;
            _animator.SetBool("run", true);
        }
        else
        {
            _move *= _speed;
            _animator.SetBool("run", false);
        }

        _move.y = velocity;
        _move.y += Physics.gravity.y * _gravityModifier * Time.deltaTime;

        if (_controller.isGrounded)
        {
            _move.y = Physics.gravity.y * _gravityModifier * Time.deltaTime;
        }

        _canJump = Physics.OverlapSphere(_groundCheckPoint.position, 0.25f, _whatIsGround).Length > 0;

        if (Input.GetKeyDown(KeyCode.Space) && _canJump)
        {
            _move.y = _jumpPower;
        }

        _controller.Move(_move * Time.deltaTime);

        _animator.SetFloat("speed", _move.magnitude);
        _animator.SetBool("onGround", _canJump);
    }
}
