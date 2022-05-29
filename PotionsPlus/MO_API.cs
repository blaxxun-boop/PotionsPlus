// Code written by KG and not by me. Obviously.

using System;
using System.Reflection;
using JetBrains.Annotations;

namespace PotionsPlus;

[PublicAPI]
public static class MO_API
{
	private static MO_API_State state = MO_API_State.NotReady;
	private static MethodInfo? AddSkillUseConditionMI;
	private static MethodInfo? MO_GetMana;
	private static MethodInfo? MO_SetMana;
	private static MethodInfo? MO_UseMana;
	private static MethodInfo? MO_ChangeClass;
	private static MethodInfo? MO_GetClassLevel;
	private static MethodInfo? MO_GetClassCurrentExperience;
	private static MethodInfo? MO_GetClassNeededExperience;
	private static MethodInfo? MO_AddExperience;
	private static MethodInfo? MO_GetCurrentClass;

	public enum Class
	{
		NONE,
		Ninja,
		Mage,
		Archer,
		Berserker,
		Rogue,
		Warlock,
		Monk,
		Paladin,
		Druid,
		Shaman,
		Deathknight,
		Engineer,
		Spartan,
		Priest
	}

	private enum MO_API_State
	{
		NotReady,
		NotInstalled,
		Ready
	}

	public static bool IsLoaded()
	{
		Init();
		return state == MO_API_State.Ready;
	}

	public static void AddSkillUseCondition(Func<bool> Method, string? message = null)
	{
		Init();
		AddSkillUseConditionMI?.Invoke(null, new object?[] { Method, message });
	}

	public static int GetMana()
	{
		int result = 0;
		Init();
		if (MO_GetMana != null)
		{
			result = (int)MO_GetMana.Invoke(null, null);
		}
		return result;
	}

	public static void SetMana(int value)
	{
		Init();
		MO_SetMana?.Invoke(null, new object[] { value });
	}

	public static void UseMana(int value)
	{
		Init();
		MO_UseMana?.Invoke(null, new object[] { value });
	}

	public static void ChangecLass(Class MOclass)
	{
		Init();
		MO_ChangeClass?.Invoke(null, new object[] { (int)MOclass });
	}

	public static int GetClassLevel(Class MOclass)
	{
		int result = 0;
		Init();
		if (MO_GetClassLevel != null)
		{
			result = (int)MO_GetClassLevel.Invoke(null, new object[] { (int)MOclass });
		}
		return result;
	}

	public static int GetClassCurrentExperience(Class MOclass)
	{
		int result = 0;
		Init();
		if (MO_GetClassCurrentExperience != null)
		{
			result = (int)MO_GetClassCurrentExperience.Invoke(null, new object[] { (int)MOclass });
		}
		return result;
	}

	public static int GetClassNeededExperience(Class MOclass)
	{
		int result = 0;
		Init();
		if (MO_GetClassNeededExperience != null)
		{
			result = (int)MO_GetClassNeededExperience.Invoke(null, new object[] { (int)MOclass });
		}
		return result;
	}

	public static void AddExperience(Class MOclass, int exp)
	{
		Init();
		MO_AddExperience?.Invoke(null, new object[] { (int)MOclass, exp });
	}

	public static Class GetCurrentClass()
	{
		int result = 0;
		Init();
		if (MO_GetCurrentClass != null)
		{
			result = (int)MO_GetCurrentClass.Invoke(null, new object[] { });
		}
		return (Class)result;
	}

	private static void Init()
	{
		if (state is MO_API_State.Ready or MO_API_State.NotInstalled)
		{
			return;
		}
		
		if (Type.GetType("MagicOverhaul.MagicOverhaul, MagicOverhaul") == null)
		{
			state = MO_API_State.NotInstalled;
			return;
		}

		state = MO_API_State.Ready;
		Type conditionsMO = Type.GetType("API.Conditions, MagicOverhaul")!;
		AddSkillUseConditionMI = conditionsMO.GetMethod("MO_AddSkillUseCondition", BindingFlags.Public | BindingFlags.Static);

		Type actionsMO = Type.GetType("API.Actions, MagicOverhaul")!;
		MO_GetMana = actionsMO.GetMethod("MO_GetMana", BindingFlags.Public | BindingFlags.Static);
		MO_SetMana = actionsMO.GetMethod("MO_SetMana", BindingFlags.Public | BindingFlags.Static);
		MO_UseMana = actionsMO.GetMethod("MO_UseMana", BindingFlags.Public | BindingFlags.Static);
		MO_ChangeClass = actionsMO.GetMethod("MO_ChangeClass", BindingFlags.Public | BindingFlags.Static);
		MO_GetClassLevel = actionsMO.GetMethod("MO_GetClassLevel", BindingFlags.Public | BindingFlags.Static);
		MO_GetClassCurrentExperience = actionsMO.GetMethod("MO_GetClassCurrentExperience", BindingFlags.Public | BindingFlags.Static);
		MO_GetClassNeededExperience = actionsMO.GetMethod("MO_GetClassNeededExperience", BindingFlags.Public | BindingFlags.Static);
		MO_AddExperience = actionsMO.GetMethod("MO_AddExperience", BindingFlags.Public | BindingFlags.Static);
		MO_GetCurrentClass = actionsMO.GetMethod("MO_GetCurrentClass", BindingFlags.Public | BindingFlags.Static);
	}
}
