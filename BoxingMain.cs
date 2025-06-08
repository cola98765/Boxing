using MelonLoader;

namespace Boxing
{
    internal sealed class BoxingMain : MelonMod
    {
        public static bool isLoaded;

        private static bool addedCustomComponents; 

        public override void OnInitializeMelon()
        {
            MelonLoader.MelonLogger.Msg(System.ConsoleColor.Green, "boxing Loaded!");
            Settings.instance.AddToModSettings("Boxing");
        }
        public override void OnSceneWasInitialized(int level, string name)
        {
            if (BoxingUtils.IsScenePlayable(name))
            {
                isLoaded = true;

                BoxingUtils.LoadItems();

            }
        }

    }
}