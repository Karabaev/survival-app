using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Damageable;
using Karabaev.Survival.Game.Hero;
using UnityEngine;

namespace Karabaev.Survival.Game.Enemy
{
  [UsedImplicitly]
  public class EnemyEntity : Entity<EnemyEntity.Context, EnemyModel, EnemyView>
  {
    private GameTime _nextAttackTime = GameTime.Min;
    private Vector3[]? _path;
    private int _currentPathIndex;

    private bool _dying;

    protected override UniTask OnCreatedAsync(Context context)
    {
      View.HitImpactPrefab = Model.Descriptor.HitImpactPrefab;
      Model.HitImpactFired.Triggered += Model_OnHitImpactFired;
      Model.CurrentHp.Changed += Model_OnCurrentHpChanged;
      Model.Target.Changed += Model_OnTargetChanged;

      Model_OnTargetChanged(null, Model.Target.Value);
      
      return UniTask.CompletedTask;
    }

    protected override void OnDisposed()
    {
      Model.HitImpactFired.Triggered -= Model_OnHitImpactFired;
      Model.CurrentHp.Changed -= Model_OnCurrentHpChanged;
      Model.Target.Changed -= Model_OnTargetChanged;

      if(Model.Target.Value != null)
        Model.Target.Value.Position.Changed -= Model_OnTargetPositionChanged;
    }

    protected override void OnTick(float deltaTime, GameTime now)
    {
      if(_dying)
        return;

      if(_path != null && _currentPathIndex < _path.Length)
      {
        var nextDestination = _path[_currentPathIndex];
        if(Vector3.Distance(View.Position, nextDestination) <= 0.3f)
        {
          _currentPathIndex++;
          if(_currentPathIndex < _path.Length)
            View.Forward = MathUtils.Direction2D(View.Position, _path[_currentPathIndex]);
        }

        View.AnimationMoving = true;
        View.Move(View.Forward * Model.Descriptor.MoveSpeed * deltaTime);
      }
       
      var heroesResult = View.CheckHeroes(Model.Descriptor.AttackDistance);
      if(heroesResult.HasValue && _nextAttackTime <= now)
        Attack(now, heroesResult.Value);

      var obstaclesResult = View.CheckObstacles(Model.Descriptor.AttackDistance);
      if(obstaclesResult.HasValue && _nextAttackTime <= now)
        Attack(now, obstaclesResult.Value);

      Model.Position = View.Position;
    }

    private void Model_OnHitImpactFired(Vector3 hitPosition) => View.ShowHitImpact(hitPosition);

    private async void Model_OnCurrentHpChanged(int oldValue, int newValue)
    {
      if(newValue > 0)
        return;

      _dying = true;
      await View.DieAsync();
      Model.Dead.Value = true;
    }

    private void Model_OnTargetChanged(HeroModel? oldValue, HeroModel? newValue)
    {
      if(oldValue != null)
        oldValue.Position.Changed -= Model_OnTargetPositionChanged;

      if(newValue == null)
      {
        _path = null;
        View.AnimationMoving = false;
        return;
      }

      newValue.Position.Changed += Model_OnTargetPositionChanged;
      UpdatePath(newValue.Position.Value);
    }

    private void Model_OnTargetPositionChanged(Vector3 oldValue, Vector3 newValue) => UpdatePath(newValue);

    private void Attack(GameTime now, RaycastTestViewModel targetsResult)
    {
      View.AttackAnimation();
      var cooldown = 1.0f / Model.Descriptor.AttackRate;
      _nextAttackTime = now.AddSeconds(cooldown);
      var target = targetsResult.Damageable;
      target.CurrentHp.Value -= Model.Descriptor.Attack;
      target.HitImpactFired.Set(targetsResult.ContactPosition);
    }

    private void UpdatePath(Vector3 destination)
    {
      _path = View.RecalculatePath(destination);
      _currentPathIndex = 0;
    }
    
    protected override EnemyModel CreateModel(Context context) => context.Model;

    protected override async UniTask<EnemyView> CreateViewAsync(Context context)
    {
      var view = Object.Instantiate(context.Model.Descriptor.Prefab, context.Parent);
      view.name = context.Model.Descriptor.Id;
      view.DamageableModel = context.Model;
      view.Position = Model.Position;
      await UniTask.Yield(PlayerLoopTiming.FixedUpdate); // wait until physics sync position
      return view;
    }

    public record Context(Transform Parent, EnemyModel Model);
  }
}