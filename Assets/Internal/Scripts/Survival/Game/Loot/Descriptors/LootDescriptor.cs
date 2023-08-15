using Karabaev.Survival.Descriptors;
using UnityEngine;

namespace Karabaev.Survival.Game.Loot.Descriptors
{
  [CreateAssetMenu(menuName = "Karabaev/NewLoot")]
  public class LootDescriptor : Descriptor
  {
    [field: SerializeField]
    public LootView Prefab { get; private set; } = null!;

    [field: SerializeField]
    public LootType Type { get; private set; }
    
    [field: SerializeField]
    public string ItemId { get; private set; } = null!;
  }
}