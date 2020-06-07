_______________________________

▼ v6.5.2

	* Instancing of the HX HPD engine (there are now 3 of it) to meet advanced/various propulsion needs.
	* Rebalanced original HPD engine to be more viable for use in very large HX ships or HX ships that simply want to go very, very far.
	* Added B9 HX tank types for LiquidFuel and for new engine modes.
	* Added/changed plumes for all engine modes.
	* Instancing of RCS thrusters (LFO versions).
	* Nerfed RTG in HX URC reactor.
	* HX URC reactor becomes full-featured reactor compatible with Near Future Electrical and Wild Blue Industries (WBI) DSEV.
	* Added support for WBI Play Mode to HX. Switch between Community Resource Pack requirements and B9 fuel options; and Classic Stock resource requirements and use Wild Blue OmniStorage.
	* Moved most existing patch files in accordance with Play Mode.	
	* Raised heat tolerances of HX parts from 2000K to 2700K.
	* Adjusted CTT locations. Most parts will now appear in Orbital Megastructures. RCS and engines will appear in Very Heavy Rocketry and Gigantic Rocketry.
_______________________________

▼ v6.5.1

	* Release for KSP 1.4.2+
	* Fix RCS effects being backward and too big
	* Update CryoTanks patch to be consistent with current CryoTanks
_______________________________

▼ v6.5.0

	* Release for KSP 1.4.2
	* Fix Tetragon Projects agency not having a title
	* Fix incorrect bulkhead profiles
	* Switch landing gear from FStextureSwitch2 to ModuleB9PartSwitch
		* Shielding now costs and masses a bit more but gives better heat tolerance
	* Fix HL Cockpit RCS transforms facing the wrong way
	* Fix Mk5 cockpit having an invalid collider
	* Fix not being able to exit Mk2b cockpit
	* Fix face normal issues on Mk2b intake cockpit
	* Smoothing adjustments on HL, Mk5, Mk2b, Mk2b intake cockpits
	* Fix 6m HL Fuselage giving warnings about transforms that don't exist
	* Disable props that no longer exist so they don't spam the log
_______________________________

▼ v6.4.0

	* Release for KSP 1.3.1
	* Bring back engine pylons for use on things other than engines
	* Unify all tank masses with the same formula
	* Reduce mass of structural tank variants a bit
	* Remove unbalanced battery option on Y1 end cap
	* CryoTanks: Bring LH2 and LH2O tank stats more in line with CryoTanks
	* CryoTanks: Add cooling to all tanks as in CryoTanks
	* CryoTanks: Fix definition of boiloff module
	* Fix a couple of legacy wings having nonexistent tech nodes
	* Fix FAR patches on S2 fuselages not properly removing ModuleLiftingSurface
_______________________________

▼ v6.3.2

	* Release for KSP 1.3
	* Fix some mismatched brackets
_______________________________

▼ v6.3.1

	* Fix CryoTanks patch not working
	* Fix wheels breaking over 10 m/s
	* Remove wheel sounds effects that weren't working
	* Increase load rating and brake torque of larger wheels
	* Increase heat tolerance of Mk2b cockpits
_______________________________

▼ v6.3.0

	* Convert all switchers on legacy tanks to B9PartSwitch.  This may be slightly craft breaking in some scenarios (contact for help)
	* Combine legacy Mk2 adapters into one part.  This may prevent some craft from loading (contact for help)
	* Split up switchers on all HL parts to simplify using them.  This may be slightly craft breaking in some scenarios (contact for help)
	* Fix fuel flow issues on certain engines
	* Fix MFT/RF patches for RCS tanks (they were broken previously)
	* Bring back landing gear using KSPWheel.  They will only show if you install KSPWheel separately.
		* Parameters are still WIP and many effects do not work
	* Fix Mk2b probe core not having control
	* Allow B9 tanks to hold LH2 if you have CryoTanks installed
	* Allow SABRE to use LH2 if you have CryoTanks installed and create B9_SABRE_Cryo folder in GameData (opt in)
_______________________________

▼ v6.2.1

	* For KSP 1.2.2
	* Replace remaining firespitter animation modules with ModuleAnimateGeneric
	* Add EVA range to some animations
	* Fix RPM color tags
	* Remove hover function from VTOLs as it was causing them to overheating
	* Add editor-attachable node to shielded docking port
	* Use new SmokeScreen feature to reduce memory consuption of exhaust effects
_______________________________

▼ v6.2.0-pre

	* Fixes for KSP 1.2
	* Use new part categories (thanks flashblade)
	* Fix some previously inconsistent part categories (thanks flashblade)
	* All data transmitters to all command pods and Kerbnet access to all drone cores (thanks flashblade)
	* Adjust subtype names to that they display better in the editor
	* Fix FAR patch on Mk2 Intake Cockpit
	* Use custom tank types in B9 rather than default B9PartSwitch ones - for now they are identical
_______________________________

▼ v6.1.2

	* For KSP 1.1.3 (didn't really require any changes)
	* Fix mesh switching issue on R1 RCS Port
	* Fix flipped node on T2 tail
	* Fix lights illuminating planets from orbit (thanks taniwha)
_______________________________

▼ 6.1.1

	* For KSP 1.1.2 (didn't really require any changes)
	* Fix RPM resource displays (credit to danfarnsy)
	* Update DRE patch (also danfarnsy)
	* Fix FAR voxelization on S2W parts
_______________________________

▼ 6.1.0

	* Updated for KSP 1.1
	* Get rid of KM_Gimbal, as most important functionality is now stock and it will not be maintained
	* Fix smoke effects
	* Increase response speed on some thermal anims
	* Move landing gear to broken, since they are broken (no way to fix them without the source files, plus they were already legacy)
	* Adjust cockpit thermals to be more in-line with stock (thanks flashblade)
	* Add tags, thanks sparkybear for the hard work

_______________________________

▼ 6.0.1

	* Fix L2 Atlas effects persisting after flameout
	* Fix smoke effects not showing up on all engines
	* Fix legacy Mk2 crew cabin not working for contracts

_______________________________

▼ 6.0.0

	* Integrate bac9's beautiful new Mk1 and Mk2 parts
	* Move nearly all fuel/mesh switching to new plugin, B9PartSwitch
	* Move old Mk1/2 parts to legacy
	* Update all compatibility patches (some untested)
	* Remove fuel switching in cargo bays - there's really not that much space
	* Remove Virgin Kalactic as a dependency
	* Make HX pack independend of core
	* Pylons are now integrated into podded engines
	* Bring back the airbrakes (now functional for no apparent reason)
	* Combine textures on HL Extension B and RCS blocks
	* Remove MP_Nazari folder - it just messes with things
	* Improve thermal anims on HX HPD
	* Fix patches applying to procedural wings
	* Dependency updates
	* Many minor tweaks

_______________________________

▼ R5.4.0

	* 1.0.5 compatiblity
	* RT compatibility by joshwoo70
	* Add recommendations to readme - mods that are more than suggested
	* Dependency updates
	* Remove Crossfeed Enabler
	* Remove KineTechAnimation
	* Add B9AnimationModules
	* F119 and turbojet now have afterburning modes
	* Medium jet pod increased in size and renamed to CFM56
	* All engines given stockalike thrust curves and thrust tweaks
	* Thermal anims moved to FXModuleAnimateThrottle or B9ModuleAnimateThrottle (except HX HPD)
	* Give intakes performance curves from stock and adjust area/speed
	* Add scenario constraints to ModuleTestSubject using stock modules mostly
	* Rename all part.cfg to part names (excluding 'B9_')
	* Delete squad jet balance config
	* Fix cargo bay nodes
	* Fix separation motors overheating
	* Update links in readme per new forum
	* Add ModuleSurfaceFX to HPD, tweak heat

_______________________________

▼ R5.3.0

	* Stack node adjustments by V8jester and M4ck
	* All textures converted to DDS
	* CrossFeedEnabler, Firespitter, JSI, KM_Gimbal, SmokeScreen updates
	* Recompile KineTechAnimation and Virgin Kalactic which have not been updated by their authors
	* Re-entry thermals should be good now
	* Cargo bays given new stock module and outer drag cubes applied
	* Wings should work in stock aero now
	* Intakes rebalanced against new stock intakes
	* bulkheadProfiles done, HX icon requires FilterExtensions
	* Compressed air thrusters work without RESGEN
	* Reconfigure tech nodes for new tech tree
	* Jet engine configs redone
	* ModuleSurfaceFX added to all engines
	* Cargo bays have inner stack nodes
	* Fix breaking changes in RPM
		* Camera can no longer be rendered in PFD background.  This was disabled in RPM.
	* Rebalanced rocket engines' Isp to new stock levels
	* Cockpits, engines, and intakes have mass, cost adjusted to match stock parts
	* 1.0.3+ heating changes, thanks Nansuchao
	* Give HX hangars custom drag cubes
	* HL-Mk4 adapter moved to legacy
	* Wings moved to legacy (now recommend B9 Procedural wings)
	* Landing gear moved to legacy (now recommend Adjustable Landing Gear)
	* HL parts moved to separate folder (still requires main folder though)
	* Added RT patch by joshwoo70

_______________________________

▼ R5.2.4

* Corrected CoL position of SH/SE/T aerodynamic surfaces in FAR 0.14.2+

* Tweaked the emissive of the HL Cockpit.

* Updated SmokeScreen to v2.4.5.0

_______________________________

▼ R5.2.3

* **NOTE:** The texture format has changed. Please ensure you remove the old `B9_Aerospace` folder before upgrading.
    * Textures converted to PNG for massive memory savings for those users not running the `Active Texture Management` mod.

* New Mk2 Crew Tank IVA with 4 seats.

_______________________________

▼ R5.2.2

* **R&D node changes:** Many parts have had their tech nodes adjusted to be more in line with the positions of stock parts.
    * A1/4/8 Spotlights moved to `electrics` with stock spotlights.
    * Basic nosecone to moved to `aerodynamicSystems`.
    * Infodrive moved to `start` node.
    * SNM Strut moved to `generalConstruction` with the SN strut.
    * Ladders and Railings moved to `advExloration` together with stock ladders.
    * VS1 engine and nosecone moved to `actuators`.
    * MT1 RCS tank moved to `advFlightControl` together with small stock radial RTS tank.
    * R1A AirCS moved to `specializedControl`.
    * R1 and R5 RCS blocks moved to `advFlightControl` with stock RCS blocks.
    * R12 moves to `largeControl`.
    * HL Cockpit moved to `heavyAerodynamics` with the rest of the HL fuselage system.
    * MK2 cargo bays moved to `supersonicFlight` with the rest of the MK2 parts.
    * 2.5m rocket parts (T2/M2) moved `specializedConstruction`.
    * EM Engine Mount moved to `aerodynamicSystems`.
    * Small airbrakes moved to `flightControl`.
    * DSIX moved to `supersonicFlight`.
    * DSI moves to `aerodynamicSystems`.
    * MSR Stack Separator moved to `advMetalworks`.
    * M27 command pod moved to `composites`.
    * All HX parts except URC generator moved to `experimentalRocketry`.

* Added agency images to ATM exclusions.

* Adjusted some costs to correct a rounding error in our formulas.

* Added switchable fuel tank to Y1 endcap.

* Fixed bottom node size on HL 2.5m adapter.

* Added Deadly Reentry support.

* Fixed model offset on HX Hub Support.
* Increased decoupler force, grab force on HX Docking Nodes.
* Increased HX connection strenghts.
* Increased HX Hangar/Side Adapter connections even more.

* Changed title of MK2 nosecones to MK1.

* Increased connections strengths on PodJets and pylons.
* Changed title of podjet pylons.

* Tweaked Jet, SABRE sound FX.

* Updated CrossFeedEnabler to v3.0.2

* Fixed wrong FAR/NEAR drag on HL Side Adapter.

* Fixed lack of IntakeAir tank in S2W intake under FAR|NEAR.

* Fixed CoL offset on HW21 wing (FAR 0.14.2+)

* Fixed base cost on MFT tanks.
* Disabled cost calculations with MFT on cockpits and LF-only crew tanks.

* Updated ModuleManager to v2.4.4.

* Fixed flag transparency on HL, MK2, S3 cockpits and MK2 cabin.

* Numerous tweaks to ModuleManager syntax in files.

_______________________________

▼ R5.2.1

* Added 4th pair of engines to D-175 Strugatsky.

* Increased thrust of Turbofans under FAR|NEAR.

* Fixed a part name in deprecated file.

* Disabled TACLS resources on 6m S2 Crew Cabin when MFT is not present.

* Adjusted mass/cost of Y1 endcap.

* Fixed per-tank masses when using ModularFuelTanks.
* Fixed usage amounts of B9_ServiceModule tank type.
* Added small servicemodule/lifesupport tank to Y1 endcap.

* Moved stock turbojets to supersonic flight tech node.
* Adjusted thrust dropoff on stock/b9 turbojet to be slightly less abrupt.

* Fixed name of MK1 Junction so that its not interpreted as a payload fairing by FAR|NEAR.


_______________________________

▼ R5.2

* Fixed alternator power on SABRE 250.

* Redone landing gear traction, torque, energy usage, suspension, deployed drag.

* Disabled CF34/TFE731 blur discs.

* Adjusted thrust dropoff of D-30F7 thrbojet.

* Added max design velocity to air-breathing engine descriptions.

* Fixed HPD closed-cycle sound.

* Increased SABRE gimbal range.

* Fixed L2 Atlas FX.

* Converted VTOL engines to ModuleEnginesFX and HotRockets! FX.

* Enabled engine response time on rocket engines.

* Added FAR/NEAR-only fuel tank to S2W intake.

* Added missing IntakeAir storage to S2W intake.

* Fixed various errors with some ModuleManager code blocks in intakes.

* Added fuel tanks to MK2, VS1 nosecones.

* Added fuel tank to PA panel adapter.

* Fixed MK2 Crew Cabin airlocks.

* Numerous MFD tweaks and fixes.

* Reduced size of some emissive textures.

* Fixed visual state of staging button prop.

* Tweaked MK2 IVA.

* Adjusted HL RPM camera positions.

* Updated CrossFeedEnabler to v3.0.1.

* Updated FireSpitter to git `a99fc36c`.

* Added transparency support to on-cockpit flags.

* Added CrossFeedEnabler to MT RCS tanks.

* Fixed ECharge/MonoPropellant capacities of many cockpits.

* Added support for Modular Fuel Tanks.
* Added preliminary support for RealFuels.

* Added support for TAC Life Support.

* Minor fixes to ModuleManager setup of FAR/NEAR wing values.

* Tweaked specular maps and UV of S2 crew tanks.

* Updated ModuleManager to v2.3.5.

* DEPRECATED parts are now on the `hidden` tech node.

* Rebalanced Squad Jets to not be stupid.


_______________________________

▼ R5.0

* 0.24.2 compatibility update.

* NOTE: Will likely break crafts (and thus saves) using 4.0c or the community patches thereof.
    * An optional download which recreates the removed parts is available, but will not help in all cases.

* NOTE: All fuselage/tank/adapter parts been rebalanced.
    * On the whole, parts are now somewhat heavier and hold considerably more fuel than before.

* NOTE: The stock drag module is now not supported.
    * FAR or NEAR are now required.

    * While the parts will function in the stock drag model and are balanced, we won't accept bug reports relating to performance under simulated soup conditions.

* Implemented fuel-tank, model, tank switching powered by FireSpitter:
    * Most parts have a switchable fuel tank, similar to Modular Fuel Tanks or Real Fuels.
    * Some parts can change shape. Some have alternate textures. Where applicable, the look of the part changes to denote the type of tank it contains.

* Added a new family of the structural parts: HX.
    * Mostly designed for large-scale orbital installations, but with enough dedication you can probably build a flying fortress or something too.

* Tetragon Projects is now an in-game agency, and may offer you contracts.
    * As a major contractor of the military-industrial complex, it takes failure very seriously, and is utterly amoral.

* Added ModuleTestSubject to applicable parts, able to be the target of a test flight contract.

* Corrected part costs for career.

* Parts rendered redundant by the above deleted, moved to deprecated.

* Implemented engine FX from HotRockets!, powered by SmokeScreen.
    * These have been customized somewhat.

* Some radially-attachable parts can now feed fuel into the root part thanks to CrossFeedEnabler.

* More variants of common fuselages have been added.

* All engines now use KM_Gimbal from the Space Shuttle Engines mod.

* RasterPropMonitor implemented, customized, built-into all cockpit IVAs.

* Larger HX parts use NodeToggle from Virgin Kalactic so that attaching stack nodes is reasonable.

* SABRE engines now use stock multi-mode engine module.

* ResGen and KineTechAnimation recompiled for 0.24.2.

* Added ModuleManager.

* All crafts updated for FAR/NEAR, new parts and rebalance.
* Added several new crafts.

* Added new HL cockpit.

* Added new MK2 crew cabin.

* Added IVAs for S2 crew cabins.
* Modified external mesh of S2 crew cabins.

* Tweaked other IVAs.

* Fixed reversed MK2 cargo bay animation.

* Numerous changes to attach nodes.

    * Some nodes have been removed. HL Extensions are now surface attached, or use the specialized stackable adapter.
    * Many other stack nodes had incorrect directions.
    * MK2 nodes adjusted to better line up with stock and SpacePlanePlus.
    * Many incorrect node sizes fixed.
    * Many incorrect node names fixed.

* Added a short (2m) MK2 bicoupler.

* Fixed missing colliders on MK2 cargo bay doors.

* H50 landing legs converted to use generic animations, will be converted to new landing leg module in the future.

* Modified SABRE meshes.

* Converted aerodynamic control surfaces to ModuleControlSurface.

* Adjusted rate for alternator modules to bring into line with new Squad values.

* Fix tweakable/flow flags for in-engine eCharge storage.

* Moved SABREs to the same research node as the RAPIER.

* Adjusted sound FX for most engines.

* Gimbal range increased on many engines.

* Refined thrust curves for jet engines, SABREs.

* Added sound FX to cargo bays and a few other animated parts.

* Redone landing gear suspension and traction values.
* Added landing gear sound and skid FX, thanks to FireSpitter.

* Fixed numerous minor issues causing warnings in log.

* All placeable lights can now change colour via tweakables.

* Many changes and clarifications to the documentation.

* Cleaned up FAR PartModule blocks, use new FAR ctrlSurfFrac parameter on control surfaces.

* Added monopropellant tanks to all cockpits, like stock.

* Ensured all cockpits and crew tanks have the appropriate science module parameters.

* Updated sensor packages for new functionality.

* Redone values for intake area black magic.

* Added autofairing to the L2 Atlas.

* Fixed drag on large airbrake.

* Fixed tweakablity of R1A AirCS resource.
* Fixed tweakablity of B9CompressedAir resource.
* Added unitCost for B9CompressedAir resource.

* Fixed spotlights lighting up planets.

* Fixed attach nodes on structural panels.

* Fixed a few errors with TGA textures.

* Normalized torque, mass, energy usage of reaction wheels in both dedicated SAS parts and cockpits/probes.

* Fixed stock drag of Vx1 nosecone.

* Added a new RCS block, the R6, a variant of the R5.

* Added flags to all cockpits and crew tanks.

* Added .version file for core mod, and embedded mods that provide one.

* Added Active Texture Management support.

* Added missing licenses.

* Added throttle animation to F119 turbofan.

* Added resourceFlowMode parameter to RCS blocks.

* Cleaned a lot of bogus stackSymmetry values from parts.

* Moved ladders and railings to Utility category.

* Added FSwheelAlignement PartModule to landing gears.

* Added effective intake area to intake descriptions.

* Fixed issues with CDP docking port collider interfering with root part.

* Added missing fuel gauges to SABREs.

* Added blur discs to commercial turbofans.

* Fixed D25 Tech pointing to a non-existent R&D node.

* R1A AirCS can now work in non-oxigenated atmospheres when used with Karbonite/KSPi intakes.

* Changed the key of the Info Drive to `o` to prevent interference with RCS.


_______________________________

▼ R4.0c

* Solved few inconsistencies in texture resolution of several parts (for example, MK2 cockpit exterior was using x2048 texture while fuselage sections were all x1024). Memory usage reduced by ~100mb.
* Added an optional low-resolution texture package where all x2048 textures are replaced with x1024 and all x1024 textures are replaced with x512. No x512 or x256 textures were modified as changing them will only save about 10-50mb, thus degrading visuals for no significant gain. To install it, overwrite the GameData folder in your KSP directory with GameData from the Textures_Reduced folder.
* Fixed the version number in the changelog. :)
* Reduced the the size of some additional textures (mostly emissives where the difference shouldn't be very noticeable).

_______________________________

▼ R4.0

* 0.22 compatibility update.
* Updated ExsurgentEngineering.dll, Firespitter.dll, KineTechAnimation.dll, ResGen.dll

* All textures converted to the uncompressed TGA format and will remain in it from this version onwards (previously, the PNG was used). Memory footprint in the game is unchanged. The benefits include sliced by approximately half, the compressed archive size being slightly smaller, and mip maps being generated properly by the engine.

* All parts are hooked into the tech tree. Parts aren't dumped to a single node, unlock gradually and, as possible, are arranged in a way that provides enough parts for you to build usable crafts at all stages of the tech tree progression. No SABREs at tier 3, no bizjets at tier 8, no shuttle control surfaces without shuttle wings and fuselages. All parts should also be compatible with the modded tech trees, as they usually preserve the original node names our configs are linked to.

* Added new landing gear parts that blend into the parent parts and fit a huge variety of aircraft. Choose between twin-wheeled and single-wheeled configuration, two different clearances and two different colors (allowing them to blend perfectly with all-purpose gray or plated black pieces). All landing gears are equipped with togglable electric motors and can be steered.

* Added new solid rocket separation boosters to replace the awkward stock one.

* Added the shielded variety of A1 light. It is mostly intended for use as the landing light, as new gear models, in contrast with stock ones, have no light sources to avoid breaking through per-pixel light limits and improve performance. Comes in two colors, same as the landing gear set.

* Added the 1.25m (MK1) aerodynamic junction part. Useful for nice transitions to radial engines, fancy boosters on rockets, and stylish UAV glider noses.

* Added new struts to finally retire the stock ones from every single craft. First variety is functionally identical to the stock one, just a bit more sturdy	* and quite better looking. Second variety is where it gets interesting: it's a strut part with an invisible middle piece. Exceptionally useful when you	* want a clean design without all the messy cables: basically, you just place two very suble blocks and imagine how structural integrity was improved somewhere inside the craft. Clean and performance-friendly. Oh, those new parts have no shadow casting, - that might help with FPS on heavy crafts which typically use hundreds of struts.

* Added MK2 cargoholds. Not very roomy, but they can still hold some useful payloads like 0.625m satellites or science modules, finally making the MK2 fuselage system useful for useful SSTO designs (!).

* Added the smaller 5-port omnidirectional RCS blocks in line with already existing linear and 12-port varieties. Added dark-colored versions of RCS parts to allow them to blend with the shielded areas of the fuselages.

* Added three new winglet sizes (2.3x1.6m, 3x2.2m, 3.2mx2.75m) and two new stabilator sizes (2.3x1.6m, 3x2.2m) to completely phase out any use of stock control surfaces from the crafts (2.3x1.6m is equivalent to the stock winglet in size). By the way, all 12 non-modular wing parts (stabilators, wingtips and winglets) in the mod are still using one single texture and maintain perfectly even texel density. No stretching, no duplicates, no distorted proportions.

* Added new SH wing module (1x4m) to close the last remaining gap in the system. Added the static trailing edge parts that can be added to the backside of SH modular wings, for the first time in history making control surfaces look passable without forcing you to cover all trailing edges with them. Seriously, those a very nice looking. Oh, and control surfaces no longer have a weird orange cross sections on the side.

* As separate ASAS/avionics parts became unnecessary since 0.21, all parts related to them (inline/radial/nosecones) are moved to the role of probe cores or sensor packages for the science system. Only exception is the plain MK2 nosecone, which is now decorative.

*  Added few small parts, in particular: a green omnidirectional light (crafts can finally have proper colored pair of navlights on their wings), large radial DSI intake that's close in performance to the RBM intake, large air brake with roughly 3 times the surface area of the old one,

* All sample crafts completely updated to make use of new struts, lights, landing gears, wing pieces and other parts. Some were significantly redesigned. New crafts added, including a lightweight MK2 SSTO, three maneuverable UAVs and a subsonic glider. Full list:

    * D-175 Strugatsky: Long-range, short-takeoff subsonic cargo transport.
    * BRV-4 Heinlein: High-performance heavy SSTO, trading off cargo transport capability for the generous fuel reserves.
    * I8-L Bradbury: High speed atmospheric crew transport.
    * SCDV Vonnegut: Advanced SSTO with the cargo transport capability.
    * TR7 van Vogt:  Light and fast MK2 SSTO with the cargo transport capability.
    * UAS-1 Barnard's Star: Extremely maneuverable lightweight UAV with thrust vectoring.
    * UAS-2 Polaris: Stable lightweight UAV.
    * UAS-3 Centauri: Extremely maneuverable MK2 UAV employing RCS and thrust vectoring instead of traditional control surfaces.
    * UL-1 Kornbluth: Subsonic personal transport with generous lift and convenient landing gear configuration.
    * VX-1 Vance: VTOL and RCS technology demonstrator, highly maneuverable and incredibly stable for it's class.
    * YF-28 Haldeman: Four-engine thrust vectoring fighter jet with extreme pitch and roll authority.

* Increased connection forces of the laggers and railings, fixed the crash tolerances.
* Added the specular maps to a few parts that lacked them.
* Revised the descriptions of multiple parts to be more informative and less so kerbal (RCS, radial control parts, etc.)
* Hypersonic mode on S3 cockpit returns and works as intended again.
* Balance changes to the SABRE Engines: Slightly less efficient air-breathing mode performance with greater thrust, slightly more efficient LFO mode with decreased thrust. Makes the engines less viable for prolonged gliding in the atmoshpere and more useful for space stuff different from barely circularizing in the LKO.
* Air consumption of the turbojet engine is increased.
* Action names on S2 crew part revised to fit into the fields.
* R1A air-powered RCS linear port ISP tweaked.
* VTOL engines rotation and steering inverted, enabled the velocity curve on VA1 and disabled the gimbal on it.
* Air brakes connection strength values added.
* Thrust vectoring engines are no longer trying to participate in roll control when they don't have a symmetrical counterpart.

_______________________________

▼ R3.3

* Compatibility patch for KSP 0.21.
* Added appropriate modules introduced by the patch to the pods
* ASAS/avionics module discontinued, all twin control parts are now using 0.21 SAS, differing only visually.
* SAS parts changed to work with 0.21 reaction wheel system, appropriate tweaks made, including changed names.
* Firespitter.dll updated to 5.3.1
* Turbofan engines updated to use new thrust-based spin mechanics.
* Example crafts updated to have electricity sources to account for 0.21 SAS and reaction wheels using it.
* Added an inline stack decoupler.

_______________________________

▼ R3.2

* Replaced ExsurgentEngineering.dll with a new version, now compatible with ModuleManager. Thanks to careo for the fix.

_______________________________

▼ R3.1c

* Hotfix for P4 & P8 structural panel configs.

_______________________________

▼ R3.1

* Fixed raised MK1 tail model to show up properly instead of straight one.
* Cargo bays should now be detected properly by FAR, shielding the contents and preventing cargo crafts with payloads from flying like bricks.
* Intake stats recalculated using a different formula (factoring the root of mass / 0.01 for unitScalar, and factoring (unitScalar*sqrt(coeff))/(unitscalar+1) into the speed). Nothing changed gameplay-wise, but that will fix improper flow reporting in KSP context windows. Nope, RBM intakes never were 10 times more powerful than others, KSP lied to you. It will continue to lie about air speed though.
* Fixed S2 Body 2m model.
* P4 and P8 reinforced structural panel nodes changed. While stock bug that forbids parenting with nodes on more than one axis is still present and unsolved, axis that works should be vertical now, which contains most useful nodes suitable for adapters and other stuff.
* SCDV Vonnegut example craft updated to use new wing models.
* S2W 3.75m adapter no longer contains LFO in structural version.
* SABRE M Precooler node sizes fixed.
* Included source files folder that was missing from R3.0c (uh-oh).

_______________________________

▼ R3.0c

* Fixed MK2 monopropellant tank fuel type.
* Fixed SABRE body parts attachment nodes and naming.
* Changed folder name to GameData to avoid issues on Linux.

_______________________________

▼ R3.0

* Total part count now at 175.
* Everything converted to be compliant with 0.20+ data system.
* Memory usage and disk space usage reduced more than x2.5 thanks to file referencing available in 0.20+.
* The mod now occupies only 73mb (!!!), less than R2.5 and less than stock parts. Loading is faster than for all stock parts combined.

* Mass, capacity and connection strength values on parts were updated using a better algorithm.
* Lift, drag, mass and connection strength values wings updated using a better algorithm. Stuff falls apart quite more hesitantly now.
* Vastly updated, improved and expanded selection of example crafts.

* Added TFE731 small subsonic high-bypass turbofan engine with surface-attached pylons in straight and raked variants.
* Added CF34 large subsonic high-bypass turbofan engine with surface-attached pylons in straight and raked variants. Perfect for heavy lifting, now used on D-175.
* Added D-30F7 turbojet engine. Perfect when brute force is required, now used on I8-L.
* Added F119 low-bypass turbofan featuring 2D thrust vectoring. Extreme control authority, used on new YF-28 example craft.
* Added SABRE Engines in 1.25m and 2.5m sizes, featuring plugin powered, separate twin modes of operation, and complementary structural and intake parts. Perfect engine for SSTO designs, used on several updated & new example crafts.

* Added VA1 VTOL jet engine, used on new VX-1 example craft.
* Added a matching nosecone part for VS1 VTOL engine and intake part for VA1 VTOL engine.
* VTOL Engines are now powered by Firespitter plugin allowing extensive and easy control over engine rotation.
* Where suitable (SABRE, ramp), intakes are using animated variable geometry dependent on air speed (powered by KineTechAnimation plugin).
* Vastly improved thrust vectoring module that takes into account all engines present on aircraft at once and controls each independently, allowing thrust vectoring to be used to assist in roll maneuvers (!!!) and overall improving stability and efficiency of thrust vectoring engines. Everything also returns to the neutral state upon thrust vectoring switch off, instead of getting stuck in the last position.

* MK2 cockpit IVA performance improved.
* S2 cockpit RCS ports now functional.
* Center of mass on all wings set to attachment node to improve stability.
* All animated parts are now using vastly improved generic animation module by Firespitter that allows in-editor toggling of state (e.g. for cargo bay doors), toggling in EVA where applicable (e.g. ladders), action names being displayed in action group editor, ability to reverse animation mid-playback, and much more.

* HL fuselage system :

    * Added 0.5m SAS.
    * Added 8m cargo hold.

* 7 new wing parts: large wingtips, winglets and stabilators.

* MK1 fuselage system:

    * Added structural & LF tank varieties in 2m and 5m (more convenient than stock pieces of quite weird length).
    * Added normal and raised tail sections.
    * Added inline ACU.
    * Added inline SAS.

* MK2 fuselage system:

    * Added MK2 to 2x1.25m bicoupler structural adapter.
    * Added inline RCS monopropellant tank.
    * Added inline SAS.
    * Added normal and raised tail sections.

* S2 fuselage system:

    * Added 2m & 6m Cargo bays.
    * Added inline SAS.

* S2W wide body fuselage system:

    * Added S2 to 4x1.25m adapter in structural and LFO versions.
    * Added S2 to 2x2.5m adapter in structural and LFO versions.
    * Added intake-less S2 to S2W adapter in structural and LFO versions.
    * S2W Intake Adapter airflow fixed.
    * S2W Intake Adapter drag fixed.
    * Added S2 to 3.75m adapter in structural version.
    * Added enormous S2W cargo bays in 2m and 6m versions.

* Rotated Radial ACU/ASAS so they can be told apart in Editor parts list.
* Added long-requested shielded docking port.
* Added aerodynamically shielded R12 RCS Thruster Block and R1 Linear RCS Port ports.
* Added R1A Air Thrust Nozzle that can be used in air-powered atmospheric reaction control system for VTOLs (powered by ResGen plugin).
* Added long-requested alternative pitot circular intake.
* Added surface-attached diverterless intake.
* Added two variable-geometry ramp intakes: both can be surface attached, one features engine mount on the back.
* Added intake/engine mount matching aforementioned intakes.
* Added airbrake part (tied to brake action group, can be rotated to variate the amount of drag produced, can be toggled or held open temporarily).
* Added radial RCS tanks in 2 sizes, matching new RCS blocks in style.
* Added triangular structural panels allowing for much more freedom in builds.
* Added A1 floodlight coming in as solo piece and in 4x & 8x arrays.
* Added configurable info drive part that allows you to write information about your crafts (action group descriptions, optimal ascent profiles, background stories) that can be displayed to anyone during flight. Used on every example craft.

_______________________________

▼ R2.5

* The notorious FAR bug with spontaneously exploding delta wings is solved completely (it turns out mesh colliders with triangular surfaces aren't supported).
* FAR configs merged with stock configs, no need to install them separately anymore.
* MK5 cockpit hatch obstruction issue completely fixed. Unfortunately, the solution involved offsetting the part, so you will have to reattach that cockpit on all your crafts. Fortunately, in contrast with wings, that would be easy to do.
* Further rebalancing of some parts.

* Updated existing example crafts:

    * D-175 Strugatsky heavy transport plane (docking clamp in the cargo hold, action group to close cargo doors, MK5 repositioned after, ACU unit)
    * BRV-4 Heinlein SSTO spaceplane (balance updates, additional control surfaces, ASAS unit)

* Added new example craft:
    * I8-L Bradbury, supersonic crew transport capable of taking 12 passengers on board.

* Added new parts:

    * Radial ASAS
    * Radial avionics (ACU)
    * MK2 ASAS nose cone
    * SH/SE 1m control surfaces

* Fixed the shading issues on S2 cockpit model.

_______________________________

▼ R2.4

* Complete rebalancing of part mass, crash tolerances, capacities and other parameters. Should result in much more sturdy and nice flying crafts.
* Warning! Wings structure had to be changed to solve the main source of issues with FAR. Point of origin was moved to attachment point for all wings (with the move compensated through CoMOffset parameter). Unfortunately, this can break existing modular wing setups you are using. Won't happen again.

* Added two example crafts:
    * D-175 Strugatsky heavy transport plane
    * BRV-4 Heinlein SSTO spaceplane

* Added new parts:
    * S2 widebody 1.25m engine mount (S2W->S2+4x1.25m adapter)
    * S2 large crew tank (6m, 6 seats)
    * S2 monopropellant tank
    * A65 2m semi-span delta wing (additional piece for the modular wing system)

* Restored C125 adapter that was mistakenly removed onwards from R2.
* Fixed large structural S2 fuselage section model, should no longer exhibit z-figthing issues.
* Fixed interior display emissive animation to be less noticeable.
* Texture improvements on some parts.

_______________________________

▼ R2.3

* Lots of config fixes to improve balance.
* Two raised part versions by Taverius (MK2 to 1.25m adapter and S2 tail section) which can be very useful to prevent tailstrikes

* New parts
    * New structural panels (2m & 1m)
    * Ladders and railings in 1m size

* Added a separate texture for the S2 LFO tank
* Fixed railing and S2 fuselage radial attachment orientation
* Fixed FAR HW21 config, and some other changes I have probably forgot

_______________________________

▼ R2.2

* Config fixes.
* Mesh collider fixes.

_______________________________

▼ R2.1

* Fixed some config errors (mismatched MK4 adapter node, wing naming, S2 LFO tank naming, HW21 lift values).
* First iteration of FAR-compliant configs by Taverius added to the package.
* New part: 1.25m to MK2 adapter.

_______________________________

▼ R2

* Second milestone release, 85 parts (6 pods, 3 IVAs).

_______________________________

▼ R1

* First release, 12 parts (2 pods, 1 IVA).
