using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.GameInput;
using Karabaev.Survival.Game.Hero;
using UnityEngine;

namespace Karabaev.Survival.Game.Player
{
  [UsedImplicitly]
  public class PlayerEntity : Entity<PlayerEntity.Context, PlayerModel, PlayerView>
  {
    protected override async UniTask OnCreatedAsync(Context context)
    {
      await CreateChildAsync<GameInputEntity, GameInputEntity.Context>(new GameInputEntity.Context(View.gameObject, Model.Input));
      var heroContext = new HeroEntity.Context(View.transform, Model.Hero, context.HeroDescriptor);
      await CreateChildAsync<HeroEntity, HeroEntity.Context>(heroContext);
      await CreateChildAsync<GameCameraEntity, GameCameraEntity.Context>(new GameCameraEntity.Context(View.transform, Model.Camera, context.CameraConfig));

      Model.Camera.Target.Value = Model.Hero.HeroObject.Value;
    }

    protected override void OnDisposed()
    {
    }

    protected override void OnTick(float deltaTime, GameTime now)
    {
      var axis = Model.Input.MainAxis;
      Model.Hero.Direction.Value = axis.normalized;
    }

    protected override PlayerModel CreateModel(Context context) => context.Model;

    protected override UniTask<PlayerView> CreateViewAsync(Context context)
    {
      var view = CommonUtils.NewObject<PlayerView>("Player", context.Parent);
      return UniTask.FromResult(view);
    }

    public record Context(Transform Parent, PlayerModel Model, HeroDescriptor HeroDescriptor, GameCameraConfig CameraConfig);
  }
}