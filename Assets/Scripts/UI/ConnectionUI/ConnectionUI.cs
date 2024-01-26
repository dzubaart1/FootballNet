using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FootBallNet.UI
{
    public class ConnectionUI : UIBehaviour
    {
        [SerializeField] private CreateRoomPanel _createRoomPanel;
        [SerializeField] private JoinRoomPanel _joinRoomPanel;

        protected override void Awake()
        {
            base.Awake();

            _createRoomPanel.ClickBackBtnEvent += ShowJoinRoomBtn;
            _joinRoomPanel.ClickCreateRoomBtnEvent += ShowCreateRoomBtn;

            Engine.GetService<NetworkService>().JoinedRoomEvent += OnJoinedRoom;
        }

        private void OnDestroy()
        {
            _createRoomPanel.ClickBackBtnEvent -= ShowJoinRoomBtn;
            _joinRoomPanel.ClickCreateRoomBtnEvent -= ShowCreateRoomBtn;
        }

        private void ShowJoinRoomBtn()
        {
            _joinRoomPanel.gameObject.SetActive(true);
            _createRoomPanel.gameObject.SetActive(false);
        }

        private void ShowCreateRoomBtn()
        {
            _joinRoomPanel.gameObject.SetActive(false);
            _createRoomPanel.gameObject.SetActive(true);
        }

        private void OnJoinedRoom()
        {
            gameObject.SetActive(false);
        }
    }
}