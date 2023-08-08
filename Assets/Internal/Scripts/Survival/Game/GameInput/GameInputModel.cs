using Karabaev.GameKit.Entities.Reactive;
using UnityEngine;

namespace Karabaev.Survival.Game.GameInput
{
  public class GameInputModel
  {
    public Vector2 MainAxis { get; set; }

    public Vector2 AuxMouseButtonDragAxis { get; set; }

    public float MouseWheelAxis { get; set; }

    public ReactiveTrigger<Vector2> FireFired { get; }

    public ReactiveTrigger ReloadFired { get; }

    public ReactiveProperty<bool> Enabled { get; }

    public GameInputModel()
    {
      MainAxis = Vector2.zero;
      AuxMouseButtonDragAxis = Vector2.zero;
      MouseWheelAxis = 0.0f;
      FireFired = new ReactiveTrigger<Vector2>();
      ReloadFired = new ReactiveTrigger();
      Enabled = new ReactiveProperty<bool>(true);
    }
  }
}