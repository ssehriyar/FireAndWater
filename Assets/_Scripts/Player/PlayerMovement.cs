using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private bool _keyPressed;
	private Vector2 _input;
	private Rigidbody _rigidbody;
	[SerializeField] private float _moveSpeed;
	[SerializeField] private float _jumpForce;
	[SerializeField] private float _fallAcceleration;
	[SerializeField] private float _fallMaxSpeed;
	[SerializeField] private float _groundCheckRadius;
	[SerializeField] private LayerMask _groundLayerMask;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		Move();
		Jump();
		Fall();

		_rigidbody.velocity = _input;
	}

	private void Move()
	{
		if (Input.GetKey(KeyCode.A)) _input.x = -_moveSpeed;
		else if (Input.GetKey(KeyCode.D)) _input.x = _moveSpeed;
		else _input.x = 0f;
	}

	private void Jump()
	{
		if (IsGrounded && Input.GetKeyDown(KeyCode.W))
		{
			_keyPressed = true;
			_input.y = _jumpForce;
		}
	}

	private void Fall()
	{
		if (IsGrounded == false)
		{
			_input.y -= _fallAcceleration;
			if (_input.y <= 0)
			{
				_input.y = Mathf.Abs(_input.y) >= _fallMaxSpeed ? -_fallMaxSpeed : _input.y;
			}
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			_keyPressed = false;
		}
		if (IsGrounded && _keyPressed == false)
		{
			_input.y = 0;
		}
	}

	private bool IsGrounded => Physics.CheckSphere(transform.position, _groundCheckRadius, _groundLayerMask);

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, _groundCheckRadius);
	}
}
