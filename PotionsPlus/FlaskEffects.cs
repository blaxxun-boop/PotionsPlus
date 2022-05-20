using HarmonyLib;

namespace PotionsPlus;

public static class FlaskEffects
{
	[HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
	private class ReduceDamageTaken
	{
		private static void Prefix(Character __instance, HitData hit)
		{
			if (__instance is not Player player)
			{
				return;
			}

			if (player.GetSEMan().HaveStatusEffect("Flask_of_Elements"))
			{
				hit.m_damage.m_fire *= 1 - PotionsPlus.flaskOfElementsFireDamageTakenReduction.Value / 100f;
				hit.m_damage.m_lightning *= 1 - PotionsPlus.flaskOfElementsLightningDamageTakenReduction.Value / 100f;
			}
			
			if (player.GetSEMan().HaveStatusEffect("Flask_of_Fortification"))
			{
				hit.m_damage.m_blunt *= 1 - PotionsPlus.flaskOfFortificationBluntDamageTakenReduction.Value / 100f;
				hit.m_damage.m_slash *= 1 - PotionsPlus.flaskOfFortificationSlashDamageTakenReduction.Value / 100f;
				hit.m_damage.m_pierce *= 1 - PotionsPlus.flaskOfFortificationPierceDamageTakenReduction.Value / 100f;
			}
		}
	}
}
