Crossfeed Enabler
by NathanKell

A partmodule: adds a fuel crossfeed between the part it's added to, and the part this part is surface-attached to. Use it for radial tanks (or wings or...)

License CC-BY-SA

Source on GitHub at https://github.com/NathanKell/CrossFeedEnabler/

Installation: Extract to GameData. By default includes cfg to apply to radial RCS tanks, the mini radial RCS from Realism Overhaul, and all procedural wings. (NOTE: Requires ModuleManager, which by now you really should have.)

To add to other parts:
Add:
MODULE
{
	name = ModuleCrossFeed
}
to the cfg, or do it via MM.
For example, create a MM node and add it to some cfg.

@PART[YourPartNameHere]
{
	MODULE
	{
		name = ModuleCrossFeed
	}
}

=================
Changelog:
=================
v2.2 \/
*Update to .24.2

v2.1 \/
*Update to .24.1

v2   \/
*Add support for Stretchies and PP tanks.
*Add support for KSPX parts (Supernovy)
*Update to 0.24

v1   \/
Initial release