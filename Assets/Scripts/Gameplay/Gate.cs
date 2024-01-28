using TMPro;
using UnityEngine;

namespace FootBallNet.Gameplay
{
    public class Gate : MonoBehaviour
    {
        public Color Color => _color;
        public int ID => _id;

        [SerializeField] private TextMeshProUGUI _playerInfo;
        private MeshRenderer _meshRenderer;

        private Color _color;
        private int _id;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetColor(Color color)
        {
            _color = color;
            _meshRenderer.material.color = color;
        }

        public void SetID(int id)
        {
            _id = id;
        }
    }
}
