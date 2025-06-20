# Boxing
Boxing is a mod that expands on the piling formula some other mods use.
Instead of using blueprints it uses custom interaction in inspect menu.
And Unlike blueprints condition is preserved, meaning food will not magically gain contition.

![Assets/20250615215430_1.jpg](https://raw.githubusercontent.com/cola98765/Boxing/refs/heads/master/Assets/20250615215430_1.jpg)

## Packages will decay
Even if original author did not define decay rate for the package, if the base item had decay rate it will be applied to package too. (configurable)

in addition to dynamically created boxed items in this modcomponent package, base config file has [ItemPiles](https://github.com/Thekillergreece/FoodPackByTKG) and [FoodPackByTKG](https://github.com/Atlas-Lumi/ItemPiles) as examples

## Installation

* Download the latest version from releases and place the .dll and provided .txt into the mods folder
* depends on [ModSettings](https://github.com/DigitalzombieTLD/ModSettings/)

## Config file format
BoxingList is where most things are actualyl defined for this mod
`GEAR_Soda;GEAR_SodaBox;6;GEAR_EmptyBox;3;1;2;0;0;0;inherit`

|GEAR_Soda			|GEAR_SodaBox		|6						|GEAR_EmptyBox			|3;1;2				| 0;0;0			| inherit										|
|-------------------|-------------------|-----------------------|-----------------------|-------------------|---------------|-----------------------------------------------|
|Source gear name	|Boxed gear name	|Number of items in box	|Empty box gear name	|Stack size X;Y;Z	| Offset X;Y;Z	| rotation mode: inherit/ignore/[TODO: random]	|

* Use `#` as comments in this file
* Only frist 3 fields are used if you use your own assets
* for the dynamic boxing to work boxed name has to be saem as single item with `Box` at the end
* Empty box gear name can be empty, then items will not be held in the box
* Stack size X;Y;Z is only visual, dioes not have to match number of items in box

## Changelog

**V1.3.0**
* completely reworked what you need to create gear
	* only a box collider, generic item component, and optionally stackable behaviour is needed. things like scale of saide box collider of weight of item will be set in runtime
	* expanded config file to facilitate that change. Not much is hardcoded.
* localisation is copied from single tiems, and inventory icons are mostly generic
* added submod with boxes for most FoodPackByTKG items

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
