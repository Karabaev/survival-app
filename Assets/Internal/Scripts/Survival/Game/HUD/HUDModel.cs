using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Weapons;

namespace Karabaev.Survival.Game.HUD
{
  public class HUDModel
  {
    public int MaxHp { get; }
    
    public ReactiveProperty<int> CurrentHp { get; }
    
    public ReactiveProperty<WeaponModel> Weapon { get; }

    public HUDModel(int maxHp, ReactiveProperty<int> currentHp, ReactiveProperty<WeaponModel> weapon)
    {
      MaxHp = maxHp;
      CurrentHp = currentHp;
      Weapon = weapon;
    }
  }
}