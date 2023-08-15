using System;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Enemy.Descriptors;
using UnityEngine;

namespace Karabaev.Survival.Game.Enemy.SpawnPoint
{
  public class EnemySpawnPointView : UnityView
  {
    [field: SerializeField]
    public EnemyDescriptor Descriptor { get; private set; } = null!;
    [SerializeField]
    private float _spawnInterval = 1.0f;
    
    public Vector3 Position => transform.position;

    public TimeSpan SpawnInterval => _spawnInterval.ToSeconds();
  }
}