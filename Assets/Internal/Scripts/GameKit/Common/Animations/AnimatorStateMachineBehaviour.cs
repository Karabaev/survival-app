using System;
using UnityEngine;

namespace Karabaev.GameKit.Common.Animations
{
  public class AnimatorStateMachineBehaviour : StateMachineBehaviour
  {
    private const float StateFinishTime = 0.99f;

    public event Action<int>? StateEnter;

    public event Action<int>? StateFinish;

    public event Action<int>? StateExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => StateEnter?.Invoke(stateInfo.fullPathHash);

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => StateExit?.Invoke(stateInfo.fullPathHash);

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      if(stateInfo.normalizedTime < StateFinishTime)
        return;

      StateFinish?.Invoke(stateInfo.fullPathHash);
    }
  }
}