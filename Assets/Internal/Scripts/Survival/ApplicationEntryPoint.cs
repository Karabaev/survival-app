using Cysharp.Threading.Tasks;
using Karabaev.GameKit.AppManagement;
using Karabaev.GameKit.AppManagement.Contexts;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.GameKit.ForResources;
using Karabaev.Survival.Audio;
using Karabaev.Survival.Descriptors;
using Karabaev.Survival.Game;
using Karabaev.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Karabaev.Survival
{
  public class ApplicationEntryPoint : MonoBehaviour
  {
    [SerializeField]
    private bool _runApplication = true;
    
    private LifetimeScope _appScope = null!;

    private void Awake()
    {
      if (!_runApplication) 
      {
        Destroy(this);
        return;
      }
      
      Application.targetFrameRate = 120;
      DontDestroyOnLoad(this);
      _appScope = LifetimeScope.Create(ConfigureAppScope);
      _appScope.name = "ApplicationScope";
      transform.AddChild(_appScope);
        
      var stateMachine = _appScope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<ApplicationLoadingApplicationState, ScopeStateContext>(new ScopeStateContext(_appScope)).Forget();
    }
    
    private void OnDestroy()
    {
      if(!_runApplication)
        return;
      
      _appScope.Dispose();
    }

    private void ConfigureAppScope(IContainerBuilder builder)
    {
      builder.Register<ApplicationStateMachine>(Lifetime.Singleton);
      builder.Register<FromResourceFactory>(Lifetime.Singleton);
      builder.Register<IResourceService, ResourceService>(Lifetime.Singleton);
      builder.Register<SceneService>(Lifetime.Singleton);
      builder.Register<UIService>(Lifetime.Singleton);
      builder.Register<EntitiesManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
      builder.Register<DescriptorsAccess>(Lifetime.Singleton);
      builder.Register<AudioService>(Lifetime.Singleton);
      
      builder.Register<ApplicationLoadingApplicationState>(Lifetime.Transient);
      builder.Register<GameApplicationState>(Lifetime.Transient);
    }
  }
}