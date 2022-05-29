using System.Linq;
using Groups;
using UnityEngine;

namespace PotionsPlus;

public class GroupPotion : SE_Stats
{
	public static bool effectApplied = false;
	public HitData.DamageType damageType;
	public int range;

	public override void Setup(Character character)
	{
		base.Setup(character);
		if (character == Player.m_localPlayer && !effectApplied)
		{
			foreach (PlayerReference groupPlayer in API.GroupPlayers().Where(p => p != PlayerReference.fromPlayer(Player.m_localPlayer)))
			{
				Vector3 groupPlayerPos = ZNet.m_instance.m_players.FirstOrDefault(p => p.m_characterID.m_userID == groupPlayer.peerId).m_position;
				if (range == 0 || Utils.DistanceXZ(character.transform.position, groupPlayerPos) < range)
				{
					ZRoutedRpc.instance.InvokeRoutedRPC(groupPlayer.peerId, "PotionsPlus Potion Activated", name);
				}
			}
		}
	}

	public override void ModifyAttack(Skills.SkillType skill, ref HitData hitData)
	{
		if ((int)damageType == 0)
		{
			return;
		}
		
		float chopDamage = hitData.m_damage.m_chop;
		float pickaxeDamage = hitData.m_damage.m_pickaxe;
		float totalDamage = hitData.GetTotalDamage() - pickaxeDamage - chopDamage;
		hitData.m_damage = new HitData.DamageTypes { m_chop = chopDamage, m_pickaxe = pickaxeDamage };

		switch (damageType)
		{
			case HitData.DamageType.Fire:
			{
				hitData.m_damage.m_fire = totalDamage;
				break;
			}
			case HitData.DamageType.Frost:
			{
				hitData.m_damage.m_frost = totalDamage;
				break;
			}
			case HitData.DamageType.Lightning:
			{
				hitData.m_damage.m_lightning = totalDamage;
				break;
			}
			case HitData.DamageType.Poison:
			{
				hitData.m_damage.m_poison = totalDamage;
				break;
			}
			case HitData.DamageType.Spirit:
			{
				hitData.m_damage.m_spirit = totalDamage;
				break;
			}
		}
	}
}