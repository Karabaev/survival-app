using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.GameInput;
using Karabaev.Survival.Game.Hero;

namespace Karabaev.Survival.Game.Player
{
  public class PlayerModel
  {
    public GameInputModel Input { get; }
    
    public HeroModel Hero { get; }
    
    public GameCameraModel Camera { get; }

    public PlayerModel(GameInputModel input, HeroModel hero, GameCameraModel camera)
    {
      Input = input;
      Hero = hero;
      Camera = camera;
    }
  }
}