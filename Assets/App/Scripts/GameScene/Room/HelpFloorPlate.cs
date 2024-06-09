using App.Scripts.GameScene.GameItems;
using App.Scripts.MainMenuScene;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.GameScene.Room
{
    public class HelpFloorPlate : MonoBehaviour
    {
        [SerializeField] private float requiredTimeToHelp;
        [SerializeField] private GameObject ufo;
        private float _timeInHelpZone = 0f;

        private void OnTriggerStay(Collider other)
        {
            HelpMovableObject helpMovableObject = other.GetComponent<HelpMovableObject>();
            if (helpMovableObject != null)
            {
                _timeInHelpZone += Time.deltaTime;
                if (_timeInHelpZone > requiredTimeToHelp)
                {
                    StartKidnappingProcess(helpMovableObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _timeInHelpZone = 0f;
        }

        private void StartKidnappingProcess(HelpMovableObject helpMovableObject)
        {
            helpMovableObject.IsBeingKidnapped = true;
            AudioManager.Instance.StopBackgroundMusic();
            AudioManager.Instance.PlayUfoSound();
            MoveUFOAndObject(helpMovableObject);
        }

        private void MoveUFOAndObject(HelpMovableObject helpMovableObject)
        {
            ufo.transform.DOMove(ufo.transform.position * 1.5f, 1f);
            helpMovableObject.transform.DOMove(ufo.transform.position * 2, 2f);
            helpMovableObject.transform.DOScale(0, 2f).OnComplete(() =>
            {
                Destroy(helpMovableObject.gameObject);
                Destroy(ufo.gameObject);
                AudioManager.Instance.PlayBackgroundMusic();
            });
        }
    }
}