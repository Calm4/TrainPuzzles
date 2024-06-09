using UnityEngine;

namespace App.Scripts.MainMenuScene
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField]
        private AudioSource musicAudioSource;
        [SerializeField]
        private AudioSource sfxAudioSource;

        [SerializeField] private AudioClip backgroundMusic; 
        [SerializeField] private AudioClip alarmClockSound; 
        [SerializeField] private AudioClip ufoSound; 
        [SerializeField] private AudioClip defeatSound; 
        [SerializeField] private AudioClip completeSound; 

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
                return;
            }

            PlayBackgroundMusic();
        }

        public void ButtonClickSound()
        {
            sfxAudioSource.PlayOneShot(ufoSound);
        }
        public void PlayBackgroundMusic()
        {
            musicAudioSource.clip = backgroundMusic;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }

        public void StopBackgroundMusic()
        {
            musicAudioSource.Stop();
        }
        public void PlayAlarmClockSound()
        {
            sfxAudioSource.PlayOneShot(alarmClockSound);
        }

        public void PlayUfoSound()
        {
            sfxAudioSource.PlayOneShot(ufoSound);
        }

        public void PlayDefeatSound()
        {
            sfxAudioSource.PlayOneShot(defeatSound);
        }

        public void PlayCompleteSound()
        {
            sfxAudioSource.PlayOneShot(completeSound);
        }
    }
}