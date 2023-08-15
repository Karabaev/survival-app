using Cysharp.Threading.Tasks;
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
    private NavMeshView _navMeshView = null!;
    [SerializeField, HideInInspector]
    private CharacterController _characterController = null!;
    
    private NavMeshPath? _path;
    
    public Vector3 Position
    {
      get => transform.position;
      set => transform.position = value;
    }

    public Vector3 Forward
    {
      get => transform.forward;
      set => transform.forward = value;
    }

    public bool AnimationMoving
    {
      set => _animationView.Moving = value;
    }

    public IDamageableModel DamageableModel { set; get; } = null!;

    public HitImpactView HitImpactPrefab { private get; set; } = null!;
    
    public void Move(Vector3 velocity) => _characterController.Move(velocity);

    public RaycastTestViewModel? CheckObstacles(float distance) => CheckTargets(distance, LayerMask.GetMask("Obstacles"));

    public RaycastTestViewModel? CheckHeroes(float distance) => CheckTargets(distance, LayerMask.GetMask("Heroes"));

    private RaycastTestViewModel? CheckTargets(float distance, int layerMask)
    {
      var ray = new Ray(_collider.bounds.center, transform.forward);
      var raycastResult = Physics.Raycast(ray, out var hitInfo, distance, layerMask);
      if(!raycastResult)
        return null;
      
      var damageable = hitInfo.collider.RequireComponent<IDamageableView>();
      return new RaycastTestViewModel(damageable.DamageableModel, hitInfo.point);
    }

    public Vector3[] RecalculatePath(Vector3 destination) => _navMeshView.CalculatePath(destination);

    public void AttackAnimation() => _animationView.Attack();

    public UniTask DieAsync()
    {
      _collider.enabled = false;
      return _animationView.StartAndWaitDieAsync();
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
      _navMeshView = this.RequireComponent<NavMeshView>();
      _characterController = this.RequireComponent<CharacterController>();
    }
  }
}