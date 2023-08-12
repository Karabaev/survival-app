using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Obstacles;

namespace Karabaev.Survival.Game.Location
{
  public class LocationModel
  {
    public ReactiveCollection<ObstacleModel> Obstacles { get; }

    public LocationModel() => Obstacles = new ReactiveCollection<ObstacleModel>();
  }
}