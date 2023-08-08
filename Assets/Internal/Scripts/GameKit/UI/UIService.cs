using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.ForResources;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Karabaev.UI
{
  [UsedImplicitly]
  public class UIService
  {
    private Dictionary<GameObject, int> _uiElements = new(4);

    public Canvas MainCanvas { get; private set; } = null!;

    public async UniTask InitializeAsync(string uiPath, FromResourceFactory factory, CancellationToken cancellationToken = default)
    {
      MainCanvas = await factory.CreateAsync<Canvas>(uiPath, null, cancellationToken);
      MainCanvas.name = "=====UI=====";
      Object.DontDestroyOnLoad(MainCanvas);

      foreach(var element in _uiElements)
        element.Key.DestroyObject();

      _uiElements.Clear();
    }

    public async UniTask<T> CreateAsync<T>(string resource, FromResourceFactory factory, int priority = -1) where T : MonoBehaviour
    {
      var instance = await factory.CreateAsync<T>(resource, null);
      Attach(instance.gameObject, priority);
      return instance;
    }

    public async UniTask<MonoBehaviour> CreateAsync(Type controllerType, string resource, FromResourceFactory factory, int priority = -1)
    {
      var instance = await factory.CreateAsync(resource);
      Attach(instance, priority);
      return (MonoBehaviour)instance.RequireComponent(controllerType);
    }

    public void Show(GameObject element, int priority = -1)
    {
      element.SetActive(true);
      Attach(element, priority);
    }

    public void Hide(GameObject element)
    {
      element.SetActive(false);
      Detach(element);
    }

    public void Remove(GameObject element)
    {
      Hide(element);
      element.DestroyObject();
    }

    private void Attach(GameObject element, int priority = -1)
    {
      element.transform.SetParent(MainCanvas.transform, false);
      element.transform.localScale = Vector3.one;
      _uiElements[element] = priority;

      if(priority == -1)
        element.transform.SetAsFirstSibling();
      else
        SortElements();
    }

    private void Detach(GameObject element) => _uiElements.Remove(element);

    private void SortElements()
    {
      _uiElements = _uiElements.OrderBy(e => e.Value).ToDictionary(e => e.Key, e => e.Value);

      foreach(var element in _uiElements)
        element.Key.transform.SetAsLastSibling();
    }
  }
}