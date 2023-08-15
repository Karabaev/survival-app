using System;
using Karabaev.GameKit.Common;
using Karabaev.Survival.Game.Enemy.Descriptors;
using UnityEngine;

namespace Karabaev.Survival.Game.Enemy.SpawnPoint
{
  public class EnemySpawnPointModel
  {
    public Vector3 Position { get; }
    
    public EnemyDescriptor Descriptor { get; }

    public TimeSpan SpawnInterval { get; }

    public GameTime NextSpawnTime { get; set; }
    
    public bool Enabled { get; set; }

    public EnemySpawnPointModel(Vector3 position, EnemyDescriptor descriptor, TimeSpan spawnInterval)
    {
      Position = position;
      Descriptor = descriptor;
      SpawnInterval = spawnInterval;
      NextSpawnTime = GameTime.Min;
      Enabled = true;
    }
  }
}