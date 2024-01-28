using Photon.Pun;
using System;
using UnityEngine;

namespace FootBallNet.UI
{
    public class ChooseColorPanel : MonoBehaviour
    {
        [SerializeField] private Transform _colorHolder;
        [SerializeField] private ColorListItem _colorListItemPrefab;

        private ColorsService _colorsService;

        private void Awake()
        {
            _colorsService = Engine.GetService<ColorsService>();
            //_colorsService.InitColorsListEvent += InitColorsScrollView;

            InitColorsScrollView();
        }

        private void OnDestroy()
        {
            //_colorsService.InitColorsListEvent -= InitColorsScrollView;
        }

        private void InitColorsScrollView()
        {
            foreach(var color in _colorsService.PlayersColors.Keys)
            {
                var colorItem = Instantiate(_colorListItemPrefab, _colorHolder);
                colorItem.SetColorID(color);
            }
        }
    }
}
