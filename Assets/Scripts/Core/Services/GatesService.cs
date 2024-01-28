using FootBallNet.Gameplay;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace FootBallNet
{
    public class GatesService : Service<GatesConfiguration>
    {
        public int LocalGateID;
        public IReadOnlyDictionary<Gate, Player> PlayersGates => _playersGates;
        private Dictionary<Gate, Player> _playersGates;

        private Transform _gatesHolder;
        private float _gatesRadius = 4.5f;
        private Gate _localPlayerGate;

        private SceneSwitchingService _sceneSwitchingService;
        private NetworkService _networkService;

        public override Task InitializeServiceAsync()
        {
            _playersGates = new Dictionary<Gate, Player>();

            _sceneSwitchingService = Engine.GetService<SceneSwitchingService>();
            _sceneSwitchingService.SceneLoadedEvent += SpawnGates;
            _networkService = Engine.GetService<NetworkService>();
            _networkService.PlayerEneteredRoomEvent += OnPlayerEntetered;

            _gatesHolder = Engine.CreateObject("GatesHolder").GetComponent<Transform>();

            return Task.CompletedTask;
        }

        public Gate GetGateByPlayer(Player player)
        {
            foreach (var gate in _playersGates.Keys)
            {
                if (_playersGates[gate] == player)
                {
                    return gate;
                }
            }

            throw new Exception("There is not gate in the storage");
        }

        public Gate GetGateByID(int gateID)
        {
            foreach (var gate in _playersGates.Keys)
            {
                if (gate.ID == gateID)
                {
                    return gate;
                }
            }

            throw new Exception("There is not gate in the storage");
        }

        public Player GetPlayerByGate(int gateID)
        {
            return _playersGates[GetGateByID(gateID)];
        }

        public void SetLocalPlayerGate(Gate gate)
        {
            _localPlayerGate = gate;
        }

        private void OnPlayerEntetered(Player player)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var gate = TryGetFreeGate();
                _playersGates[gate] = player;

                Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_UpdatePlayerGate), player, gate.ID);
                DebugPlayersGate();
            }
        }

        private void SpawnGates()
        {
            if (_sceneSwitchingService.CurrentScene.name != EScene.PlayScene.ToString())
            {
                return;
            }

            int angleStep = 360 / Configuration.GatesCount;

            for (int i = 0; i < Configuration.GatesCount; i++)
            {
                var gate = Engine.Instantiate(Configuration.GatePrefab, _gatesHolder);
                gate.SetID(i);
                _playersGates.Add(gate, null);

                var tr = gate.GetComponent<Transform>();
                var shiftVector = new Vector3(Mathf.Sin(angleStep * i * Mathf.PI / 180.0f)*_gatesRadius, 0, Mathf.Cos(angleStep * i * Mathf.PI / 180.0f) *_gatesRadius);;

                tr.localPosition = shiftVector;
                tr.LookAt(_gatesHolder.position);
                tr.localPosition += tr.up;
            }
        }

        private Gate TryGetFreeGate()
        {
            foreach(var gate in _playersGates.Keys)
            {
                if(_playersGates[gate] is null)
                {
                    return gate;
                }
            }

            throw new Exception("There is not free gates");
        }

        public void DebugPlayersGate()
        {
            string s = "PlayersGate:\n";
            foreach (var gate in _playersGates.Keys)
            {
                if(_playersGates[gate] is null)
                {
                    s += $"{gate.ID}: null\n";
                }
                else
                {
                    s += $"{gate.ID}: {_playersGates[gate].UserId}\n";
                }
            }
            Debug.Log(s);
        }

        public override void DestroyService()
        {
            _sceneSwitchingService.SceneLoadedEvent -= SpawnGates;
            _networkService.PlayerEneteredRoomEvent -= OnPlayerEntetered;
        }

        public override void ResetService()
        {
        }
    }
}
