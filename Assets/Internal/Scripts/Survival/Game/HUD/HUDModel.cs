using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Player;
using Karabaev.Survival.Game.Weapons;

namespace Karabaev.Survival.Game.HUD
{
  public class HUDModel
  {
    public int MaxHp { get; }
    
    public ReactiveProperty<int> CurrentHp { get; }
    
    public ReactiveProperty<WeaponModel> ActiveWeapon { get; }
    
    public InventoryModel Inventory { get; }

    public HUDModel(int maxHp, ReactiveProperty<int> currentHp, ReactiveProperty<WeaponModel> activeWeapon, InventoryModel inventory)

    {
      MaxHp = maxHp;
      CurrentHp = currentHp;
      ActiveWeapon = activeWeapon;
      Inventory = inventory;
    }
  }
}