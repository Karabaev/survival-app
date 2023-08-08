using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.Player;
using Karabaev.Survival.Game.Weapons;

namespace Karabaev.Survival.Game
{
  [UsedImplicitly]
  public class GameEntity : Entity<GameEntity.Context, GameModel, GameView>
  {
    protected override async UniTask OnCreatedAsync(Context context)
    {
      var playerContext = new PlayerEntity.Context(View.transform, context.HeroDescriptor, context.WeaponDescriptor, context.CameraConfig);
      await CreateChildAsync<PlayerEntity, PlayerEntity.Context>(playerContext);
    }

    protected override GameModel CreateModel(Context context) => new();

    protected override UniTask<GameView> CreateViewAsync(Context context)
    {
      var view = CommonUtils.NewObject<GameView>("Game");
      return UniTask.FromResult(view);
    }

    public record Context(HeroDescriptor HeroDescriptor, WeaponDescriptor WeaponDescriptor, GameCameraConfig CameraConfig);
  }
}