using UnityEngine;

namespace Karabaev.Survival.Audio
{
  public class AudioService
  {
    private const float SFXVolume = 1.0f;
    
    public void PlaySFX(AudioSource source, AudioClip clip) => source.PlayOneShot(clip, SFXVolume);
  }
}