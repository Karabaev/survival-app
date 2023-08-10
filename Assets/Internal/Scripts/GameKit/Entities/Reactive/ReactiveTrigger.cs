using Cysharp.Threading.Tasks;

namespace Karabaev.GameKit.Entities.Reactive
{
  public class ReactiveTrigger
  {
    public delegate void TriggeredHandler();

    public event TriggeredHandler? Triggered;

    public void Set() => Triggered?.Invoke();
  }

  public class ReactiveTrigger<T>
  {
    public delegate void TriggeredHandler(T value);

    public event TriggeredHandler? Triggered;

    public void Set(T value) => Triggered?.Invoke(value);
  }

  public class AsyncReactiveTrigger
  {
    public delegate UniTask TriggeredHandler();
    
    public event TriggeredHandler? Triggered;
    
    public UniTask Set() => Triggered?.Invoke() ?? UniTask.CompletedTask;
  }
}