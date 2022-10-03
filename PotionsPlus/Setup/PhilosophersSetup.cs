using ItemManager;
using SkillManager;
using UnityEngine;

namespace PotionsPlus;

public static class PhilosophersSetup
{
	public static void initializePhilosophersStones(AssetBundle assets)
	{
		void AddStatusEffectModifier(Item item)
		{
			SE_Stats statusEffect = (SE_Stats)item.Prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_equipStatusEffect;
			statusEffect.m_raiseSkill = Skill.fromName("Alchemy");
			statusEffect.m_raiseSkillModifier = PotionsPlus.philosophersStoneXpGainFactor.Value;
			PotionsPlus.philosophersStoneXpGainFactor.SettingChanged += (_, _) => statusEffect.m_raiseSkillModifier = PotionsPlus.philosophersStoneXpGainFactor.Value;
		}

		Item item = new(assets, "PhilosopherStoneBlue");
		item.Crafting.Add("opcauldron", 1);
		item.RequiredItems.Add("Flask_of_Elements", 5);
		AddStatusEffectModifier(item);

		item = new Item(assets, "PhilosopherStoneGreen");
		item.Crafting.Add("opcauldron", 1);
		item.RequiredItems.Add("Flask_of_Fortification", 5);
		AddStatusEffectModifier(item);

		item = new Item(assets, "PhilosopherStoneRed");
		item.Crafting.Add("opcauldron", 1);
		item.RequiredItems.Add("Flask_of_the_Gods", 5);
		AddStatusEffectModifier(item);
		
		item = new Item(assets, "PhilosopherStonePurple");
		item.Crafting.Add("opcauldron", 1);
		item.RequiredItems.Add("Flask_of_Second_Wind", 5);
		AddStatusEffectModifier(item);
		
		AddStatusEffectModifier(new Item(assets, "PhilosopherStoneBlack"));
	}
}
