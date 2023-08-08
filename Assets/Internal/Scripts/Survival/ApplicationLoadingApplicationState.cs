using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.AppManagement;
using Karabaev.GameKit.AppManagement.Contexts;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.ForResources;
using Karabaev.Survival.Game;
using Karabaev.UI;

namespace Karabaev.Survival
{
  [UsedImplicitly]
  public class ApplicationLoadingApplicationState : ApplicationState<ScopeStateContext>
  {
    public override async UniTask EnterAsync(ScopeStateContext context)
    {
      ScopeRegistry.AppScope = context.ParentScope;
      ServiceLocator.Resolver = context.ParentScope.Container;

      var fromResourceFactory = Resolve<FromResourceFactory>();
      await Resolve<UIService>().InitializeAsync("UI/UI_MainCanvas", fromResourceFactory, ApplicationStateListener.ApplicationQuiteCancellation);
      await Resolve<SceneService>().OpenAsync("Bootstrap", context.ParentScope);
      await StateMachine.EnterAsync<GameApplicationState>();
    }

    public override UniTask ExitAsync() => UniTask.CompletedTask;

    public ApplicationLoadingApplicationState(ApplicationStateMachine stateMachine) : base(stateMachine) { }
  }
}