using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Entities;
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
      Model.ReloadFired.Triggered += Model_OnReloadFired;
      Model.ShootFired.Triggered += Model_OnShootFired;
      Model.HitImpactFired.Triggered += Model_OnHitImpactFired;
      
      Model_OnWeaponChanged(null!, Model.Weapon.Value);
      
      return UniTask.CompletedTask;
    }

    protected override void OnDisposed()
    {
      View.LootContacted -= View_OnLootContacted;

      Model.CurrentHp.Changed -= Model_OnCurrentHpChanged;
      Model.Weapon.Changed -= Model_OnWeaponChanged;
      Model.ReloadFired.Triggered -= Model_OnReloadFired;
      Model.ShootFired.Triggered -= Model_OnShootFired;
      Model.HitImpactFired.Triggered -= Model_OnHitImpactFired;
    }

    protected override void OnTick(float deltaTime, GameTime now)
    {
      if(Model.CurrentHp.Value <= 0)
        return;
      
      var direction = Model.MoveDirection.Value;
      
      var direction3D = new Vector3(direction.x, 0f, direction.y);
      var forwardDirection = Vector3.Dot(View.Forward, direction3D);
      var rightDirection = Vector3.Dot(View.Right, direction3D);
      View.AnimationVelocity = new Vector2(rightDirection, forwardDirection);
      
      var velocity = direction.normalized * Model.CurrentMoveSpeed * deltaTime;
      View.Move(new Vector3(velocity.x, 0.0f, velocity.y));

      var lookDirection = Model.LookDirection.Value;
      View.Forward = new Vector3(lookDirection.x, 0.0f, lookDirection.y);
      
      Model.Position.Value = View.Position;
    }

    private void View_OnLootContacted(string lootId) => Model.LootContactFired.Set(lootId);

    private void Model_OnCurrentHpChanged(int oldValue, int newValue)
    {
      if(newValue > 0)
        return;
      
      View.Die();
    }

    private void Model_OnWeaponChanged(WeaponModel oldValue, WeaponModel newValue)
    {
      View.DrawWeapon();
      View.Weapon = newValue.Descriptor;
    }

    private void Model_OnReloadFired() => View.Reload();

    private void Model_OnShootFired()
    {
      var shootResult = View.Shoot();
      
      if(!shootResult.HasValue)
        return;

      var target = shootResult.Value.Damageable;
      target.CurrentHp.Value -= Model.Weapon.Value.Descriptor.Damage;
      target.HitImpactFired.Set(shootResult.Value.ContactPosition);
    }
    
    private void Model_OnHitImpactFired(Vector3 value)
    {
      // todo
    }

    protected override HeroModel CreateModel(Context context) => context.Model;

    protected override UniTask<HeroView> CreateViewAsync(Context context)
    {
      var view = Object.Instantiate(context.Descriptor.Prefab, context.Parent);
      view.name = "Hero";
      view.Position = context.SpawnPosition;
      view.Rotation = Quaternion.identity;
      view.FootStepSounds = context.Descriptor.FootStepSounds;
      view.DamageableModel = context.Model;
      Resolver.Inject(view);
      return UniTask.FromResult(view);
    }

    public record Context(Transform Parent, HeroModel Model, HeroDescriptor Descriptor, Vector3 SpawnPosition);
  }
}