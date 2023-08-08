using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.AppManagement;
using Karabaev.GameKit.AppManagement.Contexts;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Descriptors;
using VContainer;
using VContainer.Unity;

namespace Karabaev.Survival.Game
{
  [UsedImplicitly]
  public class GameApplicationState : ApplicationState<DummyStateContext>
  {
    private LifetimeScope _scope = null!;
    private GameEntity _rootEntity = null!;
    
    public override async UniTask EnterAsync(DummyStateContext context)
    {
      await Resolve<SceneService>().OpenAsync("Game", () => _scope = CreateScope(ScopeRegistry.AppScope));

      var descriptorsAccess = Resolve<DescriptorsAccess>();
      var heroesRegistry = descriptorsAccess.HeroesRegistry;
      var weaponsRegistry = descriptorsAccess.WeaponsRegistry;
      _rootEntity = Resolve<EntitiesManager>().CreateEntity<GameEntity>(Resolver);
      await _rootEntity.InitializeAsync(new GameEntity.Context(heroesRegistry.Heroes.PickRandom(), weaponsRegistry.Weapons.PickRandom()));
    }

    public override UniTask ExitAsync()
    {
      _rootEntity.Dispose();
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