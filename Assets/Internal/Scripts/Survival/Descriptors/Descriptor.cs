using UnityEngine;

namespace Karabaev.Survival.Descriptors
{
  public abstract class Descriptor : ScriptableObject
  {
    [field: SerializeField]
    public string Id { get; private set; } = null!;
  }
}