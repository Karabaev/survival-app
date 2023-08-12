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
      Model.CurrentHp.Changed += Model_OnCurrentHpChanged;
      return UniTask.CompletedTask;
    }

    protected override void OnDisposed()
    {
      Model.HitImpactFired.Triggered -= Model_OnHitImpactFired;
      Model.CurrentHp.Changed -= Model_OnCurrentHpChanged;
    }

    private void Model_OnHitImpactFired(Vector3 hitPosition) => View.ShowHitImpact(hitPosition);

    private async void Model_OnCurrentHpChanged(int oldValue, int newValue)
    {
      if(newValue > 0)
        return;

      await View.DestroyAsync();
      Model.DestroyFired.Set();
    }

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