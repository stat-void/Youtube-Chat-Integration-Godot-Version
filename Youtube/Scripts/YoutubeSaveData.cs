using System;
using System.IO;
using SimpleJSON;
using Godot;

namespace Void.YoutubeAPI
{
    /// <summary>
    /// Class to recall the location where data should be stored.
    /// Note that currently it's only designed to properly address Windows desktop builds.
    /// </summary>
    public static class YoutubeSaveData
    {
        private static string basePath = "";
        private const string _dataName = "YT_API_Data.json";
        private static JSONNode _data;

        private static bool _initialized = false;

        public static JSONNode GetData()
        {
            if (!_initialized)
                Initialize();
            return _data;
        }

        public static void SaveData()
        {
            if (!_initialized)
                return;

            if (File.Exists(basePath + $"\\{_dataName}"))
                File.WriteAllText(basePath + $"\\{_dataName}", _data.ToString());
        }

        public static void DeleteData()
        {
            if (!_initialized)
                return;

            if (File.Exists(basePath + $"\\{_dataName}"))
                File.Delete(basePath + $"\\{_dataName}");
        }


        private static void Initialize()
        {
            try
            {
                GD.Print("Youtube data path initialization started.");
                SetDirectory(AppContext.BaseDirectory);
            }
            catch (UnauthorizedAccessException e)
            {
                GD.PrintErr($"Unable to create or load JSON data file - {e.Message}");
                //GD.Print($"Using Unity persistent data path.");
                //SetDirectory(Application.persistentDataPath);
                return;
            }

            _initialized = true;

            GD.Print($"Save path set as {basePath + $"\\{_dataName}"}");
        }

        private static void SetDirectory(string baseDirectory)
        {
            if (!File.Exists(baseDirectory + $"\\{_dataName}"))
            {
                _data = JSON.Parse("{}");
                File.WriteAllText(baseDirectory + $"\\{_dataName}", _data.ToString());
            }
            else
            {
                string text = File.ReadAllText(baseDirectory + $"\\{_dataName}");
                _data = JSONNode.Parse(text);
            }

            basePath = baseDirectory;
        }
    }
}

