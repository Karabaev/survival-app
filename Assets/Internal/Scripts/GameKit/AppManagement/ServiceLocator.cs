using VContainer;

namespace Karabaev.GameKit.AppManagement
{
  public static class ServiceLocator
  {
    public static IObjectResolver Resolver { get; set; } = null!;
    
    public static T Resolve<T>() => Resolver.Resolve<T>();
  }
}