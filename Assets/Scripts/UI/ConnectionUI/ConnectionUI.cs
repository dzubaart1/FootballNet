using UnityEngine;

namespace FootBallNet.UI
{
    public class ConnectionUI : UIBehaviour
    {
        [SerializeField] private CreateRoomPanel _createRoomPanel;
        [SerializeField] private JoinRoomPanel _joinRoomPanel;
        [SerializeField] private ChooseColorPanel _chooseColorPanel;

        protected override void Awake()
        {
            base.Awake();

            _createRoomPanel.ClickBackBtnClickEvent += ShowJoinRoomBtn;
            _createRoomPanel.ClickCreateRoomBtnClilEvent += ShowChooseColorPanel;
            _joinRoomPanel.ClickCreateRoomBtnEvent += ShowCreateRoomBtn;
            _joinRoomPanel.ClickRoomBtnEvent += ShowChooseColorPanel;
            
            Engine.GetService<ColorsService>().PlayerChooseColorEvent += OnPlayerChooseColor;
        }

        private void OnDestroy()
        {
            _createRoomPanel.ClickBackBtnClickEvent -= ShowJoinRoomBtn;
            _createRoomPanel.ClickCreateRoomBtnClilEvent -= ShowChooseColorPanel;
            _joinRoomPanel.ClickCreateRoomBtnEvent -= ShowCreateRoomBtn;
            _joinRoomPanel.ClickRoomBtnEvent -= ShowChooseColorPanel;

            Engine.GetService<ColorsService>().PlayerChooseColorEvent -= OnPlayerChooseColor;
        }

        private void ShowJoinRoomBtn()
        {
            _joinRoomPanel.gameObject.SetActive(true);
            _createRoomPanel.gameObject.SetActive(false);
            _chooseColorPanel.gameObject.SetActive(false);
        }

        private void ShowCreateRoomBtn()
        {
            _joinRoomPanel.gameObject.SetActive(false);
            _createRoomPanel.gameObject.SetActive(true);
            _chooseColorPanel.gameObject.SetActive(false);
        }

        private void ShowChooseColorPanel()
        {
            _joinRoomPanel.gameObject.SetActive(false);
            _createRoomPanel.gameObject.SetActive(false);
            _chooseColorPanel.gameObject.SetActive(true);
        }

        private void OnPlayerChooseColor()
        {
            gameObject.SetActive(false);
        }
    }
}