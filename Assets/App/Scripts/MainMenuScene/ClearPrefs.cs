using UnityEngine;

namespace App.Scripts.MainMenuScene
{
    public class ClearPrefs : MonoBehaviour
    {
        private static ClearPrefs Instance { get; set; }
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                PlayerPrefs.DeleteAll();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
