using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common.Utils;
using UnityEngine;

namespace Karabaev.GameKit.Common.Animations
{
  [PublicAPI]
  public class AnimationEventsComponent : MonoBehaviour
  {
    [SerializeField, HideInInspector]
    private Animator _animator = null!;
    [SerializeField, HideInInspector]
    private List<string> _layerNames = null!;

    public event Action<int>? StateEnter;

    public event Action<int>? StateFinish;

    public event Action<int>? StateExit;

    private AnimatorStateMachineBehaviour[] _behaviours = null!;

    private readonly Dictionary<int, List<Action>> _animationStartHandlers = new();
    private readonly Dictionary<int, List<Action>> _animationFinishHandlers = new();
    private readonly Dictionary<int, List<Action>> _animationExitHandlers = new();
    private readonly Dictionary<int, List<UniTaskCompletionSource>> _animationStartWaiters = new();
    private readonly Dictionary<int, List<UniTaskCompletionSource>> _animationFinishWaiters = new();
    private readonly Dictionary<int, List<UniTaskCompletionSource>> _animationExitWaiters = new();
    private readonly List<Action> _modificationHandlersList = new();

    public UniTask WaitForAnimationStart(string stateName, int layerIndex)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationStartWaiters.ContainsKey(hash))
        _animationStartWaiters[hash] = new List<UniTaskCompletionSource>();

      var completionSource = new UniTaskCompletionSource();
      _animationStartWaiters[hash].Add(completionSource);
      return completionSource.Task;
    }

    public UniTask WaitForAnimationFinish(string stateName, int layerIndex)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationFinishWaiters.ContainsKey(hash))
        _animationFinishWaiters[hash] = new List<UniTaskCompletionSource>();

      var completionSource = new UniTaskCompletionSource();
      _animationFinishWaiters[hash].Add(completionSource);
      return completionSource.Task;
    }

    public UniTask WaitForAnimationExit(string stateName, int layerIndex)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationExitWaiters.ContainsKey(hash))
        _animationExitWaiters[hash] = new List<UniTaskCompletionSource>();

      var completionSource = new UniTaskCompletionSource();
      _animationExitWaiters[hash].Add(completionSource);
      return completionSource.Task;
    }

    public void RegisterAnimationStart(string stateName, int layerIndex, Action action)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationStartHandlers.ContainsKey(hash))
        _animationStartHandlers[hash] = new List<Action>();

      _animationStartHandlers[hash].Add(action);
    }

    public void RegisterAnimationFinish(string stateName, int layerIndex, Action action)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationFinishHandlers.ContainsKey(hash))
        _animationFinishHandlers[hash] = new List<Action>();

      _animationFinishHandlers[hash].Add(action);
    }

    public void RegisterAnimationExit(string stateName, int layerIndex, Action action)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationExitHandlers.ContainsKey(hash))
        _animationExitHandlers[hash] = new List<Action>();

      _animationExitHandlers[hash].Add(action);
    }

    public void UnregisterAnimationStart(string stateName, int layerIndex, Action handler)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationStartHandlers.ContainsKey(hash))
        return;

      _modificationHandlersList.Add(() => _animationStartHandlers[hash].Remove(handler));
    }

    public void UnregisterAnimationFinish(string stateName, int layerIndex, Action handler)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationFinishHandlers.ContainsKey(hash))
        return;

      _modificationHandlersList.Add(() => _animationFinishHandlers[hash].Remove(handler));
    }

    public void UnregisterAnimationExit(string stateName, int layerIndex, Action handler)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationExitHandlers.ContainsKey(hash))
        return;

      _modificationHandlersList.Add(() => _animationExitHandlers[hash].Remove(handler));
    }

    public void UnregisterAllAnimationsStart(string stateName, int layerIndex)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationStartHandlers.ContainsKey(hash))
        return;

      _animationStartHandlers[hash].Clear();
    }

    public void UnregisterAllAnimationsFinish(string stateName, int layerIndex)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationFinishHandlers.ContainsKey(hash))
        return;

      _animationFinishHandlers[hash].Clear();
    }

    public void UnregisterAllAnimationsExit(string stateName, int layerIndex)
    {
      var hash = ComputeStateHash(stateName, layerIndex);

      if(!_animationExitHandlers.ContainsKey(hash))
        return;

      _animationExitHandlers[hash].Clear();
    }

    private int ComputeStateHash(string stateName, int layerIndex)
    {
      var requiredLayerName = _layerNames[layerIndex];
      return Animator.StringToHash($"{requiredLayerName}.{stateName}");
    }

    private void OnEnterState(int stateHash)
    {
      if(_animationStartHandlers.ContainsKey(stateHash))
        _animationStartHandlers[stateHash].ForEach(h => h.Invoke());

      if(_animationStartWaiters.ContainsKey(stateHash))
      {
        var waiters = _animationStartWaiters[stateHash];
        waiters.ForEach(w => w.TrySetResult());
        waiters.Clear();
      }

      StateEnter?.Invoke(stateHash);
    }

    private void OnFinishState(int stateHash)
    {
      if(_animationFinishHandlers.ContainsKey(stateHash))
        _animationFinishHandlers[stateHash].ForEach(h => h.Invoke());

      if(_animationFinishWaiters.ContainsKey(stateHash))
      {
        var waiters = _animationFinishWaiters[stateHash];
        waiters.ForEach(w => w.TrySetResult());
        waiters.Clear();
      }

      StateFinish?.Invoke(stateHash);
    }

    private void OnExitState(int stateHash)
    {
      if(_animationExitHandlers.ContainsKey(stateHash))
        _animationExitHandlers[stateHash].ForEach(h => h.Invoke());

      if(_animationExitWaiters.ContainsKey(stateHash))
      {
        var waiters = _animationExitWaiters[stateHash];
        waiters.ForEach(w => w.TrySetResult());
        waiters.Clear();
      }

      StateExit?.Invoke(stateHash);
    }

    private void OnValidate()
    {
      _animator = this.RequireComponentInChildren<Animator>();
#if UNITY_EDITOR
      UnityEditor.Animations.AnimatorController controller;

      if(_animator.runtimeAnimatorController is AnimatorOverrideController overrideController)
        controller = (UnityEditor.Animations.AnimatorController)overrideController.runtimeAnimatorController;
      else
        controller = (UnityEditor.Animations.AnimatorController)_animator.runtimeAnimatorController;

      foreach(var layer in controller.layers)
      {
        foreach(var state in layer.stateMachine.states)
          AddBehaviourToState(state.state);

        foreach(var stateMachine in layer.stateMachine.stateMachines)
          AddBehaviourToStatesInStateMachine(stateMachine);
      }

      _layerNames = controller.layers.Select(l => l.name).ToList();
#endif
    }

    private void OnEnable()
    {
      _behaviours = _animator.GetBehaviours<AnimatorStateMachineBehaviour>();

      foreach(var behaviour in _behaviours)
      {
        behaviour.StateEnter += OnEnterState;
        behaviour.StateFinish += OnFinishState;
        behaviour.StateExit += OnExitState;
      }
    }

    private void OnDisable()
    {
      foreach(var behaviour in _behaviours)
      {
        behaviour.StateEnter -= OnEnterState;
        behaviour.StateFinish -= OnFinishState;
        behaviour.StateExit -= OnExitState;
      }

      foreach(var (_, waiters) in _animationStartWaiters)
      {
        foreach(var waiter in waiters)
          waiter.TrySetCanceled();
      }

      foreach(var (_, waiters) in _animationFinishWaiters)
      {
        foreach(var waiter in waiters)
          waiter.TrySetCanceled();
      }

      foreach(var (_, waiters) in _animationExitWaiters)
      {
        foreach(var waiter in waiters)
          waiter.TrySetCanceled();
      }
    }

    private void Update()
    {
      if(_modificationHandlersList.Count == 0)
        return;

      _modificationHandlersList.ForEach(m => m.Invoke());
      _modificationHandlersList.Clear();
    }

#if UNITY_EDITOR

    private void AddBehaviourToStatesInStateMachine(UnityEditor.Animations.ChildAnimatorStateMachine stateMachine)
    {
      foreach(var state in stateMachine.stateMachine.states)
      {
        if(state.state.behaviours.Contains(b => b.GetType() == typeof(AnimatorStateMachineBehaviour)))
          continue;

        state.state.AddStateMachineBehaviour<AnimatorStateMachineBehaviour>();
      }

      foreach(var childStateMachine in stateMachine.stateMachine.stateMachines)
        AddBehaviourToStatesInStateMachine(childStateMachine);
    }

    private void AddBehaviourToState(UnityEditor.Animations.AnimatorState state)
    {
      if(state.behaviours.Contains(b => b.GetType() == typeof(AnimatorStateMachineBehaviour)))
        return;

      state.AddStateMachineBehaviour<AnimatorStateMachineBehaviour>();
    }

#endif
  }
}