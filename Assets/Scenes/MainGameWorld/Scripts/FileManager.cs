/*
 * Author: bzgeb
 *
 * From: https://github.com/UnityTechnologies/UniteNow20-Persistent-Data/blob/main/FileManager.cs
 *
 * Adapted By: Liam Angus
 */

using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    public class FileManager
    {
        public static bool WriteToFile(string a_FileName, string a_FileContents)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);
            Debug.Log("Writing to file: " + fullPath);
            try
            {
                File.WriteAllText(fullPath, a_FileContents);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {fullPath} with exception {e}");
                return false;
            }
        }

        public static bool LoadFromFile(string a_FileName, out string result)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);
            Debug.Log("Loading from " + fullPath);
            try
            {
                result = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read from {fullPath} with exception {e}");
                result = "";
                return false;
            }
        }
        
        public static void SaveData(String filename, String jsonData)
        {
            Debug.Log("Attempting to Save Data...");
            Debug.Log($"Save Data: {jsonData}");
            WriteToFile(filename, jsonData);
            Debug.Log("Data Saved!");
        }

        public static T LoadData<T>(String filename, T defaultData) where T : new()
        {
            if (FileManager.LoadFromFile(filename, out var json))
            {
                Debug.Log("Load complete");
                return JsonConvert.DeserializeObject<T>(json);
            }
            Debug.Log("Load failed");
            Debug.Log("Returning default data");
            return defaultData;
        }
    }
}