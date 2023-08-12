using Cysharp.Threading.Tasks;
using Karabaev.GameKit.Common.Animations;
using Karabaev.GameKit.Common.Utils;
using UnityEngine;

namespace Karabaev.Survival.Game.Obstacles
{
  public class ObstacleAnimationView : MonoBehaviour
  {
    private const int LayerIndex = 0;
    
    private static readonly int DestroyHash = Animator.StringToHash("Destroy");
    private const string DestroyStateName = "Destroy";
    
    [SerializeField, HideInInspector]
    private AnimationEventsComponent _animationEvents = null!;
    [SerializeField, HideInInspector]
    private Animator _animator = null!;

    public UniTask StartAndWaitDestroyAsync()
    {
      var task = _animationEvents.WaitForAnimationFinish(DestroyStateName, LayerIndex);
      _animator.SetTrigger(DestroyHash);
      return task;
    }

    private void OnValidate()
    {
      _animationEvents = this.RequireComponent<AnimationEventsComponent>();
      _animator = this.RequireComponent<Animator>();
    }
  }
}