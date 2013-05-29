using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// TODO: render stack icons. some based regular fuel levels, others like 
//    intake air do as % of max
public class HydraEngineController : PartModule
{
	public VInfoBox meter;

	public VInfoBox Meter {
		get {
			if (meter != null)
				return meter;
			return SetupMeter ();
		}
       
	}

	[KSPField(isPersistant=false)]
	public ConfigNode
		primaryEngine;

	[KSPField(isPersistant=false)]
	public ConfigNode
		secondaryEngine;

	[KSPField(isPersistant=false)]
	public string
		primaryModeName = "Primary";

	[KSPField(isPersistant=false)]
	public string
		secondaryModeName = "Secondary";

	[KSPField(guiActive=true, isPersistant=true, guiName="Current Mode")]
	public string
		currentMode;

	[KSPAction("Switch Engine Mode")]
	public void SwitchAction (KSPActionParam param)
	{
		SwitchEngine ();
	}

	public override void OnAwake ()
	{
//		Debug.Log ("OnAwake()");
	}

	public override void OnActive ()
	{
//		Debug.Log ("OnActive()");
	}

	public override void OnStart (StartState state)
	{
//		Debug.Log (String.Format ("OnActive({0})", state));
	}

	public override void OnInactive ()
	{
//		Debug.Log ("OnInactive()");
	}

	void Log (string format, params object[] args)
	{
		Debug.Log (String.Format (format, args));

	}

	public override void OnLoad (ConfigNode node)
	{
//		Debug.Log ("OnLoad()");

//		Debug.Log ("ActiveEngine: " + ActiveEngine);
//		Debug.Log ("current: " + current);
   
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
				part.AddModule (primaryEngine);
			else
				part.AddModule (secondaryEngine);

		}
	}

	public ModuleEngines ActiveEngine {
		get { return (ModuleEngines)part.Modules ["ModuleEngines"]; }

	}

	[KSPEvent(guiActive=true, guiName="Switch Engine Mode")]
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

		// swap data

		var engineActive = ActiveEngine.getIgnitionState;

		//ActiveEngine.Actions ["ShutdownAction"].Invoke(new KSPActionParam(KSPActionGroup.None, KSPActionType.Activate));

		ActiveEngine.propellants = new List<ModuleEngines.Propellant> ();


		if (meter != null) {
			part.stackIcon.RemoveInfo (meter);
			meter = null;

		}
       
//        part.stackIcon.ClearInfoBoxes();

		ActiveEngine.velocityCurve = new FloatCurve ();
		ActiveEngine.atmosphereCurve = new FloatCurve ();

		//ActiveEngine.propellants.Clear();
		//ActiveEngine.velocityCurve = new FloatCurve();
		//ActiveEngine.atmosphereCurve = new FloatCurve();

		//ActiveEngine.OnAwake();
		ActiveEngine.Fields.Load (nextEngine);

		if (nextEngine.HasValue ("useVelocityCurve") && (nextEngine.GetValue ("useVelocityCurve").ToLowerInvariant () == "true")) {
			ActiveEngine.velocityCurve.Load (nextEngine.GetNode ("velocityCurve"));
		} else {
			ActiveEngine.useVelocityCurve = false;
		}

		foreach (ConfigNode n in nextEngine.nodes) {
			if (n.name == "PROPELLANT") {
				var prop = new ModuleEngines.Propellant ();
				prop.Load (n);
				ActiveEngine.propellants.Add (prop);
			}
		}

		ActiveEngine.SetupPropellant ();
		//var vboxinfo = part.stackIcon.DisplayInfo();
		//ActiveEngine.OnStart(PartModule.StartState.PreLaunch);
		//ActiveEngine.Actions ["ActivateAction"].Invoke(new KSPActionParam(KSPActionGroup.None, KSPActionType.Activate));


		if (engineActive)
			ActiveEngine.Actions ["ActivateAction"].Invoke (new KSPActionParam (KSPActionGroup.None, KSPActionType.Activate));
        
	}

	public void FixedUpdate ()
	{
		if (HighLogic.LoadedSceneIsEditor)
			return;

		if (ActiveEngine == null)
			return;
		//LogEngineStats();
		UpdateStackIcon ();

		//CheckPropellantUse();


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
        


		// linear scale
		var value = (currentAmt - minAmt) / (maxAmt - minAmt);

		// log scale? maybe?

		//var value = Math.Log(currentAmt - minAmt) / Math.Log(maxAmt - minAmt);


		Meter.SetValue ((float)value);
	}

	void LogEngineStats ()
	{
		var msg = "\nengine stats:\n";
		msg += String.Format ("• finalThrust: {0}\n", ActiveEngine.finalThrust);
		msg += String.Format ("• requestedThrust: {0}\n", ActiveEngine.requestedThrust);
		var missing = ActiveEngine.requestedThrust - ActiveEngine.finalThrust;
		msg += String.Format ("• thrust lost: {0}\n", missing);
		Debug.Log (msg);

	}
//	void CheckPropellantUse ()
//	{
//
////		var msg = "";
//		foreach (var propellant in ActiveEngine.propellants) {
//
//			//                    propellant.name, propellant.currentRequirement, propellant.currentAmount, propellant.isDeprived);
//			var missing = propellant.currentRequirement - propellant.currentAmount;
////			msg += String.Format ("propellant: {0} missing:{1}", propellant.name, missing);
//            
//		}
////		Debug.Log (msg);
//    
//	}
}

