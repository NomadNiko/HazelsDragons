using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public static class SoundManager 
{
    public enum Sound {
        DragonJump,
        Score,
        Lose,
        GameMusic,
        IntroMusic,
    }

    public static void PlaySound(Sound sound, float volume) {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound), volume);
    }

    public static AudioClip GetAudioClip(Sound sound) {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray) {
            if (soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }

        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

}
