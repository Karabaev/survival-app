using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.ForResources;
using Karabaev.Survival.Game.Configuration;
using Karabaev.Survival.Game.Enemy.Descriptors;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.Loot.Descriptors;
using Karabaev.Survival.Game.Obstacles;
using Karabaev.Survival.Game.Weapons.Descriptors;

namespace Karabaev.Survival.Descriptors
{
  [UsedImplicitly]
  [PublicAPI]
  public class DescriptorsAccess
  {
    private readonly IResourceService _resourceService;

    public HeroesRegistry HeroesRegistry { get; private set; } = null!;

    public WeaponsRegistry WeaponsRegistry { get; private set; } = null!;

    public LootRegistry LootRegistry { get; private set; } = null!;

    public EnemiesRegistry EnemiesRegistry { get; private set; } = null!;
    
    public ObstaclesRegistry ObstaclesRegistry { get; private set; } = null!;

    public GameCameraConfig CameraConfig { get; private set; } = null!;
    
    public GameConfig GameConfig { get; private set; } = null!;

    public async UniTask InitializeAsync()
    {
      HeroesRegistry = await _resourceService.LoadAsync<HeroesRegistry>("Descriptors/DR_Heroes");
      WeaponsRegistry = await _resourceService.LoadAsync<WeaponsRegistry>("Descriptors/Weapons/DR_Weapons");
      LootRegistry = await _resourceService.LoadAsync<LootRegistry>("Descriptors/Loot/DR_Loot");
      EnemiesRegistry = await _resourceService.LoadAsync<EnemiesRegistry>("Descriptors/Enemies/DR_Enemies");
      ObstaclesRegistry = await _resourceService.LoadAsync<ObstaclesRegistry>("Descriptors/Obstacles/DR_Obstacles");
      CameraConfig = await _resourceService.LoadAsync<GameCameraConfig>("Descriptors/DR_CameraConfig");
      GameConfig = await _resourceService.LoadAsync<GameConfig>("Descriptors/DR_GameConfig");
    }

    public DescriptorsAccess(IResourceService resourceService) => _resourceService = resourceService;
  }
}