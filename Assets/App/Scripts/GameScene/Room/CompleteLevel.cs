using System;
using App.Scripts.GameScene.GameItems;
using App.Scripts.MainMenuScene;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Scripts.GameScene.Room
{
    public class CompleteLevel : MonoBehaviour
    {
        private static CompleteLevel _instance;
        public static CompleteLevel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CompleteLevel>();
                }
                return _instance;
            }
        }
        private LevelsManager _levelsManager;
        private int _levelID;
        private const string LevelScenePrefix = "LevelScene_";


        [SerializeField, Range(0, 5)] private float kidnappingDuration;
        [SerializeField] private Transform kidnappingPosition;

        private void Awake()
        {
            _levelsManager = LevelsManager.Instance; 
        }

        private void Start()
        {
            GetCurrentLevelID();
        }

        private void GetCurrentLevelID()
        {
            string levelName = SceneManager.GetActiveScene().name;
            string levelNumberString = levelName.Substring(LevelScenePrefix.Length);
            
            if (Int32.TryParse(levelNumberString, out int levelID))
            {
                _levelID = levelID;
            }
            else
            {
                Debug.LogError("Unable to parse level ID from scene name.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovableObject>())
            {
                KidnapPlayer(other);
            }
        }

        private void KidnapPlayer(Collider player)
        {
            player.transform.DOMove(kidnappingPosition.position, kidnappingDuration);
            player.transform.DOScale(0, kidnappingDuration).OnComplete(
                () => LevelComplete(player));
        }

        private void LevelComplete(Collider player)
        {
            AudioManager.Instance.StopBackgroundMusic();
            AudioManager.Instance.PlayCompleteSound();
            
            _levelsManager.MarkLevelAsPassed(_levelID); 
            
            Destroy(player.gameObject);
            
            AudioManager.Instance.PlayBackgroundMusic();
            LoadingScenesDirectory.Instance.LoadLevelsListScene();
        }

        public bool LevelIsComplete()
        {
            return _levelsManager.IsLevelPassed(_levelID);
        }
    }
}
