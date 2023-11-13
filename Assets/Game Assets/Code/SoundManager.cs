using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class SoundManager : Singleton<SoundManager>
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip[] clips;
        public float volume = 1.0f;
        public float pitch = 1.0f;
        public bool isMusic = false;
        public bool loop = false;
        [HideInInspector] public bool mute = false;

        [HideInInspector] public AudioSource source;
    }

    public List<Sound> sounds = new List<Sound>();

    private void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clips[0];
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            if (sound.isMusic)
            {
                sound.source.spatialBlend = 0; // Music should be 2D
            }
        }
    }

    public void PlaySound(string soundName, bool loop = false, float volume = -1f, float pitch = -1f)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound != null)
        {
            sound.source.volume = sound.mute ? 0f : volume;
            
            if (volume == -1) sound.source.volume = sound.volume;
            if (pitch == -1) sound.source.pitch = sound.pitch;
            sound.source.loop = loop;
            
            if (sound.clips.Length > 1)
            {
                sound.source.clip = sound.clips[Random.Range(0, sound.clips.Length)];
            }
            sound.source.Play();
        }
    }

    public void StopSound(string soundName)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound != null)
        {
            sound.source.Stop();
        }
    }

    public void PauseSound(string soundName)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound != null)
        {
            if (sound.source.isPlaying)
            {
                sound.source.Pause();
            }
        }
    }

    public void ToggleMute(string soundName)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound != null)
        {
            sound.mute = !sound.mute;
            sound.source.mute = sound.mute;
        }
    }

    public void SetVolume(string soundName, float volume)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound != null)
        {
            sound.volume = Mathf.Clamp(volume, 0f, 1f);
            sound.source.volume = sound.mute ? 0f : sound.volume;
        }
    }

    private const float defaultFadeDuration = 1.5f;
    public void FadeSound(string soundName, float targetVolume, float duration = defaultFadeDuration)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound != null)
        {
            if (sound.mute) return;
            sound.source.DOFade(targetVolume, duration);
        }
    }
}
