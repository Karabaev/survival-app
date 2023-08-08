using System;
using System.Collections.Generic;
using Karabaev.GameKit.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Karabaev.Survival.Game.GameInput
{
  public class GameInputView : UnityView
  {
    public event Action<Vector2>? FireClicked;

    public event Action<Vector2>? AxisChanged;

    private Vector2 _axis;
    
    private void Update()
    {
      if(Input.GetAxis("Fire1") != 0 && !IsPointerOverUI())
        FireClicked?.Invoke(Input.mousePosition);

      var axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

      if(axis != _axis)
      {
        AxisChanged?.Invoke(axis);
        _axis = axis;
      }
    }
    
    private bool IsPointerOverUI()
    {
      if(!EventSystem.current.IsPointerOverGameObject())
        return false;
      
      var pointerEventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

      var results = new List<RaycastResult>();
      EventSystem.current.RaycastAll(pointerEventData, results);

      return results.Count > 0;
    }
  }
}