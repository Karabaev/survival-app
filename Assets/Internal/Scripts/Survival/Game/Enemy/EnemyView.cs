using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Damageable;
using UnityEngine;
using UnityEngine.AI;

namespace Karabaev.Survival.Game.Enemy
{
  public class EnemyView : UnityView, IDamageableView
  {
    [SerializeField, HideInInspector]
    private EnemyAnimationView _animationView = null!;
    [SerializeField, HideInInspector]
    private Collider _collider = null!;
    [SerializeField, HideInInspector]
    private NavMeshAgent _navMeshAgent = null!;
    
    public Vector3 Position
    {
      set => transform.position = value;
    }

    public bool AnimationMoving
    {
      set => _animationView.Moving = value;
    }

    public IDamageableModel DamageableModel { set; get; } = null!;

    public HitImpactView HitImpactPrefab { private get; set; } = null!;

    public float MoveSpeed
    {
      set => _navMeshAgent.speed = value;
    }

    public Vector3 Destination
    {
      set => _navMeshAgent.destination = value;
    }
    
    public void Attack() => _animationView.Attack();

    public void Die()
    {
      _animationView.RandomDie();
      _collider.enabled = false;
    }

    public void ShowHitImpact(Vector3 hitPosition)
    {
      var hitImpactObject = Instantiate(HitImpactPrefab);
      hitImpactObject.transform.position = hitPosition;
      hitImpactObject.Play();
    }
    
    private void OnValidate()
    {
      _animationView = this.RequireComponent<EnemyAnimationView>();
      _collider = this.RequireComponent<Collider>();
      _navMeshAgent = this.RequireComponent<NavMeshAgent>();
    }
  }
}