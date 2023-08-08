using Cysharp.Threading.Tasks;
using VContainer;

namespace Karabaev.GameKit.AppManagement
{
  public interface IApplicationState
  {
    UniTask ExitAsync();
  }
  
  public abstract class ApplicationState<TContext> : IApplicationState
  {
    protected ApplicationStateMachine StateMachine { get; }

    protected T Resolve<T>() => ServiceLocator.Resolve<T>();

    protected IObjectResolver Resolver => ServiceLocator.Resolver;
    
    public abstract UniTask EnterAsync(TContext context);

    public abstract UniTask ExitAsync();

    protected ApplicationState(ApplicationStateMachine stateMachine) => StateMachine = stateMachine;
  }
}