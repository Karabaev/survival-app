using System;
using System.Collections.Generic;
using Karabaev.GameKit.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Karabaev.Survival.Game.GameInput
{
  public class GameInputView : UnityView
  {
    public bool Enabled
    {
      set
      {
        enabled = value;
        MouseWheelAxis = 0.0f;
      }
    }

    public Vector2 MainAxis { get; private set; }
    
    public float MouseWheelAxis { get; private set; }
    
    public Vector2 MousePosition => Input.mousePosition;

    public event Action<Vector2>? FireClicked;

    public event Action<Vector2>? AuxMouseButtonDown;
    
    public event Action<Vector2>? AuxMouseButtonUp;
    
    private void Update()
    {
      if(Input.GetAxis("Fire1") != 0 && !IsPointerOverUI())
        FireClicked?.Invoke(Input.mousePosition);

      MainAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
      MouseWheelAxis = Input.GetAxis("Mouse ScrollWheel");
      
      // todo change to axis
      if(Input.GetMouseButtonDown(1) && !IsPointerOverUI())
        AuxMouseButtonDown?.Invoke(Input.mousePosition);

      if(Input.GetMouseButtonUp(1))
        AuxMouseButtonUp?.Invoke(Input.mousePosition);
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