using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.GameInput;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.Loot;
using Karabaev.Survival.Game.Player;
using Karabaev.Survival.Game.Weapons;
using UnityEngine;

namespace Karabaev.Survival.Game
{
  public class GameModel
  {
    public PlayerModel Player { get; }
    
    public ReactiveCollection<LootModel> Loot { get; }

    public GameModel(HeroDescriptor heroDescriptor, WeaponDescriptor weaponDescriptor)
    {
      var input = new GameInputModel();
      var weaponModel = new WeaponModel(weaponDescriptor);
      var hero = new HeroModel(weaponModel, heroDescriptor.MaxHp, heroDescriptor.MaxHp, heroDescriptor.MoveSpeed, input.FireFired, input.ReloadFired);
      var camera = new GameCameraModel(Vector3.zero, Vector3.zero, input);
      Player = new PlayerModel(input, hero, camera);
      Loot = new ReactiveCollection<LootModel>();
    }
  }
}