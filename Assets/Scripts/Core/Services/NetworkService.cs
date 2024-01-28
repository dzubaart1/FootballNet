using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace FootBallNet
{
    public class NetworkService : Service<NetworkConfiguration>, IConnectionCallbacks, IMatchmakingCallbacks, IInRoomCallbacks,
        ILobbyCallbacks, IWebRpcCallback, IErrorInfoCallback
    {
        public event Action<IReadOnlyCollection<RoomInfo>> UpdatedRoomsListEvent;
        public event Action<Player> PlayerLeftRoomEvent;
        public event Action<Player> PlayerEneteredRoomEvent;
        public event Action JoinedRoomEvent;

        public NetPlayer.NetworkPlayer NetworkPlayer { get; private set; }
        
        public override Task InitializeServiceAsync()
        {
            PhotonNetwork.AddCallbackTarget(this);
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.ConnectUsingSettings();

            return Task.CompletedTask;
        }

        public override void ResetService() {}
        public override void DestroyService() {}

        public void SetCurrentNetworkPlayer(NetPlayer.NetworkPlayer networkPlayer)
        {
            Debug.Log("Set current Network Player");
            NetworkPlayer = networkPlayer;
        }

        #region PUNCallbacks
        public void OnConnected() {}

        public void OnConnectedToMaster()
        {
            Debug.Log("Connected to master.");
            PhotonNetwork.JoinLobby();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected.");
        }

        public void OnJoinedLobby()
        {
            Debug.Log("Joined lobby.");
        }

        public void OnLeftLobby() {}
        public void OnCreatedRoom() {}
        public void OnCreateRoomFailed(short returnCode, string message) {}

        public void OnJoinedRoom()
        {
            Debug.Log("On Joined Room.");

            if (PhotonNetwork.IsMasterClient)
            {
                var networkBehaviour = PhotonNetwork
                    .InstantiateRoomObject(Configuration.NetworkPrefabPath, Vector3.zero, Quaternion.identity)
                    .GetComponent<NetworkBehaviour>();
                networkBehaviour.transform.SetParent(Engine.Behaviour.transform);
                Engine.InitializeNetworkBehaviour(networkBehaviour);
               
                var networkPlayer = PhotonNetwork.Instantiate("NetworkPlayer", Vector3.zero, Quaternion.identity, 0, new[] { PhotonNetwork.MasterClient }).GetComponent<NetPlayer.NetworkPlayer>();
                Engine.GetService<NetworkService>().SetCurrentNetworkPlayer(networkPlayer);
                PlayerEneteredRoomEvent?.Invoke(PhotonNetwork.MasterClient);

                Engine.GetService<ColorsService>().InitAvailabaleColorsByDefault();
            }

            JoinedRoomEvent?.Invoke();
        }

        public void OnJoinRoomFailed(short returnCode, string message) {}
        public void OnJoinRandomFailed(short returnCode, string message) {}

        public void OnLeftRoom()
        {
            Debug.Log("On Left Room");
            Engine.DestroyNetworkBehaviour();
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("Player Enter Room");
            
            if (PhotonNetwork.IsMasterClient)
            {
                var colorsService = Engine.GetService<ColorsService>();
                var gatesService = Engine.GetService<GatesService>();

                var networkPlayer = PhotonNetwork.Instantiate("NetworkPlayer", Vector3.zero, Quaternion.identity, 0, new []{newPlayer}).GetComponent<NetPlayer.NetworkPlayer>();
                Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_SetCurrentNetworkPlayer), newPlayer, networkPlayer.GetID());
                
                Debug.Log("Player Enter Room Master");
                PlayerEneteredRoomEvent?.Invoke(newPlayer);
                PhotonNetwork.NickName = "Master";
            }
            else
            {
                PhotonNetwork.NickName = "Player" + (PhotonNetwork.CurrentRoom.PlayerCount - 1);
            }
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
                PlayerLeftRoomEvent?.Invoke(otherPlayer);
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Room list updated.");
            UpdatedRoomsListEvent?.Invoke(roomList);
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {}
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {}
        public void OnMasterClientSwitched(Player newMasterClient) {}
        public void OnRegionListReceived(RegionHandler regionHandler) {}
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) {}
        public void OnCustomAuthenticationFailed(string debugMessage) {}
        public void OnFriendListUpdate(List<FriendInfo> friendList) {}
        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) {}
        public void OnWebRpcResponse(OperationResponse response) {}
        public void OnErrorInfo(ErrorInfo errorInfo) {}
        #endregion
    }
}
