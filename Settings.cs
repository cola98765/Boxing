using ModSettings;

namespace Boxing
{
    internal class Settings : JsonModSettings
    {
        internal static Settings instance = new Settings();

        [Section("Preferences")]

        [Name("Copy decay")]
        [Description("Copies decay value of food items onto the packaged non-food item. Requires restart")]
        public bool decay = true;

        [Name("Decay scale")]
        [Description("Since it's no longer food other bonuses might not apply, or simply you think packaged stuff should last longer. Requires restart")]
        [Slider(0f, 1f, 41, NumberFormat = "{0:0.00}")]
        public float decaybonus = 1f;

    }
}
