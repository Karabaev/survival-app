using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Descriptors;
using Karabaev.Survival.Game.Enemy;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.Location;
using Karabaev.Survival.Game.Loot;
using Karabaev.Survival.Game.Player;
using Karabaev.Survival.Game.Weapons;
using VContainer;
using Object = UnityEngine.Object;

namespace Karabaev.Survival.Game
{
  [UsedImplicitly]
  public class GameEntity : Entity<GameEntity.Context, GameModel, GameView>
  {
    [Inject]
    private readonly DescriptorsAccess _descriptorsAccess = null!;

    private readonly Dictionary<LootModel, LootEntity> _lootEntities = new();
    private readonly Dictionary<EnemyModel, EnemyEntity> _enemyEntities = new();

    protected override async UniTask OnCreatedAsync(Context context)
    {
      var playerContext = new PlayerEntity.Context(View.transform, Model.Player, context.HeroDescriptor, context.CameraConfig);

      var loadingTasks = new List<UniTask>
      {
        CreateChildAsync<PlayerEntity, PlayerEntity.Context>(playerContext),
        CreateChildAsync<LocationEntity, LocationEntity.Context>(new LocationEntity.Context(View.transform, Model.Location))
      };

      await UniTask.WhenAll(loadingTasks);
      
      Model.Player.LootContactFired.Triggered += Model_PlayerLootContactFired;
      
      Model.Loot.ItemAdded += Model_OnLootAdded;
      Model.Loot.ItemRemoved += Model_OnLootRemoved;

      Model.Enemies.ItemAdded += Model_OnEnemyAdded;
      Model.Enemies.ItemRemoved += Model_OnEnemyRemoved;
      
      var lootSpawnPoints = Object.FindObjectsOfType<LootSpawnPoint>();

      foreach(var spawnPoint in lootSpawnPoints)
      {
        var descriptor = _descriptorsAccess.LootRegistry.Loot.First(l => l.Id == spawnPoint.LootId);
        Model.Loot.Add(new LootModel($"{descriptor.Id}_{RandomUtils.RandomString()}", spawnPoint.Position, descriptor));
        spawnPoint.DestroyObject();
      }
    }

    protected override void OnDisposed()
    {
      Model.Player.LootContactFired.Triggered -= Model_PlayerLootContactFired;
      
      Model.Loot.ItemAdded -= Model_OnLootAdded;
      Model.Loot.ItemRemoved -= Model_OnLootRemoved;
      Model.Enemies.ItemAdded -= Model_OnEnemyAdded;
      Model.Enemies.ItemRemoved -= Model_OnEnemyRemoved;
    }
    
    protected override void OnTick(float deltaTime, GameTime now)
    {
      foreach(var spawnPoint in Model.Location.EnemySpawnPoints)
      {
        if(now < spawnPoint.NextSpawnTime)
          continue;

        Model.Enemies.Add(new EnemyModel(spawnPoint.Descriptor, Model.Player.Hero, spawnPoint.Position));
        spawnPoint.NextSpawnTime = now.Add(spawnPoint.SpawnInterval);
      }
    }

    private void Model_PlayerLootContactFired(string lootId)
    {
      var lootModel = Model.Loot.Collection.First(l => l.Id == lootId);

      Action<LootModel> handler = lootModel.Descriptor.Type switch
      {
        LootType.Weapon => CollectWeaponLoot,
        LootType.Ammo => CollectAmmoLoot,
        LootType.FirstAid => CollectFirstAid,
        _ => throw new ArgumentOutOfRangeException()
      };
      
      handler.Invoke(lootModel);
    }

    private async void Model_OnLootAdded(LootModel newItem, int index)
    {
      var entity = await CreateChildAsync<LootEntity, LootEntity.Context>(new LootEntity.Context(View.transform, newItem));
      _lootEntities.Add(newItem, entity);
    }

    private void Model_OnLootRemoved(LootModel oldItem, int index)
    {
      _lootEntities.Remove(oldItem, out var entity);
      DisposeChild(entity);
    }
    
    private async void Model_OnEnemyAdded(EnemyModel newItem, int index)
    {
      var entity = await CreateChildAsync<EnemyEntity, EnemyEntity.Context>(new EnemyEntity.Context(View.transform, newItem));
      _enemyEntities.Add(newItem, entity);

      newItem.Dead.Changed += OnDeadChanged;

      void OnDeadChanged(bool _, bool newValue)
      {
        if(!newValue)
          return;
        
        Model.Enemies.Remove(newItem);
      }
    }

    private void Model_OnEnemyRemoved(EnemyModel oldItem, int index)
    {
      _enemyEntities.Remove(oldItem, out var entity);
      DisposeChild(entity);
      // todo unsubscribe from Dead.Changed
    }

    private void CollectWeaponLoot(LootModel loot)
    {
      var weaponDescriptor = _descriptorsAccess.WeaponsRegistry.Weapons.First(w => w.Id == loot.Descriptor.ItemId);

      var inventoryWeapons = Model.Player.Inventory.Weapons;
      var foundWeapon = inventoryWeapons.Collection.FirstOrDefault(w => w.Descriptor.Id == weaponDescriptor.Id);

      if(foundWeapon != null)
      {
        foundWeapon.ReserveAmmo.Value += weaponDescriptor.Magazine;
      }
      else
      {
        foundWeapon = new WeaponModel(weaponDescriptor, 0);
        inventoryWeapons.Add(foundWeapon);
      }

      Model.Player.ActiveWeapon.Value = foundWeapon;
      Model.Loot.Remove(loot);
    }

    private void CollectAmmoLoot(LootModel loot)
    {
      var weaponDescriptor = _descriptorsAccess.WeaponsRegistry.Weapons.First(w => w.Id == loot.Descriptor.ItemId);
      var inventoryWeapon = Model.Player.Inventory.Weapons.Collection.FirstOrDefault(w => w.Descriptor.Id == weaponDescriptor.Id);

      if(inventoryWeapon == null)
        return;

      inventoryWeapon.ReserveAmmo.Value += weaponDescriptor.Magazine;
      Model.Loot.Remove(loot);
    }

    private void CollectFirstAid(LootModel loot)
    {
      Model.Player.Hero.CurrentHp.Value += 100;
      Model.Loot.Remove(loot);
    }
    
    protected override GameModel CreateModel(Context context) => new(context.HeroDescriptor, context.WeaponDescriptor);

    protected override UniTask<GameView> CreateViewAsync(Context context)
    {
      var view = CommonUtils.NewObject<GameView>("Game");
      return UniTask.FromResult(view);
    }

    public record Context(HeroDescriptor HeroDescriptor, WeaponDescriptor WeaponDescriptor, GameCameraConfig CameraConfig);
  }
}