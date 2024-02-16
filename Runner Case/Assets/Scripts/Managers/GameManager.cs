using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        #region Variables

        private float _playerMoney;
        public float PlayerMoney
        {
            get => _playerMoney;
            set
            {
                _playerMoney = value;
                EventManager.Instance.OnMoneyChange.Invoke();
            }
        }
        
        private float _playerHealth;
        public float PlayerHealth
        {
            get => _playerHealth;
            set
            {
                _playerHealth = value;
                EventManager.Instance.OnPlayerHealthChange.Invoke();
            }
        }
        
        private float _playerScore;
        public float PlayerScore
        {
            get => _playerScore;
            set
            {
                _playerScore = value;
                EventManager.Instance.OnScoreChange.Invoke();
            }
        }
        
        private float _playerHighScore;
        public float PlayerHighScore
        {
            get => _playerHighScore;
            set
            {
                _playerHighScore = value;
                EventManager.Instance.OnHighScoreChange.Invoke();
            }
        }

        [Header("Pools")]
        [SerializeField] Pool coinPool;
        [SerializeField] Pool wayPartPool;
        public Pool CoinPool => coinPool;
        public Pool WayPartPool => wayPartPool;
        public bool HasGameStart { get; private set; }
        
        #endregion

        #region Unity Functions

        private void Awake()
        {
            LoadData();
        }
        private void OnEnable()
        {
            EventManager.Instance.OnGameStart.AddListener(() => HasGameStart = true);
            EventManager.Instance.OnGameEnd.AddListener(() => HasGameStart = false);
        }
        private void OnDisable()
        {
            SaveData();
        }

        #endregion

        #region Save - Load

        private void LoadData()
        {
            _playerHealth = 3;
            _playerMoney = PlayerPrefs.GetFloat("PlayerMoney", 0);
            _playerHighScore = PlayerPrefs.GetFloat("PlayerHighScore", 0);
        }

        private void SaveData()
        {
            PlayerPrefs.SetFloat("PlayerMoney", _playerMoney);
            PlayerPrefs.SetFloat("PlayerHighScore", _playerHighScore);
        }

        #endregion
    }
}
