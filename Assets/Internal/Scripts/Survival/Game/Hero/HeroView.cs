using System;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Loot;
using Karabaev.Survival.Game.Weapons;
using UnityEngine;

namespace Karabaev.Survival.Game.Hero
{
  public class HeroView : UnityView
  {
    [SerializeField, HideInInspector]
    private HeroAnimationView _animationView = null!;
    [SerializeField, HideInInspector]
    private CharacterController _characterController = null!;

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
      set => transform.forward = value;
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

    private WeaponView? _weaponInstance;
    public WeaponDescriptor Weapon
    {
      set
      {
        if(_weaponInstance != null)
          _weaponInstance.DestroyObject();

        var slot = this.RequireComponentInChild<Transform>(value.SlotName);
        _weaponInstance = Instantiate(value.EquippedPrefab, slot);
        _animationView.Controller = value.AnimatorController;
      }
    }

    public event Action<string>? LootContacted;
    
    public void Move(Vector3 velocity) => _characterController.Move(velocity);

    public void Shot()
    {
      if(_weaponInstance == null)
        return;
      
      _animationView.RandomShot();
      _weaponInstance.PlayMuzzleAsync().Forget();
    }

    public void Reload() => _animationView.Reload();

    public void Die() => _animationView.Die();

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
    }
  }
}