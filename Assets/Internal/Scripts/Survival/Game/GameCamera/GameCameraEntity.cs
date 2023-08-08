using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Entities;
using UnityEngine;

namespace Karabaev.Survival.Game.GameCamera
{
  [UsedImplicitly]
  public class GameCameraEntity : Entity<GameCameraEntity.Context, GameCameraModel, GameCameraView>
  {
    private GameCameraConfig _config = null!;
    
    protected override UniTask OnCreatedAsync(Context context)
    {
      _config = context.Config;
      View.FocusRadius = _config.FocusRadius;
      View.FocusCentering = _config.FocusCentering;
      View.Position = Model.Position.Value;
      View.Rotation = Model.Rotation.Value;
      View.Target = Model.Target.Value;
      View.Zoom = _config.InitialZoomDistance;
      View.OrbitAngles = _config.InitialOrbitAngles;
      Model.Camera.Value = View.Camera;
      
      Model.Position.Changed += Model_OnPositionChanged;
      Model.Rotation.Changed += Model_OnRotationChanged;
      Model.Target.Changed += Model_OnTargetChanged;
      return UniTask.CompletedTask;
    }

    protected override void OnDisposed()
    {
      Model.Position.Changed -= Model_OnPositionChanged;
      Model.Rotation.Changed -= Model_OnRotationChanged;
      Model.Target.Changed -= Model_OnTargetChanged;
    }

    protected override void OnTick(float deltaTime, GameTime now)
    {
      HandleZoom();
      HandleOrbitAngles(deltaTime);
    }

    private void HandleZoom()
    {
      var inputWheelAxis = Model.Input.MouseWheelAxis;
      if(inputWheelAxis == 0.0f)
        return;
      
      var maxZoom = _config.MaxZoomDistance;
      var minZoom = _config.MinZoomDistance;
      var zoomSensitivity = _config.ZoomSensitivity;

      View.Zoom = Mathf.Clamp(View.Zoom - inputWheelAxis * zoomSensitivity, minZoom, maxZoom);
    }

    private void HandleOrbitAngles(float deltaTime)
    {
      var input = Model.Input.AuxMouseButtonDragAxis;
      
      var maxAngle = _config.MaxOrbitAngle;
      var minAngle = _config.MinOrbitAngle;

      var invertedInput = new Vector2(-input.y, -input.x);
      var orbitAngles = View.OrbitAngles + _config.RotationSensitivity * deltaTime * invertedInput;
      orbitAngles.x = Mathf.Clamp(orbitAngles.x, minAngle, maxAngle);
      switch(orbitAngles.y)
      {
        case < 0f:
          orbitAngles.y += 360f;
          break;
        case >= 360f:
          orbitAngles.y -= 360f;
          break;
      }
      
      View.OrbitAngles = orbitAngles;
    }

    private void Model_OnPositionChanged(Vector3 oldValue, Vector3 newValue) => View.Position = newValue;

    private void Model_OnRotationChanged(Vector3 oldValue, Vector3 newValue) => View.Rotation = newValue;

    private void Model_OnTargetChanged(Transform? oldValue, Transform? newValue) => View.Target = newValue;

    protected override GameCameraModel CreateModel(Context context) => context.Model;

    protected override UniTask<GameCameraView> CreateViewAsync(Context context)
    {
      var view = Object.Instantiate(context.Config.Prefab, context.Parent);
      view.name = "Camera";
      return UniTask.FromResult(view);
    }

    public record Context(Transform Parent, GameCameraModel Model, GameCameraConfig Config);
  }
}