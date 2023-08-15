using Karabaev.Survival.Game.Loot.Descriptors;
using UnityEngine;

namespace Karabaev.Survival.Game.Loot
{
  public class LootSpawnPointModel
  {
    public Vector3 Position { get; }

    public LootDescriptor Descriptor { get; }

    public LootSpawnPointModel(Vector3 position, LootDescriptor descriptor)
    {
      Descriptor = descriptor;
      Position = position;
    }
  }
}