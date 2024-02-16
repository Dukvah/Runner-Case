using System.Collections.Generic;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        #region Variables

        [Header("UI Menu Panels")]
        [SerializeField] GameObject startPanel;
        [SerializeField] GameObject inGamePanel;
        [SerializeField] GameObject endGamePanel;
    
        [Header("UI Objects")]
        [SerializeField] TextMeshProUGUI healthText;
        [SerializeField] GameObject healthIcon;
        [SerializeField] TextMeshProUGUI moneyText;
        [SerializeField] GameObject moneyIcon;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] Animator scoreTextAnimator;
        [SerializeField] TextMeshProUGUI endGameScoreText;
        [SerializeField] GameObject newHighScoreIcon;
        [SerializeField] Button startButton, restartButton; // Buttons to start and restart the game
    
        [Tooltip("Symbols for abbreviating large values")]
        [SerializeField] List<string> moneyMulti = new();

        #endregion

        #region Unity Functions

        private void Awake()
        {
            ButtonInitialize();
        }
    
        private void Start()
        {
            EventManager.Instance.OnMoneyChange.Invoke();
            EventManager.Instance.OnPlayerHealthChange.Invoke();
        
            ShowPanel(startPanel);
        }
    
        private void OnEnable()
        {
            EventManager.Instance.OnGameStart.AddListener(GameStart);
            EventManager.Instance.OnGameEnd.AddListener(GameEnd);
            EventManager.Instance.OnGameRestart.AddListener(() => SceneManager.LoadScene(0));
        
            EventManager.Instance.OnPlayerHealthChange.AddListener(SetHealthText);
            EventManager.Instance.OnMoneyChange.AddListener(SetMoneyText);
            EventManager.Instance.OnScoreChange.AddListener(SetScoreText);
            EventManager.Instance.OnHighScoreChange.AddListener(OnHighScoreChanged);
        }

        private void OnDisable()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.OnGameStart.RemoveListener(GameStart);
                EventManager.Instance.OnGameEnd.RemoveListener(GameEnd);
                EventManager.Instance.OnGameRestart.RemoveListener(() => SceneManager.LoadScene(0));
            
                EventManager.Instance.OnPlayerHealthChange.RemoveListener(SetHealthText);
                EventManager.Instance.OnMoneyChange.RemoveListener(SetMoneyText);
                EventManager.Instance.OnScoreChange.RemoveListener(SetScoreText);
                EventManager.Instance.OnHighScoreChange.RemoveListener(OnHighScoreChanged);

            }
        }

        #endregion

        #region UI Functions

        private void ButtonInitialize()
        {
            startButton.onClick.AddListener(() => EventManager.Instance.OnGameStart.Invoke());
            restartButton.onClick.AddListener(() => EventManager.Instance.OnGameRestart.Invoke());
        }

        private void GameStart()
        {
            ShowPanel(inGamePanel);
        }

        private void GameEnd()
        {
            endGameScoreText.text = $"High Score : {GameManager.Instance.PlayerHighScore.ToString()}\nScore : {GameManager.Instance.PlayerScore.ToString()}";
        
            ShowPanel(endGamePanel);
        }

        private void OnHighScoreChanged()
        {
            newHighScoreIcon.SetActive(true);
            scoreTextAnimator.enabled = true;
        }
    
        private void SetHealthText()
        {
            healthIcon.transform.DORewind();
            healthIcon.transform.DOPunchScale(Vector3.one, 0.5f, 2, 1);

            healthText.text = GameManager.Instance.PlayerHealth.ToString();
        }
    
        private void SetMoneyText()
        {
            moneyIcon.transform.DORewind();
            moneyIcon.transform.DOPunchScale(Vector3.one, 0.5f, 2, 1);

            int moneyDigit = GameManager.Instance.PlayerMoney.ToString().Length;
            int value = (moneyDigit - 1) / 3;
            if (value < 1)
            {
                moneyText.text = GameManager.Instance.PlayerMoney.ToString();
            }
            else
            {
                float temp = GameManager.Instance.PlayerMoney / Mathf.Pow(1000, value);
                moneyText.text = temp.ToString("F2") + " " + moneyMulti[value];
            }
        }
        private void SetScoreText()
        {
            scoreText.text = $"Score : {GameManager.Instance.PlayerScore}";
        
            if (GameManager.Instance.PlayerScore > GameManager.Instance.PlayerHighScore)  
            {
                GameManager.Instance.PlayerHighScore = GameManager.Instance.PlayerScore;
            }
        }
    
        private void ShowPanel(GameObject panel)
        {
            CloseAllPanels();
            panel.SetActive(true);
        }
    
        private void CloseAllPanels()
        {
            startPanel.SetActive(false);
            inGamePanel.SetActive(false);
            endGamePanel.SetActive(false);
        }

        #endregion
    }
}
