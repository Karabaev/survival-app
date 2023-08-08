using Karabaev.GameKit.Entities.Reactive;

namespace Karabaev.Survival.Game.Weapons
{
  public class WeaponModel
  {
    public WeaponDescriptor Descriptor { get; }

    public ReactiveProperty<int> CurrentMagazine { get; }
    
    public ReactiveProperty<int> ReserveAmmo { get; }

    public WeaponModel(WeaponDescriptor descriptor)
    {
      Descriptor = descriptor;
      CurrentMagazine = new ReactiveProperty<int>(-1);
      ReserveAmmo = new ReactiveProperty<int>(-1);
    }
  }
}