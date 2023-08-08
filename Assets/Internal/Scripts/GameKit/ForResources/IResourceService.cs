using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Karabaev.GameKit.ForResources
{
  public interface IResourceService
  {
    UniTask<T> LoadAsync<T>(string path, CancellationToken cancellationToken = default) where T : Object;

    T Load<T>(string path) where T : Object;

    void Unload(Object resource);
  }
}