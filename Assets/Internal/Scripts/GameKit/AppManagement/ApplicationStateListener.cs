using System.Threading;
using UnityEngine;

namespace Karabaev.GameKit.AppManagement
{
  public static class ApplicationStateListener
  {
    private static readonly CancellationTokenSource ApplicationQuiteCts = new();
    
    public static bool Quitting { get; private set; }

    public static CancellationToken ApplicationQuiteCancellation => ApplicationQuiteCts.Token;
    
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
      Application.quitting += () =>
      {
        Quitting = true;
        ApplicationQuiteCts.Cancel();
        ApplicationQuiteCts.Dispose();
      };
      var obj = Object.Instantiate(Resources.Load<GameObject>("PF_Application"));
      obj.name = "=====Application=====";
    }
  }
}