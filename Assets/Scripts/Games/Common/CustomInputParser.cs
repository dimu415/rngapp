using System.Collections.Generic;
using UnityEngine;

public static class CustomInputParser
{
    public static List<string> BuildItems(string rawInput, int count, string defaultPrefix)
    {
        List<string> items = new List<string>();
        if (!string.IsNullOrWhiteSpace(rawInput))
        {
            string[] tokens = rawInput.Split(new[] { '\n', '\r', ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                string trimmed = token.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    items.Add(trimmed);
                }
            }
        }

        count = Mathf.Max(2, count);

        for (int i = items.Count; i < count; i++)
        {
            items.Add($"{defaultPrefix} {i + 1}");
        }

        if (items.Count > count)
        {
            items.RemoveRange(count, items.Count - count);
        }

        return items;
    }
}
