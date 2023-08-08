using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Karabaev.GameKit.ForResources
{
  [UsedImplicitly]
  public class ResourceService : IResourceService
  {
    public async UniTask<T> LoadAsync<T>(string path, CancellationToken cancellationToken = default) where T : Object
    {
      var loadAsync = Resources.LoadAsync<T>(path);
      var resource = await loadAsync.ToUniTask(cancellationToken: cancellationToken);
      return (T)resource;
    }

    public T Load<T>(string path) where T : Object => Resources.Load<T>(path);

    public void Unload(Object resource) => Resources.UnloadAsset(resource);
  }
}