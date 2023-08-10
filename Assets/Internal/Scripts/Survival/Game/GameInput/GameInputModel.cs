using Karabaev.GameKit.Entities.Reactive;
using UnityEngine;

namespace Karabaev.Survival.Game.GameInput
{
  public class GameInputModel
  {
    public Vector2 MainAxis { get; set; }

    public Vector2 AuxMouseButtonDragAxis { get; set; }

    public float MouseWheelAxis { get; set; }

    public Vector2 MousePosition { get; set; }

    public ReactiveTrigger FireButtonDownFired { get; }
    
    public ReactiveTrigger FireButtonUpFired { get; }

    public ReactiveTrigger FireFired { get; }

    public ReactiveTrigger ReloadFired { get; }

    public ReactiveProperty<bool> Enabled { get; }

    public GameInputModel()
    {
      MainAxis = Vector2.zero;
      AuxMouseButtonDragAxis = Vector2.zero;
      MouseWheelAxis = 0.0f;
      MousePosition = Vector2.zero;
      FireButtonDownFired = new ReactiveTrigger();
      FireButtonUpFired = new ReactiveTrigger();
      FireFired = new ReactiveTrigger();
      ReloadFired = new ReactiveTrigger();
      Enabled = new ReactiveProperty<bool>(true);
    }
  }
}