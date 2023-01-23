using System;
using System.Linq;
using System.Reflection;
using NavySpade.Modules.Configuration.Runtime.SO;
using NavySpade.Modules.Configuration.Runtime.Utils;
using QFSW.QC;
using UnityEngine;

namespace NavySpade.Commands.Configuration
{
    [CommandPrefix("cfg.")]
    public static class ObjectConfigConsoleCommands
    {
        private const BindingFlags FieldsFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private static string RegexField(string input)
        {
            input = input.ToLower();
            var splitted = input.Split('<', '>');

            return splitted.Length > 1 ? splitted[1] : input.Replace("_", "");
        }

        private static FieldInfo[] GetSerializableFields(Type type)
        {
            var allFields = type.GetFields(FieldsFlags);

            foreach (var f in allFields)
            {
                if (f.IsStatic == false && (f.GetCustomAttribute(typeof(SerializeField)) != null || f.IsPublic))
                {
                }
            }

            var fields = allFields.Where(f =>
                f.IsStatic == false && (f.GetCustomAttribute(typeof(SerializeField)) != null || f.IsPublic));

            return fields.ToArray();
        }

        [Command("printall")]
        public static void PrintAllConfigs()
        {
            var allConfigs = ObjectConfigLocator.GetAllConfigs();
            for (var i = 0; i < allConfigs.Count; i++)
            {
                Debug.Log($"{i} : {allConfigs[i].name.ToLower()}");
            }
        }

        [Command("members.printall")]
        public static void PrintConfigMembers(ObjectConfig config)
        {
            var type = config.GetType();
            var fields = GetSerializableFields(type);

            Debug.Log("fields: ");
            for (var i = 0; i < fields.Length; i++)
            {
                Debug.Log($"{i} : {RegexField(fields[i].Name)} {RegexField(fields[i].FieldType.Name)}");
            }
        }

        [Command("members.get")]
        public static object PrintMemberValue(ObjectConfig config, string member)
        {
            var fieldInfo = int.TryParse(member, out var result)
                ? GetSerializableFields(config.GetType())[result]
                : GetSerializableFields(config.GetType()).First(f => RegexField(f.Name) == RegexField(member));

            var obj = fieldInfo.GetValue(config);
            Debug.Log(obj);

            return obj;
        }

        [Command("members.set.int")]
        public static void SetBoolMemberValue(ObjectConfig config, string member, int value)
        {
            SetMemberValue<object>(config, member, value);
        }

        [Command("members.set.bool")]
        public static void SetBoolMemberValue(ObjectConfig config, string member, bool value)
        {
            SetMemberValue(config, member, value);
        }

        [Command("members.set")]
        private static void SetMemberValue<T>(ObjectConfig config, string member, T value)
        {
            var fieldInfo = int.TryParse(member, out var result)
                ? GetSerializableFields(config.GetType())[result]
                : GetSerializableFields(config.GetType()).First(f => f.Name.ToLower() == member.ToLower());

            fieldInfo.SetValue(config, value);
        }

        public class ConfigParser : BasicCachedQcParser<ObjectConfig>
        {
            public override ObjectConfig Parse(string value)
            {
                if (int.TryParse(value, out var result))
                {
                    return ObjectConfigLocator.GetAllConfigs()[result];
                }

                var configs = ObjectConfigLocator.GetAllConfigs();
                return configs.First(cfg => cfg.name.ToLower() == value.ToLower());
            }
        }
    }
}