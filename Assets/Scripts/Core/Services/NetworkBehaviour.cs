
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

namespace FootBallNet
{
    public class NetworkBehaviour : MonoBehaviourPun
    {
        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                transform.SetParent(Engine.Behaviour.transform);
                Engine.InitializeNetworkBehaviour(this);
            }
        }

        [PunRPC]
        public void RPC_LoadScene(int sceneIndex)
        {
            Engine.GetService<SceneSwitchingService>().LoadScene(sceneIndex);
        }

        [PunRPC]
        public void RPC_UnloadScene(int sceneIndex)
        {
            Engine.GetService<SceneSwitchingService>().UnloadScene(sceneIndex);
        }

        [PunRPC]
        public void RPC_SetCurrentNetworkPlayer(int photonViewID)
        {
            var networkPlayer = PhotonNetwork.GetPhotonView(photonViewID).GetComponent<NetPlayer.NetworkPlayer>();

            if (networkPlayer is null)
            {
                Debug.LogError("PhotonView doesn't have NetworkPlayer Component!!!");
                return;
            }

            Engine.GetService<NetworkService>().SetCurrentNetworkPlayer(networkPlayer);
        }

        [PunRPC]
        public void RPC_TryInitPlayerColorsRequest(Player player, Player owner, int photonViewID)
        {
            var colorsService = Engine.GetService<ColorsService>();
            var gatesService = Engine.GetService<GatesService>();

            var colorID = colorsService.GetColorIDByPlayer(owner);
            if(colorID == -1)
            {
                return;
            }

            var gateID = gatesService.GetGateByPlayer(owner).ID;

            Engine.RPC(nameof(RPC_UpdatePlayerColors), player, photonViewID, gateID, colorID);
        }

        [PunRPC]
        public void RPC_RegisterPlayerColorsRequest(Player player, int photonViewID, int colorID)
        {
            var colorsService = Engine.GetService<ColorsService>();
            var gatesService = Engine.GetService<GatesService>();

            colorsService.RegisterPlayerColor(player, colorID);
            var gateID = gatesService.GetGateByPlayer(player).ID;

            colorsService.DebugPlayersColor();
            Engine.RPC(nameof(RPC_UpdatePlayerColors), RpcTarget.All, photonViewID, gateID, colorID);
        }

        [PunRPC]
        public void RPC_UpdatePlayerColors(int photonViewID, int gateID, int colorID)
        {
            var colorsService = Engine.GetService<ColorsService>();

            var netPlayer = PhotonNetwork.GetPhotonView(photonViewID).GetComponent<NetPlayer.NetworkPlayer>();
            colorsService.PaintAllNetworkPlayerStaff(netPlayer, gateID, colorID);
        }

        [PunRPC]
        public void RPC_UpdatePlayerGate(int gateID)
        {
            var gatesService = Engine.GetService<GatesService>();
            var inputService = Engine.GetService<InputService>();

            var gate = gatesService.GetGateByID(gateID);
            gatesService.SetLocalPlayerGate(gate);
            
            inputService.LocalPlayer.transform.position = gate.transform.position;
        }
    }
}