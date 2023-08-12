using Karabaev.GameKit.Common;

namespace Karabaev.Survival.Game.Enemy.States
{
  // todokmo
  public interface IEnemyState
  {
    IEnemyState ToIdle();
    
    IEnemyState ToRunning();

    IEnemyState ToAttacking();

    IEnemyState ToDying();

    void OnTick(float deltaTime, GameTime now);
  }

  public class IdleEnemyState : IEnemyState
  {
    public IEnemyState ToIdle() => this;

    public IEnemyState ToRunning() => new RunningEnemyState();

    public IEnemyState ToAttacking() => new AttackingEnemyState();

    public IEnemyState ToDying() => new DyingEnemyState();

    public void OnTick(float deltaTime, GameTime now) { }
  }

  public class RunningEnemyState : IEnemyState
  {
    public IEnemyState ToIdle() => new IdleEnemyState();

    public IEnemyState ToRunning() => this;

    public IEnemyState ToAttacking() => new AttackingEnemyState();

    public IEnemyState ToDying() => new DyingEnemyState();

    public void OnTick(float deltaTime, GameTime now)
    {
    }
  }

  public class AttackingEnemyState : IEnemyState
  {
    public IEnemyState ToIdle() => new IdleEnemyState();

    public IEnemyState ToRunning() => new RunningEnemyState();

    public IEnemyState ToAttacking() => this;

    public IEnemyState ToDying() => new DyingEnemyState();

    public void OnTick(float deltaTime, GameTime now)
    {
    }
  }

  public class DyingEnemyState : IEnemyState
  {
    public IEnemyState ToIdle() => new IdleEnemyState();

    public IEnemyState ToRunning() => new RunningEnemyState();

    public IEnemyState ToAttacking() => new AttackingEnemyState();

    public IEnemyState ToDying() => this;

    public void OnTick(float deltaTime, GameTime now)
    {
    }
  }
}