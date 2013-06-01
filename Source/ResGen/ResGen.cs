using System.Collections.Generic;
using UnityEngine;
using System;

namespace ResGen {
public abstract class Module : PartModule {
	[KSPField] public ResField	INPUT;
	[KSPField] public ResField	OUTPUT;
	[KSPField] public ResField	BADINPUT;
	[KSPField] public ResField	BADOUTPUT;
	[KSPField] public double	rate			= 1;
	[KSPField] public double	update		= 0.1;
	[KSPField] public bool		useToggle		= false;
	[KSPField] public bool		externalToEVAOnly	= false;
	[KSPField] public bool		isActive		= true;
	[KSPField] public string	guiName		= "Toggle";

	// Reduce Calcs Per Second
	private double	TM_Flyhook	= 0;

	//Others
	protected	ConfigNode	node;
	protected	ConfigNode	tnode;

	// Globals
	private static Dictionary<string, ResList>	resList = new Dictionary<string, ResList>();

	[KSPEvent] public virtual void Toggle() {
		isActive = !isActive;
		Events["Toggle"].guiName = guiName + (isActive ? ": On" : ": Off");
	}
	// --- ON AWAKE ---
	public override void OnAwake() {
		ResList temp;

		if (!resList.ContainsKey(part.partName)) {
			temp		= new ResList();
			temp.inres	= new List<ResDat>();
			temp.ihres	= new List<ResDat>();
			temp.onres	= new List<ResDat>();
			temp.ohres	= new List<ResDat>();
			temp.binres	= new List<ResDat>();
			temp.bihres	= new List<ResDat>();
			temp.bonres	= new List<ResDat>();
			temp.bohres	= new List<ResDat>();
			resList.Add(part.partName, temp);
		}else temp = resList[part.partName];

		if (INPUT == null)	INPUT = new ResField(ref temp.inres, ref temp.ihres, ref node);
		if (OUTPUT== null)	OUTPUT= new ResField(ref temp.onres, ref temp.ohres, ref node);

		if (BADINPUT == null)	BADINPUT = new ResField(ref temp.binres, ref temp.bihres, ref node);
		if (BADOUTPUT== null)	BADOUTPUT= new ResField(ref temp.bonres, ref temp.bohres, ref node);

	}

	// --- ON LOAD ---
	public override void OnLoad(ConfigNode node) {
		if (!HighLogic.LoadedSceneIsFlight) return;
		ResList temp= resList[part.partName];
		INPUT.nres	= temp.inres;
		INPUT.hres	= temp.ihres;
		OUTPUT.nres	= temp.onres;
		OUTPUT.hres	= temp.ohres;
		BADINPUT.nres	= temp.binres;
		BADINPUT.hres	= temp.bihres;
		BADOUTPUT.nres	= temp.bonres;
		BADOUTPUT.hres	= temp.bohres;
		TM_Flyhook	= 0;

		if (useToggle) {
			Events["Toggle"].guiActive		= true;
			Events["Toggle"].active			= true;
			Events["Toggle"].externalToEVAOnly	= externalToEVAOnly;
			Events["Toggle"].guiName		= guiName + (isActive ? ": On" : ": Off");
		}

		if (node.HasNode("DATA")) {
			this.node = node.GetNode("DATA");
		}else	this.node = new ConfigNode("DATA");

		if (node.HasValue("isActive")) {
			isActive = bool.Parse(node.GetValue("isActive"));
		}
	}

	// --- SAVE ---
	public override void OnSave(ConfigNode node) {
		if (!HighLogic.LoadedSceneIsFlight) return;
		node.AddValue("isActive", isActive);
		node.AddNode(this.node);
	}

	// --- UPDATE ---
	public override void OnUpdate() {
		if (isActive) TM_Flyhook += TimeWarp.deltaTime;
		if (TM_Flyhook >= update) {
			double delta;
			ResField input = INPUT, output = OUTPUT;
			initRate(ref TM_Flyhook);
			if (TM_Flyhook <= 0) {
				TM_Flyhook = 0;
				return;
			}
			delta = TM_Flyhook;

			canGet(ref	delta, ref INPUT);
			print("ResGen: " + delta);
			if (delta <= 0) {
				delta = TM_Flyhook;
				input = BADINPUT;
				output = BADOUTPUT;
			print("ResGen: A " + delta);
				canGet(ref	delta, ref BADINPUT);
			print("ResGen: B " + delta);
				canSto(ref	delta, ref BADOUTPUT);
			print("ResGen: C " + delta);
			}else	canSto(ref	delta, ref OUTPUT);

			TM_Flyhook = 0;
			if (delta <= 0) return;

			foreach (ResDat resi in input.nres) {
				double needed = delta;
				getFRate(ref needed, resi);
				part.RequestResource(resi.id, needed);
			}
			foreach (ResDat resi in input.hres) {
				double needed = delta;
				getFRate(ref needed, resi);

				Resource res = ResourceLibrary.resourceDefinitions[resi.id];

/*
				if (!res.isAdvanced) {
				}else if (!node.HasNode(res.name)) {
					tnode = node.AddNode(res.name);
				}else tnode = node.GetNode(res.name);
*/
				res.RequestResource(ref node, part, needed);

/*
				if (res.isAdvanced) node.SetNode(res.name, tnode);
*/
			}

			foreach (ResDat resi in output.nres) {
				double needed = delta;
				stoFRate(ref needed, resi);
				part.RequestResource(resi.id, -needed);
			}
			foreach (ResDat resi in output.hres) {
				double needed = delta;
				stoFRate(ref needed, resi);
				Resource res = ResourceLibrary.resourceDefinitions[resi.id];

/*
				if (!res.isAdvanced) {
				}else if (!node.HasNode(res.name)) {
					tnode = node.AddNode(res.name);
				}else tnode = node.GetNode(res.name);
*/
				res.RequestResource(ref node, part, -needed);
/*
				if (res.isAdvanced) node.SetNode(res.name, tnode);
*/
			}
		}
	}
	protected abstract void initRate(ref double delta);
	protected abstract void getFRate(ref double delta, ResDat resi);
	protected abstract void getRRate(ref double delta, ResDat resi);
	protected abstract void stoFRate(ref double delta, ResDat resi);
	protected abstract void stoRRate(ref double delta, ResDat resi);

	private void canGet(ref double delta, ref ResField input) {
		foreach (ResDat resi in input.nres) {
			if (delta <= 0) break;
			double needed = delta;
			List<PartResource> RES_GRAB = new List<PartResource>();
			part.GetConnectedResources(resi.id, RES_GRAB);

			getFRate(ref needed, resi);
			foreach (PartResource res in RES_GRAB) {
				if (needed <= res.amount) {
					needed = 0;
					break;
				}else needed -= res.amount;
			}

			if (needed <= 0) continue;
			getRRate(ref needed, resi);
			delta -= needed;
		}
		foreach (ResDat resi in input.hres) {
			if (delta <= 0) break;
			double needed = delta, has = 0;
			Resource res = ResourceLibrary.resourceDefinitions[resi.id];
/*
			if (!res.isAdvanced) {
			}else if (!node.HasNode(res.name)) {
				tnode = node.AddNode(res.name);
			}else tnode = node.GetNode(res.name);
*/
			has = res.amount(ref node, part);

/*
			if (res.isAdvanced) node.SetNode(res.name, tnode);
*/
			getFRate(ref needed, resi);
			if (needed <= has) { continue; } else needed -= has;
			getRRate(ref needed, resi);
			delta -= needed;
		}
	}
	private void canSto(ref double delta, ref ResField output) {
		foreach (ResDat resi in output.nres) {
			if (delta <= 0) break;
			double needed = delta;
			List<PartResource> RES_GRAB = new List<PartResource>();
			part.GetConnectedResources(resi.id, RES_GRAB);

			stoFRate(ref needed, resi);
			foreach (PartResource res in RES_GRAB) {
				needed += res.amount;
				if (needed <= res.maxAmount) {
					needed = 0;
					break;
				}else needed -= res.maxAmount;
			}

			if (needed <= 0) continue;
			stoRRate(ref needed, resi);
			delta -= needed;
		}
		foreach (ResDat resi in output.hres) {
			if (delta <= 0) break;
			double needed = delta, has = 0;
			Resource res = ResourceLibrary.resourceDefinitions[resi.id];
			stoFRate(ref needed, resi);

/*
			if (!res.isAdvanced) {
			}else if (!node.HasNode(res.name)) {
				tnode = node.AddNode(res.name);
			}else tnode = node.GetNode(res.name);
*/
			has = res.maxAmount(ref node, part);
			needed += res.amount(ref node, part);

/*
			if (res.isAdvanced) node.SetNode(res.name, tnode);
*/

			if (needed <= has) { continue; } else needed -= has;
			stoRRate(ref needed, resi);
			delta -= needed;
		}
	}
	public class ResField : IConfigNode {
		public List<ResDat> nres;
		public List<ResDat> hres;
		public ConfigNode node;
		public ResField(ref List<ResDat> nres, ref List<ResDat> hres, ref ConfigNode node) {
			this.nres = nres;
			this.hres = hres;
			this.node = node;
		}
		public void Load(ConfigNode node) {
			if (node.HasValue("name") && node.HasValue("rate")) {
				ResDat resi	= new ResDat();
				resi.rate	= double.Parse(node.GetValue("rate"));
				string name = node.GetValue("name");
				{
					PartResourceDefinition temp = PartResourceLibrary.Instance.GetDefinition(name);
					if (temp != null) {
						resi.id = temp.id;
						nres.Add(resi);
						return;
					}
				}
				{
					Resource temp = ResourceLibrary.GetDefinition(name);
					if (temp != null) {
						temp.Load(ref node, ref this.node);
						resi.id = temp.id;
						hres.Add(resi);
						return;
					}
				}
			}
		}
		public void Save(ConfigNode node) {}
	}
	public struct ResDat {
		public int		id;
		public double	rate;
	}
	public struct ResList {
		public List<ResDat> inres;
		public List<ResDat> ihres;
		public List<ResDat> onres;
		public List<ResDat> ohres;
		public List<ResDat> binres;
		public List<ResDat> bihres;
		public List<ResDat> bonres;
		public List<ResDat> bohres;
	}
}
public abstract class Resource {
	public virtual double amount(ref ConfigNode node, Part part) {
		return 0;
	}
	public virtual double maxAmount(ref ConfigNode node, Part part) {
		return 0;
	}
	public virtual double RequestResource(ref ConfigNode node, Part part, double demand) {
		return 0;
	}
	public virtual void Load(ref ConfigNode resnode, ref ConfigNode partnode) {}

	public int		id;
	public string	name;
	public bool		isAdvanced;
	protected Resource() {
		name		= GetType().FullName;
		id		= name.GetHashCode();
	}
}
public class ResourceLibrary {
	public static Dictionary<int, Resource>	resourceDefinitions	= new Dictionary<int, Resource>();
	public static Dictionary<string, int>	resID				= new Dictionary<string, int>();
	public static string resourceExtension {
		get { return PartResourceLibrary.Instance.resourceExtension; }
	}
	public static string resourcePath {
		get { return PartResourceLibrary.Instance.resourcePath; }
	}
	public static Resource GetDefinition(int id) {
		return resourceDefinitions[id];
	}
	public static Resource GetDefinition(string name) {
		if (name == null) return null;
		{
			int id;
			if (resID.ContainsKey(name)) {
				id = resID[name];
			}else id = name.GetHashCode();
			if (resourceDefinitions.ContainsKey(id)) return resourceDefinitions[id];
		}
		{
			string asmName = name.Split(new Char[] {'.'}, 2)[0];
			if (asmName == null) return null;
			Type t = Type.GetType(name + ", " + asmName, false);
			if (t == null || t.BaseType != typeof(Resource)) return null;
			Resource resi = (Resource) Activator.CreateInstance(t);

			if (resourceDefinitions.ContainsKey(resi.id)) {
				while (resourceDefinitions.ContainsKey(++resi.id));
				resID.Add(name, resi.id);
			}
			resourceDefinitions.Add(resi.id, resi);
			return resi;
		}
	}
}

public class ResGenMultVesselCrew : Module {
	protected override void initRate(ref double delta) {
		delta *= rate;
		delta *= part.vessel.GetCrewCount();
	}
	protected override void getFRate(ref double delta, ResDat resi) {
		delta *= resi.rate;
		delta *= part.vessel.GetCrewCount();
	}
	protected override void getRRate(ref double delta, ResDat resi) {
		delta /= resi.rate;
		delta /= part.vessel.GetCrewCount();
	}
	protected override void stoFRate(ref double delta, ResDat resi) {
		delta *= resi.rate;
		delta *= part.vessel.GetCrewCount();
	}
	protected override void stoRRate(ref double delta, ResDat resi) {
		delta /= resi.rate;
		delta /= part.vessel.GetCrewCount();
	}
}
public class ResGenDivVesselCrew : Module {
	protected override void initRate(ref double delta) {
		delta /= rate;
		delta /= part.vessel.GetCrewCount();
	}
	protected override void getFRate(ref double delta, ResDat resi) {
		delta /= resi.rate;
		delta /= part.vessel.GetCrewCount();
	}
	protected override void getRRate(ref double delta, ResDat resi) {
		delta *= resi.rate;
		delta *= part.vessel.GetCrewCount();
	}
	protected override void stoFRate(ref double delta, ResDat resi) {
		delta /= resi.rate;
		delta /= part.vessel.GetCrewCount();
	}
	protected override void stoRRate(ref double delta, ResDat resi) {
		delta *= resi.rate;
		delta *= part.vessel.GetCrewCount();
	}
}
public class ResGenDivPartCrew : Module {
	protected override void initRate(ref double delta) {
		delta /= rate;
		delta /= part.protoModuleCrew.Count;
	}
	protected override void getFRate(ref double delta, ResDat resi) {
		delta /= resi.rate;
		delta /= part.protoModuleCrew.Count;
	}
	protected override void getRRate(ref double delta, ResDat resi) {
		delta *= resi.rate;
		delta *= part.protoModuleCrew.Count;
	}
	protected override void stoFRate(ref double delta, ResDat resi) {
		delta /= resi.rate;
		delta /= part.protoModuleCrew.Count;
	}
	protected override void stoRRate(ref double delta, ResDat resi) {
		delta *= resi.rate;
		delta *= part.protoModuleCrew.Count;
	}
}
public class ResGenMultPartCrew : Module {
	protected override void initRate(ref double delta) {
		delta *= rate;
		delta *= part.protoModuleCrew.Count;
	}
	protected override void getFRate(ref double delta, ResDat resi) {
		delta *= resi.rate;
		delta *= part.protoModuleCrew.Count;
	}
	protected override void getRRate(ref double delta, ResDat resi) {
		delta /= resi.rate;
		delta /= part.protoModuleCrew.Count;
	}
	protected override void stoFRate(ref double delta, ResDat resi) {
		delta *= resi.rate;
		delta *= part.protoModuleCrew.Count;
	}
	protected override void stoRRate(ref double delta, ResDat resi) {
		delta /= resi.rate;
		delta /= part.protoModuleCrew.Count;
	}
}
public class ResGen : Module {
	protected override void initRate(ref double delta) {
		delta *= rate;
	}
	protected override void getFRate(ref double delta, ResDat resi) {
		delta *= resi.rate;
	}
	protected override void getRRate(ref double delta, ResDat resi) {
		delta /= resi.rate;
	}
	protected override void stoFRate(ref double delta, ResDat resi) {
		delta *= resi.rate;
	}
	protected override void stoRRate(ref double delta, ResDat resi) {
		delta /= resi.rate;
	}
}

public class PartCrew : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return part.protoModuleCrew.Count;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		return part.protoModuleCrew.Capacity;
	}
}
public class VesselCrew : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return part.vessel.GetCrewCount();
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		return part.vessel.GetCrewCapacity();
	}
}
public class AtmosphereHasOxygen : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return (FlightGlobals.currentMainBody.atmosphereContainsOxygen) ? 5 : 0;
	}
}
public class AtmosphericOxygen : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return (FlightGlobals.currentMainBody.atmosphereContainsOxygen) ? part.vessel.atmDensity : 0;
	}
}
public class PlanetWater : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return (part.vessel.Splashed) ? 5 : 0;
	}
}
public class GeeForce : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return part.vessel.geeForce;
	}
}
public class Explode : Resource {
	public override double maxAmount(ref ConfigNode node, Part part) {
		return 5;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		part.explode();
		return demand;
	}
}
public class KillKerbalInPart : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return part.protoModuleCrew.Count;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		return part.protoModuleCrew.Capacity;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		ProtoCrewMember Kerbal = part.protoModuleCrew[0];
		part.RemoveCrewmember(Kerbal);
		Kerbal.Die();
		return demand;
	}
}
public class KillKerbalInVessel : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return part.vessel.GetCrewCount();
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		return part.vessel.GetCrewCapacity();
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		ProtoCrewMember Kerbal = part.vessel.GetVesselCrew()[0];
		Kerbal.KerbalRef.InPart.RemoveCrewmember(Kerbal);
		Kerbal.Die();
		return demand;
	}
}
public class KillPartCrew : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return (part.protoModuleCrew.Count > 1) ? 5 : 0;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		return (part.protoModuleCrew.Count > 1) ? 5 : 0;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		foreach (ProtoCrewMember Kerbal in part.protoModuleCrew) {
			part.RemoveCrewmember(Kerbal);
			Kerbal.Die();
		}
		return demand;
	}
}
public class KillVesselCrew : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return (part.vessel.GetCrewCount() > 1) ? 5 : 0;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		return (part.vessel.GetCrewCount() > 1) ? 5 : 0;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		foreach (ProtoCrewMember Kerbal in part.vessel.GetVesselCrew()) {
			Kerbal.KerbalRef.InPart.RemoveCrewmember(Kerbal);
			Kerbal.Die();
		}
		return demand;
	}
}
public class OnPlanet : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		if (part.vessel.Landed || part.vessel.Splashed) {
			if (node.HasValue("planet") && part.vessel.mainBody.name == node.GetValue("planet")) {
				return 5;
			}
		}
		return 0;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("planet")) {
			string planetname = resnode.GetValue("planet");
			foreach (CelestialBody planet in FlightGlobals.fetch.bodies) {
				if (planetname == planet.name) {
					ConfigNode temp = new ConfigNode(name);
					temp.AddValue("planet",planetname);
					partnode.AddNode(temp);
					break;
				}
			}
		}
	}
	public OnPlanet() { isAdvanced = true; }
}
public class Sunlight : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		float lastUpdate = 0, lastValue = 0;
		if (node.HasValue("LastUpdate")) {
			lastUpdate	= float.Parse(node.GetValue("LastUpdate"));
			lastValue	= float.Parse(node.GetValue("LastValue"));
		}else{
			node.AddValue("LastUpdate", (Time.time + 0.5f).ToString());
			node.AddValue("LastValue", "0");
		}
		if (Time.time > lastUpdate) {
			Vector3 sun		= Planetarium.fetch.Sun.position - part.orgPos;
			sun.Normalize();	sun *= 1000;
			lastValue	= Physics.Linecast(part.orgPos, sun) ? 0 : 5;
			node.SetValue("LastUpdate", (Time.time + 0.5f).ToString());
			node.SetValue("LastValue", lastValue.ToString());
		}
		return lastValue;
	}
	public Sunlight() { isAdvanced = true; }
}}
