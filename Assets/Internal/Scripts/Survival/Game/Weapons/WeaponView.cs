using System;
using Cysharp.Threading.Tasks;
using Karabaev.GameKit.Common.Utils;
using UnityEngine;

namespace Karabaev.Survival.Game.Weapons
{
  public class WeaponView : MonoBehaviour
  {
    [field: SerializeField, HideInInspector]
    public Transform ProjectileSpawnPoint { get; private set; } = null!;
    [SerializeField, HideInInspector]
    private Transform _muzzle = null!;
    [field: SerializeField, HideInInspector]
    public AudioSource ShotAudioSource { get; private set; } = null!;

    private void Awake() => _muzzle.SetActive(false);

    public async UniTaskVoid PlayMuzzleAsync()
    {
      _muzzle.SetActive(true);
      await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
      _muzzle.SetActive(false);
    }

    private void OnValidate()
    {
      ProjectileSpawnPoint = this.RequireComponentInChild<Transform>("ProjectileSpawnPoint");
      _muzzle = this.RequireComponentInChild<Transform>("Muzzle");
      ShotAudioSource = this.RequireComponent<AudioSource>();
    }
  }
}