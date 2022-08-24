using ItemManager;
using UnityEngine;

namespace PotionsPlus;

public static class ManaPotionSetup
{
	public static void initializeManaPotions(AssetBundle assets)
	{
		Item potion = new(assets, "Lesser_Mana_Potion");
		if (MO_API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 1);
			potion.RequiredItems.Add("Thistle", 1);
			potion.RequiredItems.Add("GreydwarfEye", 1);
			potion.RequiredItems.Add("Dandelion", 1);
		}
		Utils.ConvertConsumeSEStats<ManaPotion>(potion.Prefab);
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.weakManaPotionCooldown);
		Utils.SEValue(potion, (effect, value) => ((ManaPotion)effect).manaToRestore = value, PotionsPlus.weakManaPotionManaRestoration);
		
		potion = new Item(assets, "Large_Mana_Potion");
		if (MO_API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 2);
			potion.RequiredItems.Add("Thistle", 3);
			potion.RequiredItems.Add("GreydwarfEye", 3);
			potion.RequiredItems.Add("Dandelion", 3);
		}
		Utils.ConvertConsumeSEStats<ManaPotion>(potion.Prefab);
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.giantManaPotionCooldown);
		Utils.SEValue(potion, (effect, value) => ((ManaPotion)effect).manaToRestore = value, PotionsPlus.giantManaPotionManaRestoration);
		
		potion = new Item(assets, "Grand_Mana_Potion");
		if (MO_API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 2);
			potion.RequiredItems.Add("Thistle", 4);
			potion.RequiredItems.Add("GreydwarfEye", 4);
			potion.RequiredItems.Add("Dandelion", 4);
		}
		Utils.ConvertConsumeSEStats<ManaPotion>(potion.Prefab).manaToRestore = 99999999;
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.overflowingManaPotionCooldown);
	}
}
