using Karabaev.GameKit.Common.Utils;
using Karabaev.Survival.Game.Loot.Descriptors;
using UnityEngine;

namespace Karabaev.Survival.Game.Loot
{
  public class LootModel
  {
    public string Id { get; }
    
    public Vector3 Position { get; }
    
    public LootDescriptor Descriptor { get; }

    public LootModel(Vector3 position, LootDescriptor descriptor)
    {
      Id = $"{descriptor.Id}_{RandomUtils.RandomString()}";
      Position = position;
      Descriptor = descriptor;
    }
  }
}