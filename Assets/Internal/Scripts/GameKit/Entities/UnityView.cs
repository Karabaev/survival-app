using System;
using Karabaev.GameKit.Common;
using Karabaev.GameKit.Common.Utils;
using UnityEngine;

namespace Karabaev.GameKit.Entities
{
  public abstract class UnityView : MonoBehaviour, IDisposable
  {
    public void Dispose()
    {
      if(ApplicationStateListener.Quitting)
        return;
      
      this.DestroyObject();
      OnDisposed();
    }

    protected virtual void OnDisposed() { }
  }
}