using App.Scripts.MainMenuScene;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace App.Scripts.LevelsListScene
{
    public class LevelsListManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelsContainer;
        [SerializeField] private GameObject levelPrefab;
        [SerializeField] private Button backToMainMenu;
        [SerializeField] private Color levelComplete;
        [SerializeField] private Color levelNotComplete;

        private LevelsManager _levelsManager;
        private const int NumberOfNonGameScenes = 2;
        private int _levelsCount;

        private void Start()
        {
            _levelsManager = FindObjectOfType<LevelsManager>(); 
            _levelsCount = SceneManager.sceneCountInBuildSettings - NumberOfNonGameScenes;
            backToMainMenu.onClick.AddListener(() => LoadingScenesDirectory.Instance.LoadMainMenu());
            
            InitializeLevels();
        }
    
        private void InitializeLevels()
        {
            ClearLevelsList();
            for (var i = 1; i <= _levelsCount; i++) 
            {
                CreateLevelButton(i);
            }
        }

        private void CreateLevelButton(int levelNumber)
        {
            bool isLevelPassed = _levelsManager.IsLevelPassed(levelNumber); 
            GameObject levelUI = Instantiate(levelPrefab, transform.position, Quaternion.identity);
            levelUI.transform.SetParent(levelsContainer.transform);

            Button button = levelUI.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => LoadLevel(levelNumber));

            TMP_Text levelText = button.GetComponentInChildren<TMP_Text>();
            levelText.text = "Level " + levelNumber; 
            levelText.color = isLevelPassed ? levelComplete : levelNotComplete;
        }

        private void LoadLevel(int levelNumber)
        {
            _levelsManager.LoadLevel(levelNumber + 1); 
            AudioManager.Instance.PlayBackgroundMusic();
        }

        private void ClearLevelsList()
        {
            foreach (Transform child in levelsContainer.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
