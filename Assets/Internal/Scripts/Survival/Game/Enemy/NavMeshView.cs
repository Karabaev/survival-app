using UnityEngine;
using UnityEngine.AI;

namespace Karabaev.Survival.Game.Enemy
{
  public class NavMeshView : MonoBehaviour
  {
    [SerializeField]
    private int _agentTypeId;
    [SerializeField]
    private int[] _movementAreaIndexes = null!;

    private NavMeshPath? _path;
    private NavMeshQueryFilter _filter;

    private readonly Vector3[] _pathBuffer = new Vector3[30];
    
    private void Awake()
    {
      var mask = 0;
      
      foreach(var areaIndex in _movementAreaIndexes)
        mask |= 1 << areaIndex;

      _filter = new NavMeshQueryFilter { agentTypeID = _agentTypeId, areaMask = mask };
    }

    public Vector3[] CalculatePath(Vector3 destination)
    {
      _path ??= new NavMeshPath();
      NavMesh.CalculatePath(transform.position, destination, _filter, _path);
      _path.GetCornersNonAlloc(_pathBuffer);
      return _pathBuffer;
    }
  }
}