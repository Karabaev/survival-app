using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.GameInput;
using Karabaev.Survival.Game.Player;

namespace Karabaev.Survival.Game
{
  [UsedImplicitly]
  public class GameEntity : Entity<GameEntity.Context, GameModel, GameView>
  {
    protected override async UniTask OnCreatedAsync(Context context)
    {
      await CreateChildAsync<GameInputEntity, GameInputEntity.Context>(new GameInputEntity.Context(View.transform));
      await CreateChildAsync<PlayerEntity, PlayerEntity.Context>(new PlayerEntity.Context(View.transform));
    }

    protected override GameModel CreateModel(Context context)
    {
      throw new NotImplementedException();
    }

    protected override UniTask<GameView> CreateViewAsync(Context context)
    {
      throw new NotImplementedException();
    }

    public record Context();
  }

  public class GameView : UnityView { }

  public class GameModel { }
}