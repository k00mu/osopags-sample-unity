using UnityEngine;
using UnityEditor;
using Osopags.Models;
using Osopags.Core;
using System.Threading.Tasks;
using System;

namespace Osopags.Editor
{
    public class OsopagsSettingsEditor : EditorWindow
    {
        private OsopagsSettings settings;
        private Vector2 scrollPosition;
        private readonly string[] environments = { "Development", "Production" };

        private bool isTestingConnection = false;

        [MenuItem("Osopags/Settings")]
        public static void ShowWindow()
        {
            GetWindow<OsopagsSettingsEditor>("Osopags Settings");
        }

        private void OnEnable()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            settings = Resources.Load<OsopagsSettings>("OsopagsSettings");
            if (settings == null)
            {
                settings = OsopagsSettings.CreateDefaultSettings();

                var path = "Assets/Resources";
                if (!AssetDatabase.IsValidFolder(path))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                AssetDatabase.CreateAsset(settings, "Assets/Resources/OsopagsSettings.asset");
                AssetDatabase.SaveAssets();
            }
        }

        private void OnGUI()
        {
            if (settings == null)
            {
                LoadSettings();
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Osopags Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            // Environment selector
            int currentEnvIndex = System.Array.IndexOf(environments, settings.CurrentEnvironment);
            int newEnvIndex = EditorGUILayout.Popup("Current Environment", currentEnvIndex, environments);
            if (currentEnvIndex != newEnvIndex)
            {
                settings.CurrentEnvironment = environments[newEnvIndex];
                EditorUtility.SetDirty(settings);
            }

            EditorGUILayout.Space(10);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawEnvironmentSettings("Development", settings.Config.Development);
            EditorGUILayout.Space(20);
            DrawEnvironmentSettings("Production", settings.Config.Production);

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Validate Settings"))
            {
                ValidateAllSettings();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }
        }

        private void DrawEnvironmentSettings(string label, OsopagsConfig config)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            config.BaseUrl = EditorGUILayout.TextField("Base URL", config.BaseUrl);
            config.ClientId = EditorGUILayout.TextField("Client ID", config.ClientId);

            config.GameNamespace = EditorGUILayout.TextField("Game Namespace", config.GameNamespace);
            config.EnableDebugLog = EditorGUILayout.Toggle("Enable Debug Log", config.EnableDebugLog);

            EditorGUI.indentLevel--;

            // Test Connection button
            using (new EditorGUI.DisabledGroupScope(isTestingConnection))
            {
                if (GUILayout.Button("Test Connection"))
                {
                    TestConnectionHandler(config);
                }
            }

            if (isTestingConnection)
            {
                EditorGUILayout.LabelField("Testing connection...", EditorStyles.miniLabel);
            }

            // Show warning if this is the current environment and validation fails
            if (label == settings.CurrentEnvironment && !ValidateConfig(config))
            {
                EditorGUILayout.HelpBox("Current environment configuration is incomplete!", MessageType.Warning);
            }

            EditorGUILayout.EndVertical();
        }

        private void TestConnectionHandler(OsopagsConfig config)
        {
            if (!ValidateConfig(config))
            {
                EditorUtility.DisplayDialog("Test Connection", "Please fill in all required fields.", "OK");
                return;
            }

            isTestingConnection = true;
            Repaint();

            OsopagsSDK.Instance.IAM.TestConnection(config,
                result =>
                {
                    isTestingConnection = false;
                    EditorUtility.DisplayDialog("Test Connection", "Connection successful!", "OK");
                    Repaint();
                },
                error =>
                {
                    isTestingConnection = false;
                    EditorUtility.DisplayDialog("Test Connection", $"Connection failed: {error.message}", "OK");
                    Repaint();
                }
            );
        }

        private bool ValidateConfig(OsopagsConfig config)
        {
            return !string.IsNullOrEmpty(config.BaseUrl) &&
                   !string.IsNullOrEmpty(config.ClientId) &&
                   !string.IsNullOrEmpty(config.GameNamespace);
        }

        private void ValidateAllSettings()
        {
            bool isValid = true;
            string messages = "Validation Results:\n\n";

            void ValidateEnvironment(string env, OsopagsConfig config)
            {
                messages += $"{env} Environment:\n";

                if (string.IsNullOrEmpty(config.BaseUrl))
                {
                    messages += "- Base URL is missing\n";
                    isValid = false;
                }
                else if (!config.BaseUrl.StartsWith("http://") && !config.BaseUrl.StartsWith("https://"))
                {
                    messages += "- Base URL should start with 'http://' or 'https://'\n";
                    isValid = false;
                }

                if (string.IsNullOrEmpty(config.ClientId))
                {
                    messages += "- Client ID is missing\n";
                    isValid = false;
                }

                if (string.IsNullOrEmpty(config.GameNamespace))
                {
                    messages += "- Game Namespace is missing\n";
                    isValid = false;
                }

                messages += "\n";
            }

            // Only validate the currently selected environment
            switch (settings.CurrentEnvironment)
            {
                case "Development":
                    ValidateEnvironment("Development", settings.Config.Development);
                    break;
                case "Production":
                    ValidateEnvironment("Production", settings.Config.Production);
                    break;
            }

            if (isValid)
            {
                if (EditorUtility.DisplayDialog("Settings Validation",
                    $"{settings.CurrentEnvironment} settings are valid. Would you like to test the connection to the current environment?",
                    "Test Connection", "Close"))
                {
                    TestConnectionHandler(settings.GetCurrentConfig());
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Settings Validation", messages, "Close");
            }
        }
    }
}