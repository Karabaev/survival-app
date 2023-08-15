using Cysharp.Threading.Tasks;
using Karabaev.GameKit.Common.Animations;
using Karabaev.GameKit.Common.Utils;
using UnityEngine;

namespace Karabaev.Survival.Game.Enemy
{
  public class EnemyAnimationView : MonoBehaviour
  {
    private const int LayerIndex = 0;

    private static readonly int MovingHash = Animator.StringToHash("Moving");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private const string DieStateName = "Die";
    
    [SerializeField, HideInInspector]
    private Animator _animator = null!;
    [SerializeField, HideInInspector]
    private AnimationEventsComponent _animationEvents = null!;

    public bool Moving
    {
      set => _animator.SetBool(MovingHash, value);
    }
    
    public UniTask StartAndWaitDieAsync()
    {
      var task = _animationEvents.WaitForAnimationFinish(DieStateName, LayerIndex);
      _animator.SetTrigger(DieHash);
      return task;
    }

    public void Attack() => _animator.SetTrigger(AttackHash);

    private void OnValidate()
    {
      _animator = this.RequireComponent<Animator>();
      _animationEvents = this.RequireComponent<AnimationEventsComponent>();
    }
  }
}