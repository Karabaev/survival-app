using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Obstacles;
using UnityEngine;

namespace Karabaev.Survival.Game.Location
{
  [UsedImplicitly]
  public class LocationEntity : Entity<LocationEntity.Context, LocationModel, LocationView>
  {
    private readonly Dictionary<ObstacleModel, (ObstacleEntity, ReactiveProperty<int>.ValueChangedHandler)> _obstacleEntities = new();

    protected override UniTask OnCreatedAsync(Context context)
    {
      Model.Obstacles.ItemRemoved += Model_OnObstacleRemoved;
      
      var obstacleTasks = new List<UniTask>(View.Obstacles.Length);
      foreach(var obstacleView in View.Obstacles)
        obstacleTasks.Add(CreateObstacleAsync(obstacleView));

      return UniTask.WhenAll(obstacleTasks);
    }

    protected override void OnDisposed()
    {
      Model.Obstacles.ItemRemoved -= Model_OnObstacleRemoved;
    }

    private async void Model_OnObstacleRemoved(ObstacleModel oldItem, int index)
    {
      View.RecalculateNavMesh();
      _obstacleEntities.Remove(oldItem, out var entity);
      oldItem.CurrentHp.Changed -= entity.Item2;
      await oldItem.DestroyFired.Set();
      DisposeChild(entity.Item1);
    }

    private void Model_OnObstacleCurrentHpChanged(ObstacleModel model, int newValue)
    {
      if(newValue > 0)
        return;

      Model.Obstacles.Remove(model);
    }

    private async UniTask CreateObstacleAsync(ObstacleView view)
    {
      var model = new ObstacleModel(view.Descriptor);
      var entity = await CreateChildAsync<ObstacleEntity, ObstacleEntity.Context>(new ObstacleEntity.Context(model, view));

      _obstacleEntities.Add(model, (entity, HpChangedHandler));
      model.CurrentHp.Changed += HpChangedHandler;
      Model.Obstacles.Add(model);

      void HpChangedHandler(int _, int newValue) => Model_OnObstacleCurrentHpChanged(model, newValue);
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