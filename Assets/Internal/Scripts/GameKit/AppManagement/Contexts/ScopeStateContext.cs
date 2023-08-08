using VContainer.Unity;

namespace Karabaev.GameKit.AppManagement.Contexts
{
  public readonly struct ScopeStateContext
  {
    public LifetimeScope ParentScope { get; }

    public ScopeStateContext(LifetimeScope parentScope) => ParentScope = parentScope;
  }
}