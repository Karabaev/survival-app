using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Karabaev.GameKit.ForResources;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.Weapons;

namespace Karabaev.Survival.Descriptors
{
  [UsedImplicitly]
  public class DescriptorsAccess
  {
    private readonly IResourceService _resourceService;

    public HeroesRegistry HeroesRegistry { get; private set; } = null!;

    public WeaponsRegistry WeaponsRegistry { get; private set; } = null!;

    public async UniTask InitializeAsync()
    {
      HeroesRegistry = await _resourceService.LoadAsync<HeroesRegistry>("Descriptors/DR_Heroes");
      WeaponsRegistry = await _resourceService.LoadAsync<WeaponsRegistry>("Descriptors/DR_Weapons");
    }

    public DescriptorsAccess(IResourceService resourceService) => _resourceService = resourceService;
  }
}