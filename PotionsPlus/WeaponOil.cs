namespace PotionsPlus;

public class WeaponOil : SE_Stats
{
	public override void ModifyAttack(Skills.SkillType skill, ref HitData hitData)
	{
		if (skill == Skills.SkillType.Bows || (m_character is Player player && player.GetCurrentWeapon() == player.m_unarmedWeapon.m_itemData))
		{
			return;
		}
		
		hitData.m_damage.Modify(1 + PotionsPlus.weaponOilDamageIncrease.Value / 100f);
	}
}
