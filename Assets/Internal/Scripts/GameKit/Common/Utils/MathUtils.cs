using UnityEngine;

namespace Karabaev.GameKit.Common.Utils
{
public static class MathUtils
  {
    public static Vector3 RotateAroundX(Vector3 localPoint, float angle)
    {
      var angleRadians = Mathf.Deg2Rad * angle;
      var y = localPoint.y * Mathf.Cos(angleRadians) - localPoint.z * Mathf.Sin(angleRadians);
      var z = localPoint.y * Mathf.Sin(angleRadians) + localPoint.z * Mathf.Cos(angleRadians);
      return new Vector3(localPoint.x, y, z);
    }
    
    public static Vector3 RotateAroundY(Vector3 center, Vector3 localPoint, float angle) => center + RotateAroundY(localPoint, angle);

    public static Vector3 RotateAroundY(Vector3 worldPoint, float angle)
    {
      var angleRadians = Mathf.Deg2Rad * angle;
      var x = worldPoint.x * Mathf.Cos(angleRadians) - worldPoint.z * Mathf.Sin(angleRadians);
      var z = worldPoint.x * Mathf.Sin(angleRadians) + worldPoint.z * Mathf.Cos(angleRadians);
      return new Vector3(x, worldPoint.y, z);
    }

    public static Vector3 RotateAroundZ(Vector3 localPoint, float angle)
    {
      var angleRadians = Mathf.Deg2Rad * angle;
      var x = localPoint.x * Mathf.Cos(angleRadians) - localPoint.y * Mathf.Sin(angleRadians);
      var y = localPoint.x * Mathf.Sin(angleRadians) + localPoint.y * Mathf.Cos(angleRadians);
      return new Vector3(x, y, localPoint.z);
    }
    
    public static Vector3 RotatePoint(Vector3 center, Vector3 localPoint, Vector3 eulerAngles)
    {
      var aroundX = RotateAroundX(localPoint, eulerAngles.x);
      var aroundY = RotateAroundY(aroundX, eulerAngles.y);
      var aroundZ = RotateAroundZ(aroundY, eulerAngles.z);
      return center + aroundZ;
    }

    public static float PercentFrom(float sourceValue, float percent) => sourceValue * (percent / 100);

    public static bool IsFuzzyEquals(float a, float b, float fuzzy = 0.0005f) => Mathf.Abs(a - b) <= fuzzy;

    public static bool IsFuzzyEquals(Vector3 a, Vector3 b, float fuzzy = 0.0005f)
      => IsFuzzyEquals(a.x, b.x, fuzzy) && IsFuzzyEquals(a.y, b.y, fuzzy) && IsFuzzyEquals(a.z, b.z, fuzzy);

    public static float Bezier(float start, float middle, float end, float t)
    {
      if(t <= 0)
        return start;

      if(t >= 1)
        return end;

      var result = (start * ((1 - t) * (1 - t))) + (middle * 2 * t * (1 - t)) + (end * (t * t));
      return result;
    }

    public static Vector3 Direction(Vector3 from, Vector3 to) => (to - from).normalized;

    public static Vector3 Direction2D(Vector3 from, Vector3 to)
    {
      var direction = Direction(from, to);
      direction.y = 0;
      return direction;
    }
    
    /// <summary>
    /// Возвращает беззнаковый угол в градусах.
    /// </summary>
    public static float Angle2D(Vector3 from, Vector3 to) => Vector2.Angle(new Vector2(from.x, from.z), new Vector2(to.x, to.z));

    public static float SignedAngle2D(Vector3 from, Vector3 to) => Vector2.SignedAngle(new Vector2(from.x, from.z), new Vector2(to.x, to.z));
    
    public static float Angle2D(Vector2 p1, Vector2 p2)
    {
      var oa = p2.x - p1.x;
      var ab = p2.y - p1.y;
      var ob = Mathf.Sqrt(oa * oa + ab * ab);

      var sin = ab / ob;
      var cos = oa / ob;

      var result = sin > 0 ? Mathf.Acos(cos) : -Mathf.Acos(cos);
      if(result < 0)
      {
        result += 2 * Mathf.PI;
      }

      return result;
    }

    public static float Distance2D(Vector3 a, Vector3 b)
    {
      var leg1 = a.x - b.x;
      var leg2 = a.z - b.z;
      return Mathf.Sqrt(leg1 * leg1 + leg2 * leg2);
    }

    public static Vector3 Reflect2D(Vector3 inDirection, Vector3 normal)
    {
      var result = Vector2.Reflect(new Vector2(inDirection.x, inDirection.z), new Vector2(normal.x, normal.z));
      return new Vector3(result.x, 0, result.y);
    }
  }
}