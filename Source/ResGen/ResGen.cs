using System.Collections.Generic;
using UnityEngine;
using System;

namespace ResGen {
public abstract class Module : PartModule {
	[KSPField] public ResField	INPUT;
	[KSPField] public ResField	OUTPUT;
	[KSPField] public ResField	BADINPUT;
	[KSPField] public ResField	BADOUTPUT;
	[KSPField] public ElseNode	ELSE;
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
	protected	bool		useElse;
	protected	bool		hasElse;
	protected	bool		hasBad;

	[KSPEvent] public virtual void Toggle() {
		isActive = !isActive;
		Events["Toggle"].guiName = guiName + (isActive ? ": On" : ": Off");
	}
	// --- ON AWAKE ---
	public override void OnAwake() {
		ResList temp;

		if (!ResourceLibrary.resList.ContainsKey(part.partName)) {
			temp		= new ResList();
			temp.inres	= new List<ResDat>();
			temp.ihres	= new List<ResDat>();
			temp.onres	= new List<ResDat>();
			temp.ohres	= new List<ResDat>();
			temp.binres	= new List<ResDat>();
			temp.bihres	= new List<ResDat>();
			temp.bonres	= new List<ResDat>();
			temp.bohres	= new List<ResDat>();
			ResourceLibrary.resList.Add(part.partName, temp);
		}else temp = ResourceLibrary.resList[part.partName];

		if (INPUT == null)	INPUT = new ResField(ref temp.inres, ref temp.ihres, part);
		if (OUTPUT== null)	OUTPUT= new ResField(ref temp.onres, ref temp.ohres, part);

		if (BADINPUT == null)	BADINPUT = new ResField(ref temp.binres, ref temp.bihres, part);
		if (BADOUTPUT== null)	BADOUTPUT= new ResField(ref temp.bonres, ref temp.bohres, part);

		if (ELSE	== null)	ELSE= new ElseNode(part);
	}

	// --- ON LOAD ---
	public override void OnLoad(ConfigNode node) {
		if (!HighLogic.LoadedSceneIsFlight) return;
		if (node.HasValue("hasElse")) {
			hasElse = bool.Parse(node.GetValue("hasElse"));
		}else	hasElse = ResourceLibrary.elseList.ContainsKey(part.partName);

		if (node.HasValue("useElse")) {
			useElse = bool.Parse(node.GetValue("useElse"));
		}else	useElse = false;

		ResList temp	= useElse ? ResourceLibrary.elseList[part.partName] : ResourceLibrary.resList[part.partName];
		INPUT.nres		= temp.inres;
		INPUT.hres		= temp.ihres;
		OUTPUT.nres		= temp.onres;
		OUTPUT.hres		= temp.ohres;
		BADINPUT.nres	= temp.binres;
		BADINPUT.hres	= temp.bihres;
		BADOUTPUT.nres	= temp.bonres;
		BADOUTPUT.hres	= temp.bohres;
		TM_Flyhook		= 0;

		hasBad = (temp.binres.Count > 0) || (temp.bihres.Count > 0) || (temp.bonres.Count > 0) || (temp.bohres.Count > 0);

		if (useToggle) {
			Events["Toggle"].guiActive		= true;
			Events["Toggle"].active			= true;
			Events["Toggle"].externalToEVAOnly	= externalToEVAOnly;
			Events["Toggle"].guiName		= guiName + (isActive ? ": On" : ": Off");
		}

		if (node.HasNode("DATA")) {
			this.node = node.GetNode("DATA");
		}else{
			this.node = new ConfigNode("DATA");
			INPUT.OnLoad(ref this.node);
			OUTPUT.OnLoad(ref this.node);
			BADINPUT.OnLoad(ref this.node);
			BADOUTPUT.OnLoad(ref this.node);
			node.AddNode(this.node);
		}
		if (node.HasValue("isActive")) {
			isActive = bool.Parse(node.GetValue("isActive"));
		}
	}

	// --- SAVE ---
	public override void OnSave(ConfigNode node) {
		if (!HighLogic.LoadedSceneIsFlight) return;

		if (hasElse && useElse) {
			hasElse = false;
			node.ClearValues();
			ResourceLibrary.elseNode[part.partName].CopyTo(node);
		}
		node.AddValue("isActive",	isActive);
		node.AddValue("useElse",	useElse);
		node.AddValue("hasElse",	hasElse);
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
			if (delta <= 0) {
				if (hasBad) {
					delta = TM_Flyhook;
					input = BADINPUT;
					output = BADOUTPUT;
					canGet(ref	delta, ref BADINPUT);
					if (delta <= 0) {
						if (hasElse) OnLoad(ResourceLibrary.elseNode[part.partName]);
						return;
					}
					canSto(ref	delta, ref BADOUTPUT);
				}else{
					if (hasElse) OnLoad(ResourceLibrary.elseNode[part.partName]);
					return;
				}
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
				ConfigNode	tnode;
				getFRate(ref needed, resi);

				Resource res = ResourceLibrary.resourceDefinitions[resi.id];

				if (!res.isAdvanced) {
					tnode = node;
				}else if (!node.HasNode(res.name + resi.count)) {
					tnode = node.AddNode(res.name + resi.count);
				}else tnode = node.GetNode(res.name + resi.count);

				res.RequestResource(ref tnode, part, needed);
			}

			foreach (ResDat resi in output.nres) {
				double needed = delta;
				stoFRate(ref needed, resi);
				part.RequestResource(resi.id, -needed);
			}
			foreach (ResDat resi in output.hres) {
				double needed = delta;
				ConfigNode	tnode;
				stoFRate(ref needed, resi);
				Resource res = ResourceLibrary.resourceDefinitions[resi.id];

				if (!res.isAdvanced) {
					tnode = node;
				}else if (!node.HasNode(res.name + resi.count)) {
					tnode = node.AddNode(res.name + resi.count);
				}else tnode = node.GetNode(res.name + resi.count);

				res.RequestResource(ref tnode, part, -needed);
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
			ConfigNode	tnode;
			Resource res = ResourceLibrary.resourceDefinitions[resi.id];

			if (!res.isAdvanced) {
				tnode = node;
			}else if (!node.HasNode(res.name + resi.count)) {
				tnode = node.AddNode(res.name + resi.count);
			}else tnode = node.GetNode(res.name + resi.count);
			has = res.amount(ref tnode, part);

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
			ConfigNode	tnode;
			Resource res = ResourceLibrary.resourceDefinitions[resi.id];
			stoFRate(ref needed, resi);

			if (!res.isAdvanced) {
				tnode = node;
			}else if (!node.HasNode(res.name + resi.count)) {
				tnode = node.AddNode(res.name + resi.count);
			}else tnode = node.GetNode(res.name + resi.count);

			has = res.maxAmount(ref tnode, part);
			needed += res.amount(ref tnode, part);

			if (needed <= has) { continue; } else needed -= has;
			stoRRate(ref needed, resi);
			delta -= needed;
		}
	}
	public class ResField : IConfigNode {
		public List<ResDat> nres;
		public List<ResDat> hres;
		public Part part;
		public ResField(ref List<ResDat> nres, ref List<ResDat> hres, Part part) {
			this.nres = nres;
			this.hres = hres;
			this.part = part;
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
						ResourceLibrary.Load(ref temp, ref part.partName, ref node, ref resi);
						resi.id = temp.id;
						hres.Add(resi);
						return;
					}
				}
			}
		}
		public void Save(ConfigNode node) {}
		public void OnLoad(ref ConfigNode partnode) {
			ConfigNode[] nodes;
			foreach (ResDat resi in hres) {
				if (ResourceLibrary.configNodes.ContainsKey(resi.id) && ResourceLibrary.configNodes[resi.id].HasNode(part.partName)) {
					nodes = ResourceLibrary.configNodes[resi.id].GetNodes(part.partName);
					nodes[resi.count].CopyTo(partnode.AddNode(ResourceLibrary.resourceDefinitions[resi.id].name + resi.count));
				}
			}
		}
	}
	public class ElseNode : IConfigNode {
		private Part part;
		public ElseNode(Part part) {
			this.part	  = part;
		}
		public void Load(ConfigNode node) {
			if (!ResourceLibrary.elseList.ContainsKey(part.partName)) {
				ResList temp= new ResList();
				temp.inres	= new List<ResDat>();
				temp.ihres	= new List<ResDat>();
				temp.onres	= new List<ResDat>();
				temp.ohres	= new List<ResDat>();
				temp.binres	= new List<ResDat>();
				temp.bihres	= new List<ResDat>();
				temp.bonres	= new List<ResDat>();
				temp.bohres	= new List<ResDat>();
				ResourceLibrary.elseList.Add(part.partName, temp);

				ResField INPUT		= new ResField(ref temp.inres,	ref temp.ihres,	part);
				ResField OUTPUT		= new ResField(ref temp.onres,	ref temp.ohres,	part);
				ResField BADINPUT		= new ResField(ref temp.binres,	ref temp.bihres,	part);
				ResField BADOUTPUT	= new ResField(ref temp.bonres,	ref temp.bohres,	part);

				foreach (ConfigNode n in node.GetNodes("INPUT"))	INPUT		.Load(n);
				foreach (ConfigNode n in node.GetNodes("OUTPUT"))	OUTPUT	.Load(n);
				foreach (ConfigNode n in node.GetNodes("BADINPUT"))	BADINPUT	.Load(n);
				foreach (ConfigNode n in node.GetNodes("BADOUTPUT"))	BADOUTPUT	.Load(n);
				node.ClearNodes();
				node.AddValue("useElse", true);
				node.AddValue("hasElse", false);
				ResourceLibrary.elseNode.Add(part.partName, node);
			}
		}
		public void Save(ConfigNode node) {}
	}

	public struct ResDat {
		public int		id;
		public double	rate;
		public uint		count;
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
	public static Dictionary<string,	Module.ResList>	resList			= new Dictionary<string,	Module.ResList>();
	public static Dictionary<string,	Module.ResList>	elseList			= new Dictionary<string,	Module.ResList>();
	public static Dictionary<string,	ConfigNode>		elseNode			= new Dictionary<string,	ConfigNode>();
	public static Dictionary<int,		ConfigNode>		configNodes			= new Dictionary<int,		ConfigNode>();
	public static Dictionary<int,		ConfigNode>		globalNodes			= new Dictionary<int,		ConfigNode>();
	public static Dictionary<int,		Resource>		resourceDefinitions	= new Dictionary<int,		Resource>();
	public static Dictionary<string,	int>			resID				= new Dictionary<string,	int>();
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
	public static void Load(ref Resource resi, ref string partName, ref ConfigNode resnode, ref Module.ResDat res) {
		ConfigNode node;
		ConfigNode[] nodes;

		if (!configNodes.ContainsKey(resi.id)) {
			node = new ConfigNode("DATA");
			configNodes.Add(resi.id, node);
		}else	node = configNodes[resi.id];

		ConfigNode temp = new ConfigNode(partName);
		resi.Load(ref resnode, ref temp);
		if (temp.values.Count > 0) node.AddNode(temp);
		nodes = node.GetNodes(partName);
		if (nodes.Length > 0) res.count = (uint) nodes.Length - 1;
		else res.count = 0;
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
		return (FlightGlobals.currentMainBody.atmosphereContainsOxygen) ? TimeWarp.CurrentRate : 0;
	}
}
public class Temperature : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return part.temperature * TimeWarp.CurrentRate;
	}
}
public class AtmosphericOxygen : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return (FlightGlobals.currentMainBody.atmosphereContainsOxygen) ? part.vessel.atmDensity : 0;
	}
}
public class PlanetWater : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return (part.vessel.Splashed) ? TimeWarp.CurrentRate : 0;
	}
}
public class GeeForce : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return part.vessel.geeForce;
	}
}
public class Explode : Resource {
	public override double maxAmount(ref ConfigNode node, Part part) {
		return TimeWarp.CurrentRate;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		part.explode();
		return demand;
	}
}
public class Detonate : Resource {
	public override double maxAmount(ref ConfigNode node, Part part) {
		return TimeWarp.CurrentRate;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {

	//apply some kind of physics force into the center of the vessel?
/*
		if (node.HasValue("size")) {
			size = int.Parse(node.GetValue("size"));
		}else size = 1;
*/

		part.explosionPotential = 1000;
		part.maxTemp = -1000;
//		part.explode();
		return demand;
	}
/*
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("size")) {
			string size = resnode.GetValue("size");
			partnode.AddValue("size", size);
		}
	}
	public Detonate() { isAdvanced = true; }
*/
}
public class KillKerbalInPart : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		double setTime = 0, timeSave = 0;
		if (node.HasValue("setTime")) {
			setTime	= double.Parse(node.GetValue("setTime"));
		}else	setTime	= 1;

		if (node.HasValue("TimeSave")) {
			timeSave	= double.Parse(node.GetValue("TimeSave"));
		}else	node.AddValue("TimeSave", (Planetarium.GetUniversalTime() + setTime).ToString());

		if (Planetarium.GetUniversalTime() > timeSave) {
			node.SetValue("TimeSave", (Planetarium.GetUniversalTime() + setTime).ToString());
			return part.protoModuleCrew.Count;
		}
		return 0;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		double setTime = 0, timeSave = 0;
		if (node.HasValue("setTime")) {
			setTime	= double.Parse(node.GetValue("setTime"));
		}else	setTime	= 1;

		if (node.HasValue("TimeSave")) {
			timeSave	= double.Parse(node.GetValue("TimeSave"));
		}else	node.AddValue("TimeSave", (Planetarium.GetUniversalTime() + setTime).ToString());

		if (Planetarium.GetUniversalTime() > timeSave) {
			node.SetValue("TimeSave", (Planetarium.GetUniversalTime() + setTime).ToString());
			return part.protoModuleCrew.Count * 2;
		}
		return 0;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		ProtoCrewMember Kerbal = part.protoModuleCrew[0];
		part.RemoveCrewmember(Kerbal);
		Kerbal.Die();
		return demand;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("setTime")) {
			string setTime = resnode.GetValue("setTime");
			partnode.AddValue("setTime", setTime);
		}
	}
	public KillKerbalInPart() { isAdvanced = true; }
}
public class KillKerbalInVessel : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		double setTime = 0, timeSave = 0;
		if (node.HasValue("setTime")) {
			setTime	= double.Parse(node.GetValue("setTime"));
		}else	setTime	= 1;

		if (node.HasValue("TimeSave")) {
			timeSave	= double.Parse(node.GetValue("TimeSave"));
		}else	node.AddValue("TimeSave", (Planetarium.GetUniversalTime() + setTime).ToString());

		if (Planetarium.GetUniversalTime() > timeSave) {
			node.SetValue("TimeSave", (Planetarium.GetUniversalTime() + setTime).ToString());
			return part.vessel.GetCrewCount();
		}
		return 0;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		double setTime = 0, timeSave = 0;
		if (node.HasValue("setTime")) {
			setTime	= double.Parse(node.GetValue("setTime"));
		}else	setTime	= 1;

		if (node.HasValue("TimeSave")) {
			timeSave	= double.Parse(node.GetValue("TimeSave"));
		}else	node.AddValue("TimeSave", (Planetarium.GetUniversalTime() + setTime).ToString());

		if (Planetarium.GetUniversalTime() > timeSave) {
			node.SetValue("TimeSave", (Planetarium.GetUniversalTime() + setTime).ToString());
			return part.vessel.GetCrewCount() * 2;
		}
		return 0;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		ProtoCrewMember Kerbal = part.vessel.GetVesselCrew()[0];
		Kerbal.KerbalRef.InPart.RemoveCrewmember(Kerbal);
		Kerbal.Die();
		return demand;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("setTime")) {
			string setTime = resnode.GetValue("setTime");
			partnode.AddValue("setTime", setTime);
		}
	}
	public KillKerbalInVessel() { isAdvanced = true; }
}
public class KillPartCrew : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return (part.protoModuleCrew.Count > 1) ? TimeWarp.CurrentRate : 0;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		return (part.protoModuleCrew.Count > 1) ? TimeWarp.CurrentRate * 2: 0;
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
		return (part.vessel.GetCrewCount() > 1) ? TimeWarp.CurrentRate : 0;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		return (part.vessel.GetCrewCount() > 1) ? TimeWarp.CurrentRate * 2: 0;
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
				return TimeWarp.CurrentRate;
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
			lastValue	= Physics.Linecast(part.orgPos, sun) ? 0 : TimeWarp.CurrentRate;
			node.SetValue("LastUpdate", (Time.time + 0.5f).ToString());
			node.SetValue("LastValue", lastValue.ToString());
		}
		return lastValue;
	}
	public Sunlight() { isAdvanced = true; }
}
public class Timer : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		double setTime = 0, timeSave = 0;
		if (node.HasValue("setTime")) {
			setTime	= double.Parse(node.GetValue("setTime"));
		}else	setTime	= 1;

		if (node.HasValue("TimeSave")) {
			timeSave	= double.Parse(node.GetValue("TimeSave"));
		}else	node.AddValue("TimeSave", (Planetarium.GetUniversalTime() + setTime).ToString());

		if (Planetarium.GetUniversalTime() > timeSave) {
			node.SetValue("TimeSave", (Planetarium.GetUniversalTime() + setTime).ToString());
			return TimeWarp.CurrentRate;
		}
		return 0;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("setTime")) {
			string setTime = resnode.GetValue("setTime");
			partnode.AddValue("setTime", setTime);
		}
	}
	public Timer() { isAdvanced = true; }
}
public class GlobalVar : Resource {
	private double GlobalThing(ref ConfigNode node, Part part) {
		double value;
		string name		= node.GetValue("varName");
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("GlobalVar");

		if (!mine.HasNode(part.vessel.id.ToString())) {
			mine = mine.AddNode(part.vessel.id.ToString());
		}else mine = mine.GetNode(part.vessel.id.ToString());

		if (!mine.HasValue(name)) {
			if (node.HasValue("checked")) {
				value = double.Parse(ResourceLibrary.globalNodes[id].GetNode("GlobalVar").GetValue(name));
				node.AddValue("value", value);
			}else if (!node.HasValue("value")) {
				node.AddValue("checked",true);
				return 0;
			}else value = double.Parse(node.GetValue("value"));

			mine.AddValue(name, value);
		}else value = double.Parse(mine.GetValue(name));

		if (node.HasValue("checked"))		node.RemoveValue("checked");
		if (node.HasValue("value"))		node.SetValue("value",value.ToString());

		return value;
	}
	public override double amount(ref ConfigNode node, Part part) {
		return GlobalThing(ref node, part);
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		GlobalThing(ref node, part);
		return double.MaxValue;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("GlobalVar").GetNode(part.vessel.id.ToString());
		string name		= node.GetValue("varName");
		double value	= double.Parse(mine.GetValue(name));

		if (value >= demand) {
			value -= demand;
		}else value = 0;

		mine.SetValue(name, value.ToString());
		return value;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		string name, value;
		ConfigNode mine;
		if (!resnode.HasValue("varName")) return;
		name = resnode.GetValue("varName");
		partnode.AddValue("varName", name);

		if (resnode.HasValue("value")) {
			value = resnode.GetValue("value");
		}else value = "0";

		if (value == null) value = "0";

		if (!ResourceLibrary.globalNodes.ContainsKey(id)) {
			mine = new ConfigNode();
			ResourceLibrary.globalNodes.Add(id, mine);
		}else	mine = ResourceLibrary.globalNodes[id];

		if (!mine.HasNode("GlobalVar")) {
			mine = mine.AddNode("GlobalVar");
		}else	mine = mine.GetNode("GlobalVar");

		if (!mine.HasValue(name)) {
			mine.AddValue(name, value);
		}
	}
	public GlobalVar() { isAdvanced = true; }
}
public class GlobalSet : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return TimeWarp.CurrentRate * 10000;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		return TimeWarp.CurrentRate * 20000;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("GlobalVar").GetNode(part.vessel.id.ToString());
		string name		= node.GetValue("varName");
		if (mine == null || !mine.HasValue(name)) return 5;
		double value	= double.Parse(mine.GetValue(name));

		value = demand;
		mine.SetValue(name, value.ToString());
		return value;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("varName"))	partnode.AddValue("varName",	resnode.GetValue("varName"));
	}
	public GlobalSet() { isAdvanced = true; }
}
public class GlobalIsGreater : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("GlobalVar").GetNode(part.vessel.id.ToString());
		string name		= node.GetValue("varName");
		if (mine == null || !mine.HasValue(name)) return 5;
		double value	= double.Parse(mine.GetValue(name));
		double comp		= double.Parse(node.GetValue(name));
		return (value > comp) ? TimeWarp.CurrentRate : 0;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("GlobalVar").GetNode(part.vessel.id.ToString());
		string name		= node.GetValue("varName");
		if (mine == null || !mine.HasValue(name)) return 5;
		double value	= double.Parse(mine.GetValue(name));
		double comp		= double.Parse(node.GetValue(name));
		return (value > comp) ? TimeWarp.CurrentRate * 2: 0;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("varName"))	partnode.AddValue("varName",	resnode.GetValue("varName"));
		if (resnode.HasValue("value"))	partnode.AddValue("value",	resnode.GetValue("value"));
	}
	public GlobalIsGreater() { isAdvanced = true; }
}
public class GlobalIsLess : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("GlobalVar").GetNode(part.vessel.id.ToString());
		string name		= node.GetValue("varName");
		if (mine == null || !mine.HasValue(name)) return 5;
		double value	= double.Parse(mine.GetValue(name));
		double comp		= double.Parse(node.GetValue(name));
		return (value < comp) ? TimeWarp.CurrentRate : 0;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("GlobalVar").GetNode(part.vessel.id.ToString());
		string name		= node.GetValue("varName");
		if (mine == null || !mine.HasValue(name)) return 5;
		double value	= double.Parse(mine.GetValue(name));
		double comp		= double.Parse(node.GetValue(name));
		return (value < comp) ? TimeWarp.CurrentRate  * 2: 0;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("varName"))	partnode.AddValue("varName",	resnode.GetValue("varName"));
		if (resnode.HasValue("value"))	partnode.AddValue("value",	resnode.GetValue("value"));
	}
	public GlobalIsLess() { isAdvanced = true; }
}
public class LocalVar : Resource {
	private double LocalThing(ref ConfigNode node, Part part) {
		double value;
		string name		= node.GetValue("varName");
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("LocalVar");

		if (!mine.HasNode(part.flightID.ToString())) {
			mine = mine.AddNode(part.flightID.ToString());
		}else mine = mine.GetNode(part.flightID.ToString());

		if (!mine.HasValue(name)) {
			if (node.HasValue("checked")) {
				value = double.Parse(ResourceLibrary.globalNodes[id].GetNode("LocalVar").GetValue(name));
				node.AddValue("value", value);
			}else if (!node.HasValue("value")) {
				node.AddValue("checked",true);
				return 0;
			}else value = double.Parse(node.GetValue("value"));

			mine.AddValue(name, value);
		}else value = double.Parse(mine.GetValue(name));

		if (node.HasValue("checked"))		node.RemoveValue("checked");
		if (node.HasValue("value"))		node.SetValue("value",value.ToString());

		return value;
	}
	public override double amount(ref ConfigNode node, Part part) {
		return LocalThing(ref node, part);
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		LocalThing(ref node, part);
		return double.MaxValue;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("LocalVar").GetNode(part.flightID.ToString());
		string name		= node.GetValue("varName");
		double value	= double.Parse(mine.GetValue(name));

		if (value >= demand) {
			value -= demand;
		}else value = 0;

		mine.SetValue(name, value.ToString());
		return value;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		string name, value;
		ConfigNode mine;
		if (!resnode.HasValue("varName")) return;
		name = resnode.GetValue("varName");
		partnode.AddValue("varName", name);

		if (resnode.HasValue("value")) {
			value = resnode.GetValue("value");
		}else value = "0";

		if (value == null) value = "0";

		if (!ResourceLibrary.globalNodes.ContainsKey(id)) {
			mine = new ConfigNode();
			ResourceLibrary.globalNodes.Add(id, mine);
		}else	mine = ResourceLibrary.globalNodes[id];

		if (!mine.HasNode("LocalVar")) {
			mine = mine.AddNode("LocalVar");
		}else	mine = mine.GetNode("LocalVar");

		if (!mine.HasValue(name)) {
			mine.AddValue(name, value);
		}
	}
	public LocalVar() { isAdvanced = true; }
}
public class LocalSet : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		return TimeWarp.CurrentRate * 10000;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		return TimeWarp.CurrentRate * 20000;
	}
	public override double RequestResource(ref ConfigNode node, Part part, double demand) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("LocalVar").GetNode(part.flightID.ToString());
		string name		= node.GetValue("varName");
		if (mine == null || !mine.HasValue(name)) return 5;
		double value	= double.Parse(mine.GetValue(name));

		value = demand;
		mine.SetValue(name, value.ToString());
		return value;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("varName"))	partnode.AddValue("varName",	resnode.GetValue("varName"));
	}
	public LocalSet() { isAdvanced = true; }
}
public class LocalIsGreater : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("LocalVar").GetNode(part.flightID.ToString());
		string name		= node.GetValue("varName");
		if (mine == null || !mine.HasValue(name)) return 5;
		double value	= double.Parse(mine.GetValue(name));
		double comp		= double.Parse(node.GetValue(name));
		return (value > comp) ? TimeWarp.CurrentRate : 0;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("LocalVar").GetNode(part.flightID.ToString());
		string name		= node.GetValue("varName");
		if (mine == null || !mine.HasValue(name)) return 5;
		double value	= double.Parse(mine.GetValue(name));
		double comp		= double.Parse(node.GetValue(name));
		return (value > comp) ? TimeWarp.CurrentRate * 2: 0;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("varName"))	partnode.AddValue("varName",	resnode.GetValue("varName"));
		if (resnode.HasValue("value"))	partnode.AddValue("value",	resnode.GetValue("value"));
	}
	public LocalIsGreater() { isAdvanced = true; }
}
public class LocalIsLess : Resource {
	public override double amount(ref ConfigNode node, Part part) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("LocalVar").GetNode(part.flightID.ToString());
		string name		= node.GetValue("varName");
		if (mine == null || !mine.HasValue(name)) return 5;
		double value	= double.Parse(mine.GetValue(name));
		double comp		= double.Parse(node.GetValue(name));
		return (value < comp) ? TimeWarp.CurrentRate : 0;
	}
	public override double maxAmount(ref ConfigNode node, Part part) {
		ConfigNode mine	= ResourceLibrary.globalNodes[id].GetNode("LocalVar").GetNode(part.flightID.ToString());
		string name		= node.GetValue("varName");
		if (mine == null || !mine.HasValue(name)) return 5;
		double value	= double.Parse(mine.GetValue(name));
		double comp		= double.Parse(node.GetValue(name));
		return (value < comp) ? TimeWarp.CurrentRate * 2: 0;
	}
	public override void Load(ref ConfigNode resnode, ref ConfigNode partnode) {
		if (resnode.HasValue("varName"))	partnode.AddValue("varName",	resnode.GetValue("varName"));
		if (resnode.HasValue("value"))	partnode.AddValue("value",	resnode.GetValue("value"));
	}
	public LocalIsLess() { isAdvanced = true; }
}}
