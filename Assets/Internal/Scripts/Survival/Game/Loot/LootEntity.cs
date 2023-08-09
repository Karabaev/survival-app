using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Entities;
using UnityEngine;

namespace Karabaev.Survival.Game.Loot
{
  [UsedImplicitly]
  public class LootEntity : Entity<LootEntity.Context, LootModel, LootView>
  {
    protected override LootModel CreateModel(Context context) => context.Model;

    protected override UniTask<LootView> CreateViewAsync(Context context)
    {
      var view = Object.Instantiate(context.Model.Descriptor.Prefab, context.Parent);
      view.name = context.Model.Id;
      view.Id = context.Model.Id;
      view.Position = context.Model.Position;
      return UniTask.FromResult(view);
    }

    public record Context(Transform Parent, LootModel Model);
  }
}