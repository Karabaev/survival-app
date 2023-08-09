using System;
using System.Collections.Generic;
using UnityEngine;

namespace Karabaev.Survival.Game.Weapons
{
  [CreateAssetMenu(menuName = "Karabaev/WeaponsRegistry")]
  public class WeaponsRegistry : ScriptableObject
  {
    [SerializeField]
    private WeaponDescriptor[] _weapons = null!;

    public IReadOnlyList<WeaponDescriptor> Weapons => _weapons;
  }

  [Serializable]
  public record WeaponDescriptor
  {
    [field: SerializeField]
    public string Id { get; private set; } = null!;
    
    [field: SerializeField]
    public GameObject EquippedPrefab { get; private set; } = null!;

    [field: SerializeField]
    public string SlotName { get; private set; } = null!;

    [field: SerializeField]
    public AnimatorOverrideController AnimatorController { get; private set; } = null!;

    [field: SerializeField]
    public int Magazine { get; private set; }
    
    [field: SerializeField]
    public GameObject ProjectilePrefab { get; private set; } = null!;
  }
}