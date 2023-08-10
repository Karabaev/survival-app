using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.GameInput;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.HUD;
using Karabaev.Survival.Game.Weapons;

namespace Karabaev.Survival.Game.Player
{
  public class PlayerModel
  {
    public GameInputModel Input { get; }
    
    public HeroModel Hero { get; }
    
    public ReactiveProperty<WeaponModel> Weapon { get; }

    public GameCameraModel Camera { get; }
    
    public HUDModel HUD { get; }

    public ReactiveTrigger<string> LootContactFired { get; }

    public PlayerModel(GameInputModel input, HeroDescriptor heroDescriptor, WeaponDescriptor weaponDescriptor, GameCameraModel camera)
    {
      LootContactFired = new ReactiveTrigger<string>();
      Weapon = new ReactiveProperty<WeaponModel>(new WeaponModel(weaponDescriptor, 0));
      Hero = new HeroModel(Weapon, heroDescriptor.MaxHp, heroDescriptor.MaxHp, heroDescriptor.MoveSpeed, LootContactFired);
      HUD = new HUDModel(Hero.MaxHp, Hero.CurrentHp, Weapon);
      Camera = camera;
      Input = input;
    }
  }
}