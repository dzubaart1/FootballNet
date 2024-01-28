using System;
using UnityEngine;
using UnityEngine.UI;

namespace FootBallNet.UI
{
    public class ColorListItem : MonoBehaviour
    {
        public event Action<Color> ClickButtonEvent;

        private Image _image;
        private Button _button;
        private int _colorID;

        private ColorsService _colorService;

        private void Awake()
        {
            _colorService = Engine.GetService<ColorsService>();

            _image = GetComponent<Image>();
            _button = GetComponent<Button>();

            _button.onClick.AddListener(OnClickButton);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClickButton);
        }

        public void SetColorID(int colorID)
        {
            _colorID = colorID;
            _image.color = Engine.GetService<ColorsService>().Configuration.ColorsList[_colorID];
        }

        private void OnClickButton()
        {
            _colorService.SetLocalPlayerColor(_colorID);
        }
    }
}
