using Photon.Pun;

namespace FootBallNet
{
    public class NetworkBehaviour : MonoBehaviourPun
    {
        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient == false)
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
        public void RPC_ActivateObject(int photonViewID)
        {
            var view = PhotonNetwork.GetPhotonView(photonViewID);
            view.gameObject.SetActive(true);
        }

        [PunRPC]
        public void RPC_SetNetworkObjectParentAsNetworkHolder(int photonViewID)
        {
            var view = PhotonNetwork.GetPhotonView(photonViewID);
            view.transform.SetParent(Engine.GetService<NetworkCachedService>().Container);
        }
    }
}