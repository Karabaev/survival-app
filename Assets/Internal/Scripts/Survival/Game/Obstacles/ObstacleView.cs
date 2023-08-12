using Cysharp.Threading.Tasks;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Damageable;
using Unity.AI.Navigation;
using UnityEngine;

namespace Karabaev.Survival.Game.Obstacles
{
  public class ObstacleView : UnityView, IDamageableView
  {
    [SerializeField, HideInInspector]
    private ObstacleAnimationView _animationView = null!;
    [SerializeField, HideInInspector]
    private NavMeshModifierVolume _modifierVolume = null!;
    [SerializeField, HideInInspector]
    private Collider _collider = null!;
    
    [field: SerializeField]
    public ObstacleDescriptor Descriptor { get; private set; } = null!;

    public IDamageableModel DamageableModel { get; set; } = null!;
    
    public void ShowHitImpact(Vector3 hitPosition)
    {
      var hitImpactObject = Instantiate(Descriptor.HitImpactPrefab);
      hitImpactObject.transform.position = hitPosition;
      hitImpactObject.Play();
    }

    public UniTask DestroyAsync()
    {
      _modifierVolume.enabled = false;
      _collider.enabled = false;
      return _animationView.StartAndWaitDestroyAsync();
    }

    private void OnValidate()
    {
      _animationView = this.RequireComponent<ObstacleAnimationView>();
      _modifierVolume = this.RequireComponent<NavMeshModifierVolume>();
      _collider = this.RequireComponent<Collider>();
    }
  }
}