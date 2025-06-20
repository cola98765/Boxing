using Il2Cpp;
using Il2CppSystem;
using Il2CppTLD.IntBackedUnit;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.UIElements;


namespace Boxing
{
    internal static class BoxingUtils
    {
        public static Panel_Inventory inventory;
        public static GameObject treebark = Addressables.LoadAssetAsync<GameObject>("GEAR_Treebark").WaitForCompletion();

        public static List<GearItem> pilable = [];
        public static List<GearItem> unpilable = [];
        public static List<int> pilesize = [];

        public static GameObject GetPlayer()
        {
            return GameManager.GetPlayerObject();
        }
        public static bool LoadItems()
        {
            //file format
            //gear_source;gear_target;number;gear_box;stack size x;y;z; margin x;y;z
            //# comments
            //only first 3 are required if "gear_target" does not use dynamic object creation
            //for the dynamic creation to work "gear_target" name has to equal "gear_source" + "Box"
            //eg. "GEAR_Soda" -> "GEAR_SodaBox"
            //"stack size" does not have to match "number"
            //"gear_box" can be empty if you want to create simple stack
            //TODO 'gear_box' has restrictions, generalize it even more
            //TODO it may be possible to create gear bypassing .modcomponent entirely
            if (File.Exists("Mods\\BoxingList.txt") && pilable.Count == 0)
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
                        if (temp[0] == '#') continue;

                        MelonLoader.MelonLogger.Msg("blueprint: " + temp);
                        string[] packline = temp.Split(';');
                        if (packline == null)
                        {
                            sr.Close();
                            return false;
                        }
                        GearItem source;
                        {
                            source = GearItem.LoadGearItemPrefab(packline[0]);
                        }
                        GearItem target = GearItem.LoadGearItemPrefab(packline[1]);
                        if (source == null || target == null)
                        {
                            MelonLoader.MelonLogger.Msg("one of items is null, skipping");
                            continue;
                        }
                        pilable.Add(source);
                        unpilable.Add(target);
                        pilesize.Add(System.Int32.Parse(packline[2]));
                        if (packline[0] + "Box" == packline[1])
                        {
                            int[] size = { System.Int32.Parse(packline[4]), System.Int32.Parse(packline[5]), System.Int32.Parse(packline[6]) };
                            int[] count = { 0, 0, 0 };
                            //cause we can't have nice things
                            float[] margin = { float.Parse(packline[7]), float.Parse(packline[8]), float.Parse(packline[9]) };
                            float[] offset = { 0, 0, 0 };
                                                       
                            if (source.GetComponentInChildren<LiquidItem>() != null)
                            {
                                target.GearItemData.m_BaseWeight = System.Int32.Parse(packline[2]) * ItemWeight.FromKilograms(source.GetComponent<LiquidItem>().m_LiquidCapacity.ToQuantity(1f));
                            }
                            else if (source.GetComponentInChildren<StackableItem>())
                            {
                                target.GearItemData.m_BaseWeight = System.Int32.Parse(packline[2]) * source.GearItemData.m_BaseWeight / source.GetComponentInChildren<StackableItem>().m_DefaultUnitsInItem;
                            }
                            else
                            {
                                target.GearItemData.m_BaseWeight = System.Int32.Parse(packline[2]) * source.GearItemData.m_BaseWeight;
                            }
                            //check collider
                            BoxCollider sourceCollider = source.GetComponentInChildren<BoxCollider>();
                            Vector3 position = Vector3.zero;
                            position.y = (size[1] * sourceCollider.size.y + 0.005f) / 2;
                            target.GetComponent<BoxCollider>().center = position;
                            
                            position.x = size[0] * (sourceCollider.size.x + margin[0]);
                            position.y = size[1] * (sourceCollider.size.y + margin[1]) + 0.005f;
                            position.z = size[2] * (sourceCollider.size.z + margin[2]);
                            target.GetComponent<BoxCollider>().size = position;
                            //this trusts that even if collider is wrong size it's at least centered
                            offset[0] = -sourceCollider.center.x;
                            offset[1] = -sourceCollider.center.y + (sourceCollider.size.y / 2);
                            offset[2] = -sourceCollider.center.z;
                            target.transform.localScale = sourceCollider.transform.localScale;
                            Vector3 sourcesize = Vector3.Scale(sourceCollider.size, sourceCollider.transform.localScale);
                            
                            //add box if config asks for it
                            GearItem box = GearItem.LoadGearItemPrefab(packline[3]);
                            if (box != null)
                            {
                                //this assumes that box is 1/4m x 1/8m x 1/4m
                                position = box.GetComponentsInChildren<Transform>()[1].localScale;
                                position.x *= 4 * size[0] * (sourcesize.x + margin[0]);
                                position.y *= 4 * size[1] * (sourcesize.y + margin[1]);
                                position.z *= 4 * size[2] * (sourcesize.z + margin[2]);
                                GameObject localbox = new GameObject("Box");
                                localbox.transform.localScale = position;
                                localbox.AddComponent<MeshFilter>().mesh = box.GetComponentInChildren<MeshFilter>().mesh;
                                localbox.AddComponent<MeshRenderer>().sharedMaterials = box.GetComponentInChildren<MeshRenderer>().sharedMaterials;
                                localbox.transform.parent = target.transform;

                            }


                            //add individual items in box
                            MeshFilter[] sourceMesh = source.GetComponentsInChildren<MeshFilter>(true);
                            for (int i = 0; i < (size[0] * size[1] * size[2]); i++)
                            {
                                for (int j = 0; j < sourceMesh.Length; j++)
                                {
                                    //there are so many ways to group items, some have it on root, some have multiple, but not all are used
                                    if (!(sourceMesh[j].name.Contains("LOD1") || sourceMesh[j].name.Contains("Open") || sourceMesh[j].name.Contains("Used") || sourceMesh[j].name.Contains("Old") ||
                                        (sourceMesh[j].transform.parent != null && (sourceMesh[j].transform.parent.name.Contains("LOD1") || sourceMesh[j].transform.parent.name.Contains("Open") || sourceMesh[j].transform.parent.name.Contains("Used") || sourceMesh[j].transform.parent.name.Contains("Old")))))
                                    {
                                        GameObject can = new GameObject("can");
                                        can.AddComponent<MeshFilter>().mesh = sourceMesh[j].mesh;
                                        can.AddComponent<MeshRenderer>().sharedMaterials = sourceMesh[j].GetComponent<MeshRenderer>().sharedMaterials;
                                        if (packline[10] != "ignore") can.transform.rotation = sourceMesh[j].transform.rotation;
                                        position = sourceMesh[j].transform.localPosition;
                                        position.x += offset[0] + ((1 - size[0] + count[0] * 2) * (sourcesize.x + margin[0]) / 2);
                                        position.y += /*offset[1]*/ + 0.005f + (count[1] * (sourcesize.y + margin[1]));
                                        position.z += offset[2] + ((1 - size[2] + count[2] * 2) * (sourcesize.z + margin[2]) / 2);
                                        can.transform.localPosition = position;
                                        can.transform.localScale = sourceMesh[j].transform.lossyScale;
                                        can.transform.parent = target.transform;
                                    }
                                }
                                count[0]++;
                                if (count[0] == size[0])
                                {
                                    count[0] = 0;
                                    count[2]++;
                                }
                                if (count[2] == size[2])
                                {
                                    count[2] = 0;
                                    count[1]++;
                                }
                            }
                            MelonLoader.MelonLogger.Msg("mesh added for: " + packline[1]);
                        }
                        //add all decay
                        if (Settings.instance.decay)
                        {
                            if (target.GearItemData.m_DailyHPDecay == 0)
                            {
                                if (source.m_FoodItem != null && target.m_FoodItem == null)
                                {
                                    float sourceInside = source.m_FoodItem.m_DailyHPDecayInside / source.GearItemData.MaxHP * 100f;
                                    float sourceOutside = source.m_FoodItem.m_DailyHPDecayOutside / source.GearItemData.MaxHP * 100f;
                                    if (sourceInside != 0 || sourceOutside != 0)
                                    {
                                        target.GearItemData.m_ConditionType = source.GearItemData.m_ConditionType;
                                        if (sourceInside > sourceOutside) target.GearItemData.m_DailyHPDecay = sourceOutside * Settings.instance.decaybonus / source.GearItemData.MaxHP * 100f;
                                        else target.GearItemData.m_DailyHPDecay = sourceInside * Settings.instance.decaybonus / source.GearItemData.MaxHP * 100f;
                                        MelonLoader.MelonLogger.Msg("decay added for: " + packline[1]);
                                    }
                                }
                                else if (source.GearItemData.m_DailyHPDecay > 0)
                                {
                                    target.GearItemData.m_ConditionType = source.GearItemData.m_ConditionType;
                                    target.GearItemData.m_DailyHPDecay = source.GearItemData.m_DailyHPDecay / source.GearItemData.MaxHP * 100f; //does not get the bonus as it should not stack
                                    MelonLoader.MelonLogger.Msg("decay added for: " + packline[1]);
                                }
                            }
                            else if (source.GearItemData.m_DailyHPDecay == 0 && source.m_FoodItem == null) 
                            {
                                //rare case where stacked item has decay and source does not like TKG flour
                                source.GearItemData.m_ConditionType = target.GearItemData.m_ConditionType;
                                source.GearItemData.m_DailyHPDecay = target.GearItemData.m_DailyHPDecay / target.GearItemData.MaxHP * 100f ; //does not get the bonus as it should not stack
                                MelonLoader.MelonLogger.Msg("decay added for: " + packline[0]);
                            }
                        }
                    }
                    sr.Close();
                }
                MelonLoader.MelonLogger.Msg("blueprints loaded: " + pilable.Count);
                return true;
            }
            return false;            
        }
        public static bool IsItemPileable(string name)
        {
            for(int i = 0; i < pilable.Count; i++)
            {
                if (pilable[i].name == name) return true;
            }
            return false;
        }

        public static bool IsItemUnPileable(string name)
        {
            for (int i = 0; i < unpilable.Count; i++)
            {
                if (unpilable[i].name == name) return true;
            }
            return false;
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
