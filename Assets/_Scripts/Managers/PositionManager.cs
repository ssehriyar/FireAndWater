using UnityEngine;

namespace MyGame
{
	public class PositionManager : MonoBehaviour
	{
		[SerializeField] private Transform[] _playerPositions;

		public Vector3 GetPlayerPosition(int actorNumber) => _playerPositions[actorNumber].position;
	}
}
