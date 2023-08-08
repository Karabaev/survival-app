using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Karabaev.GameKit.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Karabaev.GameKit.Entities
{
  public interface IEntity : IDisposable
  {
    void Tick(float deltaTime, GameTime now);
  }
  
  public interface IEntity<in TContext> : IEntity
  {
    UniTask InitializeAsync(TContext context);
  }
  
  public abstract class Entity<TContext, TModel, TView> : IEntity<TContext> where TView : IDisposable
  {
    [Inject]
    private IObjectResolver _objectResolver = null!;

    protected TModel Model { get; private set; } = default!;
    
    protected TView View { get; private set; } = default!;

    protected IObjectResolver Resolver => _objectResolver;
    
    protected List<IEntity> Children { get; private set; } = default!;

    private bool _initialized;
    private List<IDisposable> _disposables = null!;
    
    public async UniTask InitializeAsync(TContext context)
    {
      Model = CreateModel(context);
      View = await CreateViewAsync(context);
      Children = new List<IEntity>();
      _disposables = new List<IDisposable>();
      
      await OnCreatedAsync(context);

      _initialized = true;
    }

    public void Dispose()
    {
      foreach(var disposable in _disposables)
        disposable.Dispose();
      
      foreach(var child in Children)
        child.Dispose();

      View.Dispose();
      OnDisposed();
    }

    void IEntity.Tick(float deltaTime, GameTime now)
    {
      if(!_initialized)
        return;
      
      foreach(var entity in Children)
        entity.Tick(deltaTime, now);
      
      OnTick(deltaTime, now);
    }

    protected async UniTask<TChildEntity> CreateChildAsync<TChildEntity, TChildContext>(TChildContext context) where TChildEntity : IEntity<TChildContext>
    {
      var entity = Activator.CreateInstance<TChildEntity>();
      Inject(entity);
      Children.Add(entity);
      await entity.InitializeAsync(context);
      return entity;
    }

    protected void DisposeChild(IEntity child)
    {
      child.Dispose();
      Children.Remove(child);
    }
    
    protected void Inject(object target) => _objectResolver.Inject(target);

    protected void RegisterDisposable(IDisposable disposable) => _disposables.Add(disposable);

    protected virtual UniTask OnCreatedAsync(TContext context) => UniTask.CompletedTask;
    
    protected virtual void OnTick(float deltaTime, GameTime now) { }
    
    protected virtual void OnDisposed() { }

    protected abstract TModel CreateModel(TContext context);
    
    protected abstract UniTask<TView> CreateViewAsync(TContext context);
  }
}