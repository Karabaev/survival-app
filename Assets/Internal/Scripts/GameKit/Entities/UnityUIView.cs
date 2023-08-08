using System;
using Karabaev.UI;
using UnityEngine;
using VContainer;

namespace Karabaev.GameKit.Entities
{
  public abstract class UnityUIView : MonoBehaviour, IDisposable
  {
    [Inject]
    private readonly UIService _uiService = null!;
    
    public void Dispose()
    {
      _uiService.Remove(gameObject);
      OnDisposed();
    }
    
    protected virtual void OnDisposed() { }
  }
}