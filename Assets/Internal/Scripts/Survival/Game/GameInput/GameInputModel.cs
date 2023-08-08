using Karabaev.GameKit.Entities.Reactive;
using UnityEngine;

namespace Karabaev.Survival.Game.GameInput
{
  public class GameInputModel
  {
    public Vector2 Axis { get; set; }
    
    public ReactiveTrigger<Vector2> FireFired { get; }
    
    public ReactiveTrigger ReloadFired { get; }

    public GameInputModel()
    {
      FireFired = new ReactiveTrigger<Vector2>();
      ReloadFired = new ReactiveTrigger();
    }
  }
}