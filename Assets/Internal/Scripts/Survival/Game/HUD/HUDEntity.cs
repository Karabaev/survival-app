using Cysharp.Threading.Tasks;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Weapons;

namespace Karabaev.Survival.Game.HUD
{
  public class HUDEntity : UIEntity<HUDEntity.Context, HUDModel, HUDView>
  {
    protected override UniTask OnCreatedAsync(Context context)
    {
      View.SetHp(Model.CurrentHp.Value, Model.MaxHp);
      View.SetAmmo(Model.Weapon.Value.CurrentMagazine.Value, Model.Weapon.Value.ReserveAmmo.Value);

      Model.Weapon.Changed += Model_OnWeaponChanged;
      Model.Weapon.Value.CurrentMagazine.Changed += Model_OnWeaponMagazineChanged;
      Model.Weapon.Value.ReserveAmmo.Changed += Model_OnReserveAmmonChanged;
      return UniTask.CompletedTask;
    }

    protected override void OnDisposed()
    {
      Model.Weapon.Changed -= Model_OnWeaponChanged;
      Model.Weapon.Value.CurrentMagazine.Changed -= Model_OnWeaponMagazineChanged;
      Model.Weapon.Value.ReserveAmmo.Changed -= Model_OnReserveAmmonChanged;
    }

    private void Model_OnWeaponChanged(WeaponModel oldValue, WeaponModel newValue)
    {
      oldValue.CurrentMagazine.Changed -= Model_OnWeaponMagazineChanged;
      oldValue.ReserveAmmo.Changed -= Model_OnReserveAmmonChanged;
      newValue.CurrentMagazine.Changed += Model_OnWeaponMagazineChanged;
      newValue.ReserveAmmo.Changed += Model_OnReserveAmmonChanged;
      
      View.SetAmmo(Model.Weapon.Value.CurrentMagazine.Value, Model.Weapon.Value.ReserveAmmo.Value);
    }

    private void Model_OnWeaponMagazineChanged(int oldValue, int newValue) => View.SetAmmo(newValue, Model.Weapon.Value.ReserveAmmo.Value);

    private void Model_OnReserveAmmonChanged(int oldValue, int newValue) => View.SetAmmo(Model.Weapon.Value.CurrentMagazine.Value, newValue);

    protected override HUDModel CreateModel(Context context) => context.Model;

    protected override string ViewPrefabPath => "UI/PF_HUD";

    public record Context(HUDModel Model);
  }
}