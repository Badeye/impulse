﻿using Impulse.Progress;
using Impulse.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Impulse.Levels
{
    public static class LevelLoader
    {
        public static bool IsLoaded { get; private set; }

        public static List<Level> LoadLevels()
        {
            int min = Constants.firstLevel;
            int max = Constants.lastLevel;
            int levelsLoadingLength = max - min + 1;

            List<Level> levelsLoading = new List<Level>();
            for (int i = min; i <= max; i++)
            {
                levelsLoading.Add(LoadLevel(i));
            }
            return levelsLoading;
        }

        public static Level LoadLevel(int id)
        {
            Debug.Log(id);
            Level level = null;
            try
            {
                GameObject go = (GameObject)Resources.Load("Prefabs/Levels/" + id);
                if (go != null)
                {
                    level = go.GetComponent<Level>();
                }
            }
            catch (UnityException e)
            {
                Debug.Log(e);
            }

            if (level == null)
            {
                Debug.Log("LevelLoader: Levelprefab could not be found.");
                return null;
            }

            IsLoaded = true;
            return level;
        }

        //public static int LoadLevels()
        //{
        //    //= new Level[levelsLoadingLength

        //    Level l = null;
        //    int currentHighest = 1;
        //    for (int i = 0; i < Constants.lastLevel; i++)
        //    {
        //        GameObject go = (GameObject)Resources.Load("Prefabs/Levels/" + i);

        //        if (go != null)
        //        {
        //            l = go.GetComponent<Level>();

        //            if (l.id > currentHighest)
        //                currentHighest = l.id;
        //        }
        //    }
        //    return currentHighest;
        //}

        public static void SaveLevel(Level level)
        {
#if UNITY_EDITOR
            Object prefab = UnityEditor.EditorUtility.CreateEmptyPrefab("Assets/Resources/Prefabs/" + level.id + ".prefab");
            UnityEditor.EditorUtility.ReplacePrefab(LevelManager.GetActiveLevel().gameObject, prefab, UnityEditor.ReplacePrefabOptions.ReplaceNameBased);
            //activelevel
#endif
        }
    }
}