using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Karabaev.Survival.Descriptors
{
  public abstract class DescriptorsRegistry<T> : ScriptableObject where T : Descriptor
  {
    [SerializeField]
    private T[] _values = null!;

    public IReadOnlyDictionary<string, T> Values { get; private set; } = null!;

    private void Awake() => Values = _values.ToDictionary(descriptor => descriptor.Id, descriptor => descriptor);
  }
}