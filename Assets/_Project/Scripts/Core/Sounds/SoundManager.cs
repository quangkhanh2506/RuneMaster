using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Serialization;

namespace Core
{
    public class SoundManager : SingletonMono<SoundManager>
    {
        public bool isMuteBGM = false;
        public bool isMuteSFX = false;
        [SerializeField]
        private List<SoundFXItem> soundFXItems = new List<SoundFXItem>();
        private Dictionary<SoundIndex, SoundFXItem> dicSoundFXs = new Dictionary<SoundIndex, SoundFXItem>();            
        private List<AudioClip> lsSoundBGMs = new List<AudioClip>();
        private AudioSource bgmSource;

        public bool isCancelLoading;

        private void Awake()
        {            
            dicSoundFXs.Clear();
            for(int i = 0; i < soundFXItems.Count; i++)
            {
                dicSoundFXs.Add(soundFXItems[i].soundFXIndex, soundFXItems[i]);                
            }
            bgmSource = GetComponent<AudioSource>();
        }

        public void AddSoundBGM(AudioClip soundBGM)
        {
            if (isMuteBGM)
                return;
            
            lsSoundBGMs.Clear();
            lsSoundBGMs.Add(soundBGM);
        }

        public void PlaySoundBGM(float volume = 1, bool isLoop = false)
        {
            bgmSource.clip = lsSoundBGMs[0];
            bgmSource.Play();
            bgmSource.volume = 0;
            bgmSource.DOFade(1, 0.25f);            
        }


        public void ResumeSoundBGM()
        {          
            bgmSource.Play();
        }

        public void PauseSoundBGM()
        {
          
            bgmSource.Pause();
        }

        public float GetTimeBGM()
        {
         
            if (bgmSource.clip != null)
            {
                return bgmSource.time;
            }
            return 0;
        }

        public void StopSoundBGM()
        {
            
            bgmSource.Stop();
        }

        public void PlaySoundFX(SoundIndex soundIndex, bool isLoop = false)
        {
            isMuteSFX = true;
            if (isMuteSFX)
                return;
            EazySoundManager.PlaySound(dicSoundFXs[soundIndex].soundFXClip, isLoop);
        }

        public void PauseSoundFX (SoundIndex soundIndex)
        {
            Audio audio = EazySoundManager.GetSoundAudio(dicSoundFXs[soundIndex].soundFXClip);
            
            if (audio != null)
            {                                
                audio.Stop();
                
            }

        }

        public void StopAll()
        {
            EazySoundManager.StopAllSounds();
        }

        public bool CheckSoundFXAvailable(SoundIndex soundIndex)
        {
            Audio audio = EazySoundManager.GetSoundAudio(dicSoundFXs[soundIndex].soundFXClip);

            if (audio != null && audio.IsPlaying)
            {
                return true;
            }

            return false;
        }

        public void Mute()
        {
            isMuteBGM = true;
        }

        public void Unmute()
        {
            for (SoundIndex i = SoundIndex.Click; i < SoundIndex.COUNT; i++)
            {
                PauseSoundFX(i);
            }
            isMuteBGM = false;
        }

        public void GetSongGameplay(int indexMod, string nameSong)
        {
            if (indexMod == 0)
            {
                UIManager.Instance.ShowUI(UIIndex.UILoading);                
                //UILoading uILoading = (UILoading)UIManager.Instance.FindUIVisible(UIIndex.UILoading);
                AudioClip songAudioClip = Resources.Load("Sounds/Inst-" + nameSong) as AudioClip;
               
                
            }
        }

        public float GetSongLength()
        {
            if (lsSoundBGMs.Count > 0)
            {
                return lsSoundBGMs[0].length;
            }

            return 0;
        }
        
    }

    public enum SoundIndex
    {
        Click = 0,
        One,
        Two,
        Three,
        Go,
        SoundMenu,
        GameOver,
        MissNote,
        ConfirmMenu,
        Victory,
        COUNT,
    }
    [Serializable]
    public class SoundFXItem
    {
        public SoundIndex soundFXIndex;
        public AudioClip soundFXClip;
    }
}

