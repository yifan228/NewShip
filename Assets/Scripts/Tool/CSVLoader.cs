using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CSVLoader
{
    public static Dictionary<string, T> LoadCSV<T>(string csvText, string rootContextKey) where T : new()
    {
        var dict = new Dictionary<string, T>();

        var lines = csvText.Split('\n');
        var headers = lines[0].Trim().Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            var values = lines[i].Trim().Split(',');
            if (values.Length != headers.Length) continue;

            var row = new Dictionary<string, string>();
            for (int j = 0; j < headers.Length; j++)
                row[headers[j]] = values[j];

            T obj = new T();
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            // 賦值欄位（可支援 string 與 JSON 型別）
            foreach (var field in fields)
            {
                if (!row.TryGetValue(field.Name, out var raw)) continue;

                if (field.FieldType == typeof(string))
                {
                    field.SetValue(obj, raw); // 等下再做模板解析
                }
                else if (field.FieldType.IsValueType || field.FieldType.IsClass)
                {
                    try
                    {
                        object deserialized = JsonUtility.FromJson(raw, field.FieldType);
                        field.SetValue(obj, deserialized);
                    }
                    catch
                    {
                        Debug.LogWarning($"JSON parse failed for field {field.Name} with value {raw}");
                    }
                }
            }

            // 模板字串欄位再解析
            var context = new Dictionary<string, object> { { rootContextKey, obj } };
            foreach (var field in fields)
            {
                if (field.FieldType != typeof(string)) continue;
                string raw = field.GetValue(obj) as string;
                if (string.IsNullOrEmpty(raw)) continue;

                string parsed = TemplateResolver.Parse(raw, context);
                field.SetValue(obj, parsed);
            }

            // 取得 KeyString 作為字典鍵
            var keyField = type.GetField("KeyString");
            if (keyField == null) throw new Exception("需要 KeyString 欄位作為 Dictionary Key");
            string key = keyField.GetValue(obj) as string;

            dict[key] = obj;
        }

        return dict;
    }
}

public static class TemplateResolver
{
    public static string Parse(string template, Dictionary<string, object> context)
    {
        return Regex.Replace(template, @"\$\{([a-zA-Z0-9_.]+)\}", match =>
        {
            var expr = match.Groups[1].Value;
            var parts = expr.Split('.', 2);
            if (parts.Length != 2) return match.Value;

            if (context.TryGetValue(parts[0], out var root))
            {
                var obj = root;
                foreach (var prop in parts[1].Split('.'))
                {
                    var pi = obj.GetType().GetProperty(prop);
                    obj = pi?.GetValue(obj);
                    if (obj == null) return match.Value;
                }
                return obj.ToString();
            }

            return match.Value;
        });
    }
}
