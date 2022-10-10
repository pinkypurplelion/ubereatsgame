using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// A helper class for saving and loading JSON files within the file system.
    /// From: https://github.com/UnityTechnologies/UniteNow20-Persistent-Data/blob/main/FileManager.cs
    /// </summary>
    /// <author>bzgeb</author>
    /// <modifier>Liam Angus</modifier>
    public static class FileManager
    {
        /// <summary>
        /// Will write the given string to a file at the given path.
        /// </summary>
        /// <param name="aFileName">The file name</param>
        /// <param name="aFileContents">The file data</param>
        /// <returns>True if the file wrote successfully, False otherwise</returns>
        public static bool WriteToFile(string aFileName, string aFileContents)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, aFileName);
            Debug.Log("Writing to file: " + fullPath);
            try
            {
                File.WriteAllText(fullPath, aFileContents);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {fullPath} with exception {e}");
                return false;
            }
        }

        /// <summary>
        /// Will load data from the given file
        /// </summary>
        /// <param name="aFileName">The location of the file</param>
        /// <param name="result">The file data</param>
        /// <returns>True if the file was read, False otherwise</returns>
        public static bool LoadFromFile(string aFileName, out string result)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, aFileName);
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
        
        /// <summary>
        /// Will save the given JSON data to the provided file.
        /// </summary>
        /// <param name="filename">The name of the file to be saved</param>
        /// <param name="jsonData">The JSON data to be saved</param>
        public static void SaveData(string filename, string jsonData)
        {
            Debug.Log("Attempting to Save Data...");
            Debug.Log($"Save Data: {jsonData}");
            WriteToFile(filename, jsonData);
            Debug.Log("Data Saved!");
        }

        /// <summary>
        /// Will load the JSON data from the provided file into the given object.
        /// </summary>
        /// <param name="filename">The location of the JSON data to be loaded</param>
        /// <param name="defaultData">Default data to be returned if the given file does not exist</param>
        /// <typeparam name="T">The type of the object to be returned by parsing JSON data</typeparam>
        /// <returns>An object of given type that contains the JSON data</returns>
        public static T LoadData<T>(string filename, T defaultData) where T : new()
        {
            if (LoadFromFile(filename, out var json))
            {
                Debug.Log($"Load complete: {json}");
                return JsonConvert.DeserializeObject<T>(json);
            }
            Debug.Log("Load failed");
            Debug.Log("Returning default data");
            return defaultData;
        }
    }
}