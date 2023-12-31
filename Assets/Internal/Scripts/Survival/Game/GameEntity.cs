﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Descriptors;
using Karabaev.Survival.Game.Configuration;
using Karabaev.Survival.Game.Enemy;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.Location;
using Karabaev.Survival.Game.Loot;
using Karabaev.Survival.Game.Loot.Descriptors;
using Karabaev.Survival.Game.Player;
using Karabaev.Survival.Game.Weapons;
using Karabaev.Survival.Game.Weapons.Descriptors;
using VContainer;

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
      await CreateChildAsync<LocationEntity, LocationEntity.Context>(new LocationEntity.Context(View.transform, Model.Location));
      var playerContext = new PlayerEntity.Context(View.transform, Model.Player, context.HeroDescriptor, Model.Location.HeroSpawnPosition, context.CameraConfig,
        context.GameConfig);
      await CreateChildAsync<PlayerEntity, PlayerEntity.Context>(playerContext);
      
      Model.Player.LootContactFired.Triggered += Model_OnPlayerLootContactFired;
      Model.Player.HeroDied.Triggered += Model_OnPlayerHeroDied;
      Model.Loot.ItemAdded += Model_OnLootAdded;
      Model.Loot.ItemRemoved += Model_OnLootRemoved;

      Model.Enemies.ItemAdded += Model_OnEnemyAdded;
      Model.Enemies.ItemRemoved += Model_OnEnemyRemoved;
      
      foreach(var spawnPoint in Model.Location.LootSpawnPoints)
      {
        var descriptor = spawnPoint.Descriptor;
        Model.Loot.Add(new LootModel(spawnPoint.Position, descriptor));
      }
    }
    
    protected override void OnDisposed()
    {
      Model.Player.LootContactFired.Triggered -= Model_OnPlayerLootContactFired;
      Model.Player.HeroDied.Triggered -= Model_OnPlayerHeroDied;

      Model.Loot.ItemAdded -= Model_OnLootAdded;
      Model.Loot.ItemRemoved -= Model_OnLootRemoved;
      Model.Enemies.ItemAdded -= Model_OnEnemyAdded;
      Model.Enemies.ItemRemoved -= Model_OnEnemyRemoved;
    }
    
    protected override void OnTick(float deltaTime, GameTime now)
    {
      foreach(var spawnPoint in Model.Location.EnemySpawnPoints)
      {
        if(!spawnPoint.Enabled || now < spawnPoint.NextSpawnTime)
          continue;

        Model.Enemies.Add(new EnemyModel(spawnPoint.Descriptor, Model.Player.Hero, spawnPoint.Position));
        spawnPoint.NextSpawnTime = now.Add(spawnPoint.SpawnInterval);
      }
    }

    private void Model_OnPlayerLootContactFired(string lootId)
    {
      var lootModel = Model.Loot.Collection.First(l => l.Id == lootId);

      Action<LootModel> handler = lootModel.Descriptor.Type switch
      {
        LootType.Weapon => CollectWeaponLoot,
        LootType.Ammo => CollectAmmoLoot,
        LootType.FirstAid => CollectFirstAid,
        _ => throw new NotImplementedException()
      };
      
      handler.Invoke(lootModel);
    }

    private void Model_OnPlayerHeroDied()
    {
      Model.Enemies.Collection.ForEach(e => e.Target.Value = null);
      Model.Location.EnemySpawnPoints.ForEach(s => s.Enabled = false);
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

      if(oldItem.Descriptor.PossibleLoot.Length <= 0)
        return;
      
      var loot = new LootModel(oldItem.Position, oldItem.Descriptor.PossibleLoot.PickRandom());
      Model.Loot.Add(loot);
    }

    private void CollectWeaponLoot(LootModel loot)
    {
      var weaponDescriptor = _descriptorsAccess.WeaponsRegistry.Values[loot.Descriptor.ItemId];

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
      var inventoryWeapon = Model.Player.Inventory.Weapons.Collection.FirstOrDefault(w => w.Descriptor.Id == loot.Descriptor.ItemId);

      if(inventoryWeapon == null)
        return;

      var weaponDescriptor = _descriptorsAccess.WeaponsRegistry.Values[loot.Descriptor.ItemId];
      inventoryWeapon.ReserveAmmo.Value += weaponDescriptor.Magazine;
      Model.Loot.Remove(loot);
    }

    private void CollectFirstAid(LootModel loot)
    {
      if(Model.Player.Hero.CurrentHp.Value == Model.Player.Hero.MaxHp)
        return;
      
      var descriptor = _descriptorsAccess.FirstAidsRegistry.Values[loot.Descriptor.ItemId];
      Model.Player.Hero.CurrentHp.Value += descriptor.HealValue;
      Model.Loot.Remove(loot);
    }
    
    protected override GameModel CreateModel(Context context) => new(context.HeroDescriptor, context.WeaponDescriptor);

    protected override UniTask<GameView> CreateViewAsync(Context context)
    {
      var view = CommonUtils.NewObject<GameView>("Game");
      return UniTask.FromResult(view);
    }

    public record Context(HeroDescriptor HeroDescriptor, WeaponDescriptor WeaponDescriptor, GameCameraConfig CameraConfig, GameConfig GameConfig);
  }
}