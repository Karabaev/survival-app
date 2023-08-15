using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Enemy;
using Karabaev.Survival.Game.Enemy.SpawnPoint;
using Karabaev.Survival.Game.Obstacles;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Karabaev.Survival.Game.Location
{
  public class LocationView : UnityView
  {
    [SerializeField, HideInInspector]
    private NavMeshSurface _navMeshSurface = null!;
    [field: SerializeField, HideInInspector]
    public ObstacleView[] Obstacles { get; private set; } = null!;
    [field: SerializeField, HideInInspector]
    public EnemySpawnPointView[] EnemySpawnPoints { get; private set; } = null!;
    
    private AsyncOperation? _lastUpdateOperation;
    
    public void RecalculateNavMesh()
    {
      if(_lastUpdateOperation is { isDone: false })
      {
        _lastUpdateOperation.completed -= OnUpdateCompleted;
        NavMeshBuilder.Cancel(_navMeshSurface.navMeshData);
      }
      _lastUpdateOperation = _navMeshSurface.UpdateNavMesh(_navMeshSurface.navMeshData);
      _lastUpdateOperation.completed += OnUpdateCompleted;
    }

    private void OnUpdateCompleted(AsyncOperation operation)
    {
      if(_lastUpdateOperation == null)
        return;
      
      _lastUpdateOperation.completed -= OnUpdateCompleted;
      _lastUpdateOperation = null;
    }

    private void OnValidate()
    {
      _navMeshSurface = this.RequireComponent<NavMeshSurface>();
      Obstacles = GetComponentsInChildren<ObstacleView>();
      EnemySpawnPoints = GetComponentsInChildren<EnemySpawnPointView>();
    }
  }
}