using System;
using App.Scripts.MainMenuScene;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace App.Scripts.GameScene.UI
{
    public class GamePauseUI : MonoBehaviour
    {
        private static GamePauseUI _instance;

        public static GamePauseUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GamePauseUI>();
                }

                return _instance;
            }
        }

        [SerializeField] private CanvasGroup gameWindowCanvasGroup;
        [SerializeField] private CanvasGroup gameButtonCanvasGroup;
        [SerializeField] private Button retryButton;
        [SerializeField] private Image turnsPanel;
        [SerializeField] private TMP_Text turnsTextField;

        public event Action<bool> OnGamePause;
        private bool _isGamePaused;

        private SwipeSystem _swipeSystem;

        void Start()
        {
            InitializeUI();
            InitializeButtons();
        }

        private void InitializeUI()
        {
            _swipeSystem = FindObjectOfType<SwipeSystem>();
            if (_swipeSystem != null)
            {
                _swipeSystem.OnInteractWithDangerObject += GameOver;
            }

            LevelTurnsCount.Instance.OnTurnsCountChanged += ChangeTurnsCountUI;
            LevelTurnsCount.Instance.OnLevelSwipesOver += GameOver;

            turnsTextField.text = LevelTurnsCount.Instance.GetRemainingTurns().ToString();
            gameObject.SetActive(true);
            gameButtonCanvasGroup.alpha = 1f;
            ShowPauseMenu(false);
        }

        private void InitializeButtons()
        {
            retryButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayBackgroundMusic();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }
        private void ChangeTurnsCountUI(int turnsCount)
        {
            turnsTextField.text = turnsCount.ToString();
        }

        private void GameOver()
        {
            gameObject.SetActive(false);
        }

        private void ShowPauseMenu(bool isPaused)
        {
            gameButtonCanvasGroup.gameObject.SetActive(!isPaused);
            turnsPanel.gameObject.SetActive(!isPaused);
            retryButton.gameObject.SetActive(!isPaused);
            gameWindowCanvasGroup.gameObject.SetActive(isPaused);
        }

        public void PauseGame()
        {
            _isGamePaused = true;
            ShowPauseMenu(_isGamePaused);
            OnGamePause?.Invoke(_isGamePaused);
        }

        public void UnPauseGame()
        {
            _isGamePaused = false;
            ShowPauseMenu(_isGamePaused);
            OnGamePause?.Invoke(_isGamePaused);
        }

        public void ExitFromLevel()
        {
            LoadingScenesDirectory.Instance.LoadLevelsListScene();
        }

        private void OnDestroy()
        {
            if (_swipeSystem != null)
            {
                _swipeSystem.OnInteractWithDangerObject -= GameOver;
            }

            if (LevelTurnsCount.Instance)
            {
                LevelTurnsCount.Instance.OnTurnsCountChanged -= ChangeTurnsCountUI;
                LevelTurnsCount.Instance.OnLevelSwipesOver -= GameOver;
            }
        }
    }
}