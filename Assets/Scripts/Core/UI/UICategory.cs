using System;
using UnityEngine;

namespace FootBallNet
{

    [RequireComponent(typeof(CanvasGroup))]
    public class UICategory : MonoBehaviour
    {
        public Action<bool> VisibilityChangedEvent;
        public bool IsVisible { get; private set; }

        protected CanvasGroup CanvasGroup;

        protected virtual void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetVisibility(bool visible)
        {
            CanvasGroup.interactable = visible;
            CanvasGroup.blocksRaycasts = visible;
            CanvasGroup.alpha = visible ? 1f : 0f;

            IsVisible = visible;
            VisibilityChangedEvent?.Invoke(visible);
        }

        public void SetInteractable(bool interactable)
        {
            CanvasGroup.interactable = interactable;
        }
    }
}