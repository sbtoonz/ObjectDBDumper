using System.Collections.Generic;
using System.IO;
using System.Text;
using BepInEx;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ObjectDBDumper
{
    public class YML_Format
    {
        public static string Serializers(ItemDropStruct data)
        {
            var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var yml = serializer.Serialize(data);
            return yml;
        }
        
        public static ItemDropStruct ReadSerializedData(string s)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var tmp = deserializer.Deserialize<ItemDropStruct>(s);
            return tmp;
        }
        
        public static void WriteSerializedData(string data)
        {
            if (File.ReadAllText(Paths.ConfigPath + "/itemdump.yml", Encoding.Default).Length == data.Length)
            {
                return;
            }
            if(File.ReadAllText(Paths.ConfigPath + "/itemdump.yml", Encoding.Default).Length > 0)
            {
                File.AppendAllText(Paths.ConfigPath + "/itemdump.yml", data);
            }
            else
            {
                File.WriteAllText(Paths.ConfigPath + "/itemdump.yml", data);
            }

        }
    }

    public struct ItemDropStruct
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string m_sharedName;
        [YamlMember(Alias = "DropInfo", ApplyNamingConventions = false)]
        public ItemDrop_Info DropInfo;
    }

    public struct ItemDrop_Info
    {
        [YamlMember(Alias = "stack", ApplyNamingConventions = false)]
        public int m_stack;
        [YamlMember(Alias = "durability", ApplyNamingConventions = false)]
        public float m_durability;
        [YamlMember(Alias = "qualiy", ApplyNamingConventions = false)]
        public int m_quality;
        [YamlMember(Alias = "variant", ApplyNamingConventions = false)]
        public int m_variant;
        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string m_sharedName;
        [YamlMember(Alias = "DLC", ApplyNamingConventions = false)]
        public string m_DLC;
        [YamlMember(Alias = "description", ApplyNamingConventions = false)]
        public string m_description;
        [YamlMember(Alias = "max stack", ApplyNamingConventions = false)]
        public int m_maxStack;
        [YamlMember(Alias = "max quality", ApplyNamingConventions = false)]
        public int m_maxQuality;
        [YamlMember(Alias = "weight", ApplyNamingConventions = false)]
        public float m_weight;
        [YamlMember(Alias = "value", ApplyNamingConventions = false)]
        public int m_value;
        [YamlMember(Alias = "teleportable", ApplyNamingConventions = false)]
        public bool m_teleportable;
        [YamlMember(Alias = "food", ApplyNamingConventions = false)]
        public float m_food;
        [YamlMember(Alias = "food stamina", ApplyNamingConventions = false)]
        public float m_foodStamina;
        [YamlMember(Alias = "food burn", ApplyNamingConventions = false)]
        public float m_foodBurnTime;
        [YamlMember(Alias = "food regen time", ApplyNamingConventions = false)]
        public float m_foodRegen;
        [YamlMember(Alias = "armor", ApplyNamingConventions = false)]
        public float m_armor;
        [YamlMember(Alias = "armor per level", ApplyNamingConventions = false)]
        public float m_armorPerLevel;
        [YamlMember(Alias = "block power", ApplyNamingConventions = false)]
        public float m_blockPower;
        [YamlMember(Alias = "block power per level", ApplyNamingConventions = false)]
        public float m_blockPowerPerLevel;
        [YamlMember(Alias = "deflection force", ApplyNamingConventions = false)]
        public float m_deflectionForce;
        [YamlMember(Alias = "deflection force per level", ApplyNamingConventions = false)]
        public float m_deflectionForcePerLevel;
        [YamlMember(Alias = "timed block bonus", ApplyNamingConventions = false)]
        public float m_timedBlockBonus;
        [YamlMember(Alias = "tool tier", ApplyNamingConventions = false)]
        public int m_toolTier;
        [YamlMember(Alias = "attack force", ApplyNamingConventions = false)]
        public float m_attackForce;
        [YamlMember(Alias = "backstab bonus", ApplyNamingConventions = false)]
        public float m_backstabBonus;
        [YamlMember(Alias = "dodgeable", ApplyNamingConventions = false)]
        public bool m_dodgeable;
        [YamlMember(Alias = "blockable", ApplyNamingConventions = false)]
        public bool m_blockable;
        [YamlMember(Alias = "use durability", ApplyNamingConventions = false)]
        public bool m_useDurability;
        [YamlMember(Alias = "destroy broken", ApplyNamingConventions = false)]
        public bool m_destroyBroken;
        [YamlMember(Alias = "can be repaired", ApplyNamingConventions = false)]
        public bool m_canBeReparied;
        [YamlMember(Alias = "max durability", ApplyNamingConventions = false)]
        public float m_maxDurability;
        [YamlMember(Alias = "durability per level", ApplyNamingConventions = false)]
        public float m_durabilityPerLevel;
        [YamlMember(Alias = " use durability drain", ApplyNamingConventions = false)]
        public float m_useDurabilityDrain;
        [YamlMember(Alias = "durability drain", ApplyNamingConventions = false)]
        public float m_durabilityDrain;
        [YamlMember(Alias = "ammo type", ApplyNamingConventions = false)]
        public string m_ammoType;
    }
}