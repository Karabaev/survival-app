using System.Diagnostics.CodeAnalysis;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using UnityEngine;

namespace Karabaev.Survival.Game.GameCamera
{
  [SuppressMessage("ReSharper", "Unity.RedundantHideInInspectorAttribute")]
  public class GameCameraView : UnityView
  {
    [field: SerializeField, HideInInspector]
    public Camera Camera { get; private set; } = null!;

    public float FocusCentering { private get; set; }
    
    public float FocusRadius { private get; set; }
    
    public Vector3 Position
    {
      set => transform.position = value;
    }

    public Vector3 Rotation
    {
      set => transform.rotation = Quaternion.Euler(value);
    }

    public Transform? Target { private get; set; }

    public float Zoom { get; set; }

    public Vector2 OrbitAngles { get; set; }

    private Vector3 _focusPoint;
    
    private void LateUpdate()
    {
      if(Target == null)
        return;

      var targetPosition = Target.position;
      
      var distance = Vector3.Distance(targetPosition, _focusPoint);
      var t = 1.0f;
      if(distance > 0.01f && FocusCentering > 0f)
        t = Mathf.Pow(1f - FocusCentering, Time.deltaTime);

      if(distance > FocusRadius)
        t = Mathf.Min(t, FocusRadius / distance);

      _focusPoint = Vector3.Lerp(targetPosition, _focusPoint, t);

      var lookRotation = Quaternion.Euler(OrbitAngles);
      var lookDirection = lookRotation * Vector3.forward;
      var lookPosition = _focusPoint - lookDirection * Zoom;
      transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    private void OnValidate() => Camera = this.RequireComponent<Camera>();
  }
}