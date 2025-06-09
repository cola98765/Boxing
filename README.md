# Boxing
Boxing is a mod that expands on the piling formula some other mods use.
Instead of using blueprints it uses custom interaction in inspect menu.
And Unlike blueprints condition is preserved, meaning food will not magically gain contition.

## Packages will decay
Even if original author did not define decay rate for the package, if the base item had decay rate it will be applied to package too. (configurable)

## However, this mod does not add any piled items on it's own,
Base config file has [ItemPiles](https://github.com/Thekillergreece/FoodPackByTKG) and [FoodPackByTKG](https://github.com/Atlas-Lumi/ItemPiles) as examples

## Installation

* Download the latest version from releases and place the .dll and provided .txt into the mods folder
* depends on [ModSettings](https://github.com/DigitalzombieTLD/ModSettings/)

## Changelog

**V1.1.2:**
* added per item decay time multiplier in config file as workaround to me being unable to figure out why not only simply copying values can give different results, with recorded values bieng 1, 2, or 4,

**V1.1.1:**
* fix condition actually applying when unpacking
* added path for pile to define decay on unpiled item (eg TKG flour ingeriting decay from vanilla)
* added checks for not full food, powder, or liquid items
* added ability to comment in config file

**V1.1:**
* Packaged items decay

**V1.0:**
* initial release

## Credits

TonisGaming for making [Leatherworks](https://github.com/TonisGaming/Leatherworks) that broke and I had to fix which inspired me to make this mod.
