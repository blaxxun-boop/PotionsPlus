using ItemManager;
using UnityEngine;

namespace PotionsPlus;

public static class EquipmentSetup
{
	public static void initializeEquipment(AssetBundle assets)
	{
		Item alchemyEquipment = new(assets, "Odins_Alchemy_Wand");
		alchemyEquipment.Crafting.Add(CraftingTable.Forge, 2);
		alchemyEquipment.RequiredItems.Add("FineWood", 8);
		alchemyEquipment.RequiredItems.Add("Copper", 3);
		alchemyEquipment.RequiredItems.Add("SurtlingCore", 1);
		alchemyEquipment.RequiredUpgradeItems.Add("FineWood", 4);
		alchemyEquipment.RequiredUpgradeItems.Add("Copper", 1);

		alchemyEquipment = new Item(assets, "Odins_Wizard_Hat");
		alchemyEquipment.Crafting.Add(CraftingTable.Workbench, 5);
		alchemyEquipment.MaximumRequiredStationLevel = 5;
		alchemyEquipment.RequiredItems.Add("LinenThread", 20);
		alchemyEquipment.RequiredItems.Add("SurtlingCore", 5);
		alchemyEquipment.RequiredUpgradeItems.Add("LinenThread", 10);
		alchemyEquipment.RequiredUpgradeItems.Add("SurtlingCore", 2);

		alchemyEquipment = new Item(assets, "Odins_Warlock_Hat");
		alchemyEquipment.Crafting.Add(CraftingTable.Workbench, 5);
		alchemyEquipment.MaximumRequiredStationLevel = 5;
		alchemyEquipment.RequiredItems.Add("LinenThread", 20);
		alchemyEquipment.RequiredItems.Add("SurtlingCore", 5);
		alchemyEquipment.RequiredUpgradeItems.Add("LinenThread", 10);
		alchemyEquipment.RequiredUpgradeItems.Add("SurtlingCore", 2);

		alchemyEquipment = new Item(assets, "Odins_Dragon_Staff");
		alchemyEquipment.Crafting.Add(CraftingTable.Workbench, 3);
		alchemyEquipment.MaximumRequiredStationLevel = 5;
		alchemyEquipment.RequiredItems.Add("Odins_Alchemy_Wand", 1);
		alchemyEquipment.RequiredItems.Add("ElderBark", 40);
		alchemyEquipment.RequiredItems.Add("BlackMetal", 40);
		alchemyEquipment.RequiredItems.Add("SurtlingCore", 1);
		alchemyEquipment.RequiredUpgradeItems.Add("ElderBark", 20);
		alchemyEquipment.RequiredUpgradeItems.Add("BlackMetal", 20);

		GameObject smokeScreen = PrefabManager.RegisterPrefab(assets, "Staff_Smoke_Cloud");
		smokeScreen.AddComponent<PotionsPlus.SmokescreenOwner>();

		smokeScreen.GetComponent<TimedDestruction>().m_timeout = PotionsPlus.smokeScreenTTL.Value;
		PotionsPlus.smokeScreenTTL.SettingChanged += (_, _) => smokeScreen.GetComponent<TimedDestruction>().m_timeout = PotionsPlus.smokeScreenTTL.Value;
	}
}
