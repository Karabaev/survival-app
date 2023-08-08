using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
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
      set => transform.position = value;
    }
    
    public Quaternion Rotation
    {
      set => transform.rotation = value;
    }

    private Vector2 _velocity;
    public Vector2 Velocity
    {
      set
      {
        _velocity = value;
        _animationView.Velocity = value;
      }
    }

    private GameObject? _weaponInstance;
    public WeaponDescriptor Weapon
    {
      set
      {
        if(_weaponInstance != null)
          _weaponInstance.DestroyObject();

        _weaponInstance = Instantiate(value.Prefab);
        _animationView.Controller = value.AnimatorController;
      }
    }

    public void Move(Vector3 velocity) => _characterController.Move(velocity);

    public void Shot() => _animationView.RandomShot();

    public void Reload() => _animationView.Reload();

    public void Die() => _animationView.Die();

    private void Update()
    {
      if(_velocity.sqrMagnitude == 0)
        return;

      _characterController.Move(new Vector3(_velocity.x, 0.0f, _velocity.y));
    }

    private void OnValidate()
    {
      _animationView = this.RequireComponent<HeroAnimationView>();
      _characterController = this.RequireComponent<CharacterController>();
    }
  }
}