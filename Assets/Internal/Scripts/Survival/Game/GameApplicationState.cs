using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.AppManagement;
using Karabaev.GameKit.AppManagement.Contexts;

namespace Karabaev.Survival.Game
{
  [UsedImplicitly]
  public class GameApplicationState : ApplicationState<DummyStateContext>
  {
    public override async UniTask EnterAsync(DummyStateContext context)
    {
      await Resolve<SceneService>().OpenAsync("Game", ScopeRegistry.AppScope);
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    public GameApplicationState(ApplicationStateMachine stateMachine) : base(stateMachine) { }
  }
}