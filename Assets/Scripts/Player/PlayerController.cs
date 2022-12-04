using Effects;
using UnityEngine;

namespace Player
{
    // class controls character's hp & ui changes, detects if character is alive, if character is enemy or player side
    // applies effect and detects character's death

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private int _maxHp = 30;
        [SerializeField] private PlayerUIController _playerUI;
        [SerializeField] private bool _isPlayer;

        private HPController _hpController;
        private bool _isAlive = true;
        private Effect _currentEffect;

        public bool IsPlayer => _isPlayer;
        public bool IsAlive => _isAlive;
        public int CurrentHP => _hpController.CurrentHP;

        public delegate void OnCharacterDiedEvent();
        public event OnCharacterDiedEvent OnCharacterDied;

        private void Awake()
        {
            _hpController = new HPController(_maxHp);

            _hpController.OnHPChanged += OnHPChanged;
            _hpController.OnPlayerDied += OnPlayerDied;
            _hpController.OnProtectionDestroyed += OnProtectionDestroyed;

            _playerUI.Setup(_maxHp);
        }

        private void OnDestroy()
        {
            _hpController.OnHPChanged -= OnHPChanged;
            _hpController.OnPlayerDied -= OnPlayerDied;
            _hpController.OnProtectionDestroyed -= OnProtectionDestroyed;
        }

        public void ApplyEffect(Effect effect)
        {
            _currentEffect = effect;
            _currentEffect.Run(_hpController);

            if (effect.Type == EffectType.Poison)
            {
                _playerUI.AddPoison();
                _currentEffect.OnFinished += () => _playerUI.RemovePoison();
            }

            if (effect.Type == EffectType.Protection)
            {
                _playerUI.AddProtection(_hpController.ProtectionHP);
            }
        }

        public void Restart()
        {
            _hpController.Restart();
            _playerUI.Setup(_maxHp);
            _isAlive = true;
        }

        private void OnHPChanged()
        {
            _playerUI.ChangeHPValue(_hpController.CurrentHP, _hpController.ProtectionHP);
        }

        private void OnPlayerDied()
        {
            _isAlive = false;
            _playerUI.ChangeHPValue(_hpController.CurrentHP, _hpController.ProtectionHP);
            OnCharacterDied?.Invoke();
        }

        private void OnProtectionDestroyed()
        {
            _currentEffect?.ForceComplete();
            _playerUI.RemoveProtection();
        }
    }
}
