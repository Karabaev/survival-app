using System;
using System.Collections.Generic;
using UnityEngine;

namespace Karabaev.Survival.Game.Hero
{
  [CreateAssetMenu(menuName = "Karabaev/HeroesRegistry")]
  public class HeroesRegistry : ScriptableObject
  {
    [SerializeField]
    private HeroDescriptor[] _heroes = null!;

    public IReadOnlyList<HeroDescriptor> Heroes => _heroes;
  }

  [Serializable]
  public record HeroDescriptor
  {
    [field: SerializeField]
    public HeroView Prefab { get; private set; } = null!;

    [field: SerializeField]
    public int MaxHp { get; private set; }
    
    [field: SerializeField]
    public float MoveSpeed { get; private set; }
  }
}