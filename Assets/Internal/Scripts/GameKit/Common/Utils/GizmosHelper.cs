using UnityEditor;
using UnityEngine;

namespace Karabaev.GameKit.Common.Utils
{
  public static class GizmosHelper
  {
    public static void DrawString(string text, Vector3 worldPos, Color? textColor = null, Color? backColor = null)
    {
      Handles.BeginGUI();
      var restoreTextColor = GUI.color;
      var restoreBackColor = GUI.backgroundColor;

      GUI.color = textColor ?? Color.white;
      GUI.backgroundColor = backColor ?? Color.black;

      var view = SceneView.currentDrawingSceneView;
      if (view != null && view.camera != null)
      {
        // worldPos.y *= -1;
        var screenPos = view.camera.WorldToScreenPoint(worldPos);
        if(screenPos.y < 0.0f || screenPos.y > Screen.height || screenPos.x < 0.0f || screenPos.x > Screen.width || screenPos.z < 0.0f)
        {
          GUI.color = restoreTextColor;
          Handles.EndGUI();
          return;
        }
        var size = GUI.skin.label.CalcSize(new GUIContent(text));
        var rect = new Rect(screenPos.x - size.x * 0.5f, view.position.height - screenPos.y, size.x, size.y);

        GUI.Box(rect, text, EditorStyles.numberField);
        GUI.Label(rect, text);
        GUI.color = restoreTextColor;
        GUI.backgroundColor = restoreBackColor;
      }
      Handles.EndGUI();
    }
  }
}