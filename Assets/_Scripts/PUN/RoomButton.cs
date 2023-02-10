using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MyGame
{
    public class RoomButton : MonoBehaviour
    {
        private int _roomSize;
        private int _playerCount;
        private string _roomName;

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _sizeText;

        public void JoinRoomOnClick()
        {
            PhotonNetwork.JoinRoom(_roomName);
        }

        public void SetRoom(string nameInput, int sizeInput, int countInput)
        {
            _roomName = nameInput;
            _roomSize = sizeInput;
            _playerCount = countInput;
            _nameText.text = nameInput;
            _sizeText.text = countInput + "/" + sizeInput;
        }
    }
}