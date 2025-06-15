# Boxing
Boxing is a mod that expands on the piling formula some other mods use.
Instead of using blueprints it uses custom interaction in inspect menu.
And Unlike blueprints condition is preserved, meaning food will not magically gain contition.

![Assets/20250615215430_1.jpg](https://raw.githubusercontent.com/cola98765/Boxing/refs/heads/master/Assets/20250615215430_1.jpg)

## Packages will decay
Even if original author did not define decay rate for the package, if the base item had decay rate it will be applied to package too. (configurable)

in addition to couple boxed items in this modcomponent package, base config file has [ItemPiles](https://github.com/Thekillergreece/FoodPackByTKG) and [FoodPackByTKG](https://github.com/Atlas-Lumi/ItemPiles) as examples

## Installation

* Download the latest version from releases and place the .dll and provided .txt into the mods folder
* depends on [ModSettings](https://github.com/DigitalzombieTLD/ModSettings/)

## Changelog

**V1.2.0**
* created couple sample boxed vanilla items (no localisation nor inventory icons)
* adeed null check if gear is not valid

**V1.1.2:**
* fix decay rates being calculated incorrectly ignoring max HP other than 100 and taking inside decay as outside too

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
