using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Enemy;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.GameInput;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.Location;
using Karabaev.Survival.Game.Loot;
using Karabaev.Survival.Game.Obstacles;
using Karabaev.Survival.Game.Player;
using Karabaev.Survival.Game.Weapons.Descriptors;
using UnityEngine;

namespace Karabaev.Survival.Game
{
  public class GameModel
  {
    public PlayerModel Player { get; }

    public LocationModel Location { get; }

    public ReactiveCollection<LootModel> Loot { get; }
    
    public ReactiveCollection<ObstacleModel> Obstacles { get; }

    public ReactiveCollection<EnemyModel> Enemies { get; }

    public GameModel(HeroDescriptor heroDescriptor, WeaponDescriptor weaponDescriptor)
    {
      var input = new GameInputModel();
      var camera = new GameCameraModel(Vector3.zero, Vector3.zero, input);
      Player = new PlayerModel(input, heroDescriptor, weaponDescriptor, camera);
      Loot = new ReactiveCollection<LootModel>();
      Location = new LocationModel();
      Obstacles = Location.Obstacles;
      Enemies = new ReactiveCollection<EnemyModel>();
    }
  }
}