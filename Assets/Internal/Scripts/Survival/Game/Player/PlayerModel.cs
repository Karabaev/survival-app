using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.GameInput;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.HUD;

namespace Karabaev.Survival.Game.Player
{
  public class PlayerModel
  {
    public GameInputModel Input { get; }
    
    public HeroModel Hero { get; }
    
    public GameCameraModel Camera { get; }
    
    public HUDModel HUD { get; }

    public ReactiveTrigger<string> LootContactFired { get; }

    public PlayerModel(GameInputModel input, HeroModel hero, GameCameraModel camera)
    {
      Input = input;
      Hero = hero;
      Camera = camera;
      HUD = new HUDModel(Hero.MaxHp, Hero.CurrentHp, Hero.Weapon); // todokmo передавать Weapon
      LootContactFired = hero.LootContactFired;
    }
  }
}