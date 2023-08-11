using Karabaev.GameKit.Common.Utils;
using UnityEngine;

namespace Karabaev.Survival.Game.Damageable
{
  public class HitImpactView : MonoBehaviour
  {
    [SerializeField, HideInInspector]
    private ParticleSystem _particleSystem = null!;
    
    public void Play() => _particleSystem.Play(true);

    private void OnValidate() => _particleSystem = this.RequireComponent<ParticleSystem>();
  }
}