using System;
using Cysharp.Threading.Tasks;
using Karabaev.GameKit.Common.Utils;
using UnityEngine;

namespace Karabaev.Survival.Game.Weapons
{
  public class WeaponView : MonoBehaviour
  {
    private const float BulletSpeed = 50.0f;
    private static readonly TimeSpan MuzzleDuration = TimeSpan.FromSeconds(0.1f);
    
    [SerializeField, HideInInspector]
    private Transform _projectileSpawnPoint = null!;
    [SerializeField, HideInInspector]
    private Transform _muzzle = null!;
    [SerializeField, HideInInspector]
    private AudioSource _shotAudioSource = null!;

    public AudioClip ShotSound { private get; set; } = null!;

    public TrailRenderer BulletProjectilePrefab { private get; set; } = null!;
    
    private void Awake() => _muzzle.SetActive(false);

    public void Shot(Vector3 hitPosition)
    {
      _shotAudioSource.PlayOneShot(ShotSound);
      PlayMuzzleAsync().Forget();
      LaunchBulletAsync(hitPosition).Forget();
    }

    private async UniTaskVoid LaunchBulletAsync(Vector3 hitPosition)
    {
      var bullet = Instantiate(BulletProjectilePrefab);
      bullet.transform.position = _projectileSpawnPoint.position;
      
      var startPosition = bullet.transform.position;
      var distance = Vector3.Distance(startPosition, hitPosition);
      var distanceLeft = distance;
      
      while(distanceLeft > 0.0f)
      {
        bullet.transform.position = Vector3.Lerp(startPosition, hitPosition, 1 - distanceLeft / distance);
        distanceLeft -= Time.deltaTime * BulletSpeed;
        await UniTask.Yield(Application.exitCancellationToken);
      }
      
      bullet.DestroyObject();
    }

    private async UniTaskVoid PlayMuzzleAsync()
    {
      _muzzle.SetActive(true);
      await UniTask.Delay(MuzzleDuration, cancellationToken: destroyCancellationToken);
      _muzzle.SetActive(false);
    }
    
    private void OnValidate()
    {
      _projectileSpawnPoint = this.RequireComponentInChild<Transform>("ProjectileSpawnPoint");
      _muzzle = this.RequireComponentInChild<Transform>("Muzzle");
      _shotAudioSource = this.RequireComponent<AudioSource>();
    }
  }
}