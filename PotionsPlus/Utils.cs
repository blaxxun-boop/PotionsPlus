using System;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using ItemManager;
using UnityEngine;

namespace PotionsPlus;

public static class Utils
{
	private static void ItemValue<T>(Item item, Action<ItemDrop.ItemData.SharedData, T> setter, ConfigEntry<T> config)
	{
		string itemName = item.Prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_name;
		void set() => setter(item.Prefab.GetComponent<ItemDrop>().m_itemData.m_shared, config.Value);
		config.SettingChanged += (_, _) =>
		{
			if (ObjectDB.instance)
			{
				Inventory[] inventories = Player.s_players.Select(p => p.GetInventory()).Concat(UnityEngine.Object.FindObjectsOfType<Container>().Select(c => c.GetInventory())).ToArray();
				foreach (ItemDrop.ItemData itemdata in ObjectDB.instance.m_items.Select(p => p.GetComponent<ItemDrop>()).Where(c => c && c.GetComponent<ZNetView>()).Concat(ItemDrop.s_instances).Select(i => i.m_itemData).Concat(inventories.SelectMany(i => i.GetAllItems())))
				{
					if (itemName == itemdata.m_shared.m_name)
					{
						setter(itemdata.m_shared, config.Value);
					}
				}
			}
			set();
		};
		set();
	}

	public static void SEValue<T>(Item potion, Action<SE_Stats, T> setter, ConfigEntry<T> config) => ItemValue(potion, (i, c) => setter((SE_Stats)i.m_consumeStatusEffect, c), config);

	public static T ConvertConsumeSEStats<T>(GameObject item) where T : StatusEffect
	{
		ItemDrop.ItemData.SharedData shared = item.GetComponent<ItemDrop>().m_itemData.m_shared;
		StatusEffect stats = shared.m_consumeStatusEffect;
		T ownSE = ScriptableObject.CreateInstance<T>();
		shared.m_consumeStatusEffect = ownSE;

		ownSE.name = stats.name;
		foreach (FieldInfo field in stats.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
		{
			field.SetValue(ownSE, field.GetValue(stats));
		}

		return ownSE;
	}
}
