using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Karabaev.GameKit.ForResources
{
  [UsedImplicitly]
  public class FromResourceFactory
  {
    private readonly IResourceService _resourceService;
    private readonly IObjectResolver _objectResolver;
    
    public async UniTask<T> CreateAsync<T>(string path, CancellationToken token = default) where T : Object
    {
      var resource = await LoadResourceAsync<T>(path, token);
      return Instantiate(resource, null);
    }

    public async UniTask<GameObject> CreateAsync(string path, Transform? parent = null, CancellationToken token = default)
    {
      var resource = await LoadResourceAsync<GameObject>(path, token);
      var result = Instantiate(resource, parent);
      return result;
    }

    public async UniTask<T> CreateAsync<T>(string path, Transform? parent = null, CancellationToken token = default) where T : Component
    {
      var resource = await LoadResourceAsync<T>(path, token);
      var result = Instantiate(resource, parent);
      return result;
    }

    public async UniTask<IReadOnlyList<T>> CreateAsync<T>(string path, int count, CancellationToken token = default) where T : Object
    {
      var prefab = await LoadResourceAsync<T>(path, token);
      var result = new List<T>(count);

      for(var i = 0; i < count; i++)
        result.Add(Instantiate(prefab, null));

      return result;
    }

    private async UniTask<T> LoadResourceAsync<T>(string path, CancellationToken token = default) where T : Object
    {
      var resource = await _resourceService.LoadAsync<T>(path, token);

      if(!resource)
        throw new NullReferenceException($"Resource was not loaded. Resource={path}");
      
      return resource;
    }

    private T Instantiate<T>(T prefab, Transform? parent) where T : Object
    {
      var instance = Object.Instantiate(prefab, parent);

      if(instance is GameObject go)
        _objectResolver.InjectGameObject(go);
      else if(instance is Component component)
        _objectResolver.InjectGameObject(component.gameObject);

      instance.name = prefab.name;
      return instance;
    }

    public FromResourceFactory(IResourceService resourceService, IObjectResolver objectResolver)
    {
      _resourceService = resourceService;
      _objectResolver = objectResolver;
    }
  }
}