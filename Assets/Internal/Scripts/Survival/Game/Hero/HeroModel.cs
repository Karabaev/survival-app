using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Damageable;
using Karabaev.Survival.Game.Weapons;
using UnityEngine;

namespace Karabaev.Survival.Game.Hero
{
  public class HeroModel : IDamageableModel
  {
    public ReactiveProperty<Vector2> MoveDirection { get; }
    
    public ReactiveProperty<Vector2> LookDirection { get; }

    public ReactiveProperty<Vector3> Position { get; }

    public ReactiveProperty<WeaponModel> Weapon { get; }
    
    public int MaxHp { get; }
    
    public ReactiveProperty<int> CurrentHp { get; }

    public float CurrentMoveSpeed { get; }

    public ReactiveTrigger ShootFired { get; }

    public ReactiveTrigger ReloadFired { get; }

    public ReactiveProperty<Transform?> HeroObject { get; }

    public ReactiveTrigger<string> LootContactFired { get; }

    public ReactiveTrigger<Vector3> HitImpactFired { get; }
    
    public HeroModel(ReactiveProperty<WeaponModel> weapon, int maxHp, int currentHp, float moveSpeed, ReactiveTrigger<string> lootContactFired)
    {
      MoveDirection = new ReactiveProperty<Vector2>(Vector2.zero);
      LookDirection = new ReactiveProperty<Vector2>(Vector2.zero);
      Position = new ReactiveProperty<Vector3>(Vector3.zero);
      Weapon = weapon;
      MaxHp = maxHp;
      CurrentHp = new ReactiveProperty<int>(currentHp);
      CurrentMoveSpeed = moveSpeed;
      ShootFired = new ReactiveTrigger();
      ReloadFired = new ReactiveTrigger();
      HeroObject = new ReactiveProperty<Transform?>(null);
      LootContactFired = lootContactFired;
      HitImpactFired = new ReactiveTrigger<Vector3>();
    }
  }
}