using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Weapons;
using UnityEngine;

namespace Karabaev.Survival.Game.Hero
{
  public class HeroModel
  {
    public ReactiveProperty<Vector2> Direction { get; }

    public ReactiveProperty<WeaponModel> Weapon { get; }
    
    public int MaxHp { get; }
    
    public ReactiveProperty<int> CurrentHp { get; }
    
    public float CurrentMoveSpeed { get; }
    
    public ReactiveTrigger<Vector2> FireFired { get; }
    
    public ReactiveTrigger ReloadFired { get; }

    public HeroModel(WeaponModel weapon, int maxHp, int currentHp, float moveSpeed, ReactiveTrigger<Vector2> fireFired, ReactiveTrigger reloadFired)
    {
      Direction = new ReactiveProperty<Vector2>(Vector2.zero);
      Weapon = new ReactiveProperty<WeaponModel>(weapon);
      MaxHp = maxHp;
      CurrentHp = new ReactiveProperty<int>(currentHp);
      CurrentMoveSpeed = moveSpeed;
      FireFired = fireFired;
      ReloadFired = reloadFired;
    }
  }
}