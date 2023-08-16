using UnityEngine;

namespace Karabaev.Survival.Game.Configuration
{
  [CreateAssetMenu(menuName = "Karabaev/GameConfig")]
  public class GameConfig : ScriptableObject
  {
    [field: SerializeField]
    public Texture2D CursorIcon { get; private set; } = null!;
  }
}