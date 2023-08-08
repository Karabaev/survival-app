using System;
using Cysharp.Threading.Tasks;
using Karabaev.GameKit.Entities;
using UnityEngine;

namespace Karabaev.Survival.Game.Player
{
  public class PlayerEntity : Entity<PlayerEntity.Context, PlayerModel, PlayerView>
  {
    protected override PlayerModel CreateModel(Context context) => new();

    protected override UniTask<PlayerView> CreateViewAsync(Context context)
    {
      throw new NotImplementedException();
    }

    public record Context(Transform Parent);
  }
}