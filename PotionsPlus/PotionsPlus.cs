using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ItemManager;
using ServerSync;
using SkillManager;
using UnityEngine;
using LocalizationManager;
using Random = UnityEngine.Random;

namespace PotionsPlus;

[BepInPlugin(ModGUID, ModName, ModVersion)]
[BepInDependency("org.bepinex.plugins.groups", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("MagicOverhaul", BepInDependency.DependencyFlags.SoftDependency)]
public class PotionsPlus : BaseUnityPlugin
{
	private const string ModName = "PotionsPlus";
	private const string ModVersion = "4.1.7";
	private const string ModGUID = "com.odinplus.potionsplus";

	private static readonly ConfigSync configSync = new(ModName) { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

	public static readonly HashSet<string> wandProjectiles = new();

	private static ConfigEntry<Toggle> serverConfigLocked = null!;
	public static ConfigEntry<float> philosophersStoneXpGainFactor = null!;
	private static ConfigEntry<Toggle> alchemySkillEnabled = null!;
	private static ConfigEntry<Toggle> alchemySkillBonusWhenCraftingEnabled = null!;
	// Flask of Elements
	public static ConfigEntry<int> flaskOfElementsFireDamageTakenReduction = null!;
	public static ConfigEntry<int> flaskOfElementsLightningDamageTakenReduction = null!;
	public static ConfigEntry<int> flaskOfElementsTTL = null!;
	// Flask of Fortification
	public static ConfigEntry<int> flaskOfFortificationBluntDamageTakenReduction = null!;
	public static ConfigEntry<int> flaskOfFortificationSlashDamageTakenReduction = null!;
	public static ConfigEntry<int> flaskOfFortificationPierceDamageTakenReduction = null!;
	public static ConfigEntry<int> flaskOfFortificationTTL = null!;
	// Flask of Magelight
	public static ConfigEntry<float> flaskOfMagelightIntensity = null!;
	public static ConfigEntry<Color> flaskOfMagelightColor = null!;
	public static ConfigEntry<int> flaskOfMagelightTTL = null!;
	// Flask of the Gods
	public static ConfigEntry<float> flaskOfGodsHealing = null!;
	public static ConfigEntry<float> flaskOfGodsRegenMultiplier = null!;
	public static ConfigEntry<int> flaskOfGodsTTL = null!;
	// Flask of Second Wind
	public static ConfigEntry<float> flaskOfSecondWindJumpStaminaFactor = null!;
	public static ConfigEntry<float> flaskOfSecondWindRunStaminaFactor = null!;
	public static ConfigEntry<float> flaskOfSecondWindStaminaRegenMultiplier = null!;
	public static ConfigEntry<int> flaskOfSecondWindTTL = null!;
	// Grand Healing Tide Potion
	public static ConfigEntry<float> grandHealingTidePotionHealthOverTime = null!;
	public static ConfigEntry<float> grandHealingTidePotionTickInterval = null!;
	public static ConfigEntry<float> grandHealingTidePotionTTL = null!;
	// Grand Spiritual Healing Potion
	public static ConfigEntry<float> grandSpiritualHealingPotionHealthOverTime = null!;
	public static ConfigEntry<int> grandSpiritualHealingPotionCooldown = null!;
	// Grand Stamina Elixir
	public static ConfigEntry<float> grandStaminaElixirStaminaOverTime = null!;
	public static ConfigEntry<float> grandStaminaElixirTTL = null!;
	public static ConfigEntry<int> grandStaminaElixirCooldown = null!;
	// Grand Stealth Elixir
	public static ConfigEntry<int> grandStealthElixirNoiseReduction = null!;
	public static ConfigEntry<int> grandStealthElixirVisibilityReduction = null!;
	public static ConfigEntry<int> grandStealthElixirTTL = null!;
	// Medium Healing Tide Flask
	public static ConfigEntry<float> mediumHealingTideFlaskHealthOverTime = null!;
	public static ConfigEntry<float> mediumHealingTideFlaskTickInterval = null!;
	public static ConfigEntry<float> mediumHealingTideFlaskTTL = null!;
	public static ConfigEntry<int> mediumHealingTideFlaskCooldown = null!;
	// Medium Spiritual Healing Flask
	public static ConfigEntry<float> mediumSpiritualHealingFlaskHealthOverTime = null!;
	public static ConfigEntry<int> mediumSpiritualHealingFlaskCooldown = null!;
	// Medium Stamina Flask
	public static ConfigEntry<float> mediumStaminaFlaskStaminaOverTime = null!;
	public static ConfigEntry<int> mediumStaminaFlaskCooldown = null!;
	// Lesser Healing Tide Vial
	public static ConfigEntry<float> lesserHealingTideVialHealthOverTime = null!;
	public static ConfigEntry<float> lesserHealingTideVialTickInterval = null!;
	public static ConfigEntry<float> lesserHealingTideVialTTL = null!;
	public static ConfigEntry<int> lesserHealingTideVialCooldown = null!;
	// Lesser Spiritual Healing Vial
	public static ConfigEntry<float> lesserSpiritualHealingVialHealthOverTime = null!;
	public static ConfigEntry<int> lesserSpiritualHealingVialCooldown = null!;
	// Lesser Stamina Vial
	public static ConfigEntry<float> lesserStaminaVialStaminaOverTime = null!;
	public static ConfigEntry<int> lesserStaminaVialCooldown = null!;
	// Hellbroth of Flames
	public static ConfigEntry<float> hellbrothOfFlamesDamage = null!;
	// Hellbroth of Frost
	public static ConfigEntry<float> hellbrothOfFrostDamage = null!;
	// Hellbroth of Thors Fury
	public static ConfigEntry<float> hellbrothOfThorsFuryDamage = null!;
	// Hellbroth of Eternal Life
	private static ConfigEntry<float> hellbrothOfEternalLifeHealing = null!;
	// Brew of Faint Group Healing
	public static ConfigEntry<float> brewOfFaintGroupHealingHealthOverTime = null!;
	public static ConfigEntry<int> brewOfFaintGroupHealingCooldown = null!;
	private static ConfigEntry<int> brewOfFaintGroupHealingRange = null!;
	// Brew of Group Healing
	public static ConfigEntry<float> brewOfGroupHealingHealthOverTime = null!;
	public static ConfigEntry<int> brewOfGroupHealingCooldown = null!;
	private static ConfigEntry<int> brewOfGroupHealingRange = null!;
	// Brew of Grand Group Healing
	public static ConfigEntry<float> brewOfGrandGroupHealingHealthOverTime = null!;
	public static ConfigEntry<int> brewOfGrandGroupHealingCooldown = null!;
	private static ConfigEntry<int> brewOfGrandGroupHealingRange = null!;
	// Brew of Fiery Revenge
	public static ConfigEntry<float> brewOfFieryRevengeTTL = null!;
	public static ConfigEntry<int> brewOfFieryRevengeRange = null!;
	// Brew of the Icy Touch
	public static ConfigEntry<float> brewOfIcyTouchTTL = null!;
	public static ConfigEntry<int> brewOfIcyTouchRange = null!;
	// Brew of Cunning Toxicity
	public static ConfigEntry<float> brewOfCunningToxicityTTL = null!;
	public static ConfigEntry<int> brewOfCunningToxicityRange = null!;
	// Brew of Spiritual Death
	public static ConfigEntry<float> brewOfSpiritualDeathTTL = null!;
	public static ConfigEntry<int> brewOfSpiritualDeathRange = null!;
	// Brew of Thunderous Words
	public static ConfigEntry<float> brewOfThunderousWordsTTL = null!;
	public static ConfigEntry<int> brewOfThunderousWordsRange = null!;
	// Odins Wizard Hat
	private static ConfigEntry<float> wizardHatConsumeChargeReduction = null!;
	// Odins Warlock Hat
	private static ConfigEntry<float> warlockHatSmokeScreenSizeIncrease = null!;
	private static ConfigEntry<int> warlockHatSmokeScreenBlockIncrease = null!;
	private static ConfigEntry<int> warlockHatSmokeScreenDurationIncrease = null!;
	// Odins Weapon Oil
	public static ConfigEntry<int> weaponOilDamageIncrease = null!;
	public static ConfigEntry<float> weaponOilTTL = null!;
	// Odins Dragon Staff
	private static ConfigEntry<int> smokeScreenChanceToBlock = null!;
	public static ConfigEntry<float> smokeScreenTTL = null!;
	// Weak Mana Potion
	public static ConfigEntry<int> weakManaPotionManaRestoration = null!;
	public static ConfigEntry<int> weakManaPotionCooldown = null!;
	// Giant Mana Potion
	public static ConfigEntry<int> giantManaPotionManaRestoration = null!;
	public static ConfigEntry<int> giantManaPotionCooldown = null!;
	// Overflowing Mana Potion
	public static ConfigEntry<int> overflowingManaPotionCooldown = null!;

	private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
	{
		ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

		SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
		syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

		return configEntry;
	}

	private ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

	private enum Toggle
	{
		On = 1,
		Off = 0
	}

	private static Skill alchemy = null!;

	public void Awake()
	{
		Localizer.Load();

		AssetBundle assets = PrefabManager.RegisterAssetBundle("potions");

		alchemy = new Skill("Alchemy", assets.LoadAsset<Sprite>("AlcSkill"));
		alchemy.Name.Alias("pp_potion_skill_name");
		alchemy.Description.Alias("pp_potion_skill_description");
		alchemy.Configurable = false;

		serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On, "If on, the configuration is locked and can be changed by server admins only.");
		configSync.AddLockingConfigEntry(serverConfigLocked);
		alchemySkillEnabled = config("1 - General", "Alchemy Skill", Toggle.On, "Enables the Alchemy skill.");
		alchemySkillBonusWhenCraftingEnabled = config("1 - General", "Alchemy Crafting Bonus", Toggle.On, "Enable Alchemy Bonus when crafting.");
		philosophersStoneXpGainFactor = config("2 - Philosophers Stone", "Philosophers Stone XP Gain Factor", 1.25f, new ConfigDescription("Factor for Alchemy XP gain, when a Philosophers Stone is equipped.", new AcceptableValueRange<float>(1f, 3f)));
		// Flask of Elements
		flaskOfElementsFireDamageTakenReduction = config("Flask of Elements", "Fire damage reduction", 66, new ConfigDescription("Fire damage reduction during Flask of Elements effect.", new AcceptableValueRange<int>(0, 100)));
		flaskOfElementsLightningDamageTakenReduction = config("Flask of Elements", "Lightning damage reduction", 66, new ConfigDescription("Lightning damage reduction during Flask of Elements effect.", new AcceptableValueRange<int>(0, 100)));
		flaskOfElementsTTL = config("Flask of Elements", "Effect Duration", 120, new ConfigDescription("Effect duration of the Flask of Elements in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Flask of Fortification
		flaskOfFortificationBluntDamageTakenReduction = config("Flask of Fortification", "Blunt damage reduction", 66, new ConfigDescription("Blunt damage reduction during Flask of Fortification effect.", new AcceptableValueRange<int>(0, 100)));
		flaskOfFortificationSlashDamageTakenReduction = config("Flask of Fortification", "Slash damage reduction", 66, new ConfigDescription("Slash damage reduction during Flask of Fortification effect.", new AcceptableValueRange<int>(0, 100)));
		flaskOfFortificationPierceDamageTakenReduction = config("Flask of Fortification", "Pierce damage reduction", 66, new ConfigDescription("Pierce damage reduction during Flask of Fortification effect.", new AcceptableValueRange<int>(0, 100)));
		flaskOfFortificationTTL = config("Flask of Fortification", "Effect Duration", 120, new ConfigDescription("Effect duration of the Flask of Fortification in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Flask of Magelight
		flaskOfMagelightColor = config("Flask of Magelight", "Effect Color", new Color(0.75f, 0.75f, 1f), new ConfigDescription("Color of the Flask of Magelight effect."));
		flaskOfMagelightIntensity = config("Flask of Magelight", "Effect Intensity", 3f, new ConfigDescription("Intensity of the Flask of Magelight effect.", new AcceptableValueRange<float>(0.01f, 5f)));
		flaskOfMagelightTTL = config("Flask of Magelight", "Effect Duration", 300, new ConfigDescription("Effect duration of the Flask of Magelight in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Flask of the Gods
		flaskOfGodsHealing = config("Flask of the Gods", "Healing Effect", 250f, new ConfigDescription("Healing from the Flask of the Gods effect."));
		flaskOfGodsRegenMultiplier = config("Flask of the Gods", "Health Regen Factor", 1.05f, new ConfigDescription("Health regen factor from the Flask of the Gods effect.", new AcceptableValueRange<float>(1f, 5f)));
		flaskOfGodsTTL = config("Flask of the Gods", "Effect Duration", 300, new ConfigDescription("Effect duration of the Flask of the Gods in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Flask of Second Wind
		flaskOfSecondWindJumpStaminaFactor = config("Flask of Second Wind", "Jump Stamina Usage Factor", 0.75f, new ConfigDescription("Jump stamina usage factor the Flask of Second Wind effect.", new AcceptableValueRange<float>(0f, 1f)));
		flaskOfSecondWindRunStaminaFactor = config("Flask of Second Wind", "Run Stamina Usage Factor", 0.75f, new ConfigDescription("Run stamina usage factor from the Flask of Second Wind effect.", new AcceptableValueRange<float>(0f, 1f)));
		flaskOfSecondWindStaminaRegenMultiplier = config("Flask of Second Wind", "Stamina Regen Factor", 1.05f, new ConfigDescription("Stamina regeneration factor from the Flask of Second Wind effect.", new AcceptableValueRange<float>(1f, 5f)));
		flaskOfSecondWindTTL = config("Flask of Second Wind", "Effect Duration", 120, new ConfigDescription("Effect duration of the Flask of Second Wind in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Grand Healing Tide Potion
		grandHealingTidePotionHealthOverTime = config("Grand Healing Tide Potion", "Healing Effect", 95f, new ConfigDescription("Healing from the Grand Healing Tide Potion effect."));
		grandHealingTidePotionTickInterval = config("Grand Healing Tide Potion", "Tick Interval", 1f, new ConfigDescription("Tick interval for the Grand Healing Tide Potion effect.", new AcceptableValueRange<float>(1f, 10f)));
		grandHealingTidePotionTTL = config("Grand Healing Tide Potion", "Effect Duration", 10f, new ConfigDescription("Effect duration of the Grand Healing Tide Potion in seconds.", new AcceptableValueRange<float>(1f, 30f)));
		// Grand Stamina Elixir
		grandStaminaElixirStaminaOverTime = config("Grand Stamina Elixir", "Stamina Regeneration", 150f, new ConfigDescription("Stamina regeneration from the Grand Stamina Elixir effect."));
		grandStaminaElixirTTL = config("Grand Stamina Elixir", "Effect Duration", 15f, new ConfigDescription("Effect duration of the Grand Stamina Elixir effect in seconds.", new AcceptableValueRange<float>(1f, 30f)));
		grandStaminaElixirCooldown = config("Grand Stamina Elixir", "Cooldown Duration", 300, new ConfigDescription("Cooldown duration of the Grand Stamina Elixir in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Grand Spiritual Healing Potion
		grandSpiritualHealingPotionHealthOverTime = config("Grand Spiritual Healing Potion", "Healing Effect", 100f, new ConfigDescription("Healing from the Grand Spiritual Healing Potion effect."));
		grandSpiritualHealingPotionCooldown = config("Grand Spiritual Healing Potion", "Cooldown Duration", 240, new ConfigDescription("Cooldown of the Grand Spiritual Healing Potion in seconds.", new AcceptableValueRange<int>(1, 600)));
		// Grand Stealth Elixir
		grandStealthElixirVisibilityReduction = config("Grand Stealth Elixir", "Visibility Reduction", 100, new ConfigDescription("Visibility reduction while sneaking during the Grand Stealth Elixir effect.", new AcceptableValueRange<int>(0, 100)));
		grandStealthElixirNoiseReduction = config("Grand Stealth Elixir", "Noise Reduction", 100, new ConfigDescription("Noise reduction while sneaking during the Grand Stealth Elixir effect.", new AcceptableValueRange<int>(0, 100)));
		grandStealthElixirTTL = config("Grand Stealth Elixir", "Effect Duration", 60, new ConfigDescription("Effect duration of the Grand Stealth Elixir effect in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Medium Healing Tide Flask
		mediumHealingTideFlaskHealthOverTime = config("Medium Healing Tide Flask", "Healing Effect", 45f, new ConfigDescription("Healing from the Medium Healing Tide Flask effect."));
		mediumHealingTideFlaskTickInterval = config("Medium Healing Tide Flask", "Tick Interval", 1f, new ConfigDescription("Tick interval for the Medium Healing Tide Flask effect.", new AcceptableValueRange<float>(1f, 10f)));
		mediumHealingTideFlaskTTL = config("Medium Healing Tide Flask", "Effect Duration", 10f, new ConfigDescription("Effect duration of the Medium Healing Tide Flask in seconds.", new AcceptableValueRange<float>(1f, 30f)));
		mediumHealingTideFlaskCooldown = config("Medium Healing Tide Flask", "Cooldown Duration", 240, new ConfigDescription("Cooldown of the Medium Healing Tide Flask in seconds.", new AcceptableValueRange<int>(1, 600)));
		// Medium Spiritual Healing Flask
		mediumSpiritualHealingFlaskHealthOverTime = config("Medium Spiritual Healing Flask", "Healing Effect", 50f, new ConfigDescription("Healing from the Medium Spiritual Healing Flask effect."));
		mediumSpiritualHealingFlaskCooldown = config("Medium Spiritual Healing Flask", "Cooldown Duration", 240, new ConfigDescription("Cooldown of the Medium Spiritual Healing Flask in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Medium Stamina Flask
		mediumStaminaFlaskStaminaOverTime = config("Medium Stamina Flask", "Stamina Regeneration", 90f, new ConfigDescription("Stamina regeneration from the Medium Stamina Flask effect."));
		mediumStaminaFlaskCooldown = config("Medium Stamina Flask", "Cooldown Duration", 240, new ConfigDescription("Cooldown duration of the Medium Stamina Flask in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Lesser Healing Tide Vial
		lesserHealingTideVialHealthOverTime = config("Lesser Healing Tide Vial", "Healing Effect", 25f, new ConfigDescription("Healing from the Lesser Healing Tide Vial effect."));
		lesserHealingTideVialTickInterval = config("Lesser Healing Tide Vial", "Tick Interval", 1f, new ConfigDescription("Tick interval for the Lesser Healing Tide Vial effect.", new AcceptableValueRange<float>(1f, 10f)));
		lesserHealingTideVialTTL = config("Lesser Healing Tide Vial", "Effect Duration", 10f, new ConfigDescription("Effect duration of the Lesser Healing Tide Vial in seconds.", new AcceptableValueRange<float>(1f, 30f)));
		lesserHealingTideVialCooldown = config("Lesser Healing Tide Vial", "Cooldown Duration", 240, new ConfigDescription("Cooldown duration of the Lesser Healing Tide Vial in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Lesser Spiritual Healing Vial
		lesserSpiritualHealingVialHealthOverTime = config("Lesser Spiritual Healing Vial", "Healing Effect", 25f, new ConfigDescription("Healing from the Lesser Spiritual Healing Vial effect."));
		lesserSpiritualHealingVialCooldown = config("Lesser Spiritual Healing Vial", "Cooldown Duration", 240, new ConfigDescription("Cooldown duration of the Lesser Spiritual Healing Vial in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Lesser Stamina Vial
		lesserStaminaVialStaminaOverTime = config("Lesser Stamina Vial", "Stamina Regeneration", 45f, new ConfigDescription("Stamina regeneration from the Lesser Stamina Vial effect."));
		lesserStaminaVialCooldown = config("Lesser Stamina Vial", "Cooldown Duration", 240, new ConfigDescription("Cooldown duration of the Lesser Stamina Vial in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Hellbroth of Flames
		hellbrothOfFlamesDamage = config("Hellbroth of Flames", "Fire Damage", 20f, new ConfigDescription("Fire damage dealt per tick by the Hellbroth of Flames."));
		// Hellbroth of Frost
		hellbrothOfFrostDamage = config("Hellbroth of Frost", "Frost Damage", 50f, new ConfigDescription("Frost damage dealt per tick by the Hellbroth of Frost."));
		// Hellbroth of Thors Fury
		hellbrothOfThorsFuryDamage = config("Hellbroth of Thors Fury", "Lightning Damage", 60f, new ConfigDescription("Lightning damage dealt per tick by the Hellbroth of Thors Fury."));
		// Hellbroth of Eternal Life
		hellbrothOfEternalLifeHealing = config("Hellbroth of Eternal Life", "Healing", 20f, new ConfigDescription("Percentage of maximum health as healing per tick by the Hellbroth of Eternal Life.", new AcceptableValueRange<float>(0.1f, 100f)));
		// Brew of Faint Group Healing
		brewOfFaintGroupHealingHealthOverTime = config("Brew of Faint Group Healing", "Healing Effect", 25f, new ConfigDescription("Healing from the Brew of Faint Group Healing effect."));
		brewOfFaintGroupHealingCooldown = config("Brew of Faint Group Healing", "Cooldown Duration", 240, new ConfigDescription("Cooldown of the Brew of Faint Group Healing in seconds.", new AcceptableValueRange<int>(5, 900)));
		brewOfFaintGroupHealingRange = config("Brew of Faint Group Healing", "Effect Range", 30, new ConfigDescription("Range of the Brew of Faint Group Healing effect.", new AcceptableValueRange<int>(3, 200)));
		// Brew of Group Healing
		brewOfGroupHealingHealthOverTime = config("Brew of Group Healing", "Healing Effect", 50f, new ConfigDescription("Healing from the Brew of Group Healing effect."));
		brewOfGroupHealingCooldown = config("Brew of Group Healing", "Cooldown Duration", 240, new ConfigDescription("Cooldown of the Brew of Group Healing in seconds.", new AcceptableValueRange<int>(5, 900)));
		brewOfGroupHealingRange = config("Brew of Group Healing", "Effect Range", 30, new ConfigDescription("Range of the Brew of Group Healing effect.", new AcceptableValueRange<int>(3, 200)));
		// Brew of Grand Group Healing
		brewOfGrandGroupHealingHealthOverTime = config("Brew of Grand Group Healing", "Healing Effect", 125f, new ConfigDescription("Healing from the Brew of Grand Group Healing effect."));
		brewOfGrandGroupHealingCooldown = config("Brew of Grand Group Healing", "Cooldown Duration", 240, new ConfigDescription("Cooldown of the Brew of Grand Group Healing in seconds.", new AcceptableValueRange<int>(5, 900)));
		brewOfGrandGroupHealingRange = config("Brew of Grand Group Healing", "Effect Range", 30, new ConfigDescription("Range of the Brew of Grand Group Healing effect.", new AcceptableValueRange<int>(3, 200)));
		// Brew of Fiery Revenge
		brewOfFieryRevengeTTL = config("Brew of Fiery Revenge", "Effect Duration", 180f, new ConfigDescription("Effect duration for the Brew of Fiery Revenge in seconds.", new AcceptableValueRange<float>(5f, 900f)));
		brewOfFieryRevengeRange = config("Brew of Fiery Revenge", "Effect Range", 30, new ConfigDescription("Range of the Brew of Fiery Revenge effect.", new AcceptableValueRange<int>(3, 200)));
		// Brew of the Icy Touch
		brewOfIcyTouchTTL = config("Brew of the Icy Touch", "Effect Duration", 180f, new ConfigDescription("Effect duration for the Brew of the Icy Touch in seconds.", new AcceptableValueRange<float>(5f, 900f)));
		brewOfIcyTouchRange = config("Brew of the Icy Touch", "Effect Range", 30, new ConfigDescription("Range of the Brew of the Icy Touch effect.", new AcceptableValueRange<int>(3, 200)));
		// Brew of Cunning Toxicity
		brewOfCunningToxicityTTL = config("Brew of Cunning Toxicity", "Effect Duration", 180f, new ConfigDescription("Effect duration for the Brew of Cunning Toxicity in seconds.", new AcceptableValueRange<float>(5f, 900f)));
		brewOfCunningToxicityRange = config("Brew of Cunning Toxicity", "Effect Range", 30, new ConfigDescription("Range of the Brew of Cunning Toxicity effect.", new AcceptableValueRange<int>(3, 200)));
		// Brew of Spiritual Death
		brewOfSpiritualDeathTTL = config("Brew of Spiritual Death", "Effect Duration", 180f, new ConfigDescription("Effect duration for the Brew of Spiritual Death in seconds.", new AcceptableValueRange<float>(5f, 900f)));
		brewOfSpiritualDeathRange = config("Brew of Spiritual Death", "Effect Range", 30, new ConfigDescription("Range of the Brew of Spiritual Death effect.", new AcceptableValueRange<int>(3, 200)));
		// Brew of Thunderous Words
		brewOfThunderousWordsTTL = config("Brew of Thunderous Words", "Effect Duration", 180f, new ConfigDescription("Effect duration for the Brew of Thunderous Words in seconds.", new AcceptableValueRange<float>(5f, 900f)));
		brewOfThunderousWordsRange = config("Brew of Thunderous Words", "Effect Range", 30, new ConfigDescription("Range of the Brew of Thunderous Words effect.", new AcceptableValueRange<int>(3, 200)));
		// Odins Wizard Hat
		wizardHatConsumeChargeReduction = config("Odins Wizard Hat", "Charge Consumption Reduction", 40f, new ConfigDescription("Chance to not consume a Hellbroth charge, when using the Alchemy Wand or Dragon Staff while wearing the Wizard hat.", new AcceptableValueRange<float>(0f, 100f)));
		// Odins Warlock Hat
		warlockHatSmokeScreenSizeIncrease = config("Odins Warlock Hat", "Smoke Screen Size Increase", 2f, new ConfigDescription("Radius increase for the smoke screen ability of the Dragon Staff while wearing the Warlock hat.", new AcceptableValueRange<float>(0f, 5f)));
		warlockHatSmokeScreenDurationIncrease = config("Odins Warlock Hat", "Smoke Screen Duration Increase", 2, new ConfigDescription("Duration increase for the smoke screen ability of the Dragon Staff while wearing the Warlock hat in seconds.", new AcceptableValueRange<int>(0, 15)));
		warlockHatSmokeScreenBlockIncrease = config("Odins Warlock Hat", "Smoke Screen Block Chance Increase", 25, new ConfigDescription("Projectile block chance increase for the smoke screen ability of the Dragon Staff while wearing the Warlock hat.", new AcceptableValueRange<int>(0, 100)));
		// Odins Dragon Staff
		smokeScreenChanceToBlock = config("Odins Dragon Staff", "Block Chance", 50, new ConfigDescription("Chance to block projectiles for the smoke screen effect of Odins Dragon Staff.", new AcceptableValueRange<int>(0, 100)));
		smokeScreenTTL = config("Odins Dragon Staff", "Effect Duration", 8f, new ConfigDescription("Effect duration for the smoke screen effect of Odins Dragon Staff in seconds.", new AcceptableValueRange<float>(1f, 30f)));
		// Odins Weapon Oil
		weaponOilDamageIncrease = config("Odins Weapon Oil", "Damage Increase", 5, new ConfigDescription("Damage increase for weapons from the Odins Weapon Oil effect.", new AcceptableValueRange<int>(0, 100)));
		weaponOilTTL = config("Odins Weapon Oil", "Effect Duration", 30f, new ConfigDescription("Effect duration for Odins Weapon Oil in minutes.", new AcceptableValueRange<float>(1f, 120f)));
		// Weak Mana Potion
		weakManaPotionManaRestoration = config("Weak Mana Potion", "Mana Restoration", 15, new ConfigDescription("Mana restoration from the Weak Mana Potion effect."));
		weakManaPotionCooldown = config("Weak Mana Potion", "Cooldown Duration", 120, new ConfigDescription("Cooldown of the Weak Mana Potion in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Giant Mana Potion
		giantManaPotionManaRestoration = config("Giant Mana Potion", "Mana Restoration", 75, new ConfigDescription("Mana restoration from the Giant Mana Potion effect."));
		giantManaPotionCooldown = config("Giant Mana Potion", "Cooldown Duration", 180, new ConfigDescription("Cooldown of the Giant Mana Potion in seconds.", new AcceptableValueRange<int>(5, 900)));
		// Overflowing Mana Potion
		overflowingManaPotionCooldown = config("Overflowing Mana Potion", "Cooldown Duration", 300, new ConfigDescription("Cooldown of the Overflowing Mana Potion in seconds.", new AcceptableValueRange<int>(5, 900)));

		BuildingPiecesSetup.initializeBuildingPieces(assets);
		PotionsSetup.initializePotions(assets);
		EquipmentSetup.initializeEquipment(assets);
		GroupPotionSetup.initializeGroupPotions(assets);
		ManaPotionSetup.initializeManaPotions(assets);
		HellbrothSetup.initializeHellbroth(assets);
		PhilosophersSetup.initializePhilosophersStones(assets);

		Localizer.AddPlaceholder("pp_flask_elements_description", "duration", flaskOfElementsTTL, ttl => (ttl / 60f).ToString("0.#"));
		Localizer.AddPlaceholder("pp_flask_secondwind_description", "duration", flaskOfSecondWindTTL, ttl => (ttl / 60f).ToString("0.#"));
		Localizer.AddPlaceholder("pp_flask_fort_description", "duration", flaskOfFortificationTTL, ttl => (ttl / 60f).ToString("0.#"));
		Localizer.AddPlaceholder("pp_flask_god_description", "power", flaskOfGodsHealing);
		Localizer.AddPlaceholder("pp_elixir_healing_description", "power", grandHealingTidePotionHealthOverTime);
		Localizer.AddPlaceholder("pp_elixir_healing_description", "duration", grandHealingTidePotionTTL);
		Localizer.AddPlaceholder("pp_elixir_spiritual_description", "power", grandSpiritualHealingPotionHealthOverTime);
		Localizer.AddPlaceholder("pp_elixir_stam_description", "power", grandStaminaElixirStaminaOverTime);
		Localizer.AddPlaceholder("pp_elixir_stam_description", "duration", grandStaminaElixirTTL);
		Localizer.AddPlaceholder("pp_elixir_stealth_description", "duration", grandStealthElixirTTL);
		Localizer.AddPlaceholder("pp_potion_healing_description", "power", mediumHealingTideFlaskHealthOverTime);
		Localizer.AddPlaceholder("pp_potion_healing_description", "duration", mediumHealingTideFlaskTTL);
		Localizer.AddPlaceholder("pp_potion_spiritual_description", "power", mediumSpiritualHealingFlaskHealthOverTime);
		Localizer.AddPlaceholder("pp_potion_stam_description", "power", mediumStaminaFlaskStaminaOverTime);
		Localizer.AddPlaceholder("pp_vial_healing_description", "power", lesserHealingTideVialHealthOverTime);
		Localizer.AddPlaceholder("pp_vial_healing_description", "duration", lesserHealingTideVialTTL);
		Localizer.AddPlaceholder("pp_vial_spiritual_description", "power", lesserSpiritualHealingVialHealthOverTime);
		Localizer.AddPlaceholder("pp_vial_stam_description", "power", lesserStaminaVialStaminaOverTime);
		Localizer.AddPlaceholder("pp_hellbroth_of_flames_description", "power", hellbrothOfFlamesDamage);
		Localizer.AddPlaceholder("pp_hellbroth_of_frost_description", "power", hellbrothOfFrostDamage);
		Localizer.AddPlaceholder("pp_hellbroth_of_thors_fury", "power", hellbrothOfThorsFuryDamage);
		Localizer.AddPlaceholder("pp_hellbroth_of_eternal_life_description", "power", hellbrothOfEternalLifeHealing);
		Localizer.AddPlaceholder("pp_lesser_group_healing_description", "power", brewOfFaintGroupHealingHealthOverTime);
		Localizer.AddPlaceholder("pp_lesser_group_healing_description", "range", brewOfFaintGroupHealingRange);
		Localizer.AddPlaceholder("pp_medium_group_healing_description", "power", brewOfGroupHealingHealthOverTime);
		Localizer.AddPlaceholder("pp_medium_group_healing_description", "range", brewOfGroupHealingRange);
		Localizer.AddPlaceholder("pp_grand_group_healing_description", "power", brewOfGrandGroupHealingHealthOverTime);
		Localizer.AddPlaceholder("pp_grand_group_healing_description", "range", brewOfGrandGroupHealingRange);
		Localizer.AddPlaceholder("pp_brew_of_toxicity_description", "range", brewOfCunningToxicityRange);
		Localizer.AddPlaceholder("pp_brew_of_fiery_revenge_description", "range", brewOfFieryRevengeRange);
		Localizer.AddPlaceholder("pp_brew_of_icy_touch_description", "range", brewOfIcyTouchRange);
		Localizer.AddPlaceholder("pp_brew_of_spiritual_death_description", "range", brewOfSpiritualDeathRange);
		Localizer.AddPlaceholder("pp_brew_of_thunderous_words_description", "range", brewOfThunderousWordsRange);
		Localizer.AddPlaceholder("pp_odins_wizard_hat_description", "power", wizardHatConsumeChargeReduction);
		Localizer.AddPlaceholder("pp_odins_weapon_oil_description", "power", weaponOilDamageIncrease);
		Localizer.AddPlaceholder("pp_odins_weapon_oil_description", "duration", weaponOilTTL);
		Localizer.AddPlaceholder("pp_weapon_oil_description", "power", weaponOilDamageIncrease);
		Localizer.AddPlaceholder("pp_odins_dragon_staff_description", "power", smokeScreenChanceToBlock);
		Localizer.AddPlaceholder("pp_odins_dragon_staff_description", "duration", smokeScreenTTL);
		Localizer.AddPlaceholder("pp_odins_warlock_hat_description", "power", warlockHatSmokeScreenBlockIncrease);
		Localizer.AddPlaceholder("pp_odins_warlock_hat_description", "radius", warlockHatSmokeScreenSizeIncrease);
		Localizer.AddPlaceholder("pp_odins_warlock_hat_description", "duration", warlockHatSmokeScreenDurationIncrease);
		Localizer.AddPlaceholder("pp_lesser_mana_potion_description", "power", weakManaPotionManaRestoration);
		Localizer.AddPlaceholder("pp_large_mana_potion_description", "power", giantManaPotionManaRestoration);

		Assembly assembly = Assembly.GetExecutingAssembly();
		Harmony harmony = new(ModGUID);
		harmony.PatchAll(assembly);

		CheatDeathStatusEffect = assets.LoadAsset<SE_Stats>("CheatDeath");
		AlchemySkillProcStatusEffect = assets.LoadAsset<SE_Stats>("AlcSkillProc");

		PrefabManager.RegisterPrefab(assets, "PP_sfx_rebirth");
		PrefabManager.RegisterPrefab(assets, "PP_sfx_stoneuse");
		PrefabManager.RegisterPrefab(assets, "PP_sfx_brewpotiondone");
		PrefabManager.RegisterPrefab(assets, "PP_sfx_potion_smash");
		PrefabManager.RegisterPrefab(assets, "PP_sfx_brewpotion");
		PrefabManager.RegisterPrefab(assets, "PP_vfx_potionhit");
		PrefabManager.RegisterPrefab(assets, "sfx_build_alchemy");
	}

	private static SE_Stats CheatDeathStatusEffect = null!;
	private static SE_Stats AlchemySkillProcStatusEffect = null!;

	[HarmonyPatch(typeof(Character), nameof(Character.SetHealth))]
	public static class PatchCharacterSetHealth
	{
		private static bool Prefix(ref Character __instance, float health)
		{
			if (__instance != Player.m_localPlayer || health > 0 || Player.m_localPlayer.GetSEMan().GetStatusEffects().FirstOrDefault(se => se.m_category == "pp_philstone") is not { } philosopherStoneStatusEffect)
			{
				return true;
			}

			Player.m_localPlayer.GetSEMan().RemoveStatusEffect(philosopherStoneStatusEffect);

			Inventory inventory = Player.m_localPlayer.GetInventory();
			if (inventory.GetAllItems().FirstOrDefault(i => i.m_equiped && i.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Utility && i.m_shared.m_equipStatusEffect.m_category == "pp_philstone") is { } equippedPhilosopherStone)
			{
				inventory.RemoveOneItem(equippedPhilosopherStone);
				CheatDeathStatusEffect.m_ttl = 6f;
				Player.m_localPlayer.GetSEMan().AddStatusEffect(CheatDeathStatusEffect);
				__instance.Heal(__instance.GetMaxHealth());
				return false;
			}

			return true;
		}
	}

	[HarmonyPatch(typeof(ZNet), nameof(ZNet.Awake))]
	private static class RegisterRPCPatch
	{
		private static void Postfix()
		{
			ZRoutedRpc.instance.Register<float>("PotionsPlus Raise Alchemy", (_, num) => Player.m_localPlayer.RaiseSkill("Alchemy", num));
			ZRoutedRpc.instance.Register<int>("PotionsPlus Alchemy Level", (sender, num) => SubmitAlchemyLevel.playerAlchemyLevels[sender] = num);
			ZRoutedRpc.instance.Register("PotionsPlus Alchemy Proc", _ => Player.m_localPlayer.GetSEMan().AddStatusEffect(AlchemySkillProcStatusEffect));
			ZRoutedRpc.instance.Register<string>("PotionsPlus Potion Activated", onGroupPotionActivated);
		}
	}

	private static void onGroupPotionActivated(long sender, string effect)
	{
		GroupPotion.effectApplied = true;
		Player.m_localPlayer.GetSEMan().AddStatusEffect(ObjectDB.instance.GetStatusEffect(effect));
		GroupPotion.effectApplied = false;
	}

	[HarmonyPatch(typeof(Incinerator), nameof(Incinerator.OnIncinerate))]
	private static class SubmitAlchemyLevel
	{
		public static readonly Dictionary<long, int> playerAlchemyLevels = new();

		private static void Postfix(Incinerator __instance)
		{
			ZRoutedRpc.instance.InvokeRoutedRPC(__instance.m_nview.GetZDO().m_owner, "PotionsPlus Alchemy Level", Mathf.RoundToInt(Player.m_localPlayer.GetSkillFactor("Alchemy") * 100));
		}
	}

	[HarmonyPatch]
	private static class SaveCauldronUsingPlayer
	{
		public static long uid;

		private static void SetUid(Incinerator incinerator, long sender)
		{
			if (incinerator.name.StartsWith("opcauldron", StringComparison.Ordinal))
			{
				uid = sender;
			}
		}

		private static MethodInfo TargetMethod() => typeof(Incinerator).GetNestedTypes(BindingFlags.Instance | BindingFlags.NonPublic).Where(t => t.Name.Contains(nameof(Incinerator.Incinerate))).SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(m => m.Name == "MoveNext")).First();

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			Type Incinerate = TargetMethod().DeclaringType;

			MethodInfo invoke = AccessTools.DeclaredMethod(typeof(MonoBehaviour), nameof(Invoke));
			foreach (CodeInstruction instruction in instructions)
			{
				if (instruction.opcode == OpCodes.Call && instruction.OperandIs(invoke))
				{
					yield return new CodeInstruction(OpCodes.Ldarg_0);
					yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.DeclaredField(Incinerate, "<>4__this"));
					yield return new CodeInstruction(OpCodes.Ldarg_0);
					yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.DeclaredField(Incinerate, "uid"));
					yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(SaveCauldronUsingPlayer), nameof(SetUid)));
				}

				if (instruction.opcode == OpCodes.Ret)
				{
					yield return new CodeInstruction(OpCodes.Ldarg_0);
					yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.DeclaredField(Incinerate, "<>4__this"));
					yield return new CodeInstruction(OpCodes.Ldc_I8, 0L);
					yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(SaveCauldronUsingPlayer), nameof(SetUid)));
				}

				yield return instruction;
			}
		}
	}

	private static int DetermineExtraItems(int skillLevel, long sender)
	{
		int extraItems = 0;

		if (alchemySkillBonusWhenCraftingEnabled.Value == Toggle.On)
		{
			// 1-100% chance to craft an extra item. 1% per level of skill.
			if (Random.Range(1, 100) < skillLevel)
			{
				++extraItems;
			}

			// Max 25% chance to craft a 2nd extra after getting to skill level 25.
			if (skillLevel > 25f && Random.Range(1, 400) < skillLevel)
			{
				++extraItems;
			}
		}

		if (extraItems > 0)
		{
			ZRoutedRpc.instance.InvokeRoutedRPC(sender, "PotionsPlus Alchemy Proc");
		}

		return extraItems;
	}

	[HarmonyPatch(typeof(Incinerator.IncineratorConversion), nameof(Incinerator.IncineratorConversion.AttemptCraft))]
	private static class ApplyAlchemySkillOnCauldronUsage
	{
		private static void Prefix(List<ItemDrop> toAdd, out int __state) => __state = toAdd.Count;

		private static void Postfix(List<ItemDrop> toAdd, int __state, ref int __result)
		{
			if (SaveCauldronUsingPlayer.uid == 0 || toAdd.Count == __state)
			{
				return;
			}

			for (int i = toAdd.Count - 1; i >= __state; --i)
			{
				SubmitAlchemyLevel.playerAlchemyLevels.TryGetValue(SaveCauldronUsingPlayer.uid, out int skillLevel);

				for (int j = DetermineExtraItems(skillLevel, SaveCauldronUsingPlayer.uid); j > 0; --j)
				{
					toAdd.Add(toAdd[i]);
					++__result;
				}
			}
			if (alchemySkillEnabled.Value == Toggle.On)
			{
				ZRoutedRpc.instance.InvokeRoutedRPC(SaveCauldronUsingPlayer.uid, "PotionsPlus Raise Alchemy", (float)toAdd.Count);
			}
		}
	}

	[HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.DoCrafting))]
	private static class ApplyAlchemySkillOnAlchemyTableCrafting
	{
		private static void Prefix(out int __state) => __state = Game.instance.GetPlayerProfile().m_playerStats.m_crafts;

		private static void Postfix(InventoryGui __instance, Player player, int __state)
		{
			if (Game.instance.GetPlayerProfile().m_playerStats.m_crafts > __state && Player.m_localPlayer.GetCurrentCraftingStation()?.name.StartsWith("opalchemy") == true && __instance.m_craftUpgradeItem is null)
			{
				for (int i = DetermineExtraItems(Mathf.RoundToInt(player.GetSkillFactor("Alchemy") * 100), ZDOMan.instance.GetMyID()); i > 0; --i)
				{
					int quality = __instance.m_craftUpgradeItem != null ? __instance.m_craftUpgradeItem.m_quality + 1 : 1;
					player.GetInventory().AddItem(__instance.m_craftRecipe.m_item.gameObject.name, __instance.m_craftRecipe.m_amount, quality, __instance.m_craftVariant, player.GetPlayerID(), player.GetPlayerName());
				}
				player.RaiseSkill("Alchemy");
			}
		}
	}

	[HarmonyPatch(typeof(Aoe), nameof(Aoe.OnHit))]
	private class HealingBrothEffect
	{
		private static bool Prefix(Collider collider, Vector3 hitPoint, Aoe __instance, ref bool __result)
		{
			if (!__instance.name.StartsWith("Hellbroth_Life_Explosion", StringComparison.Ordinal))
			{
				return true;
			}

			__result = false;

			GameObject hitObject = Projectile.FindHitObject(collider);
			if (__instance.m_hitList.Contains(hitObject))
			{
				return true;
			}
			__instance.m_hitList.Add(hitObject);

			if (hitObject.GetComponent<Character>() is Player player)
			{
				player.Heal(player.GetMaxHealth() * hellbrothOfEternalLifeHealing.Value / 100);
				__result = true;

				__instance.m_hitEffects.Create(hitPoint, Quaternion.identity);
			}

			return false;
		}
	}

	[HarmonyPatch(typeof(RandomSpeak), nameof(RandomSpeak.Start))]
	private class PreventGhostFromTalking
	{
		private static bool Prefix(RandomSpeak __instance)
		{
			return __instance.gameObject.layer != Piece.ghostLayer;
		}
	}

	[HarmonyPatch(typeof(Attack), nameof(Attack.Start))]
	private class BypassAmmoCheck
	{
		private static void Prefix(Attack __instance, ItemDrop.ItemData weapon, ref string? __state)
		{
			if (weapon.m_dropPrefab?.name == "Odins_Dragon_Staff" && __instance.m_spawnOnTrigger?.name == "Staff_Smoke_Cloud")
			{
				__state = weapon.m_shared.m_ammoType;
				weapon.m_shared.m_ammoType = "";
			}
		}

		private static void Finalizer(ItemDrop.ItemData weapon, ref string? __state)
		{
			if (__state is not null)
			{
				weapon.m_shared.m_ammoType = __state;
			}
		}
	}

	[HarmonyPatch(typeof(Attack), nameof(Attack.UseAmmo))]
	private class ReduceAmmoUsage
	{
		private static bool Prefix(Attack __instance, ref bool __result)
		{
			switch (__instance.m_weapon.m_dropPrefab?.name)
			{
				case "Odins_Dragon_Staff" when __instance.m_spawnOnTrigger?.name == "Staff_Smoke_Cloud":
					__result = true;
					return false;
				case "Odins_Alchemy_Wand" or "Odins_Dragon_Staff" when __instance.m_character.m_helmetItem?.m_dropPrefab?.name == "Odins_Wizard_Hat":
				{
					if (__instance.m_character.GetInventory().GetAmmoItem(__instance.m_weapon.m_shared.m_ammoType) is not { } ammoItem)
					{
						return true;
					}

					if (Random.value < wizardHatConsumeChargeReduction.Value / 100f)
					{
						__instance.m_ammoItem = ammoItem;
						__result = true;
						return false;
					}
					break;
				}
			}

			return true;
		}
	}

	public class SmokescreenOwner : MonoBehaviour, IProjectile
	{
		public void Setup(Character owner, Vector3 velocity, float hitNoise, HitData hitData, ItemDrop.ItemData item)
		{
			ZDO zdo = GetComponent<ZNetView>().m_zdo;
			zdo.Set("PotionsPlus SmokeCloud Owner", owner.GetZDOID());
			zdo.Set("PotionsPlus SmokeCloud HatBonus", owner.GetComponent<VisEquipment>().m_currentHelmetItemHash == "Odins_Warlock_Hat".GetStableHashCode());
		}

		public void Start()
		{
			if (GetComponent<ZNetView>().m_zdo.GetBool("PotionsPlus SmokeCloud HatBonus"))
			{
				foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>())
				{
					ParticleSystem.ShapeModule shape = particleSystem.shape;
					shape.radius += warlockHatSmokeScreenSizeIncrease.Value;
				}

				GetComponentInChildren<SphereCollider>().radius += warlockHatSmokeScreenSizeIncrease.Value;

				TimedDestruction destruction = GetComponent<TimedDestruction>();
				destruction.CancelInvoke(nameof(TimedDestruction.DestroyNow));
				destruction.Trigger(smokeScreenTTL.Value + warlockHatSmokeScreenDurationIncrease.Value);
			}
		}

		public string GetTooltipString(int itemQuality) => "";
	}

	[HarmonyPatch(typeof(Projectile), nameof(Projectile.FixedUpdate))]
	private class SmokescreenHitBarrier
	{
		private static int ProjectileBlocker(int mask)
		{
			return mask | 1 << LayerMask.NameToLayer("blocker");
		}

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			FieldInfo maskField = AccessTools.DeclaredField(typeof(Projectile), nameof(Projectile.m_rayMaskSolids));
			foreach (CodeInstruction instruction in instructions)
			{
				yield return instruction;
				if (instruction.opcode == OpCodes.Ldsfld && instruction.OperandIs(maskField))
				{
					yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(SmokescreenHitBarrier), nameof(ProjectileBlocker)));
				}
			}
		}
	}

	[HarmonyPatch(typeof(Projectile), nameof(Projectile.OnHit))]
	private class DestroyBlockedProjectile
	{
		private static bool Prefix(Projectile __instance, Collider collider, ref bool __state)
		{
			if (collider && collider.gameObject.layer == LayerMask.NameToLayer("blocker") && collider.transform.parent?.GetComponent<ZNetView>()?.m_zdo is { } smokescreenZDO)
			{
				bool hitCharacter = false;
				if (ZNetScene.instance.FindInstance(smokescreenZDO.GetZDOID("PotionsPlus SmokeCloud Owner"))?.GetComponent<Character>() is { } smokescreenOwner && !__instance.IsValidTarget(smokescreenOwner, ref hitCharacter))
				{
					return false;
				}

				__state = true;
				Random.InitState((int)__instance.m_nview.m_zdo.m_uid.m_id);
				Random.State state = Random.state;
				bool blockIt = Random.value < (smokeScreenChanceToBlock.Value + (smokescreenZDO.GetBool("PotionsPlus SmokeCloud HatBonus") ? warlockHatSmokeScreenBlockIncrease.Value : 0)) / 100f;
				Random.state = state;

				return blockIt;
			}
			return true;
		}

		[HarmonyPriority(Priority.Low)]
		private static void Postfix(Projectile __instance, Collider collider, bool __state)
		{
			if (__state && __instance.m_didHit && __instance.m_stayAfterHitStatic)
			{
				ZNetScene.instance.Destroy(__instance.gameObject);
			}
		}
	}

	[HarmonyPatch(typeof(Projectile), nameof(Projectile.SpawnOnHit))]
	private class TransferDamageToAoeProjectile
	{
		private static IProjectile UpdateAoeProjectileDamage(IProjectile newProjectile, IProjectile spawning)
		{
			string projectileName = ((Component)spawning).name;
			if (wandProjectiles.Any(p => projectileName.StartsWith(p, StringComparison.Ordinal)))
			{
				Aoe aoe = (Aoe)newProjectile;
				Projectile projectile = (Projectile)spawning;

				aoe.m_damage.Add(projectile.m_damage);
				aoe.m_attackForce += projectile.m_attackForce;
				aoe.m_backstabBonus += projectile.m_backstabBonus;
				aoe.m_statusEffect += projectile.m_statusEffect;
			}
			return newProjectile;
		}

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			MethodInfo projectile = AccessTools.DeclaredMethod(typeof(GameObject), nameof(GameObject.GetComponent), Array.Empty<Type>(), new[] { typeof(IProjectile) });
			foreach (CodeInstruction instruction in instructions)
			{
				yield return instruction;
				if (instruction.opcode == OpCodes.Callvirt && instruction.OperandIs(projectile))
				{
					yield return new CodeInstruction(OpCodes.Ldarg_0);
					yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(TransferDamageToAoeProjectile), nameof(UpdateAoeProjectileDamage)));
				}
			}
		}
	}
}
