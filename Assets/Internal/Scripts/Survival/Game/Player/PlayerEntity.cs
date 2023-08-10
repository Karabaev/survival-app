using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
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
      await CreateChildAsync<GameInputEntity, GameInputEntity.Context>(new GameInputEntity.Context(View.gameObject, Model.Input));
      var heroContext = new HeroEntity.Context(View.transform, Model.Hero, context.HeroDescriptor);
      await CreateChildAsync<HeroEntity, HeroEntity.Context>(heroContext);
      await CreateChildAsync<GameCameraEntity, GameCameraEntity.Context>(new GameCameraEntity.Context(View.transform, Model.Camera, context.CameraConfig));
      await CreateChildAsync<HUDEntity, HUDEntity.Context>(new HUDEntity.Context(Model.HUD));
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
      Model.Hero.MoveDirection.Value = Model.Input.MainAxis;
      Model.Hero.LookDirection.Value = CalculateHeroLookDirection();

      var currentWeapon = Model.ActiveWeapon.Value;
      
      if(_shooting && now >= _nextShootTime && currentWeapon.CurrentMagazine.Value > 0 && !_reloadFinishTime.HasValue)
      {
        Model.Hero.ShootFired.Set();
        currentWeapon.CurrentMagazine.Value--;

        var fireRate = currentWeapon.Descriptor.FireRate;
        var cooldown = 1.0f / fireRate;
        _nextShootTime = now.AddSeconds(cooldown);
      }

      if(now >= _reloadFinishTime)
      {
        var ammoToMagazine = currentWeapon.Descriptor.Magazine - currentWeapon.CurrentMagazine.Value;

        if(currentWeapon.ReserveAmmo.Value >= ammoToMagazine)
        {
          currentWeapon.ReserveAmmo.Value -= ammoToMagazine;
          currentWeapon.CurrentMagazine.Value += ammoToMagazine;
        }
        else
        {
          currentWeapon.CurrentMagazine.Value += currentWeapon.ReserveAmmo.Value;
          currentWeapon.ReserveAmmo.Value = 0;
        }

        _reloadFinishTime = null;
      }
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
    
    private Vector2 CalculateHeroLookDirection()
    {
      var camera = Model.Camera.Camera.Value;
      var cameraTransform = camera.transform;
      var sin = Mathf.Sin(Mathf.Deg2Rad * cameraTransform.localRotation.eulerAngles.x);
      var depth = cameraTransform.position.y / sin;
      var mousePosition = Model.Input.MousePosition;
      var mouseWorldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, depth));
      var lookDirection = MathUtils.Direction2D(Model.Hero.Position.Value, mouseWorldPosition);
      return new Vector2(lookDirection.x, lookDirection.z);
    }
    
    protected override PlayerModel CreateModel(Context context) => context.Model;

    protected override UniTask<PlayerView> CreateViewAsync(Context context)
    {
      var view = CommonUtils.NewObject<PlayerView>("Player", context.Parent);
      return UniTask.FromResult(view);
    }

    public record Context(Transform Parent, PlayerModel Model, HeroDescriptor HeroDescriptor, GameCameraConfig CameraConfig);
  }
}