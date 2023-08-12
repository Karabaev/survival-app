using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Weapons;

namespace Karabaev.Survival.Game.HUD
{
  [UsedImplicitly]
  public class HUDEntity : UIEntity<HUDEntity.Context, HUDModel, HUDView>
  {
    protected override UniTask OnCreatedAsync(Context context)
    {
      Model.ActiveWeapon.Changed += Model_OnWeaponChanged;
      Model.ActiveWeapon.Value.CurrentMagazine.Changed += Model_OnWeaponMagazineChanged;
      Model.ActiveWeapon.Value.ReserveAmmo.Changed += Model_OnReserveAmmoChanged;
      Model.Inventory.Weapons.ItemAdded += Model_OnInventoryWeaponAdded;
      Model.Inventory.Weapons.ItemRemoved += Model_OnInventoryWeaponRemoved;
      Model.CurrentHp.Changed += Model_OnCurrentHpChanged;

      Model_OnCurrentHpChanged(Model.CurrentHp.Value, Model.MaxHp);
      var activeWeapon = Model.ActiveWeapon.Value;
      View.SetActiveWeapon(activeWeapon.Descriptor.Icon, activeWeapon.CurrentMagazine.Value, activeWeapon.ReserveAmmo.Value);
      
      var inventoryWeapons = Model.Inventory.Weapons;
      for(var i = 0; i < inventoryWeapons.Collection.Count; i++)
        Model_OnInventoryWeaponAdded(inventoryWeapons.Collection[i], i);

      return UniTask.CompletedTask;
    }

    protected override void OnDisposed()
    {
      Model.ActiveWeapon.Changed -= Model_OnWeaponChanged;
      Model.ActiveWeapon.Value.CurrentMagazine.Changed -= Model_OnWeaponMagazineChanged;
      Model.ActiveWeapon.Value.ReserveAmmo.Changed -= Model_OnReserveAmmoChanged;
      Model.Inventory.Weapons.ItemAdded -= Model_OnInventoryWeaponAdded;
      Model.Inventory.Weapons.ItemRemoved -= Model_OnInventoryWeaponRemoved;
      Model.CurrentHp.Changed -= Model_OnCurrentHpChanged;
    }

    private void Model_OnWeaponChanged(WeaponModel oldValue, WeaponModel newValue)
    {
      oldValue.CurrentMagazine.Changed -= Model_OnWeaponMagazineChanged;
      oldValue.ReserveAmmo.Changed -= Model_OnReserveAmmoChanged;
      newValue.CurrentMagazine.Changed += Model_OnWeaponMagazineChanged;
      newValue.ReserveAmmo.Changed += Model_OnReserveAmmoChanged;
      
      View.SetActiveWeapon(newValue.Descriptor.Icon, newValue.CurrentMagazine.Value, newValue.ReserveAmmo.Value);
    }

    private void Model_OnWeaponMagazineChanged(int oldValue, int newValue) => View.UpdateActualAmmo(newValue, Model.ActiveWeapon.Value.ReserveAmmo.Value);

    private void Model_OnReserveAmmoChanged(int oldValue, int newValue) => View.UpdateActualAmmo(Model.ActiveWeapon.Value.CurrentMagazine.Value, newValue);

    private void Model_OnInventoryWeaponAdded(WeaponModel newItem, int index)
    {
      View.SetWeaponInInventory(index, newItem.Descriptor.Icon, newItem.CurrentMagazine.Value, newItem.ReserveAmmo.Value);

      newItem.CurrentMagazine.Changed += (_, newValue) => Model_OnInventoryWeaponCurrentMagazineChanged(index, newValue);
      newItem.ReserveAmmo.Changed += (_, newValue) => Model_OnInventoryWeaponReserveAmmoChanged(index, newValue);
    }

    private void Model_OnInventoryWeaponRemoved(WeaponModel oldItem, int index)
    {
      View.RemoveWeaponFromInventory(index);
      // todo unsubscribe from CurrentMagazine.Changed and ReserveAmmo.Changed
    }

    private void Model_OnInventoryWeaponCurrentMagazineChanged(int weaponIndex, int newValue) => 
      View.UpdateAmmoInInventory(weaponIndex, newValue, Model.Inventory.Weapons[weaponIndex].ReserveAmmo.Value);

    private void Model_OnInventoryWeaponReserveAmmoChanged(int weaponIndex, int newValue) => 
      View.UpdateAmmoInInventory(weaponIndex, Model.Inventory.Weapons[weaponIndex].CurrentMagazine.Value, newValue);

    private void Model_OnCurrentHpChanged(int oldValue, int newValue) => View.SetHp(newValue, Model.MaxHp);

    protected override HUDModel CreateModel(Context context) => context.Model;

    protected override string ViewPrefabPath => "UI/PF_HUD";

    public record Context(HUDModel Model);
  }
}