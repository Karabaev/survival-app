using Karabaev.GameKit.Common.Utils;
using UnityEngine;

namespace Karabaev.Survival.Game.Enemy
{
  public class EnemyAnimationView : MonoBehaviour
  {
    private static readonly int MovingHash = Animator.StringToHash("Moving");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int DieHash = Animator.StringToHash("Die");
    private static readonly int DieTypeHash = Animator.StringToHash("DieType");
    
    [SerializeField, HideInInspector]
    private Animator _animator = null!;

    public bool Moving
    {
      set => _animator.SetBool(MovingHash, value);
    }
    
    public void RandomDie()
    {
      _animator.SetTrigger(DieHash);
      _animator.SetInteger(DieTypeHash, Random.Range(0, 2));
    }

    public void Attack() => _animator.SetTrigger(AttackHash);

    private void OnValidate() => _animator = this.RequireComponent<Animator>();
  }
}