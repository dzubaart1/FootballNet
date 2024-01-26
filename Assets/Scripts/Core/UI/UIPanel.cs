using System;
using UnityEngine;

namespace FootBallNet
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        public Action<bool> VisibilityChangedEvent;

        public bool HideOnLoad = false;
        public bool IsVisible { get; private set; } = false;

        protected CanvasGroup CanvasGroup;

        protected virtual void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();

            SetVisibility(!HideOnLoad);
        }

        public void SetVisibility(bool isVisible)
        {
            CanvasGroup.interactable = isVisible;
            CanvasGroup.blocksRaycasts = isVisible;
            CanvasGroup.alpha = isVisible ? 1f : 0f;

            IsVisible = isVisible;
            VisibilityChangedEvent?.Invoke(isVisible);
        }
    }
}
