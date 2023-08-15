using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.GameCamera;
using Karabaev.Survival.Game.GameInput;
using Karabaev.Survival.Game.Hero;
using Karabaev.Survival.Game.HUD;
using Karabaev.Survival.Game.Weapons;
using Karabaev.Survival.Game.Weapons.Descriptors;
using UnityEngine;

namespace Karabaev.Survival.Game.Player
{
  public class PlayerModel
  {
    public GameInputModel Input { get; }
    
    public HeroModel Hero { get; }
    
    public ReactiveProperty<WeaponModel> ActiveWeapon { get; }

    public GameCameraModel Camera { get; }
    
    public HUDModel HUD { get; }

    public InventoryModel Inventory { get; }
    
    public ReactiveTrigger<string> LootContactFired { get; }
    
    public ReactiveTrigger HeroDied { get; }

    public PlayerModel(GameInputModel input, HeroDescriptor heroDescriptor, WeaponDescriptor weaponDescriptor, GameCameraModel camera)
    {
      LootContactFired = new ReactiveTrigger<string>();
      ActiveWeapon = new ReactiveProperty<WeaponModel>(new WeaponModel(weaponDescriptor, 0));
      Hero = new HeroModel(ActiveWeapon, heroDescriptor.MaxHp, heroDescriptor.MaxHp, heroDescriptor.MoveSpeed, LootContactFired);
      Inventory = new InventoryModel(ActiveWeapon.Value);
      HUD = new HUDModel(Hero.MaxHp, Hero.CurrentHp, ActiveWeapon, Inventory);
      Camera = camera;
      Input = input;
      HeroDied = new ReactiveTrigger();
    }
  }
}