using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Descriptors;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.Loot;
using Karabaev.Survival.Game.Player;
using Karabaev.Survival.Game.Weapons;
using UnityEngine;
using VContainer;

namespace Karabaev.Survival.Game
{
  [UsedImplicitly]
  public class GameEntity : Entity<GameEntity.Context, GameModel, GameView>
  {
    [Inject]
    private readonly DescriptorsAccess _descriptorsAccess = null!;

    private readonly Dictionary<LootModel, LootEntity> _lootEntities = new();

    protected override async UniTask OnCreatedAsync(Context context)
    {
      var playerContext = new PlayerEntity.Context(View.transform, Model.Player, context.HeroDescriptor, context.CameraConfig);
      await CreateChildAsync<PlayerEntity, PlayerEntity.Context>(playerContext);
      
      Model.Player.LootContactFired.Triggered += Model_PlayerLootContactFired;
      
      Model.Loot.ItemAdded += Model_OnLootAdded;
      Model.Loot.ItemRemoved += Model_OnLootRemoved;

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
    }

    private void Model_PlayerLootContactFired(string lootId)
    {
      var lootModel = Model.Loot.Collection.First(l => l.Id == lootId);
      
      switch(lootModel.Descriptor.Type)
      {
        case LootType.Weapon:
          var weaponDescriptor = _descriptorsAccess.WeaponsRegistry.Weapons.First(w => w.Id == lootModel.Descriptor.ItemId);
          Model.Player.Hero.Weapon.Value = new WeaponModel(weaponDescriptor);
          break;
        case LootType.Ammo:
          Model.Player.Hero.Weapon.Value.ReserveAmmo.Value += 30;
          break;
        case LootType.Health:
          Model.Player.Hero.CurrentHp.Value += 100;
          break;
      }

      Model.Loot.Remove(lootModel);
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

    protected override GameModel CreateModel(Context context) => new(context.HeroDescriptor, context.WeaponDescriptor);

    protected override UniTask<GameView> CreateViewAsync(Context context)
    {
      var view = CommonUtils.NewObject<GameView>("Game");
      return UniTask.FromResult(view);
    }

    public record Context(HeroDescriptor HeroDescriptor, WeaponDescriptor WeaponDescriptor, GameCameraConfig CameraConfig);
  }
}