using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.GameInput;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.Weapons;
using UnityEngine;

namespace Karabaev.Survival.Game.Player
{
  [UsedImplicitly]
  public class PlayerEntity : Entity<PlayerEntity.Context, PlayerModel, PlayerView>
  {
    protected override async UniTask OnCreatedAsync(Context context)
    {
      await CreateChildAsync<GameInputEntity, GameInputEntity.Context>(new GameInputEntity.Context(View.gameObject));
      var heroContext = new HeroEntity.Context(View.transform, Model.Hero, context.HeroDescriptor);
      await CreateChildAsync<HeroEntity, HeroEntity.Context>(heroContext);
    }

    protected override void OnDisposed()
    {
    }

    protected override void OnTick(float deltaTime, GameTime now)
    {
      var axis = Model.Input.Axis;
      Model.Hero.Direction.Value = axis.normalized;
    }

    protected override PlayerModel CreateModel(Context context)
    {
      var heroDescriptor = context.HeroDescriptor;
      var inputModel = new GameInputModel();
      var weaponModel = new WeaponModel(context.WeaponDescriptor);
      var heroModel = new HeroModel(weaponModel, heroDescriptor.MaxHp, heroDescriptor.MaxHp, heroDescriptor.MoveSpeed, inputModel.FireFired, inputModel.ReloadFired);
      return new(inputModel, heroModel);
    }

    protected override UniTask<PlayerView> CreateViewAsync(Context context)
    {
      var view = CommonUtils.NewObject<PlayerView>("Player", context.Parent);
      return UniTask.FromResult(view);
    }

    public record Context(Transform Parent, HeroDescriptor HeroDescriptor, WeaponDescriptor WeaponDescriptor);
  }
}