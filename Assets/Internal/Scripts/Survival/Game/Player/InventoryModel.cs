using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Weapons;

namespace Karabaev.Survival.Game.Player
{
  public class InventoryModel
  {
    public ReactiveCollection<WeaponModel> Weapons { get; }

    public InventoryModel(WeaponModel initialWeapon)
    {
      Weapons = new ReactiveCollection<WeaponModel>();
      Weapons.Add(initialWeapon);
    }
  }
}