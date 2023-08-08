namespace Karabaev.GameKit.Common.Utils
{
  public static class LayerUtils
  {
    /// <summary>
    /// Проверяет, что указанный слой подходит под маску слоев.
    /// </summary>
    /// <param name="layerMask">Маска слоев.</param>
    /// <param name="layerIndex">Индекс слоя из коллекции слоев проекта.</param>
    /// <returns></returns>
    public static bool ContainsLayer(int layerMask, int layerIndex)
    {
      var checkingMask = 1 << layerIndex;
      return (layerMask & checkingMask) != 0;
    }
  }
}