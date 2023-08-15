using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Enemy.SpawnPoint;
using Karabaev.Survival.Game.Loot;
using Karabaev.Survival.Game.Obstacles;
using UnityEngine;

namespace Karabaev.Survival.Game.Location
{
  [UsedImplicitly]
  public class LocationEntity : Entity<LocationEntity.Context, LocationModel, LocationView>
  {
    private readonly Dictionary<ObstacleModel, (ObstacleEntity, ReactiveTrigger.TriggeredHandler)> _obstacleEntities = new();

    protected override UniTask OnCreatedAsync(Context context)
    {
      Model.Obstacles.ItemRemoved += Model_OnObstacleRemoved;
      
      var obstacleTasks = new List<UniTask>(View.Obstacles.Length);
      foreach(var obstacleView in View.Obstacles)
        obstacleTasks.Add(CreateObstacleAsync(obstacleView));

      var enemySpawnPoints = new List<EnemySpawnPointModel>(View.EnemySpawnPoints.Length);
      foreach(var spawnPoint in View.EnemySpawnPoints)
        enemySpawnPoints.Add(new EnemySpawnPointModel(spawnPoint.Position, spawnPoint.Descriptor, spawnPoint.SpawnInterval));

      Model.EnemySpawnPoints = enemySpawnPoints;
      
      var lootSpawnPoints = new List<LootSpawnPointModel>(View.LootSpawnPoints.Length);
      foreach(var spawnPoint in View.LootSpawnPoints)
        lootSpawnPoints.Add(new LootSpawnPointModel(spawnPoint.Position, spawnPoint.Descriptor));

      Model.HeroSpawnPosition = View.HeroSpawnPoint.Position;
      
      Model.LootSpawnPoints = lootSpawnPoints;
      
      return UniTask.WhenAll(obstacleTasks);
    }

    protected override void OnDisposed()
    {
      Model.Obstacles.ItemRemoved -= Model_OnObstacleRemoved;
    }

    private void Model_OnObstacleRemoved(ObstacleModel oldItem, int index)
    {
      _obstacleEntities.Remove(oldItem, out var entity);
      oldItem.DestroyFired.Triggered -= entity.Item2;
      View.RecalculateNavMesh();
      DisposeChild(entity.Item1);
    }

    private void Model_OnObstacleCurrentHpChanged(ObstacleModel model) => Model.Obstacles.Remove(model);

    private async UniTask CreateObstacleAsync(ObstacleView view)
    {
      var model = new ObstacleModel(view.Descriptor);
      var entity = await CreateChildAsync<ObstacleEntity, ObstacleEntity.Context>(new ObstacleEntity.Context(model, view));

      _obstacleEntities.Add(model, (entity, ObstacleDestroyHandler));
      model.DestroyFired.Triggered += ObstacleDestroyHandler;
      Model.Obstacles.Add(model);

      void ObstacleDestroyHandler() => Model_OnObstacleCurrentHpChanged(model);
    }

    protected override LocationModel CreateModel(Context context) => context.Model;

    protected override UniTask<LocationView> CreateViewAsync(Context context)
    {
      var view = Object.FindObjectOfType<LocationView>();
      context.Parent.AddChild(view);
      return UniTask.FromResult(view);
    }

    public record Context(Transform Parent, LocationModel Model);
  }
}