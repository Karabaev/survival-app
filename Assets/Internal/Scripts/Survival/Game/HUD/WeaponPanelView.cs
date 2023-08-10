using Karabaev.GameKit.Common.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Karabaev.Survival.Game.HUD
{
  public class WeaponPanelView : MonoBehaviour
  {
    [SerializeField, HideInInspector]
    private TMP_Text _indexText = null!;
    [SerializeField, HideInInspector]
    private Image _iconImage = null!;
    [SerializeField, HideInInspector]
    private TMP_Text _ammoText = null!;

    public int Index
    {
      set => _indexText.text = value.ToString();
    }
    
    public Sprite? Icon
    {
      set => _iconImage.sprite = value!;
    }

    public void SetAmmo(int magazine, int reserve) => _ammoText.text = $"{magazine} / {reserve}";
    
    private void OnValidate()
    {
      _indexText = this.RequireComponentInChild<TMP_Text>("IndexText");
      _iconImage = this.RequireComponentInChild<Image>("Icon");
      _ammoText = this.RequireComponentInChild<TMP_Text>("AmmoText");
    }
  }
}