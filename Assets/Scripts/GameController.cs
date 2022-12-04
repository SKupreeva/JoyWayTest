using Effects;
using InputControls;
using Player;
using System.Collections.Generic;
using UIControls;
using UnityEngine;

namespace GameConreols
{
    // class controls turns changing, handles map buttons clickes, manages main controllers

    public class GameController : MonoBehaviour
    {
        [SerializeField] private UIController _uiController;
        [SerializeField] private InputController _inputController;
        [SerializeField] private EffectController _effectController;
        [SerializeField] private float _enemyTurnTime = 3f;
        [SerializeField] private List<PlayerController> _characters = new List<PlayerController> ();

        private bool _isPlayerTurn = true;
        private bool _isPlayerAlive = true;
        private float _enemyTurnTimer = 0f;

        private void Awake()
        {
            Setup();
        }

        private void Start()
        {
            StartPlayerTurn();
        }

        private void Update()
        {
            _enemyTurnTimer += Time.deltaTime;
            if (!_isPlayerTurn && _enemyTurnTimer >= _enemyTurnTime && _isPlayerAlive)
            {
                _isPlayerTurn = true;
                _uiController.OnPlayerTurnStarted();
                StartPlayerTurn();
            }
        }

        private void OnDestroy()
        {
            _uiController.FinishTurnBtn.onClick.RemoveAllListeners();
            _uiController.SkipTurnBtn.onClick.RemoveAllListeners();

            _inputController.OnRKeyClicked -= RestartGame;
            _inputController.OnSpaceClicked -= StartEnemyTurn;

            _effectController.OnEffectUsed -= OnEffectUsed;
        }

        private void StartEnemyTurn()
        {
            _uiController.OnPlayerTurnEnded();
            _effectController.IncreaseStepsCount();
            _enemyTurnTimer = 0;
            _isPlayerTurn = false;

            foreach (var character in _characters)
            {
                if (!character.IsPlayer)
                {
                    _effectController.SpawnEnemyEffect();
                }
            }
        }

        private void SkipPlayerTurn()
        {
            if (!_isPlayerTurn)
            {
                return;
            }
            _effectController.Restart();
            StartEnemyTurn();
        }

        private void RestartGame()
        {
            foreach(var character in _characters)
            {
                character.Restart();
                _uiController.Setup();
            }
            _effectController.Restart();
            StartPlayerTurn();
            _isPlayerAlive = true;
        }

        private void OnCharacterDied()
        {
            int enemyCount = 0;
            int playerCount = 0;

            foreach (var character in _characters)
            {
                if (character.IsPlayer && character.IsAlive)
                {
                    playerCount++;
                }

                if (!character.IsPlayer && character.IsAlive)
                {
                    enemyCount++;
                }
            }

            if(playerCount <= 0)
            {
                _uiController.OnPlayerLoose();
            }

            if(enemyCount <= 0)
            {
                _uiController.OnPlayerWon();
            }

            _effectController.Restart(); 
            _isPlayerAlive = false;
        }

        private void StartPlayerTurn()
        {
            foreach (var character in _characters)
            {
                if (character.IsPlayer)
                {
                    _effectController.SpawnPlayerEffect();
                }
            }
        }

        private void SetupTargetsLists()
        {
            var playerCharacters = new List<PlayerController>();
            var enemyCharacters = new List<PlayerController>();

            foreach (var character in _characters)
            {
                character.OnCharacterDied += OnCharacterDied;
                if (character.IsPlayer)
                {
                    playerCharacters.Add(character);
                }
                else
                {
                    enemyCharacters.Add(character);
                }
            }

            _effectController.Setup(playerCharacters, enemyCharacters);
        }

        private void Setup()
        {
            _uiController.FinishTurnBtn.onClick.AddListener(StartEnemyTurn);
            _uiController.SkipTurnBtn.onClick.AddListener(SkipPlayerTurn);

            _inputController.OnRKeyClicked += RestartGame;
            _inputController.OnSpaceClicked += SkipPlayerTurn;

            _effectController.OnEffectUsed += OnEffectUsed;

            _uiController.Setup();

            SetupTargetsLists();
        }

        private void OnEffectUsed()
        {
            _uiController.OnEffectUsed();
        }
    }
}
