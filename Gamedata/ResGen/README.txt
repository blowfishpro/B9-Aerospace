Resource Generation Module v 0.24


Purpose:
	To add in an extensible "ResGen" framework such that people can edit mods without knowing how to program.


Current Features:
	Version 0.24	0.20.2 Compatibility
		Merged "ResGen_Example" with "ResGen"
		Added in "BADINPUT" and "BADOUTPUT" they replace the "INPUT / OUTPUT" resources when there is not enough 				"INPUT" resources.

		Added in "KillVesselCrew"
		Added in "KillPartCrew"
		Added in "KillKerbalInVessel"
		Added in "KillKerbalInPart"


	Version 0.23	Finished up most of the Hidden Resource system for now;
		Resources will now use the ConfigNode system to store / pass information

		Did some theme / variations while I was at it:
		Added in "ResGen.PartCrew"
			INPUT: returns "PartCrew Count"
			OUTPUT: returns "PartCrew Capacity - Count"

		Added in "ResGen.VesselCrew"
			INPUT: returns "VesselCrew Count"
			OUTPUT: returns "VesselCrew Capacity - Count"

		Added in "ResGen.AtmosphereHasOxygen"
			INPUT: returns "5" if oxygen is in the atmosphere

		Added in "ResGen.AtmosphericOxygen"
			INPUT: returns "Atmospheric Density" if oxygen is in the atmosphere

		Added in "ResGen.Sunlight"
			INPUT: returns "5" if the part is visible to the sun.

		Added in "ResGen.PlanetWater"
			INPUT: returns "5" if the vessel is "Spashed"

		Added in "ResGen.GeeForce"
			INPUT: returns the geeForce

		Added in "ResGen.Explode"
			OUTPUT: well, will explode the part unless the inputs prohibit it

		Added in "ResGen.OnPlanet"
			requires a field "planet = name" in the resource listing.
			INPUT: Returns 5 if the code works and you are on said planet.

		Added in "ResGenMultVesselCrew":
			Resource Generation Module: the rate is multiplied by the total crew

		Added in "ResGenDivVesselCrew":
			Resource Generation Module: the rate is divided by the total crew

		Added in "ResGenMultPartCrew":
			Resource Generation Module: the rate is multiplied by the part crew

		Added in "ResGenDivPartCrew":
			Resource Generation Module: the rate is divided by the total crew

	Version 0.22	Now have an operational Hidden Resource
		Referenced as "ResGen.Sunlight"
		Is only "on or off" and a rather weak implementation; but it operates.

	Version 0.21	Bug Fixes
	Version 0.20:	Second Release,
		Introduces "ResGenWithButton", simply adds a button labeled "Toggle"
		Adds "ResGen.Module" for polymorphism, allows simplified creation of ResGen PartModules
		Adds "ResGen.Resource" for polymorphism, allows creation of "Hidden Resources" (Extreme Alpha)
		Adds "ResGen.ResourceLibrary" to mirror "PartResourceLibrary"

	Version 0.11:	Small Code Patch
	Version 0.10:	Initial Release,	"Proof of Concept"
		Single module "ResGen", features "INPUT" and "OUTPUT". Neither are required for operation.
		Will trace resources before "consuming" and adjust total consumption / generation based on
		how much can be consumed / generated.

		Resources do not need to be present in any given part, but the must be able to travel to said
		part.

Documentation (Part.cfg):
	ResGen:
		useToggle:
			Type:			BOOLEAN
			Required:		No
			Default:		false
			Description:	Turns on the button

		externalToEVAOnly:
			Type:			BOOLEAN
			Required:		No
			Default:		false
			Description:	something

		guiName:
			Type:			STRING
			Required:		No
			Default:		"Toggle"
			Description:	Changes the name of the button

		UPDATE:
			Type:			DOUBLE
			Required:		No
			Default:		0.1
			Description:	Changes the frequency of updates in seconds

		RATE:
			Type:			DOUBLE
			Required:		No
			Default:		1
			Description:	Lowers the "overall rate"

		INPUT:
			Type:			ConfigNode
			Required:		No
			Description:	If one or more are included the specified resources
						will be drained in order to create the "OUTPUT" resource.

			NAME:
				Type:			STRING
				Required:		Yes
				Description:	The name of the resource

			RATE:
				Type:			DOUBLE
				Required:		Yes
				Description:	The rate/ratio of consumption


		OUTPUT:
			Type:			ConfigNode
			Required:		No
			Description:	If one or more are included the specified resources will
			be generated at the rate the "INPUT" resources are consumed. Furthermore,
			this will limit consumption to how much can be generated.

			NAME:
				Type:			STRING
				Required:		Yes
				Description:	The name of the resource

			RATE:
				Type:			DOUBLE
				Required:		Yes
				Description:	The rate/ratio of generation


		BADINPUT:
			Type:			ConfigNode
			Required:		No
			Description:	If the INPUTS are not availiable, these are consumed instead.
						and create the "BADOUTPUT" resource.

			NAME:
				Type:			STRING
				Required:		Yes
				Description:	The name of the resource

			RATE:
				Type:			DOUBLE
				Required:		Yes
				Description:	The rate/ratio of consumption


		BADOUTPUT:
			Type:			ConfigNode
			Required:		No
			Description:	Can be limited by BADINPUT (i.e. if the input resources cannot be consumed)
						Generates an alternative output if "INPUT" resources are not availiable.

			NAME:
				Type:			STRING
				Required:		Yes
				Description:	The name of the resource

			RATE:
				Type:			DOUBLE
				Required:		Yes
				Description:	The rate/ratio of generation

Example:
(in a part.cfg)
MODULE
{
	name = ResGen
	INPUT
	{
		name = ElectricCharge
		rate = 5
	}
	INPUT
	{
		name = Water
		rate = 1
	}
	OUTPUT
	{
		name = Oxygen
		rate = 2
	}
	OUTPUT
	{
		name = Hydrogen
		rate = 1
	}
}
MODULE
{
	name = ResGen
	useToggle = true
	INPUT
	{
		name = ResGen.Sunlight
		rate = 1
	}
	OUTPUT
	{
		name = Explode
		rate = 1
	}
}
Documentation (Plugin):
	ResGen.Module:
		protected abstract void initRate(ref double delta):
			The first function to be called, this determines how rate modifies the
			"delta" (equal to "change in time"). After modification, delta is considered
			the "unweighted amount" that will be created / requested per resource.

			Example:
				delta *= rate;

		protected abstract void getFRate(ref double delta, ResDat resi):
			Get_Forward_Rate, in most cases should be identical to initRate
			This determines how the get (INPUT) rate differs from other rates (if you so desire)

			Example:
				delta *= resi.rate;

		protected abstract void getRRate(ref double delta, ResDat resi):
			Get_Reverse_Rate, in most cases should be the exact opposite operation of initRate
			This determines how the get (INPUT) rate differs from other rates (if you so desire)

			Example:
				delta /= resi.rate;

		protected abstract void stoFRate(ref double delta, ResDat resi):
			Store_Forward_Rate, in most cases should be identical to initRate
			This determines how the sto (OUTPUT) rate differs from other rates (if you so desire)

			Example:
				delta *= resi.rate;
		protected abstract void stoRRate(ref double delta, ResDat resi):
			Store_Reverse_Rate, in most cases should be the exact opposite operation of initRate
			This determines how the sto (OUTPUT) rate differs from other rates (if you so desire)

			Example:
				delta /= resi.rate;

	ResGen.Resource:
		***NOTE***
			Namespace MUST be the same as the compiled DLL

		public int id:
			A hashtag used to identify the resource (Similar to KSPs System);

		public string name:
			The FULLNAME of the CLASS that stores the resource.

			Example:
				namespace MyNameSpace {
					public class Heat : ResGen.Resource {

					}
				}
				has a name of "MyNameSpace.Heat";

		public virtual double amount(ref ConfigNode node, Part part)
			Overridable, use this to specify the "amount availiable" via override; or eventually
			just update the value as needed.

		public virtual double maxAmount(ref ConfigNode node, Part part)
			Overridable, use this to specify the "maxAmount availiable" via override; or eventually
			just update the value as needed.

		public virtual double RequestResource(ref ConfigNode node, Part part, double demand):
			Overridable, can be used with confignodes


	ResGen.ResourceLibrary:
		public static Hashtable resourceDefinitions:
			Safely stores all "HiddenResource" information... mess with this if you like crashing.

		public static Resource GetDefinition(int id):
			The still not completely safe accessor, I have no clue how C# handles objects pointers
			the whole "hiding it from the programmer" makes it more confusing.

			(Don't WRITE to the resource unless you know what you're doing)

			returns null if resource is not found

		public static Resource GetDefinition(string name):
			Another "not completely safe accessor"

			(Don't WRITE to the resource unless you know what you're doing)

			Attempts to load a resource if it is not stored
			returns null if unable to find or load the resource
