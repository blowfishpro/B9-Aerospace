using System;
using System.Collections.Generic;
using System.Linq;

// TODO: render stack icons. some based regular fuel levels, others like
//    intake air do as % of max
public class HydraEngineController : PartModule
{
	public VInfoBox meter;

	public VInfoBox Meter {
		get {
			return meter ?? SetupMeter ();
		}
       
	}

	[KSPField (isPersistant = false)]
	public ConfigNode
		primaryEngine;

	[KSPField (isPersistant = false)]
	public ConfigNode
		secondaryEngine;

	[KSPField (isPersistant = false)]
	public string
		primaryModeName = "Primary";

	[KSPField (isPersistant = false)]
	public string
		secondaryModeName = "Secondary";

	[KSPField (guiActive = true, isPersistant = true, guiName = "Current Mode")]
	public string
		currentMode;

	[KSPAction ("Switch Engine Mode")]
	public void SwitchAction (KSPActionParam param)
	{
		SwitchEngine ();
	}

	public override void OnLoad (ConfigNode node)
	{

		if (node.HasNode ("primaryEngine")) {
			primaryEngine = node.GetNode ("primaryEngine");
			secondaryEngine = node.GetNode ("secondaryEngine");
		} else {
			var prefab = (HydraEngineController)part.partInfo.partPrefab.Modules ["HydraEngineController"];
			primaryEngine = prefab.primaryEngine;
			secondaryEngine = prefab.secondaryEngine;
		}
		if (currentMode == null) {
			currentMode = primaryModeName;
		}
		if (ActiveEngine == null) {
			if (currentMode == primaryModeName)
				AddEngine (primaryEngine);
			else
				AddEngine (secondaryEngine);
		}
	}

	bool AddEngine (ConfigNode engineConfig)
	{
		var module = (ModuleEngines)part.AddModule ("ModuleEngines");
		module.Load (engineConfig);
		return true;
	}

	public ModuleEngines ActiveEngine {
		get { return (ModuleEngines)part.Modules ["ModuleEngines"]; }
	}

	[KSPEvent (guiActive = true, guiName = "Switch Engine Mode")]
	public void SwitchEngine ()
	{
		ConfigNode nextEngine;
		if (currentMode == primaryModeName) {
			currentMode = secondaryModeName;
			nextEngine = secondaryEngine;
		} else {
			currentMode = primaryModeName;
			nextEngine = primaryEngine;
		}

		var engineActive = ActiveEngine.getIgnitionState;
		
		ActiveEngine.propellants = new List<Propellant> ();

		if (meter != null) {
			part.stackIcon.RemoveInfo (meter);
			meter = null;
		}
       
		ActiveEngine.velocityCurve = new FloatCurve ();
		ActiveEngine.atmosphereCurve = new FloatCurve ();

		ActiveEngine.Fields.Load (nextEngine);
	
		if (nextEngine.HasValue ("useVelocityCurve") && (nextEngine.GetValue ("useVelocityCurve").ToLowerInvariant () == "true")) {
			ActiveEngine.velocityCurve.Load (nextEngine.GetNode ("velocityCurve"));
		} else {
			ActiveEngine.useVelocityCurve = false;
		}

		foreach (ConfigNode n in nextEngine.nodes) {
			if (n.name == "PROPELLANT") {
				var prop = new Propellant ();
				prop.Load (n);
				ActiveEngine.propellants.Add (prop);
			}
		}

		ActiveEngine.SetupPropellant ();

		if (engineActive)
			ActiveEngine.Actions ["ActivateAction"].Invoke (new KSPActionParam (KSPActionGroup.None, KSPActionType.Activate));
	}

	public override void OnFixedUpdate ()
	{
		if (HighLogic.LoadedSceneIsEditor)
			return;

		if (ActiveEngine == null)
			return;
		UpdateStackIcon ();
	}

	VInfoBox SetupMeter ()
	{
		meter = part.stackIcon.DisplayInfo ();
		meter.SetMessage ("Air");
		meter.SetProgressBarColor (XKCDColors.White);
		meter.SetProgressBarBgColor (XKCDColors.Grey);
		meter.SetLength (1f);
		meter.SetValue (0f);

		return meter;
	}

	void UpdateStackIcon ()
	{
		var propellant = ActiveEngine.propellants.Find (prop => prop.name == "IntakeAir");
		if (propellant == null)
			return;

		var sources = new List<PartResource> ();
		part.GetConnectedResources (propellant.id, sources);

		var maxAmt = 0d;
		var currentAmt = 0d;
		foreach (var source in sources) {
			maxAmt += source.maxAmount;
			currentAmt += source.amount;
		}

		var minAmt = (from modules in vessel.Parts
		              from module in modules.Modules.OfType<ModuleEngines> ()
		              from p in module.propellants
		              where p.name == "IntakeAir"
		              select module.ignitionThreshold * p.currentRequirement).Sum ();
        
		var value = (currentAmt - minAmt) / (maxAmt - minAmt);

		Meter.SetValue ((float)value);
	}
}

