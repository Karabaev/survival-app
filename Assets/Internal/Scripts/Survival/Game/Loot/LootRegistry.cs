using System;
using System.Collections.Generic;
using UnityEngine;

namespace Karabaev.Survival.Game.Loot
{
  [CreateAssetMenu(menuName = "Karabaev/LootRegistry")]
  public class LootRegistry : ScriptableObject
  {
    [SerializeField]
    private LootDescriptor[] _loot = null!;

    public IReadOnlyList<LootDescriptor> Loot => _loot;
  }
  
  [Serializable]
  public class LootDescriptor
  {
    [field: SerializeField]
    public string Id { get; private set; } = null!;

    [field: SerializeField]
    public LootView Prefab { get; private set; } = null!;

    [field: SerializeField]
    public LootType Type { get; private set; }
    
    [field: SerializeField]
    public string ItemId { get; private set; } = null!;
  }

  public enum LootType
  {
    Weapon,
    Ammo,
    Health
  }
}