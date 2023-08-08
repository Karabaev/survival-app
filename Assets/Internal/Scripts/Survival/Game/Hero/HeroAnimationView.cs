using Karabaev.GameKit.Common.Utils;
using UnityEngine;

namespace Karabaev.Survival.Game.Hero
{
  public class HeroAnimationView : MonoBehaviour
  {
    private static readonly int ForwardHash = Animator.StringToHash("Forward");
    private static readonly int RightHash = Animator.StringToHash("Right");
    
    private static readonly int ShotHash = Animator.StringToHash("Shot");
    private static readonly int ShotTypeHash = Animator.StringToHash("ShotType");
    
    private static readonly int ReloadHash = Animator.StringToHash("Reload");
    private static readonly int DeadHash = Animator.StringToHash("Dead");

    [SerializeField, HideInInspector]
    private Animator _animator = null!;

    public AnimatorOverrideController Controller
    {
      set => _animator.runtimeAnimatorController = value;
    }
    
    public Vector2 Velocity
    {
      set
      {
        _animator.SetFloat(ForwardHash, value.y);
        _animator.SetFloat(RightHash, value.x);
      }
    }

    public void RandomShot()
    {
      _animator.SetTrigger(ShotHash);
      _animator.SetInteger(ShotTypeHash, Random.Range(0, 3));
    }
    
    public void Reload() => _animator.SetTrigger(ReloadHash);
    
    public void Die() => _animator.SetTrigger(DeadHash);

    private void OnValidate() => _animator = this.RequireComponentInChildren<Animator>();
  }
}