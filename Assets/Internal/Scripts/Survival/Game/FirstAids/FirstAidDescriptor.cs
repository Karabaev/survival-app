using Karabaev.Survival.Descriptors;
using UnityEngine;

namespace Karabaev.Survival.Game.FirstAids
{
  [CreateAssetMenu(menuName = "Karabaev/NewFirstAid")]
  public class FirstAidDescriptor : Descriptor
  {
    [field: SerializeField]
    public int HealValue { get; private set; }
  }
}