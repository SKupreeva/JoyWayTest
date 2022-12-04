using DG.Tweening;
using Player;
using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    // class spawns effects on map, controls enemy effects moves, detects turn changes

    public class EffectController : MonoBehaviour
    {
        [SerializeField] private List<EffectMovesController> _effectPrefabs = new List<EffectMovesController>();
        [SerializeField] private RectTransform _effectSpawnPoint;
        [SerializeField] private Canvas _canvas;

        private List<Effect> _playerEffects = new List<Effect>();
        private List<Effect> _enemyEffects = new List<Effect>();

        private List<PlayerController> _playerCharacters = new List<PlayerController>();
        private List<PlayerController> _enemyCharacters = new List<PlayerController>();

        private List<Effect> _effectsInUse = new List<Effect>();
        private List<Effect> _effectsToRemove = new List<Effect>();

        public delegate void OnEffectUsedEvent();
        public event OnEffectUsedEvent OnEffectUsed;

        public void Setup(List<PlayerController> playerCharacters, List<PlayerController> enemyCharacters)
        {
            _playerCharacters = playerCharacters;
            _enemyCharacters = enemyCharacters;

            SetLists();
        }

        public void SpawnPlayerEffect()
        {
            foreach (var effect in _enemyEffects)
            {
                _effectsInUse.Remove(effect);
            }

            var newEffect = GetUnusedEffect();
            var targets = GetTargetListForPlayer(newEffect.Type);
            var spawnedEffect = SpawnEffect(newEffect, targets);
            spawnedEffect.OnEffectApplied += () =>
            {
                _effectsInUse.Add(newEffect);
                newEffect.OnFinished += () => _effectsToRemove.Add(newEffect);
                OnEffectUsed?.Invoke();
            };
        }

        public void SpawnEnemyEffect()
        {
            var currentEffect = _enemyEffects[0];
            foreach (var effect in _enemyEffects)
            {
                if (effect.Type == EffectType.Attack)
                {
                    currentEffect = effect;
                    break;
                }
            }

            var newEffect = SpawnEffect(currentEffect, _playerCharacters);
            if (newEffect == null)
            {
                Debug.LogError("Spawn enemy effect error. New effect is null");
                return;
            }

            var strongestPlayer = _playerCharacters[0];
            foreach (var target in _playerCharacters)
            {
                if (target.CurrentHP > strongestPlayer.CurrentHP)
                {
                    strongestPlayer = target;
                }
            }

            newEffect.transform.DOMove(strongestPlayer.transform.position, 1f).OnComplete(() => 
            {
                strongestPlayer.ApplyEffect(currentEffect);
                _effectsInUse.Add(currentEffect);
                Destroy(newEffect.gameObject);
            });
        }

        public void Restart()
        {
            foreach (Transform child in _effectSpawnPoint)
            {
                Destroy(child.gameObject);
            }
        }

        public void IncreaseStepsCount()
        {
            foreach(var effect in _effectsInUse)
            {
                effect.IncreaseStepsCount();
            }
            foreach(var effect in _effectsToRemove)
            {
                _effectsInUse.Remove(effect);
            }
            _effectsToRemove.Clear();
        }

        private EffectMovesController SpawnEffect(Effect effect, List<PlayerController> targets)
        {
            foreach (var prefab in _effectPrefabs)
            {
                if (effect.Type == prefab.EffectType)
                {
                    var effectMoves = Instantiate(prefab, _effectSpawnPoint);
                    effectMoves.GetComponent<RectTransform>().anchoredPosition = _effectSpawnPoint.position;
                    effectMoves.Setup(targets, _canvas, effect);
                    return effectMoves;
                }
            }
            return null;
        }

        private List<PlayerController> GetTargetListForPlayer(EffectType effectType)
        {
            if(effectType == EffectType.Heal || effectType == EffectType.Protection)
            {
                return _playerCharacters;
            }

            return _enemyCharacters;
        }

        private Effect GetUnusedEffect()
        {
            var newEffect = _playerEffects[Random.Range(0, _playerEffects.Count)];
            if (_effectsInUse.Contains(newEffect))
            {
                return GetUnusedEffect();
            }
            return newEffect;
        }

        private void SetLists()
        {
            _playerEffects = new List<Effect>
            {
                new Attack(3, 1),
                new Heal(1, 1),
                new Poison(1, 2),
                new Protection(5, 3)
            };

            _enemyEffects = new List<Effect>
            {
                new Attack(3, 1)
            };
        }
    }
}
