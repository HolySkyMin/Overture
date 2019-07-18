using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    public static List<Dictionary<string, object>> Read(string path)
    {
        var asset = Resources.Load<TextAsset>(path);
        Debug.Log(asset.text);
        var rows = Regex.Split(asset.text, @"\r\n|\n\r|\n|\r");
        if (rows.Length < 1)
            return new List<Dictionary<string, object>>();

        var keys = Regex.Split(rows[0], @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))");
        var list = new List<Dictionary<string, object>>();
        for(int i = 1; i < rows.Length; i++)
        {
            var elements = Regex.Split(rows[i], @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))");
            if (elements.Length < 1 || elements[0] == "")
                continue;

            var item = new Dictionary<string, object>();
            for(int j = 0; j < elements.Length; j++)
            {
                var value = elements[j].TrimStart('\"').TrimEnd('\"').Replace("\\", "");
                if (int.TryParse(value, out int n))
                    item.Add(keys[j], n);
                else if (float.TryParse(value, out float f))
                    item.Add(keys[j], f);
                else
                    item.Add(keys[j], value);
            }
            list.Add(item);
        }
        return list;
    }
}
