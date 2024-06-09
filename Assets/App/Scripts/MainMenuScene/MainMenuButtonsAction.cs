using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.MainMenuScene
{
    public class MainMenuButtonsAction : SerializedMonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;

        void Start()
        {
            playButton.onClick.AddListener(() =>  LoadingScenesDirectory.Instance.LoadLevelsListScene());
            exitButton.onClick.AddListener(Application.Quit);
        }
    }

}