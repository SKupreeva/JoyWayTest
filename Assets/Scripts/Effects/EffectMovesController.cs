using Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Effects
{
    // class implements drag and drop for spawned effects, detects collisions and checks if another collider is target
    // invokes effect applying for player characters

    public class EffectMovesController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private EffectType _effectType;

        private RectTransform _rectTransform;
        private Canvas _canvas;
        private List<PlayerController> _targets = new List<PlayerController>();
        private PlayerController _currentTarget;
        private Effect _currentEffect;

        public delegate void OnEffectAppliedEvent();
        public event OnEffectAppliedEvent OnEffectApplied;
        public EffectType EffectType => _effectType;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerController>(out var player))
            {
                _currentTarget = player;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _currentTarget = null;
        }

        public void Setup(List<PlayerController> targets, Canvas canvas, Effect currentEffect)
        {
            _canvas = canvas;

            if (targets.Count == 0)
            {
                Debug.LogError("Drag and drop setup fail (targets list is empty)");
                return;
            }

            _targets = targets;
            _currentEffect = currentEffect;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.6f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

            if (_currentTarget == null)
            {
                return;
            }

            if (_targets.Contains(_currentTarget))
            {
                _canvasGroup.alpha = 1f;
            }
            else
            {
                _canvasGroup.alpha = 0.6f;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;

            if (_currentTarget == null || !_targets.Contains(_currentTarget))
            {
                return;
            }

            _currentTarget.ApplyEffect(_currentEffect);
            OnEffectApplied?.Invoke();
            Destroy(gameObject);
        }
    }
}
