namespace PotionsPlus;

public class ManaPotion : SE_Stats
{
	public int manaToRestore;

	public override void Setup(Character character)
	{
		base.Setup(character);

		if (character == Player.m_localPlayer)
		{
			MO_API.SetMana(MO_API.GetMana() + manaToRestore);
		}
	}
}
