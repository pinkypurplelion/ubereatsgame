﻿/*
 * Author: bzgeb
 *
 * From: https://github.com/UnityTechnologies/UniteNow20-Persistent-Data/blob/main/FileManager.cs
 */

using System;
using System.IO;
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
    }
}