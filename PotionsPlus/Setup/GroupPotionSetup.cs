using Groups;
using ItemManager;
using UnityEngine;

namespace PotionsPlus;

public static class GroupPotionSetup
{
	public static void initializeGroupPotions(AssetBundle assets)
	{
		Item potion = new(assets, "Lesser_Group_Healing");
		if (API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 1);
			potion.RequiredItems.Add("Honey", 1);
			potion.RequiredItems.Add("Mushroom", 2);
		}
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.brewOfFaintGroupHealingCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTime = value, PotionsPlus.brewOfFaintGroupHealingHealthOverTime);
		Utils.ConvertConsumeSEStats<GroupPotion>(potion.Prefab).m_healthOverTimeDuration = 1f;

		potion = new Item(assets, "Medium_Group_Healing");
		if (API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 1);
			potion.RequiredItems.Add("Honey", 2);
			potion.RequiredItems.Add("Mushroom", 4);
		}
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.brewOfGroupHealingCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTime = value, PotionsPlus.brewOfGroupHealingHealthOverTime);
		Utils.ConvertConsumeSEStats<GroupPotion>(potion.Prefab).m_healthOverTimeDuration = 1f;

		potion = new Item(assets, "Grand_Group_Healing");
		if (API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 2);
			potion.RequiredItems.Add("Honey", 4);
			potion.RequiredItems.Add("Mushroom", 6);
		}
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.brewOfGrandGroupHealingCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTime = value, PotionsPlus.brewOfGrandGroupHealingHealthOverTime);
		Utils.ConvertConsumeSEStats<GroupPotion>(potion.Prefab).m_healthOverTimeDuration = 1f;

		potion = new Item(assets, "Brew_of_Cunning_Toxicity");
		if (API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 2);
			potion.RequiredItems.Add("Ooze", 10);
			potion.RequiredItems.Add("Bloodbag", 3);
			potion.RequiredItems.Add("Dandelion", 4);
		}
		Utils.ConvertConsumeSEStats<GroupPotion>(potion.Prefab).damageType = HitData.DamageType.Poison;
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.brewOfCunningToxicityTTL);
		Utils.SEValue(potion, (effect, value) => ((GroupPotion)effect).range = value, PotionsPlus.brewOfCunningToxicityRange);

		potion = new Item(assets, "Brew_of_Fiery_Revenge");
		if (API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 2);
			potion.RequiredItems.Add("Resin", 20);
			potion.RequiredItems.Add("Torch", 3);
			potion.RequiredItems.Add("SurtlingCore", 2);
		}
		Utils.ConvertConsumeSEStats<GroupPotion>(potion.Prefab).damageType = HitData.DamageType.Fire;
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.brewOfFieryRevengeTTL);
		Utils.SEValue(potion, (effect, value) => ((GroupPotion)effect).range = value, PotionsPlus.brewOfFieryRevengeRange);

		potion = new Item(assets, "Brew_of_Icy_Touch");
		if (API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 2);
			potion.RequiredItems.Add("FreezeGland", 4);
			potion.RequiredItems.Add("Carrot", 5);
			potion.RequiredItems.Add("Turnip", 5);
		}
		Utils.ConvertConsumeSEStats<GroupPotion>(potion.Prefab).damageType = HitData.DamageType.Frost;
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.brewOfIcyTouchTTL);
		Utils.SEValue(potion, (effect, value) => ((GroupPotion)effect).range = value, PotionsPlus.brewOfIcyTouchRange);

		potion = new Item(assets, "Brew_of_Spiritual_Death");
		if (API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 2);
			potion.RequiredItems.Add("Chitin", 2);
			potion.RequiredItems.Add("Entrails", 4);
		}
		Utils.ConvertConsumeSEStats<GroupPotion>(potion.Prefab).damageType = HitData.DamageType.Spirit;
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.brewOfSpiritualDeathTTL);
		Utils.SEValue(potion, (effect, value) => ((GroupPotion)effect).range = value, PotionsPlus.brewOfSpiritualDeathRange);

		potion = new Item(assets, "Brew_of_Thunderous_Words");
		if (API.IsLoaded())
		{
			potion.Crafting.Add("opalchemy", 2);
			potion.RequiredItems.Add("Obsidian", 3);
			potion.RequiredItems.Add("GreydwarfEye", 6);
			potion.RequiredItems.Add("Cloudberry", 2);
		}
		Utils.ConvertConsumeSEStats<GroupPotion>(potion.Prefab).damageType = HitData.DamageType.Lightning;
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.brewOfThunderousWordsTTL);
		Utils.SEValue(potion, (effect, value) => ((GroupPotion)effect).range = value, PotionsPlus.brewOfThunderousWordsRange);
	}
}