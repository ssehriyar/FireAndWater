using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
	private float _downForce;
	private Vector2 _input;
	private Vector3 _topPoint;
	private Rigidbody _rigidbody;
	[SerializeField] private float _moveSpeed;
	[SerializeField] private float _jumpForce;
	[SerializeField] private float _fallAcceleration;
	[SerializeField] private float _gravity;
	[SerializeField] private float _fallMaxSpeed;
	[SerializeField] private float _groundCheckRadius;
	[SerializeField] private LayerMask _groundLayerMask;
	[SerializeField] private CapsuleCollider _capsuleCollider;

	private void Start()
	{
		if (photonView.IsMine == false && GetComponent<PlayerMovement>() != null)
		{
			Destroy(GetComponent<PlayerMovement>());
		}
		_rigidbody = GetComponent<Rigidbody>();
		_topPoint = new Vector3(0, _capsuleCollider.bounds.center.y * 2, 0);
	}

	private void Update()
	{
		//if (photonView != null && photonView.IsMine == false) return;

		Move();
		Jump();
		Fall();
	}

	private void FixedUpdate()
	{
		//_rigidbody.velocity = new Vector3(_dirX, _dirY);
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
			_input.y = _jumpForce;
		}
	}

	private void Fall()
	{
		_downForce = _gravity * Time.deltaTime;
		if ((IsGrounded && _input.y < 0) || (IsTop && _input.y > 0))
		{
			_input.y = _downForce;
		}
		if (IsGrounded == false)
		{
			_input.y -= (_downForce + _fallAcceleration) * Time.deltaTime;
			if (Mathf.Abs(_input.y) > _fallMaxSpeed)
			{
				_input.y = -_fallMaxSpeed;
			}
		}
	}

	private bool IsGrounded => Physics.CheckSphere(transform.position, _groundCheckRadius, _groundLayerMask);
	private bool IsTop => Physics.CheckSphere(transform.position + _topPoint, _groundCheckRadius, _groundLayerMask);

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, _groundCheckRadius);
		Gizmos.DrawSphere(transform.position + _topPoint, _groundCheckRadius);
	}
}
