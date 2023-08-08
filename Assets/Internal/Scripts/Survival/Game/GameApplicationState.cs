using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.AppManagement;
using Karabaev.GameKit.AppManagement.Contexts;
using Karabaev.GameKit.Entities;
using VContainer;
using VContainer.Unity;

namespace Karabaev.Survival.Game
{
  [UsedImplicitly]
  public class GameApplicationState : ApplicationState<DummyStateContext>
  {
    private LifetimeScope _scope = null!;
    
    public override async UniTask EnterAsync(DummyStateContext context)
    {
      await Resolve<SceneService>().OpenAsync("Game", () => _scope = CreateScope(ScopeRegistry.AppScope));

      Resolve<EntitiesManager>().CreateEntity<GameEntity>(Resolver);
    }

    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      return UniTask.CompletedTask;
    }

    private LifetimeScope CreateScope(LifetimeScope lifetimeScope)
    {
      var scope = lifetimeScope.CreateChild(ConfigureScope);
      scope.name = "GameScope";
      return scope;
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
    }
    
    public GameApplicationState(ApplicationStateMachine stateMachine) : base(stateMachine) { }
  }
}