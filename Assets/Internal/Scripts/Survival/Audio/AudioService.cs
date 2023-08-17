using JetBrains.Annotations;
using UnityEngine;

namespace Karabaev.Survival.Audio
{
  [UsedImplicitly]
  public class AudioService
  {
    public void PlaySFX(AudioSource source, AudioClip clip) => source.PlayOneShot(clip);
  }
}