using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace FootBallNet
{
    public class ColorsService : Service<ColorsConfiguration>
    {
        public event Action PlayerChooseColorEvent;
        public event Action InitColorsListEvent;

        public IReadOnlyDictionary<int, Player> PlayersColors => _playersColors;
        private Dictionary<int, Player> _playersColors;
                
        public int LocalPlayerColorID;

        private NetworkService _networkService;
        private InputService _inputService;
        private GatesService _gatesService;

        public override Task InitializeServiceAsync()
        {
            _networkService = Engine.GetService<NetworkService>();
            _inputService = Engine.GetService<InputService>();
            _gatesService = Engine.GetService<GatesService>();

            _playersColors = new Dictionary<int, Player>();

            _networkService.PlayerEneteredRoomEvent += OnPlayerEnteredRoom;
            return Task.CompletedTask;
        }

        public void SetLocalPlayerColor(int colorID)
        {
            LocalPlayerColorID = colorID;

            _inputService.LocalPlayer.Weapon.SetWeaponColor(Configuration.ColorsList[colorID]);
            Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_RegisterPlayerColorsRequest), PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer, _networkService.NetworkPlayer.photonView.ViewID, colorID);
            PlayerChooseColorEvent?.Invoke();
        }

        public int[] GetAvailableColorsArray()
        {
            List<int> _list = new List<int>();
            foreach(var color in _playersColors.Keys)
            {
                if(_playersColors[color] == null)
                {
                    _list.Add(color);
                }
            }
            return _list.ToArray();
        }

        public void InitAvailabaleColors(int[] colors)
        {
            foreach(var colorId in colors)
            {
                _playersColors.Add(colorId, null);
            }

            InitColorsListEvent?.Invoke();
        }

        public void InitAvailabaleColorsByDefault()
        {
            for (int i = 0; i < Configuration.ColorsList.Count; i++)
            {
                _playersColors.Add(i, null);
            }

            InitColorsListEvent?.Invoke();
        }

        public void RegisterPlayerColor(Player player, int colorID)
        {
            _playersColors[colorID] = player;
        }

        public int GetColorIDByPlayer(Player player)
        {
            foreach(var color in _playersColors.Keys)
            {
                if(_playersColors[color] == player)
                {
                    return color;
                }
            }

            return -1;
        }

        public void PaintAllNetworkPlayerStaff(NetPlayer.NetworkPlayer networkPlayer, int gateID, int colorID)
        {
            networkPlayer.Weapon.SetWeaponColor(Configuration.ColorsList[colorID]);
            _gatesService.GetGateByID(gateID).SetColor(Configuration.ColorsList[colorID]);
        }

        public void DebugPlayersColor()
        {
            string s = "Players Color:\n";
            foreach(var color in _playersColors.Keys)
            {
                if (_playersColors[color] is null)
                {
                    s += $"{color}: null\n";
                }
                else
                {
                    s += $"{color}: {_playersColors[color].UserId}\n";
                }
            }
            Debug.Log(s);
        }

        private void OnPlayerEnteredRoom(Player player)
        {
            Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_InitAvailableColorsRequest), PhotonNetwork.MasterClient, player);
        }

        public override void DestroyService()
        {
        }

        public override void ResetService()
        {
        }
    }
}
