
namespace Player
{
    // class controls character's hp changes

    public class HPController
    {
        private int _maxHP;
        private int _currentHP;
        private int _protectionHP;

        public int CurrentHP
        {
            get => _currentHP;
            set
            {
                if(value == _currentHP)
                {
                    return;
                }

                if (value <= 0)
                {
                    _currentHP = 0;
                    OnPlayerDied?.Invoke();
                    return;
                }

                if (value > _maxHP)
                {
                    _currentHP = _maxHP;
                }
                else
                {
                    _currentHP = value;
                }
                
                OnHPChanged?.Invoke();
            }
        }
        public int MaxHP => _maxHP;
        public int ProtectionHP => _protectionHP;

        public delegate void OnHPChangedEvent();
        public event OnHPChangedEvent OnHPChanged;
        public event OnHPChangedEvent OnPlayerDied;
        public event OnHPChangedEvent OnProtectionDestroyed;

        public HPController(int maxHp)
        {
            _maxHP = maxHp;
            Restart();
        }

        public void ApplyDamage(int damagePoints)
        {
            if(_protectionHP <= 0)
            {
                CurrentHP -= damagePoints;
                return;
            }
            
            if(damagePoints >= _protectionHP)
            {
                _protectionHP -= damagePoints;
                CurrentHP += _protectionHP;
                _protectionHP = 0;
                OnProtectionDestroyed?.Invoke();
                return;
            }

            _protectionHP -= damagePoints;
            OnHPChanged?.Invoke();
        }

        public void ApplyHeal(int healPoints)
        {
            if(healPoints + CurrentHP > _maxHP)
            {
                CurrentHP = _maxHP;
                return;
            }

            CurrentHP += healPoints;
        }

        public void ApplyProtection(int protectionPoints)
        {
            _protectionHP += protectionPoints;
        }

        public void Restart()
        {
            CurrentHP = _maxHP;
            _protectionHP = 0;
            OnProtectionDestroyed?.Invoke();
        }

        public void RemoveProtection()
        {
            _protectionHP = 0;
            OnProtectionDestroyed?.Invoke();
        }
    }
}
