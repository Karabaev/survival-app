using Karabaev.Survival.Descriptors;
using UnityEngine;

namespace Karabaev.Survival.Game.Weapons.Descriptors
{
  [CreateAssetMenu(menuName = "Karabaev/NewWeapon")]
  public class WeaponDescriptor : Descriptor
  {
    [field: SerializeField]
    public Sprite Icon { get; private set; } = null!;
    
    [field: SerializeField]
    public WeaponView EquippedPrefab { get; private set; } = null!;

    [field: SerializeField]
    public string SlotName { get; private set; } = null!;

    [field: SerializeField]
    public AnimatorOverrideController AnimatorController { get; private set; } = null!;
    
    [field: SerializeField]
    public int Damage { get; private set; }
    
    [field: SerializeField]
    public int Magazine { get; private set; }
    
    [field: SerializeField]
    public float FireRate { get; private set; }
    
    [field: SerializeField]
    public float ReloadDuration { get; private set; }

    [field: SerializeField]
    public AudioClip ShotSound { get; private set; } = null!;
    
    [field: SerializeField]
    public GameObject ProjectilePrefab { get; private set; } = null!;
  }
}