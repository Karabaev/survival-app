using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Entities;
using UnityEngine;

namespace Karabaev.Survival.Game.Obstacles
{
  [UsedImplicitly]
  public class ObstacleEntity : Entity<ObstacleEntity.Context, ObstacleModel, ObstacleView>
  {
    protected override UniTask OnCreatedAsync(Context context)
    {
      Model.HitImpactFired.Triggered += Model_OnHitImpactFired;
      Model.DestroyFired.Triggered += Model_OnDestroyedAsync;
      return UniTask.CompletedTask;
    }

    protected override void OnDisposed()
    {
      Model.HitImpactFired.Triggered -= Model_OnHitImpactFired;
      Model.DestroyFired.Triggered -= Model_OnDestroyedAsync;
    }

    private void Model_OnHitImpactFired(Vector3 hitPosition) => View.ShowHitImpact(hitPosition);

    private UniTask Model_OnDestroyedAsync() => View.DestroyAsync();

    protected override ObstacleModel CreateModel(Context context) => context.Model;

    protected override UniTask<ObstacleView> CreateViewAsync(Context context)
    {
      var view = context.View;
      view.DamageableModel = context.Model;
      return UniTask.FromResult(view);
    }

    public record Context(ObstacleModel Model, ObstacleView View);
  }
}