using Karabaev.GameKit.Entities;
using UnityEngine;

namespace Karabaev.Survival.Game.Loot
{
  public class LootView : UnityView
  {
    public Vector3 Position
    {
      set => transform.position = value;
    }

    public string Id { get; set; } = null!;
  }
}