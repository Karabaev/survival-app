using Karabaev.Survival.Game.Loot.Descriptors;
using UnityEngine;

namespace Karabaev.Survival.Game.Loot
{
  public class LootSpawnPoint : MonoBehaviour
  {
    [field: SerializeField]
    public LootDescriptor Descriptor { get; private set; } = null!;

    public Vector3 Position => transform.position;
  }
}