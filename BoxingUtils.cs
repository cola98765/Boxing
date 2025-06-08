using Il2Cpp;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Boxing
{
    internal static class BoxingUtils
    {
        public static Panel_Inventory inventory;
        public static GameObject treebark = Addressables.LoadAssetAsync<GameObject>("GEAR_Treebark").WaitForCompletion();

        public static List<string> pilable = [];
        public static List<string> unpilable = [];
        public static List<int> pilesize = [];


        public static GameObject GetPlayer()
        {
            return GameManager.GetPlayerObject();
        }
        public static bool LoadItems()
        {
            if (File.Exists("Mods\\BoxingList.txt"))
            {
                using (StreamReader sr = File.OpenText("Mods\\BoxingList.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        string temp = sr.ReadLine();
                        if (temp == null)
                        {
                            sr.Close();
                            return false;
                        }
                        MelonLoader.MelonLogger.Msg("boxing blueprint: " + temp);
                        string[] packline = temp.Split(';');
                        if (packline == null)
                        {
                            sr.Close();
                            return false;
                        }
                        pilable.Add(packline[0]);
                        unpilable.Add(packline[1]);
                        pilesize.Add(Int32.Parse(packline[2]));
                    }
                    sr.Close();
                }
                MelonLoader.MelonLogger.Msg("boxing blueprints loaded: " + pilable.Count);
                return true;
            }
            return false;            
        }
        public static bool IsItemPileable(string gearItemName)
        {
            if (pilable.Count == 0)
            {
                if (!LoadItems()) { return false; }
            }
            for(int i = 0; i < pilable.Count; i++)
            {
                if (pilable[i] == gearItemName) return true;
            }
            return false;
        }

        public static bool IsItemUnPileable(string gearItemName)
        {
            if (unpilable.Count == 0)
            {
                if (!LoadItems()) { return false; }
            }
            for (int i = 0; i < unpilable.Count; i++)
            {
                if (unpilable[i] == gearItemName) return true;
            }
            return false;
        }

        public static T? GetComponentSafe<T>(this Component? component) where T : Component
        {
            return component == null ? default : GetComponentSafe<T>(component.GetGameObject());
        }
        public static T? GetComponentSafe<T>(this GameObject? gameObject) where T : Component
        {
            return gameObject == null ? default : gameObject.GetComponent<T>();
        }
        public static T? GetOrCreateComponent<T>(this Component? component) where T : Component
        {
            return component == null ? default : GetOrCreateComponent<T>(component.GetGameObject());
        }
        public static T? GetOrCreateComponent<T>(this GameObject? gameObject) where T : Component
        {
            if (gameObject == null)
            {
                return default;
            }

            T? result = GetComponentSafe<T>(gameObject);

            if (result == null)
            {
                result = gameObject.AddComponent<T>();
            }

            return result;
        }
        internal static GameObject? GetGameObject(this Component? component)
        {
            try
            {
                return component == null ? default : component.gameObject;
            }
            catch (System.Exception exception)
            {
                MelonLoader.MelonLogger.Msg($"Returning null since this could not obtain a Game Object from the component. Stack trace:\n{exception.Message}");
            }
            return null;
        }

        public static bool IsScenePlayable()
        {
            return !(string.IsNullOrEmpty(GameManager.m_ActiveScene) || GameManager.m_ActiveScene.Contains("MainMenu") || GameManager.m_ActiveScene == "Boot" || GameManager.m_ActiveScene == "Empty");
        }

        public static bool IsScenePlayable(string scene)
        {
            return !(string.IsNullOrEmpty(scene) || scene.Contains("MainMenu") || scene == "Boot" || scene == "Empty");
        }
        public static bool IsMainMenu(string scene)
        {
            return !string.IsNullOrEmpty(scene) && scene.Contains("MainMenu");
        }

    }

}
