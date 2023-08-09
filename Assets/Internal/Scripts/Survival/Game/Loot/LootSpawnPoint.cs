using UnityEngine;

namespace Karabaev.Survival.Game.Loot
{
  public class LootSpawnPoint : MonoBehaviour
  {
    [field: SerializeField]
    public string LootId { get; private set; } = null!;

    public Vector3 Position => transform.position;
  }
}