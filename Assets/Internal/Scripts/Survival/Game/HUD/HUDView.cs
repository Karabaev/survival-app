using System.Collections.Generic;
using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
using Karabaev.Survival.Game.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Karabaev.Survival.Game.HUD
{
  public class HUDView : UnityUIView
  {
    [SerializeField, HideInInspector]
    private TMP_Text _ammoText = null!;
    [SerializeField, HideInInspector]
    private Image _currentWeaponIcon = null!;
    [SerializeField, HideInInspector]
    private Image _hpBarFilling = null!;
    [SerializeField, HideInInspector]
    private RectTransform _weaponsPanelContainer = null!;
    [SerializeField, HideInInspector]
    private WeaponPanelView _weaponPanelTemplate = null!;

    private readonly List<WeaponPanelView> _weaponPanels = new();

    private void Awake()
    {
      _weaponPanels.Add(_weaponPanelTemplate);
      _weaponPanelTemplate.SetActive(false);
    }

    public void SetWeaponInInventory(int index, Sprite icon, int magazine, int reserve)
    {
      var newPanelsCount = index - _weaponPanels.Count + 1;
      
      if(newPanelsCount > 0)
      {
        for(var i = 0; i < newPanelsCount; i++)
        {
          var newPanel = Instantiate(_weaponPanelTemplate, _weaponsPanelContainer);
          _weaponPanels.Add(newPanel);
        }
      }

      var panel = _weaponPanels[index];
      panel.Index = index + 1;
      panel.Icon = icon;
      panel.SetAmmo(magazine, reserve);
      panel.SetActive(true);
    }

    public void RemoveWeaponFromInventory(int index)
    {
      var panel = _weaponPanels[index];
      panel.Index = -1;
      panel.Icon = null;
      panel.SetAmmo(0, 0);
      panel.SetActive(false);
    }
    
    public void UpdateAmmoInInventory(int index, int magazine, int reserve)
    {
      var panel = _weaponPanels[index];
      panel.SetAmmo(magazine, reserve);
    }

    public void SetActiveWeapon(Sprite icon, int magazine, int reserve)
    {
      _currentWeaponIcon.sprite = icon;
      UpdateActualAmmo(magazine, reserve);
    }
    
    public void UpdateActualAmmo(int magazine, int reserve) => _ammoText.text = $"{magazine} / {reserve}";

    public void SetHp(int actualHp, int maxHp) => _hpBarFilling.fillAmount = (float)actualHp / maxHp;

    private void OnValidate()
    {
      _ammoText = this.RequireComponentInChild<TMP_Text>("AmmoText");
      _currentWeaponIcon = this.RequireComponentInChild<Image>("CurrentWeaponIcon");
      _hpBarFilling = this.RequireChildRecursive("HpBar").RequireComponentInChild<Image>("Filling");
      _weaponsPanelContainer = this.RequireComponentInChild<RectTransform>("WeaponsPanel");
      _weaponPanelTemplate = _weaponsPanelContainer.RequireComponentInChildren<WeaponPanelView>();
    }
  }
}