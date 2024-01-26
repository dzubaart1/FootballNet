using System;
using UnityEngine;

namespace FootBallNet
{

    [RequireComponent(typeof(CanvasGroup), typeof(Canvas))]
    public class UIBehaviour : MonoBehaviour
    {
        public Action<bool> VisibilityChangedEvent;

        public bool HideOnLoad = false;
        public bool Visible { get; private set; } = false;

        protected CanvasGroup CanvasGroup;
        protected Canvas Canvas;

        protected virtual void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            Canvas = GetComponent<Canvas>();

            SetVisibility(!HideOnLoad);
        }

        private void Start()
        {
            if (Engine.GetService<InputService>().Platform != ETargetPlatform.VR)
            {
                ChangeRenderMode(RenderMode.ScreenSpaceOverlay);
            }
        }

        public void SetVisibility(bool isVisible)
        {
            CanvasGroup.interactable = isVisible;
            CanvasGroup.blocksRaycasts = isVisible;
            CanvasGroup.alpha = isVisible ? 1f : 0f;

            Visible = isVisible;
            VisibilityChangedEvent?.Invoke(isVisible);
        }

        private void ChangeRenderMode(RenderMode renderMode)
        {
            Canvas.renderMode = renderMode;
        }
    }
}
