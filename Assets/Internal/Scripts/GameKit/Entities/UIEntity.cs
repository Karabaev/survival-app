using Cysharp.Threading.Tasks;
using Karabaev.GameKit.ForResources;
using Karabaev.UI;
using VContainer;

namespace Karabaev.GameKit.Entities
{
  public abstract class UIEntity<TContext, TModel, TView> : Entity<TContext, TModel, TView> where TView : UnityUIView
  {
    [Inject]
    private readonly UIService _uiService = null!;
    [Inject]
    private readonly FromResourceFactory _fromResourceFactory = null!;
    
    protected abstract string ViewPrefabPath { get; }

    protected virtual int Priority => -1;
    
    protected sealed override async UniTask<TView> CreateViewAsync(TContext context)
    {
      var view = await _uiService.CreateAsync<TView>(ViewPrefabPath, _fromResourceFactory, Priority);
      OnViewCreated(context, view);
      return view;
    }

    protected virtual void OnViewCreated(TContext context, TView view) { }
  }
}