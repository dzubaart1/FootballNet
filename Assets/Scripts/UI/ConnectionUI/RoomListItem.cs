using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FootBallNet.UI
{
    public class RoomListItem : MonoBehaviour
    {
        public Action<string> RoomBtnClickEvent;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _roomName;

        private void Start()
        {
            _button.onClick.AddListener(delegate { RoomBtnClickEvent?.Invoke(_roomName.text); });
        }

        public void SetRoomName(string roomName)
        {
            _roomName.text = roomName;
        }
    }
}