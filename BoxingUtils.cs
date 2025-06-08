using Il2Cpp;
using Il2CppTLD.Cooking;
using Il2CppTLD.Gear;
using Il2CppTLD.IntBackedUnit;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;

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
                        if (Settings.instance.decay) //setting enabled decay copy
                        {
                            GearItem source = GearItem.LoadGearItemPrefab(packline[0]);
                            GearItem target = GearItem.LoadGearItemPrefab(packline[1]);
                            if (target.GearItemData.m_DailyHPDecay == 0)
                            {
                                if (source.GetComponent<FoodItem>() != null && target.GetComponent<FoodItem>() == null)
                                {
                                    float sourceInside = source.GetComponent<FoodItem>().m_DailyHPDecayInside;
                                    float sourceOutside = source.GetComponent<FoodItem>().m_DailyHPDecayInside;
                                    if (sourceInside != 0 || sourceOutside != 0)
                                    {
                                        if (sourceInside > sourceOutside) target.GearItemData.m_DailyHPDecay = sourceOutside * Settings.instance.decaybonus;
                                        else target.GearItemData.m_DailyHPDecay = sourceInside * Settings.instance.decaybonus;
                                        MelonLoader.MelonLogger.Msg("boxing decay added for: " + packline[1]);
                                    }
                                }
                                else if (source.GearItemData.m_DailyHPDecay > 0)
                                {
                                    target.GearItemData.m_DailyHPDecay = source.GearItemData.m_DailyHPDecay; //does not get the bonus as it should not stack
                                    MelonLoader.MelonLogger.Msg("boxing decay added for: " + packline[1]);
                                }
                            }
                        }
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
            for(int i = 0; i < pilable.Count; i++)
            {
                if (pilable[i] == gearItemName) return true;
            }
            return false;
        }

        public static bool IsItemUnPileable(string gearItemName)
        {
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
