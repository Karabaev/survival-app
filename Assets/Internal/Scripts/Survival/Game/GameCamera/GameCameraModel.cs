using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.GameInput;
using UnityEngine;

namespace Karabaev.Survival.Game.GameCamera
{
  public class GameCameraModel
  {
    public ReactiveProperty<Vector3> Position { get; }
    
    public ReactiveProperty<Vector3> Rotation { get; }
    
    public ReactiveProperty<Camera> Camera { get; }
    
    public ReactiveProperty<Transform?> Target { get; }
    
    public GameInputModel Input { get; }
    
    public GameCameraModel(Vector3 position, Vector3 rotation, GameInputModel input)
    {
      Position = new ReactiveProperty<Vector3>(position);
      Rotation = new ReactiveProperty<Vector3>(rotation);
      Camera = new ReactiveProperty<Camera>(null!);
      Target = new ReactiveProperty<Transform?>(null);
      Input = input;
    }
  }
}