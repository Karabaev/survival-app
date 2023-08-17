using System;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Audio;
using Karabaev.Survival.Game.Damageable;
using Karabaev.Survival.Game.Loot;
using Karabaev.Survival.Game.Weapons;
using Karabaev.Survival.Game.Weapons.Descriptors;
using UnityEngine;
using VContainer;

namespace Karabaev.Survival.Game.Hero
{
  public class HeroView : UnityView, IDamageableView
  {
    [SerializeField, HideInInspector]
    private HeroAnimationView _animationView = null!;
    [SerializeField, HideInInspector]
    private CharacterController _characterController = null!;
    [SerializeField, HideInInspector]
    private AudioSource _footStepAudioSource = null!;

    [Inject]
    private readonly AudioService _audioService = null!;
    
    public Vector3 Position
    {
      get => transform.position;
      set => transform.position = value;
    }
    
    public Quaternion Rotation
    {
      set => transform.rotation = value;
    }

    public Vector3 Forward
    {
      get => transform.forward;
      set
      {
        if(value == transform.forward)
          return;
        
        transform.forward = value;
      }
    }

    public Vector3 Right
    {
      get => transform.right;
      set => transform.right = value;
    }
    
    public Vector2 AnimationVelocity
    {
      set => _animationView.Velocity = value;
    }

    private WeaponView _weaponInstance = null!;
    public WeaponDescriptor Weapon
    {
      set
      {
        if(_weaponInstance)
          _weaponInstance.DestroyObject();

        var slot = this.RequireComponentInChild<Transform>(value.SlotName);
        _weaponInstance = Instantiate(value.EquippedPrefab, slot);
        _animationView.Controller = value.AnimatorController;
        _weaponInstance.ShotSound = value.ShotSound;
        _weaponInstance.BulletProjectilePrefab = value.ProjectilePrefab;
      }
    }

    private AudioClip[] _footStepSounds = null!; 
    public AudioClip[] FootStepSounds
    {
      set
      {
        _animationView.Step -= OnAnimationStep;
        _animationView.Step += OnAnimationStep;
        _footStepSounds = value;
      }
    }
    
    public IDamageableModel DamageableModel { get; set; } = null!;

    public event Action<string>? LootContacted;

    public void Move(Vector3 velocity) => _characterController.Move(velocity);

    public RaycastTestViewModel? Shoot()
    {
      var ray = new Ray(_characterController.bounds.center, transform.forward);
      var layerMask = LayerMask.GetMask("Obstacles", "Enemies", "Environment");

      var raycastResult = Physics.Raycast(ray, out var hitInfo, float.MaxValue, layerMask);

      if(!raycastResult)
      {
        ShowShoot(ray.direction * 100);
        return null;
      }

      ShowShoot(hitInfo.point);
      
      if(!hitInfo.collider.TryGetComponent<IDamageableView>(out var target))
        return null;

      return new RaycastTestViewModel(target.DamageableModel, hitInfo.point);
    }

    public void Reload() => _animationView.Reload();

    public void Die()
    {
      _animationView.Die();
      _characterController.enabled = false;
    }

    public void DrawWeapon() => _animationView.DrawWeapon();

    private void OnAnimationStep() => _audioService.PlaySFX(_footStepAudioSource, _footStepSounds.PickRandom());

    private void ShowShoot(Vector3 hitPosition)
    {
      _animationView.RandomShot();
      _weaponInstance.Shot(hitPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
      if(!other.TryGetComponent<LootView>(out var loot))
        return;
      
      LootContacted?.Invoke(loot.Id);
    }

    private void OnValidate()
    {
      _animationView = this.RequireComponent<HeroAnimationView>();
      _characterController = this.RequireComponent<CharacterController>();
      _footStepAudioSource = this.RequireComponentInChildren<AudioSource>();
    }
  }
}