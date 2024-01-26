﻿using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FootBallNet.UI
{
    public class CreateRoomPanel : MonoBehaviour
    {
        public event Action ClickBackBtnEvent;

        [SerializeField] private TMP_InputField _roomNameText;
        [SerializeField] private Button _createRoomBtn;
        [SerializeField] private Button _backBtn;

        private const int MAX_PLAYERS_COUNT = 5;

        private void Start()
        {
            _createRoomBtn.onClick.AddListener(OnClickCreateBtn);
            _backBtn.onClick.AddListener(OnClickBackBtn);
        }

        private void OnDestroy()
        {
            _createRoomBtn.onClick.RemoveListener(OnClickCreateBtn);
            _backBtn.onClick.RemoveListener(OnClickBackBtn);
        }

        private void OnClickBackBtn()
        {
            ClickBackBtnEvent?.Invoke();
        }

        private void OnClickCreateBtn()
        {
            if (string.IsNullOrWhiteSpace(_roomNameText.text))
            {
                return;
            }

            var options = new RoomOptions();
            options.MaxPlayers = Convert.ToByte(MAX_PLAYERS_COUNT);
            options.IsVisible = true;

            var res = PhotonNetwork.CreateRoom(_roomNameText.text, options);
        }
    }
}