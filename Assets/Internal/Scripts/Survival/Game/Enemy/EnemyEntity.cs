using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Entities;
using UnityEngine;

namespace Karabaev.Survival.Game.Enemy
{
  [UsedImplicitly]
  public class EnemyEntity : Entity<EnemyEntity.Context, EnemyModel, EnemyView>
  {
    protected override UniTask OnCreatedAsync(Context context)
    {
      View.HitImpactPrefab = Model.Descriptor.HitImpactPrefab;
      
      Model.HitImpactFired.Triggered += Model_OnHitImpactFired;
      Model.CurrentHp.Changed += Model_OnCurrentHpChanged;
      return UniTask.CompletedTask;
    }

    protected override void OnDisposed()
    {
      Model.HitImpactFired.Triggered -= Model_OnHitImpactFired;
      Model.CurrentHp.Changed -= Model_OnCurrentHpChanged;
    }

    private void Model_OnHitImpactFired(Vector3 hitPosition) => View.ShowHitImpactAsync(hitPosition);

    private void Model_OnCurrentHpChanged(int oldValue, int newValue)
    {
      if(newValue > 0)
        return;
      
      View.Die();
    }

    protected override EnemyModel CreateModel(Context context) => context.Model;

    protected override UniTask<EnemyView> CreateViewAsync(Context context)
    {
      var view = Object.Instantiate(context.Model.Descriptor.Prefab, context.Parent);
      view.name = context.Model.Descriptor.Id;
      view.Position = context.SpawnPosition;
      view.DamageableModel = context.Model;
      return UniTask.FromResult(view);
    }

    public record Context(Transform Parent, EnemyModel Model, Vector3 SpawnPosition);
  }
}