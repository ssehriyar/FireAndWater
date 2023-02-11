using UnityEngine;
using System;
using Photon.Pun;

public class RigidbodyLag : MonoBehaviourPunCallbacks, IPunObservable
{
	private Rigidbody _rigidbody;
	private Vector3 _netPosition;
	[SerializeField] private float _smoothPos;
	[SerializeField] private float _teleportDistance;

	private void Awake()
	{
		PhotonNetwork.SendRate = 35;
		PhotonNetwork.SerializationRate = 20;
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(_rigidbody.position);
			stream.SendNext(_rigidbody.velocity);
		}
		else
		{
			_netPosition = (Vector3)stream.ReceiveNext();
			_rigidbody.velocity = (Vector3)stream.ReceiveNext();
			float lag = MathF.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
			_netPosition += (_rigidbody.velocity * lag);
		}
	}

	private void FixedUpdate()
	{
		if (photonView.IsMine) return;

		//_rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _netPosition, _smoothPos * Time.fixedDeltaTime);
		_rigidbody.position = Vector3.Lerp(_rigidbody.position, _netPosition, _smoothPos * Time.fixedDeltaTime);
		if (Vector3.Distance(_rigidbody.position, _netPosition) > _teleportDistance)
		{
			_rigidbody.position = _netPosition;
		}
	}
}
