using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Karabaev.GameKit.AppManagement
{
  [UsedImplicitly]
  public class SceneService
  {

    public async UniTask OpenAsync(string sceneName, LifetimeScope scope, IProgress<float>? progress = null, float maxProgress = 0.1f)
    {
      var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

      while (!operation.isDone) {
        progress?.Report(operation.progress * maxProgress);
        await UniTask.Yield();
      }

      ServiceLocator.Resolver = scope.Container;
    }

    public async UniTask OpenAsync(string sceneName, Func<LifetimeScope> scopeFunc, IProgress<float>? progress = null, float maxProgress = 0.1f)
    {
      var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

      while (!operation.isDone) {
        progress?.Report(operation.progress * maxProgress);
        await UniTask.Yield();
      }

      ServiceLocator.Resolver = scopeFunc.Invoke().Container;
    }
  }
}