using ItemManager;
using UnityEngine;

namespace PotionsPlus;

public static class HellbrothSetup
{
	public static void initializeHellbroth(AssetBundle assets)
	{
		Item potion = new(assets, "Hellbroth_of_Flames");
		potion.Crafting.Add("opalchemy", 1);
		potion.RequiredItems.Add("Resin", 8);
		potion.RequiredItems.Add("Torch", 1);
		potion.Configurable = Configurability.Recipe;
		
		Aoe fireAoe = PrefabManager.RegisterPrefab(assets, "Hellbroth_Explosion").GetComponent<Aoe>();
		fireAoe.m_damage.m_fire = PotionsPlus.hellbrothOfFlamesDamage.Value;
		PotionsPlus.hellbrothOfFlamesDamage.SettingChanged += (_, _) => fireAoe.m_damage.m_fire = PotionsPlus.hellbrothOfFlamesDamage.Value;
		PrefabManager.RegisterPrefab(assets, "Hellbroth_Projectile");
		PrefabManager.RegisterPrefab(assets, "Hellbroth_Orb_Projectile");
		PotionsPlus.wandProjectiles.Add("Hellbroth_Orb_Projectile");
		Item hellbroth = new(assets, "Hellbroth_of_Flames_Charge");
		hellbroth.Crafting.Add("opcauldron", 1);
		hellbroth.CraftAmount = 2;
		hellbroth.RequiredItems.Add("Hellbroth_of_Flames", 3);
		hellbroth.Configurable = Configurability.Recipe;

		potion = new Item(assets, "Hellbroth_of_Frost");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("FreezeGland", 4);
		potion.RequiredItems.Add("Chain", 1);
		potion.Configurable = Configurability.Recipe;
		
		Aoe frostAoe = PrefabManager.RegisterPrefab(assets, "Hellbroth_Frost_Explosion").GetComponent<Aoe>();
		frostAoe.m_damage.m_frost = PotionsPlus.hellbrothOfFrostDamage.Value;
		PotionsPlus.hellbrothOfFrostDamage.SettingChanged += (_, _) => frostAoe.m_damage.m_frost = PotionsPlus.hellbrothOfFrostDamage.Value;
		PrefabManager.RegisterPrefab(assets, "Hellbroth_Frost_Projectile");
		PrefabManager.RegisterPrefab(assets, "Hellbroth_Frost_Orb_Projectile");
		PotionsPlus.wandProjectiles.Add("Hellbroth_Frost_Orb_Projectile");
		hellbroth = new Item(assets, "Hellbroth_of_Frost_Charge");
		hellbroth.Crafting.Add("opcauldron", 1);
		hellbroth.CraftAmount = 2;
		hellbroth.RequiredItems.Add("Hellbroth_of_Frost", 3);
		hellbroth.Configurable = Configurability.Recipe;
		
		potion = new Item(assets, "Hellbroth_of_Thors_Fury");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("Tar", 6);
		potion.RequiredItems.Add("Thunderstone", 1);
		potion.Configurable = Configurability.Recipe;
		
		Aoe lightningAoe = PrefabManager.RegisterPrefab(assets, "Hellbroth_Thors_Fury_Explosion").GetComponent<Aoe>();
		lightningAoe.m_damage.m_lightning = PotionsPlus.hellbrothOfThorsFuryDamage.Value;
		PotionsPlus.hellbrothOfThorsFuryDamage.SettingChanged += (_, _) => lightningAoe.m_damage.m_lightning = PotionsPlus.hellbrothOfThorsFuryDamage.Value;
		PrefabManager.RegisterPrefab(assets, "Hellbroth_Thors_Fury_Projectile");
		PrefabManager.RegisterPrefab(assets, "Hellbroth_Thors_Fury_Orb_Projectile");
		PotionsPlus.wandProjectiles.Add("Hellbroth_Thors_Fury_Orb_Projectile");
		hellbroth = new Item(assets, "Hellbroth_of_Thors_Fury_Charge");
		hellbroth.Crafting.Add("opcauldron", 1);
		hellbroth.CraftAmount = 2;
		hellbroth.RequiredItems.Add("Hellbroth_of_Thors_Fury", 3);
		hellbroth.Configurable = Configurability.Recipe;
		
		potion = new Item(assets, "Hellbroth_of_Eternal_Life");
		potion.Crafting.Add("opalchemy", 2);
		potion.RequiredItems.Add("Honey", 5);
		potion.RequiredItems.Add("Dandelion", 3);
		potion.Configurable = Configurability.Recipe;
		
		PrefabManager.RegisterPrefab(assets, "Hellbroth_Life_Projectile");
		PrefabManager.RegisterPrefab(assets, "Hellbroth_Life_Explosion");
		PrefabManager.RegisterPrefab(assets, "Hellbroth_Life_Orb_Projectile");
		PotionsPlus.wandProjectiles.Add("Hellbroth_Life_Orb_Projectile");
		hellbroth = new Item(assets, "Hellbroth_of_Eternal_Life_Charge");
		hellbroth.Crafting.Add("opcauldron", 1);
		hellbroth.CraftAmount = 2;
		hellbroth.RequiredItems.Add("Hellbroth_of_Eternal_Life", 3);
		hellbroth.Configurable = Configurability.Recipe;
	}
}
