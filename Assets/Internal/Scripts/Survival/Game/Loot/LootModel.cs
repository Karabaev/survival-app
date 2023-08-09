using UnityEngine;

namespace Karabaev.Survival.Game.Loot
{
  public class LootModel
  {
    public string Id { get; }
    
    public Vector3 Position { get; }
    
    public LootDescriptor Descriptor { get; }

    public LootModel(string id, Vector3 position, LootDescriptor descriptor)
    {
      Id = id;
      Position = position;
      Descriptor = descriptor;
    }
  }
}