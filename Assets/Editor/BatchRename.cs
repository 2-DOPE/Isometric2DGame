
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

public class BatchRenameTool : EditorWindow
{
    private enum RenameMode
    {
        FullName,
        FindAndReplace,
        PrefixSuffix,
        Wildcards,
        CustomPattern
    }

    private enum PrefixSuffixAction { Add, Replace }
    private enum PrefixSuffixType { Prefix, Suffix }

    private RenameMode mode = RenameMode.CustomPattern;

    // Standard modes
    private string newName = "";
    private string findText = "";
    private string replaceText = "";

    private PrefixSuffixAction prefixAction;
    private PrefixSuffixType prefixType;
    private string oldAffix = "";
    private string newAffix = "";

    private string wildcardPattern = "*";
    private string wildcardReplacement = "";

    // Custom pattern
    private string customPattern = "$name_$number:3";
    private string separator = "_";
    private int startNumber = 1;
    private int increment = 1;

    private Vector2 scroll;

    private class RenameItem
    {
        public UnityEngine.Object asset;
        public string path;
        public string oldName;
        public string newName;
        public bool isFolder;
    }

    private List<RenameItem> preview = new List<RenameItem>();

    [MenuItem("Tools/Batch Rename")]
    public static void OpenWindow()
    {
        GetWindow<BatchRenameTool>("Batch Rename");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField("BATCH RENAME", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "Select assets in the Project window, configure " +
            "your rename pattern, preview and apply.",
            MessageType.Info
        );

        EditorGUILayout.Space(8);

        mode = (RenameMode)EditorGUILayout.EnumPopup(
            "Rename Mode", mode
        );

        EditorGUILayout.Space(8);

        DrawSettings();

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Refresh Preview", GUILayout.Height(28)))
            RefreshPreview();

        EditorGUILayout.Space(8);

        DrawPreview();

        EditorGUILayout.Space(8);

        bool hasChanges = preview.Any(x =>
            x.oldName != x.newName &&
            !string.IsNullOrWhiteSpace(x.newName));

        GUI.enabled = hasChanges;

        if (GUILayout.Button("APPLY RENAME", GUILayout.Height(38)))
            ApplyRename();

        GUI.enabled = true;
    }

    private void DrawSettings()
    {
        switch (mode)
        {
            case RenameMode.FullName:

                newName = EditorGUILayout.TextField("New Name", newName);
                break;

            case RenameMode.FindAndReplace:

                findText = EditorGUILayout.TextField("Find", findText);
                replaceText = EditorGUILayout.TextField("Replace With", replaceText);
                break;

            case RenameMode.PrefixSuffix:

                prefixType = (PrefixSuffixType)EditorGUILayout.EnumPopup(
                    "Type", prefixType);

                prefixAction = (PrefixSuffixAction)EditorGUILayout.EnumPopup(
                    "Action", prefixAction);

                if (prefixAction == PrefixSuffixAction.Replace)
                    oldAffix = EditorGUILayout.TextField("Existing Text", oldAffix);

                newAffix = EditorGUILayout.TextField("New Text", newAffix);
                break;

            case RenameMode.Wildcards:

                wildcardPattern = EditorGUILayout.TextField(
                    "Pattern", wildcardPattern);

                wildcardReplacement = EditorGUILayout.TextField(
                    "Replacement", wildcardReplacement);
                break;

            case RenameMode.CustomPattern:

                DrawCustomPatternSettings();
                break;
        }
    }

    private void DrawCustomPatternSettings()
    {
        EditorGUILayout.LabelField("Naming Pattern", EditorStyles.boldLabel);

        customPattern = EditorGUILayout.TextField(
            "Pattern", customPattern);

        separator = EditorGUILayout.TextField(
            "Separator", separator);

        EditorGUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        startNumber = EditorGUILayout.IntField(
            "Start Number", startNumber);

        increment = EditorGUILayout.IntField(
            "Increment", increment);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox(
            "Available tokens:\n" +
            "$name = Original asset name\n" +
            "$number = Sequential number\n" +
            "$number:5 = Number padded to 5 digits\n" +
            "$separator = Custom separator\n\n" +
            "Example: ENV$separator$name$separator$number:3",
            MessageType.Info
        );

        if (GUILayout.Button("Insert $number:3"))
        {
            customPattern += "$number:3";
        }

        if (GUILayout.Button("Insert $separator"))
        {
            customPattern += "$separator";
        }
    }

    private void RefreshPreview()
    {
        preview.Clear();

        var selectedAssets = Selection.objects;

        int number = startNumber;

        foreach (var asset in selectedAssets)
        {
            if (asset == null)
                continue;

            string path = AssetDatabase.GetAssetPath(asset);

            if (string.IsNullOrEmpty(path) ||
                !path.StartsWith("Assets/"))
                continue;

            bool isFolder = AssetDatabase.IsValidFolder(path);

            string originalName = isFolder
                ? Path.GetFileName(path)
                : Path.GetFileNameWithoutExtension(path);

            string result = GetNewName(originalName, number);

            preview.Add(new RenameItem
            {
                asset = asset,
                path = path,
                oldName = originalName,
                newName = result,
                isFolder = isFolder
            });

            number += increment;
        }

        Repaint();
    }

    private string GetNewName(string original, int number)
    {
        switch (mode)
        {
            case RenameMode.FullName:
                return newName;

            case RenameMode.FindAndReplace:
                if (string.IsNullOrEmpty(findText))
                    return original;

                return original.Replace(findText, replaceText);

            case RenameMode.PrefixSuffix:

                if (prefixAction == PrefixSuffixAction.Add)
                {
                    return prefixType == PrefixSuffixType.Prefix
                        ? newAffix + original
                        : original + newAffix;
                }

                if (string.IsNullOrEmpty(oldAffix))
                    return original;

                if (prefixType == PrefixSuffixType.Prefix)
                {
                    return original.StartsWith(oldAffix)
                        ? newAffix + original.Substring(oldAffix.Length)
                        : original;
                }

                return original.EndsWith(oldAffix)
                    ? original.Substring(0,
                        original.Length - oldAffix.Length) + newAffix
                    : original;

            case RenameMode.Wildcards:

                try
                {
                    string regex = "^" +
                        Regex.Escape(wildcardPattern)
                            .Replace("\\*", "(.*)")
                            .Replace("\\?", "(.)") + "$";

                    return Regex.Replace(
                        original, regex, wildcardReplacement);
                }
                catch
                {
                    return original;
                }

            case RenameMode.CustomPattern:

                return ApplyPattern(original, number);
        }

        return original;
    }

    private string ApplyPattern(string original, int number)
    {
        string result = customPattern;

        result = result.Replace("$name", original);
        result = result.Replace("$separator", separator);

        result = Regex.Replace(
            result,
            @"\$number(?::(\d+))?",
            match =>
            {
                int digits = 0;

                if (match.Groups[1].Success)
                    int.TryParse(match.Groups[1].Value, out digits);

                string value = number.ToString();

                return digits > 0
                    ? value.PadLeft(digits, '0')
                    : value;
            }
        );

        return result;
    }

    private void DrawPreview()
    {
        EditorGUILayout.LabelField(
            "PREVIEW (" + preview.Count + " assets)",
            EditorStyles.boldLabel
        );

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Original", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("New Name", EditorStyles.boldLabel);

        EditorGUILayout.EndHorizontal();

        scroll = EditorGUILayout.BeginScrollView(
            scroll, GUILayout.Height(240));

        foreach (var item in preview)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(
                item.oldName, GUILayout.MinWidth(100));

            EditorGUILayout.LabelField("→", GUILayout.Width(18));

            GUI.color = item.oldName == item.newName
                ? Color.gray
                : Color.green;

            EditorGUILayout.LabelField(
                item.newName, GUILayout.MinWidth(100));

            GUI.color = Color.white;

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }

    private void ApplyRename()
    {
        if (preview.Count == 0)
            return;

        if (!EditorUtility.DisplayDialog(
            "Confirm Batch Rename",
            "Rename " + preview.Count + " selected assets?",
            "Apply",
            "Cancel"))
            return;

        int success = 0;
        int failed = 0;

        AssetDatabase.StartAssetEditing();

        try
        {
            foreach (var item in preview)
            {
                if (item.oldName == item.newName ||
                    string.IsNullOrWhiteSpace(item.newName))
                    continue;

                string error = AssetDatabase.RenameAsset(
                    item.path, item.newName);

                if (string.IsNullOrEmpty(error))
                    success++;
                else
                {
                    failed++;
                    Debug.LogWarning(
                        "Could not rename " + item.path + ": " + error);
                }
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }

        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "Batch Rename Complete",
            "Successfully renamed: " + success +
            "\nFailed: " + failed,
            "OK"
        );

        RefreshPreview();
    }
}
