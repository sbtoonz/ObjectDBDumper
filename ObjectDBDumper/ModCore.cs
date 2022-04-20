using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ServerSync;

namespace ObjectDBDumper
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class ObjectDBDumper : BaseUnityPlugin
    {
        private const string ModName = "ObjectDB Dumper";
        private const string ModVersion = "1.0";
        private const string ModGUID = "odinplus.objectdb.dumper";
        private static Harmony harmony = null!;
        ConfigSync configSync = new(ModGUID) 
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion};
        internal static ConfigEntry<bool> ServerConfigLocked = null!;
        ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
        ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        public void Awake()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            harmony = new(ModGUID);
            harmony.PatchAll(assembly);
            ServerConfigLocked = config("1 - General", "Lock Configuration", true, "If on, the configuration is locked and can be changed by server admins only.");
            configSync.AddLockingConfigEntry(ServerConfigLocked);
        }


        [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.Awake))]
        public static class DumpDbPatch
        {
            private static void Postfix(ObjectDB __instance)
            {
                if (__instance.m_items.Count <= 0 || __instance.GetItemPrefab("Amber") == null) return;
                if (!File.Exists(Paths.ConfigPath + "/itemdump.yml"))
                {
                    var file = File.Create(Paths.ConfigPath + "/itemdump.yml");
                    file.Close();
                }
                List<ItemDropStruct> newdropinfo = new List<ItemDropStruct>();
                var items = ObjectDB.instance.m_items;
                foreach (var go in items.Where(go => go.GetComponent<ItemDrop>().m_itemData.m_shared.m_icons.Length >= 1))
                {
                    ItemDrop drop = go.GetComponent<ItemDrop>();
                    ItemDropStruct dropInfo = new ItemDropStruct();

                    dropInfo.DropInfo.m_stack = drop.m_itemData.m_stack;
                    dropInfo.DropInfo.m_durability = drop.m_itemData.m_durability;
                    dropInfo.DropInfo.m_quality = drop.m_itemData.m_quality;
                    dropInfo.DropInfo.m_variant = drop.m_itemData.m_variant;
                    dropInfo.m_sharedName = Localization.instance.Localize(drop.m_itemData.m_shared.m_name);
                    dropInfo.DropInfo.m_sharedName = Localization.instance.Localize(drop.m_itemData.m_shared.m_name);
                    dropInfo.DropInfo.m_DLC = drop.m_itemData.m_shared.m_dlc;
                    dropInfo.DropInfo.m_description = drop.m_itemData.m_shared.m_description;
                    dropInfo.DropInfo.m_maxStack = drop.m_itemData.m_shared.m_maxStackSize;
                    dropInfo.DropInfo.m_maxQuality = drop.m_itemData.m_shared.m_maxQuality;
                    dropInfo.DropInfo.m_weight = drop.m_itemData.m_shared.m_weight;
                    dropInfo.DropInfo.m_value = drop.m_itemData.m_shared.m_value;
                    dropInfo.DropInfo.m_teleportable = drop.m_itemData.m_shared.m_teleportable;
                    dropInfo.DropInfo.m_food = drop.m_itemData.m_shared.m_food;
                    dropInfo.DropInfo.m_foodStamina = drop.m_itemData.m_shared.m_foodStamina;
                    dropInfo.DropInfo.m_foodBurnTime = drop.m_itemData.m_shared.m_foodBurnTime;
                    dropInfo.DropInfo.m_foodRegen = drop.m_itemData.m_shared.m_foodRegen;
                    dropInfo.DropInfo.m_armor = drop.m_itemData.m_shared.m_armor;
                    dropInfo.DropInfo.m_armorPerLevel = drop.m_itemData.m_shared.m_armorPerLevel;
                    dropInfo.DropInfo.m_blockPower = drop.m_itemData.m_shared.m_blockPower;
                    dropInfo.DropInfo.m_blockPowerPerLevel = drop.m_itemData.m_shared.m_blockPowerPerLevel;
                    dropInfo.DropInfo.m_deflectionForce = drop.m_itemData.m_shared.m_deflectionForce;
                    dropInfo.DropInfo.m_deflectionForcePerLevel = drop.m_itemData.m_shared.m_deflectionForcePerLevel;
                    dropInfo.DropInfo.m_timedBlockBonus = drop.m_itemData.m_shared.m_timedBlockBonus;
                    dropInfo.DropInfo.m_toolTier = drop.m_itemData.m_shared.m_toolTier;
                    dropInfo.DropInfo.m_attackForce = drop.m_itemData.m_shared.m_attackForce;
                    dropInfo.DropInfo.m_backstabBonus = drop.m_itemData.m_shared.m_backstabBonus;
                    dropInfo.DropInfo.m_dodgeable = drop.m_itemData.m_shared.m_dodgeable;
                    dropInfo.DropInfo.m_blockable = drop.m_itemData.m_shared.m_blockable;
                    dropInfo.DropInfo.m_useDurability = drop.m_itemData.m_shared.m_useDurability;
                    dropInfo.DropInfo.m_destroyBroken = drop.m_itemData.m_shared.m_destroyBroken;
                    dropInfo.DropInfo.m_canBeReparied = drop.m_itemData.m_shared.m_canBeReparied;
                    dropInfo.DropInfo.m_maxDurability = drop.m_itemData.m_shared.m_maxDurability;
                    dropInfo.DropInfo.m_durabilityPerLevel = drop.m_itemData.m_shared.m_durabilityPerLevel;
                    dropInfo.DropInfo.m_useDurability = drop.m_itemData.m_shared.m_useDurability;
                    dropInfo.DropInfo.m_durabilityDrain = drop.m_itemData.m_shared.m_durabilityDrain;
                    dropInfo.DropInfo.m_ammoType = drop.m_itemData.m_shared.m_ammoType;
                    newdropinfo.Add(dropInfo);
                }

                foreach (var IdropInfo in newdropinfo)
                {
                    var s = YML_Format.Serializers(IdropInfo);
                    YML_Format.WriteSerializedData(s);
                }
            }
        }
    }
}
