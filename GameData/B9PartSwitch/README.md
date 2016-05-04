# B9PartSwitch

A Kerbal Space Program plugin designed to implement switching of part meshes, resources, and nodes

This mod will not change anything by itself, but is designed to be used by other mods to enable part subtype switching

## Requirements

* KSP version 1.1.1 (build 1250) is the only supported KSP version
* [ModuleManager](http://forum.kerbalspaceprogram.com/index.php?showtopic=50533) is required.

## Installation

* Remove any previous installation of B9PartSwitch
* Make sure the latest version of ModuleManager is installed
* Copy the B9PartSwitch directory to your KSP GameData directory.

## Contributors

* [blowfish](http://forum.kerbalspaceprogram.com/index.php?/profile/119688-blowfish/) - primary developer
* [bac9](http://forum.kerbalspaceprogram.com/index.php?/profile/57757-bac9/) - author of [PartSubtypeSwitcher](https://bitbucket.org/bac9/ksp_plugins), on which this plugin is heavily based

## Source

The source can be found at [Github](https://github.com/blowfishpro/B9PartSwitch)

## License

This plugin is distributed under [LGPL v3.0](http://www.gnu.org/licenses/lgpl-3.0.en.html)

## Changelog

### 1.1.3

* Recompile against KSP 1.1.2
* Simplify part list info a bit
* Hopefully make some error messages clearer
* Various internal refactors and simplifications

### 1.1.2

* Remove FSmeshSwitch and InterstellarMeshSwitch from incompatible modules
* Recompile against KSP 1.1.1

### 1.1.1

* Fix resource cost not accounting for units per volume on tank type

### 1.1

* KSP 1.1 compatibility
* Fixed bug where having part switching on the root part would cause physics to break
* Moved UI controls to UI_ChooseOption
* Adjust default Monopropellant tank type to be closer to (new) stock values
* Use stock part mass modification
* Hopefully fix incompatible module checking
* Various refactors and simplifications which might improve performance a bit

### 1.0.1

* Fix NRE in flight scene

### 1.0.0

* Initial release