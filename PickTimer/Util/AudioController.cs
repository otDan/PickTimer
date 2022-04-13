using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PickTimer.Asset;
using Sonigon;
using UnityEngine;

namespace PickTimer.Util
{
    internal class AudioController
    {
        private static readonly List<AudioClip> TimerTicksClips = new();
        private static readonly SoundParameterIntensity SoundParameterIntensity = new(0f, UpdateMode.Continuous);

        private static void Play(AudioClip audioClip, Transform transform)
        {
            SoundParameterIntensity.intensity = 1f;
            SoundContainer soundContainer = ScriptableObject.CreateInstance<SoundContainer>();
            soundContainer.setting.volumeIntensityEnable = true;
            soundContainer.audioClip[0] = audioClip;
            SoundEvent soundEvent = ScriptableObject.CreateInstance<SoundEvent>();
            soundEvent.soundContainerArray[0] = soundContainer;
            SoundParameterIntensity.intensity = Optionshandler.vol_Sfx / 1f * ConfigController.TimerVolumeConfig.Value * Optionshandler.vol_Master;
            SoundManager.Instance.Play(soundEvent, transform, SoundParameterIntensity);
        }

        public static void PlayRandomTickClip(Transform transform)
        {
            int randomAudioClipIndex = UnityEngine.Random.Range(0, TimerTicksClips.Count);
            var audioClip = TimerTicksClips[randomAudioClipIndex];
            Play(audioClip, transform);
        }

        public static void LoadClips()
        {
            for (int i = 0; i < 12; i++)
            {
                var timerTick = AudioClipSplit(AssetManager.TimerTicksClip, i + 0.25f, i + 0.75f);
                TimerTicksClips.Add(timerTick);
            }
        }
        
        private static AudioClip AudioClipSplit(AudioClip clip, float start, float stop)
        {
            int frequency = clip.frequency;
            float timeLength = stop - start;
            int samplesLength = (int)(frequency * timeLength);
            AudioClip newClip = AudioClip.Create(clip.name + "-sub" + start + "-" + stop, samplesLength, 1, frequency, false);
            float[] data = new float[samplesLength];
            clip.GetData(data, (int)(frequency * start));
            newClip.SetData(data, 0);
            return newClip;
        }
    }
}
