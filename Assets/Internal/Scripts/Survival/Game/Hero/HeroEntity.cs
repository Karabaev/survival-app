using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Loot;
using Karabaev.Survival.Game.Weapons;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Karabaev.Survival.Game.Hero
{
  [UsedImplicitly]
  public class HeroEntity : Entity<HeroEntity.Context, HeroModel, HeroView>
  {
    protected override UniTask OnCreatedAsync(Context context)
    {
      Model.HeroObject.Value = View.transform;
      
      View.LootContacted += View_OnLootContacted;
      
      Model.CurrentHp.Changed += Model_OnCurrentHpChanged;
      Model.Weapon.Changed += Model_OnWeaponChanged;
      Model.Weapon.Value.CurrentMagazine.Changed += Model_OnWeaponMagazineChanged;
      Model.FireFired.Triggered += Model_OnFireFired;
      Model.ReloadFired.Triggered += Model_OnReloadFired;
      return UniTask.CompletedTask;
    }

    protected override void OnDisposed()
    {
      View.LootContacted -= View_OnLootContacted;

      Model.CurrentHp.Changed -= Model_OnCurrentHpChanged;
      Model.Weapon.Changed -= Model_OnWeaponChanged;
      Model.Weapon.Value.CurrentMagazine.Changed -= Model_OnWeaponMagazineChanged;
      Model.FireFired.Triggered -= Model_OnFireFired;
      Model.ReloadFired.Triggered -= Model_OnReloadFired;
    }

    private void View_OnLootContacted(string lootId) => Model.LootContactFired.Set(lootId);

    protected override void OnTick(float deltaTime, GameTime now)
    {
      var direction = Model.Direction.Value;
      var velocity = direction * Model.CurrentMoveSpeed * deltaTime;
      View.AnimationVelocity = direction * Model.CurrentMoveSpeed;
      View.Move(new Vector3(velocity.x, 0.0f, velocity.y));
    }

    private void Model_OnCurrentHpChanged(int oldValue, int newValue)
    {
      if(newValue > 0)
        return;
      
      View.Die();
    }

    private void Model_OnWeaponChanged(WeaponModel oldValue, WeaponModel newValue) => View.Weapon = newValue.Descriptor;

    private void Model_OnWeaponMagazineChanged(int oldValue, int newValue)
    {
    }

    private void Model_OnFireFired(Vector2 value)
    {
      View.Shot();
    }

    private void Model_OnReloadFired()
    {
      View.Reload();
    }
    
    protected override HeroModel CreateModel(Context context) => context.Model;

    protected override UniTask<HeroView> CreateViewAsync(Context context)
    {
      var view = Object.Instantiate(context.Descriptor.Prefab, context.Parent);
      view.name = "Hero";
      view.Position = Vector3.zero;
      view.Rotation = Quaternion.identity;
      return UniTask.FromResult(view);
    }

    public record Context(Transform Parent, HeroModel Model, HeroDescriptor Descriptor);
  }
}