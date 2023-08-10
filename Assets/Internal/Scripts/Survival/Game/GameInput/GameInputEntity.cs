using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Entities;
using UnityEngine;

namespace Karabaev.Survival.Game.GameInput
{
  [UsedImplicitly]
  public class GameInputEntity : Entity<GameInputEntity.Context, GameInputModel, GameInputView>
  {
    private Vector2 _lastDragPosition;
    private bool _auxDragging;
    
    protected override UniTask OnCreatedAsync(Context context)
    {
      View.FireClicked += View_OnFireClicked;
      View.ReloadClicked += View_OnReloadClicked;
      View.MainMouseButtonDown += View_OnMainMouseButtonDown;
      View.MainMouseButtonUp += View_OnMainMouseButtonUp;
      View.AuxMouseButtonDown += View_OnAuxMouseButtonDown;
      View.AuxMouseButtonUp += View_OnAuxMouseButtonUp;
      
      Model.Enabled.Changed += Model_OnEnabledChanged;
      
      Model_OnEnabledChanged(false, Model.Enabled.Value);
      return UniTask.CompletedTask;
    }
    
    protected override void OnDisposed()
    {
      View.FireClicked -= View_OnFireClicked;
      View.ReloadClicked -= View_OnReloadClicked;
      View.MainMouseButtonDown -= View_OnMainMouseButtonDown;
      View.MainMouseButtonUp -= View_OnMainMouseButtonUp;
      View.AuxMouseButtonDown -= View_OnAuxMouseButtonDown;
      View.AuxMouseButtonUp -= View_OnAuxMouseButtonUp;
      
      Model.Enabled.Changed -= Model_OnEnabledChanged;
    }

    protected override void OnTick(float deltaTime, GameTime now)
    {
      Model.MouseWheelAxis = View.MouseWheelAxis;
      Model.MainAxis = View.MainAxis;
      Model.AuxMouseButtonDragAxis = _auxDragging ? View.MousePosition - _lastDragPosition : Vector2.zero;
      Model.MousePosition = View.MousePosition;
      _lastDragPosition = View.MousePosition;
    }

    private void View_OnFireClicked() => Model.FireFired.Set();

    private void View_OnReloadClicked() => Model.ReloadFired.Set();

    private void View_OnMainMouseButtonDown() => Model.FireButtonDownFired.Set();

    private void View_OnMainMouseButtonUp() => Model.FireButtonUpFired.Set();

    private void View_OnAuxMouseButtonDown(Vector2 mousePosition) => _auxDragging = true;

    private void View_OnAuxMouseButtonUp(Vector2 mousePosition) => _auxDragging = false;

    private void Model_OnEnabledChanged(bool oldValue, bool newValue) => View.Enabled = newValue;

    protected override GameInputModel CreateModel(Context context) => context.Model;

    protected override UniTask<GameInputView> CreateViewAsync(Context context) => UniTask.FromResult(context.ViewObject.AddComponent<GameInputView>());

    public record Context(GameObject ViewObject, GameInputModel Model);
  }
}