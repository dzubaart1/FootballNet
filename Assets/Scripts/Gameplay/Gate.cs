using Photon.Pun;
using TMPro;
using UnityEngine;

namespace FootBallNet.Gameplay
{
    public class Gate : MonoBehaviour
    {
        public Color Color => _color;
        private Color _color;

        public int ID => _id;
        private int _id;

        public bool IsInitailized => _isInitialized;
        private bool _isInitialized;

        [SerializeField] private TextMeshProUGUI _scoreText;

        private MeshRenderer _meshRenderer;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetColor(Color color)
        {
            _color = color;
            _meshRenderer.material.color = color;
            _isInitialized = true;
            Engine.RPC(nameof(Engine.NetworkBehaviour.RPC_RemovePointToGate), RpcTarget.MasterClient, _id, 0, PhotonNetwork.MasterClient);
        }

        public void SetID(int id)
        {
            _id = id;
        }

        public void UpdateScore(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}
