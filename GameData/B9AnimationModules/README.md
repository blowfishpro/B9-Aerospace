# B9 Animation Modules

A Kerbal Space Program Plugin providing a few leightweight animation modules for the B9 Aerospace mod

B9 Aerospace can be found on the Kerbal Space Program Forum [here] (http://forum.kerbalspaceprogram.com/threads/92630)

## License

This work is distributed under the [GNU Lesser General Public License (LGPL) version 3.0] (http://www.gnu.org/licenses/lgpl-3.0.en.html)

Please note that other components of the B9 Aerospace mod are distributed under different license terms

## Contributors

* blowfish

## Included Modules

### ModuleB9AnimateBase

An abstract base class from which others derive.

#### Parameters

 * **animationName** - the name of the animation used.  If multiple animations have the same name it will find all of them
 * **responseSpeed** - how fast the animation responds to changes in the input (default is very fast)
 * **layer** - layer to set the animation on.  This only matters if there are multiple animations trying to control the same thing

### ModuleB9AnimateEngineMultiMode

*Derives from ModuleB9AnimateBase*

A module designed to animate the nozzle of multi-mode engines.  Allows variation based on throttle and mach.

#### Parameters

* **throttleCurvePrimary** - FloatCurve defining animation state vs throttle for the primary engine mode
* **throttleCurveSecondary** - FloatCurve defining animation state vs throttle for the secondary engine mode
* **machCurvePrimary** - FloatCurve defining animation state vs mach number for the primary engine mode - this is added to the throttle curve to get the final state
* **machCurveSecondary** - FloatCurve defining animation state vs mach number for the secondary engine mode - this is added to the throttle curve to get the final state
* **shutdownState** - animation state to use when the engine is shut down - default 0.0
* **idleState** - animation state to use when the engine throttle is below idleThreshold - default 1.0
* **idleThreshold** - throttle below which the engine is considered to be idling - default 0.01 (1% throttle)

### ModuleB9AnimateIntake

*Derives from ModuleB9AnimateBase*

A module that animates an intake based on mach number

#### Parameters

* **intakeClosedState** - animation state to set the intake to when it is closed - default 0.0
* **machCurve** - FloatCurve that defines animation state vs mach number when the intake is open

### ModuleB9AnimateThrottle

*Derives from ModuleB9AnimateBase*

A simple module designed to animate based off an engine throttle.  Replicates much of the functionality of the stock FXModuleAnimateThrottle, but fixes a bug that prevents the stock module from finding the animation if it is not the first one.

#### Parameters

* **engineID** - engineID of the engine module which should control this - if blank it will find the first engine module

## Changelog

### v1.0.4

* Update for KSP 1.2

### v1.0.3

* Recompile against KSP 1.1.3

### v1.0.2

* Recompile against KSP 1.1.2
* Various internal changes

### v1.0.1

* Recompile against KSP 1.1

### v1.0

* Initial release