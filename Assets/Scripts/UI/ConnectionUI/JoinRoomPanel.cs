using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FootBallNet.UI
{
    public class JoinRoomPanel : MonoBehaviour
    {
        public event Action ClickRoomBtnEvent;
        public event Action ClickCreateRoomBtnEvent;

        [SerializeField] private Transform _roomsItemsParent;
        [SerializeField] private RoomListItem _roomListItem;
        [SerializeField] private Button _backBtn;

        private void Awake()
        {
            Engine.GetService<NetworkService>().UpdatedRoomsListEvent += OnRoomListUpdate;
            _backBtn.onClick.AddListener(OnClickBackBtn);
        }

        private void OnDestroy()
        {
            Engine.GetService<NetworkService>().UpdatedRoomsListEvent -= OnRoomListUpdate;
            _backBtn.onClick.RemoveListener(OnClickBackBtn);
        }

        private void OnClickBackBtn()
        {
            ClickCreateRoomBtnEvent?.Invoke();
        }

        private void OnRoomListUpdate(IReadOnlyCollection<RoomInfo> roomList)
        {
            Debug.Log($"RoomList Count: {roomList.Count}");
            foreach (var roomItem in roomList)
            {
                var obj = Instantiate(_roomListItem, _roomsItemsParent);
                obj.SetRoomName(roomItem.Name);
                obj.RoomBtnClickEvent += JoinToRoom;
            }
        }

        private void JoinToRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
            Engine.GetService<SceneSwitchingService>().LoadScene((int)EScene.PlayScene);
            ClickRoomBtnEvent?.Invoke();
        }
    }
}
