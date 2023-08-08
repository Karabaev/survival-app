using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Entities;
using UnityEngine;

namespace Karabaev.Survival.Game.GameInput
{
  [UsedImplicitly]
  public class GameInputEntity : Entity<GameInputEntity.Context, GameInputModel, GameInputView>
  {
    protected override UniTask OnCreatedAsync(Context context)
    {
      View.FireClicked += View_OnFireClicked;
      View.AxisChanged += View_OnAxisChanged;
      return UniTask.CompletedTask;
    }

    protected override void OnDisposed()
    {
      View.FireClicked -= View_OnFireClicked;
      View.AxisChanged -= View_OnAxisChanged;
    }

    private void View_OnFireClicked(Vector2 mousePosition) => Model.FireFired.Set(mousePosition);

    private void View_OnAxisChanged(Vector2 newValue) => Model.Axis = newValue;

    protected override GameInputModel CreateModel(Context context) => new();

    protected override UniTask<GameInputView> CreateViewAsync(Context context) => UniTask.FromResult(context.ViewObject.AddComponent<GameInputView>());

    public record Context(GameObject ViewObject);
  }
}