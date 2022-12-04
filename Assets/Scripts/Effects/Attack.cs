using Player;

namespace Effects
{
    public class Attack : Effect
    {
        private int _attackPower;
        private int _stepsNumber;
        private int _stepsCount;
        private HPController _currentHPController;

        public override EffectType Type => EffectType.Attack;
        public override event IEffect.OnEffectFinishedEvent OnFinished;

        public Attack(int effectPower, int stepsNumber) : base(effectPower, stepsNumber)
        {
            _attackPower = effectPower;
            _stepsNumber = stepsNumber;
        }

        public override void Run(HPController hpController)
        {
            _currentHPController = hpController;
            _currentHPController.ApplyDamage(_attackPower);
            _stepsCount = 1;
        }

        public override void IncreaseStepsCount()
        {
            if (_stepsCount >= _stepsNumber)
            {
                OnFinished?.Invoke();
                return;
            }
            _stepsCount++;
            _currentHPController.ApplyDamage(_attackPower);
        }
    }
}
