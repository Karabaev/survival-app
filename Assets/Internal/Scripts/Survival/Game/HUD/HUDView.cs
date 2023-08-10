using Karabaev.GameKit.Common.Utils;
using Karabaev.GameKit.Entities;
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
    private Image _hpBarFilling = null!;
    
    public void SetAmmo(int magazine, int reserve) => _ammoText.text = $"{magazine} / {reserve}";

    public void SetHp(int actualHp, int maxHp) => _hpBarFilling.fillAmount = (float)actualHp / maxHp;

    private void OnValidate()
    {
      _ammoText = this.RequireComponentInChild<TMP_Text>("AmmoText");
      _hpBarFilling = this.RequireChildRecursive("HpBar").RequireComponentInChild<Image>("Filling");
    }
  }
}