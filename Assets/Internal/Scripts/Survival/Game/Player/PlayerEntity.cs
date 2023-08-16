using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Configuration;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.GameInput;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.HUD;
using Karabaev.Survival.Game.Weapons;
using UnityEngine;

namespace Karabaev.Survival.Game.Player
{
  [UsedImplicitly]
  public class PlayerEntity : Entity<PlayerEntity.Context, PlayerModel, PlayerView>
  {
    private bool _shooting;
    private GameTime _nextShootTime = GameTime.Min;
    private GameTime? _reloadFinishTime;

    protected override async UniTask OnCreatedAsync(Context context)
    {
      var heroContext = new HeroEntity.Context(View.transform, Model.Hero, context.HeroDescriptor, context.HeroSpawnPosition);
      var loadingTasks = new List<UniTask>
      {
        CreateChildAsync<GameInputEntity, GameInputEntity.Context>(new GameInputEntity.Context(View.gameObject, Model.Input)),
        CreateChildAsync<HeroEntity, HeroEntity.Context>(heroContext),
        CreateChildAsync<GameCameraEntity, GameCameraEntity.Context>(new GameCameraEntity.Context(View.transform, Model.Camera, context.CameraConfig)),
        CreateChildAsync<HUDEntity, HUDEntity.Context>(new HUDEntity.Context(Model.HUD))
      };
      
      await UniTask.WhenAll(loadingTasks);

      Model.Hero.CurrentHp.Changed += Model_OnHeroCurrentHpChanged;
      
      Model.Camera.Target.Value = Model.Hero.HeroObject.Value;

      Model.Input.FireButtonDownFired.Triggered += Model_OnFireButtonDownFired;
      Model.Input.FireButtonUpFired.Triggered += Model_OnFireButtonUpFired;
      Model.Input.ReloadFired.Triggered += Model_OnReloadFired;
      Model.Input.FirstWeaponButtonFired.Triggered += Model_OnFirstWeaponButtonFired;
      Model.Input.SecondWeaponButtonFired.Triggered += Model_OnSecondWeaponButtonFired;
      Model.Input.ThirdWeaponButtonFired.Triggered += Model_OnThirdWeaponButtonFired;
      
      Model.ActiveWeapon.Changed += Model_OnActiveWeaponChanged;
    }
    
    protected override void OnDisposed()
    {
      Model.Hero.CurrentHp.Changed -= Model_OnHeroCurrentHpChanged;

      Model.Input.FireButtonDownFired.Triggered -= Model_OnFireButtonDownFired;
      Model.Input.FireButtonUpFired.Triggered -= Model_OnFireButtonUpFired;
      Model.Input.ReloadFired.Triggered -= Model_OnReloadFired;
      Model.Input.FirstWeaponButtonFired.Triggered -= Model_OnFirstWeaponButtonFired;
      Model.Input.SecondWeaponButtonFired.Triggered -= Model_OnSecondWeaponButtonFired;
      Model.Input.ThirdWeaponButtonFired.Triggered -= Model_OnThirdWeaponButtonFired;
      
      Model.ActiveWeapon.Changed -= Model_OnActiveWeaponChanged;
    }

    protected override void OnTick(float deltaTime, GameTime now)
    {
      Model.Hero.MoveDirection.Value = CalculateHeroMoveDirection();
      Model.Hero.LookDirection.Value = CalculateHeroLookDirection();

      var currentWeapon = Model.ActiveWeapon.Value;
      
      if(_shooting && now >= _nextShootTime && currentWeapon.CurrentMagazine.Value > 0 && !_reloadFinishTime.HasValue)
        Shoot(now);

      if(now >= _reloadFinishTime)
        Reload();
    }

    private void Model_OnHeroCurrentHpChanged(int oldValue, int newValue)
    {
      if(newValue > 0)
        return;

      Model.Input.Enabled.Value = false;
      Model.HeroDied.Set();
    }

    private void Model_OnFireButtonDownFired() => _shooting = true;

    private void Model_OnFireButtonUpFired() => _shooting = false;
    
    private void Model_OnReloadFired()
    {
      if(_reloadFinishTime.HasValue)
        return;
      
      var currentWeapon = Model.ActiveWeapon.Value;
      if(currentWeapon.ReserveAmmo.Value == 0 || currentWeapon.CurrentMagazine.Value == currentWeapon.Descriptor.Magazine)
        return;
      
      Model.Hero.ReloadFired.Set();
      _reloadFinishTime = GameTime.Now.AddSeconds(currentWeapon.Descriptor.ReloadDuration);
    }

    private void Model_OnFirstWeaponButtonFired()
    {
      if(Model.Inventory.Weapons.IsEmpty)
        return;

      Model.ActiveWeapon.Value = Model.Inventory.Weapons[0];
    }

    private void Model_OnSecondWeaponButtonFired()
    {
      if(Model.Inventory.Weapons.Count < 1)
        return;

      Model.ActiveWeapon.Value = Model.Inventory.Weapons[1];
    }

    private void Model_OnThirdWeaponButtonFired()
    {
      if(Model.Inventory.Weapons.Count < 3)
        return;

      Model.ActiveWeapon.Value = Model.Inventory.Weapons[2];
    }
    
    private void Model_OnActiveWeaponChanged(WeaponModel oldValue, WeaponModel newValue)
    {
      _shooting = false;
      _reloadFinishTime = null;
    }

    private Vector2 CalculateHeroMoveDirection()
    {
      var camera = Model.Camera.Camera.Value;
      var inputAxis = Model.Input.MainAxis;

      var direction = camera.transform.forward * inputAxis.y + camera.transform.right * inputAxis.x;
      return new Vector2(direction.x, direction.z);
    }
    
    private Vector2 CalculateHeroLookDirection()
    {
      var camera = Model.Camera.Camera.Value;

      var heroPositionY = Model.Hero.Position.Value.y;
      var plane = new Plane(new Vector3(0.0f, heroPositionY, 0.0f), new Vector3(1.0f, heroPositionY, 0.0f), new Vector3(0.0f, heroPositionY, 1.0f));
      var ray = camera.ScreenPointToRay(Model.Input.MousePosition);
      plane.Raycast(ray, out var depth);
      var mouseWorldPosition = ray.GetPoint(depth);
      
      var lookDirection = MathUtils.Direction2D(Model.Hero.Position.Value, mouseWorldPosition);
      return new Vector2(lookDirection.x, lookDirection.z);
    }
    
    private void Shoot(GameTime now)
    {
      Model.Hero.ShootFired.Set();
      Model.ActiveWeapon.Value.CurrentMagazine.Value--;

      var fireRate = Model.ActiveWeapon.Value.Descriptor.FireRate;
      var cooldown = 1.0f / fireRate;
      _nextShootTime = now.AddSeconds(cooldown);
    }

    private void Reload()
    {
      var activeWeapon = Model.ActiveWeapon.Value;
      var ammoToMagazine = activeWeapon.Descriptor.Magazine - activeWeapon.CurrentMagazine.Value;

      if(activeWeapon.ReserveAmmo.Value >= ammoToMagazine)
      {
        activeWeapon.ReserveAmmo.Value -= ammoToMagazine;
        activeWeapon.CurrentMagazine.Value += ammoToMagazine;
      }
      else
      {
        activeWeapon.CurrentMagazine.Value += activeWeapon.ReserveAmmo.Value;
        activeWeapon.ReserveAmmo.Value = 0;
      }

      _reloadFinishTime = null;
    }
    
    protected override PlayerModel CreateModel(Context context) => context.Model;

    protected override UniTask<PlayerView> CreateViewAsync(Context context)
    {
      var view = CommonUtils.NewObject<PlayerView>("Player", context.Parent);
      view.CursorIcon = context.GameConfig.CursorIcon;
      return UniTask.FromResult(view);
    }

    public record Context(Transform Parent, PlayerModel Model, HeroDescriptor HeroDescriptor, Vector3 HeroSpawnPosition, GameCameraConfig CameraConfig,
      GameConfig GameConfig);
  }
}