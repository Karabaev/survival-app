using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.AppManagement.Contexts;
using VContainer;

namespace Karabaev.GameKit.AppManagement
{
  [UsedImplicitly]
  public class ApplicationStateMachine
  {
    private readonly IObjectResolver _objectResolver;
    
    private IApplicationState? _activeState;
    
    public async UniTask EnterAsync<TState, TContext>(TContext context) where TState : ApplicationState<TContext>
    {
      if(_activeState != null)
        await _activeState.ExitAsync();

      var state = CreateState<TState>();
      _activeState = state;
      await state.EnterAsync(context);
    }

    public UniTask EnterAsync<TState>() where TState : ApplicationState<DummyStateContext> => EnterAsync<TState, DummyStateContext>(default);

    private TState CreateState<TState>() where TState : IApplicationState => _objectResolver.Resolve<TState>();

    public ApplicationStateMachine(IObjectResolver objectResolver) => _objectResolver = objectResolver;
  }
}