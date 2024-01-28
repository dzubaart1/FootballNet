using FootBallNet;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootBallNet
{
    public class ScoresService : Service<ScoresConfiguration>
    {
        public IReadOnlyDictionary<Player, int> PlayersScores => _playersScores;
        private Dictionary<Player, int> _playersScores;

        private NetworkService _networkService;

        public override Task InitializeServiceAsync()
        {
            _playersScores = new Dictionary<Player, int>();

            _networkService = Engine.GetService<NetworkService>();
            _networkService.PlayerEneteredRoomEvent += OnPlayerEneteredRoom;

            return Task.CompletedTask;
        }

        public int RemovePlayerScore(Player player, int score)
        {
            if(!_playersScores.ContainsKey(player))
            {
                throw new Exception("There is not player in the storage!");
            }

            _playersScores[player] -= score;
            return _playersScores[player];
        }

        public int AddPlayerScore(Player player, int score)
        {
            if (!_playersScores.ContainsKey(player))
            {
                throw new Exception("There is not player in the storage!");
            }

            _playersScores[player] += score;
            return _playersScores[player];
        }

        public int GetPlayerScore(Player player)
        {
            if(!_playersScores.ContainsKey(player))
            {
                throw new Exception("There is not player in the storage!");
            }

            return _playersScores[player];
        }

        private void OnPlayerEneteredRoom(Player player)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _playersScores[player] = Configuration.DefaultScore;
            }
        }

        public override void DestroyService()
        {
            _networkService.PlayerEneteredRoomEvent -= OnPlayerEneteredRoom;
        }

        public override void ResetService()
        {
        }
    }
}
