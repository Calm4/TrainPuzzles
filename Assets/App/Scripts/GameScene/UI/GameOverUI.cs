using App.Scripts.GameScene.Room;
using App.Scripts.MainMenuScene;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace App.Scripts.GameScene.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup gameOverUI;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button backButton;

        [SerializeField, Range(0f, 5f)] private float gameOverShowDuration;
        
        private SwipeSystem _swipeSystem;
        private LevelTurnsCount _levelTurnsCount;
        private CompleteLevel _levelCompete;

        private bool _isLevelCompleted;

        private void Start()
        {
            InitializeUI();
            InitializeButtons();
        }

        private void InitializeUI()
        {
            _swipeSystem = FindObjectOfType<SwipeSystem>();
            if (_swipeSystem != null)
            {
                _swipeSystem.OnInteractWithDangerObject += GameOver_HittingDangerObject;
            }

            _levelTurnsCount = FindObjectOfType<LevelTurnsCount>();
            if (_levelTurnsCount != null)
            {
                _levelTurnsCount = LevelTurnsCount.Instance;
                _levelTurnsCount.OnLevelSwipesOver += GameOver_TurnsOver;
            }

            _levelCompete = FindObjectOfType<CompleteLevel>();
            if (_levelCompete != null)
            {
                _levelCompete = CompleteLevel.Instance;
            }
            
            gameOverUI.gameObject.SetActive(false);
        }

        private void InitializeButtons()
        {
            retryButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayBackgroundMusic();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
            backButton.onClick.AddListener(() => LoadingScenesDirectory.Instance.LoadLevelsListScene());
        }

        private void GameOverLaunchingActions()
        {
            AudioManager.Instance.StopBackgroundMusic();
            AudioManager.Instance.PlayDefeatSound();
            gameOverUI.gameObject.SetActive(true);
            gameOverUI.DOFade(1, gameOverShowDuration);
        }

        private void GameOver_TurnsOver()
        {
            if (_levelCompete.LevelIsComplete())
                return;

            GameOverLaunchingActions();
        }

        private void GameOver_HittingDangerObject()
        {
            GameOverLaunchingActions();
        }

        private void OnDestroy()
        {
            if (_swipeSystem != null)
            {
                _swipeSystem.OnInteractWithDangerObject -= GameOver_HittingDangerObject;
            }

            if (_levelTurnsCount != null)
            {
                _levelTurnsCount.OnLevelSwipesOver -= GameOver_TurnsOver;
            }
        }
    }
}