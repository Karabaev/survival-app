using Karabaev.GameKit.Entities;
using UnityEngine;

namespace Karabaev.Survival.Game.Player
{
  public class PlayerView : UnityView
  {
    public Texture2D CursorIcon
    {
      set
      {
        var halfWidth = value.width * 0.5f;
        var halfHeight = value.height * 0.5f;
        Cursor.SetCursor(value, new Vector2(halfWidth, halfHeight), CursorMode.ForceSoftware);
      }
    }
  }
}