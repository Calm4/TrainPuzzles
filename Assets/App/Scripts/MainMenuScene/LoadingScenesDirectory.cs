using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Scripts.MainMenuScene
{
    public class LoadingScenesDirectory : MonoBehaviour
    {
        private const string MainMenuScene = "MainMenuScene";
        private const string LevelsListScene = "LevelsListScene";

        public static LoadingScenesDirectory Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        public void LoadMainMenu()
        {
            SceneManager.LoadScene(MainMenuScene);
        }

        public void LoadLevelsListScene()
        {
            SceneManager.LoadScene(LevelsListScene);
        }
    }
}
