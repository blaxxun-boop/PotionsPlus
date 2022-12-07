using ItemManager;
using UnityEngine;

namespace PotionsPlus;

public static class PotionsSetup
{
	public static void initializePotions(AssetBundle assets)
	{
		Item potion = new(assets, "Flask_of_Elements");
		potion.Crafting.Add("opalchemy", 1);
		potion.RequiredItems.Add("Potion_Meadbase", 1);
		potion.RequiredItems.Add("FreezeGland", 2);
		potion.RequiredItems.Add("ElderBark", 4);
		potion.RequiredItems.Add("Entrails", 8);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.flaskOfElementsTTL);

		potion = new Item(assets, "Flask_of_Fortification");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("Potion_Meadbase", 1);
		potion.RequiredItems.Add("Obsidian", 2);
		potion.RequiredItems.Add("Flint", 4);
		potion.RequiredItems.Add("Stone", 8);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.flaskOfFortificationTTL);

		potion = new Item(assets, "Flask_of_the_Gods");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("Potion_Meadbase", 1);
		potion.RequiredItems.Add("Thistle", 4);
		potion.RequiredItems.Add("Flax", 4);
		potion.RequiredItems.Add("Carrot", 2);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.flaskOfGodsTTL);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTime = value, PotionsPlus.flaskOfGodsHealing);
		Utils.SEValue(potion, (effect, value) => effect.m_healthRegenMultiplier = value, PotionsPlus.flaskOfGodsRegenMultiplier);

		potion = new Item(assets, "Flask_of_Magelight");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("Potion_Meadbase", 1);
		potion.RequiredItems.Add("BoneFragments", 4);
		potion.RequiredItems.Add("FreezeGland", 4);
		potion.RequiredItems.Add("GreydwarfEye", 8);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.flaskOfMagelightTTL);
		Light Magelight = PrefabManager.RegisterPrefab(assets, "Magelight").GetComponent<Light>();
		void SetLightIntensity()
		{
			Magelight.intensity = PotionsPlus.flaskOfMagelightIntensity.Value;
			Magelight.color = PotionsPlus.flaskOfMagelightColor.Value;
		}
		SetLightIntensity();
		PotionsPlus.flaskOfMagelightIntensity.SettingChanged += (_, _) => SetLightIntensity();
		PotionsPlus.flaskOfMagelightColor.SettingChanged += (_, _) => SetLightIntensity();

		potion = new Item(assets, "Flask_of_Second_Wind");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("Potion_Meadbase", 1);
		potion.RequiredItems.Add("Ooze", 4);
		potion.RequiredItems.Add("FreezeGland", 2);
		potion.RequiredItems.Add("Feathers", 6);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.flaskOfSecondWindTTL);
		Utils.SEValue(potion, (effect, value) => effect.m_jumpStaminaUseModifier = 1 - value, PotionsPlus.flaskOfSecondWindJumpStaminaFactor);
		Utils.SEValue(potion, (effect, value) => effect.m_runStaminaDrainModifier = 1 - value, PotionsPlus.flaskOfSecondWindRunStaminaFactor);
		Utils.SEValue(potion, (effect, value) => effect.m_staminaRegenMultiplier = value, PotionsPlus.flaskOfSecondWindStaminaRegenMultiplier);

		potion = new Item(assets, "Grand_Healing_Tide_Potion");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("Ooze", 2);
		potion.RequiredItems.Add("Barley", 4);
		potion.RequiredItems.Add("Needle", 2);
		potion.RequiredItems.Add("Cloudberry", 6);

		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTime = value, PotionsPlus.grandHealingTidePotionHealthOverTime);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTimeDuration = value, PotionsPlus.grandHealingTidePotionTTL);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTimeInterval = value, PotionsPlus.grandHealingTidePotionTickInterval);

		potion = new Item(assets, "Grand_Spiritual_Healing_Potion");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("Ooze", 4);
		potion.RequiredItems.Add("Flax", 4);
		potion.RequiredItems.Add("WolfFang", 2);
		potion.RequiredItems.Add("Cloudberry", 6);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.grandSpiritualHealingPotionCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTime = value, PotionsPlus.grandSpiritualHealingPotionHealthOverTime);

		potion = new Item(assets, "Grand_Stamina_Elixir");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("LoxMeat", 2);
		potion.RequiredItems.Add("Carrot", 4);
		potion.RequiredItems.Add("Turnip", 4);
		potion.RequiredItems.Add("Cloudberry", 8);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.grandStaminaElixirCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_staminaOverTime = value, PotionsPlus.grandStaminaElixirStaminaOverTime);
		Utils.SEValue(potion, (effect, value) => effect.m_staminaOverTimeDuration = value, PotionsPlus.grandStaminaElixirTTL);

		potion = new Item(assets, "Grand_Stealth_Elixir");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("FreezeGland", 2);
		potion.RequiredItems.Add("Flax", 4);
		potion.RequiredItems.Add("Feathers", 2);
		potion.RequiredItems.Add("Carrot", 2);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.grandStealthElixirTTL);
		Utils.SEValue(potion, (effect, value) => effect.m_noiseModifier = -value / 100f, PotionsPlus.grandStealthElixirNoiseReduction);
		Utils.SEValue(potion, (effect, value) => effect.m_stealthModifier = -value / 100f, PotionsPlus.grandStealthElixirVisibilityReduction);

		potion = new Item(assets, "Medium_Healing_Tide_Flask");
		potion.Crafting.Add("opalchemy", 1);
		potion.RequiredItems.Add("Resin", 6);
		potion.RequiredItems.Add("Blueberries", 4);
		potion.RequiredItems.Add("Bloodbag", 2);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.mediumHealingTideFlaskCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTime = value, PotionsPlus.mediumHealingTideFlaskHealthOverTime);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTimeDuration = value, PotionsPlus.mediumHealingTideFlaskTTL);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTimeInterval = value, PotionsPlus.mediumHealingTideFlaskTickInterval);

		potion = new Item(assets, "Medium_Spiritual_Healing_Flask");
		potion.Crafting.Add("opalchemy", 1);
		potion.RequiredItems.Add("Ooze", 2);
		potion.RequiredItems.Add("BoneFragments", 4);
		potion.RequiredItems.Add("Bloodbag", 2);
		
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.mediumSpiritualHealingFlaskCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTime = value, PotionsPlus.mediumSpiritualHealingFlaskHealthOverTime);
		((SE_Stats)potion.Prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_consumeStatusEffect).m_healthOverTimeDuration = 1f;

		potion = new Item(assets, "Medium_Stamina_Flask");
		potion.Crafting.Add("opalchemy", 1);
		potion.RequiredItems.Add("Resin", 4);
		potion.RequiredItems.Add("Blueberries", 4);
		potion.RequiredItems.Add("Bloodbag", 2);
		
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.mediumStaminaFlaskCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_staminaOverTime = value, PotionsPlus.mediumStaminaFlaskStaminaOverTime);
		((SE_Stats)potion.Prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_consumeStatusEffect).m_staminaOverTimeDuration = 1f;

		potion = new Item(assets, "Lesser_Healing_Tide_Vial");
		potion.Crafting.Add("opalchemy", 1);
		potion.RequiredItems.Add("Honey", 2);
		potion.RequiredItems.Add("Raspberry", 4);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.lesserHealingTideVialCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTime = value, PotionsPlus.lesserHealingTideVialHealthOverTime);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTimeDuration = value, PotionsPlus.lesserHealingTideVialTTL);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTimeInterval = value, PotionsPlus.lesserHealingTideVialTickInterval);

		potion = new Item(assets, "Lesser_Spiritual_Healing_Vial");
		potion.Crafting.Add("opalchemy", 1);
		potion.RequiredItems.Add("Dandelion", 2);
		potion.RequiredItems.Add("Raspberry", 4);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.lesserSpiritualHealingVialCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_healthOverTime = value, PotionsPlus.lesserSpiritualHealingVialHealthOverTime);
		((SE_Stats)potion.Prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_consumeStatusEffect).m_healthOverTimeDuration = 1f;

		potion = new Item(assets, "Lesser_Stamina_Vial");
		potion.Crafting.Add("opalchemy", 1);
		potion.RequiredItems.Add("Honey", 2);
		potion.RequiredItems.Add("Mushroom", 4);
		
		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value, PotionsPlus.lesserStaminaVialCooldown);
		Utils.SEValue(potion, (effect, value) => effect.m_staminaOverTime = value, PotionsPlus.lesserStaminaVialStaminaOverTime);
		((SE_Stats)potion.Prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_consumeStatusEffect).m_staminaOverTimeDuration = 1f;
		
		potion = new Item(assets, "Odins_Weapon_Oil");
		potion.Crafting.Add("opalchemy", 1);
		potion.RequiredItems.Add("Tar", 8);
		potion.RequiredItems.Add("Crystal", 1);

		Utils.SEValue(potion, (effect, value) => effect.m_ttl = value * 60f, PotionsPlus.weaponOilTTL);
		Utils.ConvertConsumeSEStats<WeaponOil>(potion.Prefab);

		potion = new Item(assets, "Potion_Meadbase");
		potion["Tide"].Crafting.Add("opcauldron", 1);
		potion["Tide"].RequiredItems.Add("Grand_Healing_Tide_Potion", 1);
		potion["Spiritual"].Crafting.Add("opcauldron", 1);
		potion["Spiritual"].RequiredItems.Add("Grand_Spiritual_Healing_Potion", 1);
		potion["Table"].Crafting.Add("opalchemy", 2);
		potion["Table"].RequiredItems.Add("Honey", 2);
		potion["Table"].RequiredItems.Add("YmirRemains", 4);
	}
}
