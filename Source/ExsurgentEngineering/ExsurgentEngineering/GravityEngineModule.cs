using System;
using UnityEngine;
using System.Linq;

namespace ExsurgentEngineering
{
	public class GravityEngineModule : PartModule
	{
		[KSPField(guiActive=false, isPersistant=true)]
        public bool
			gravityEnabled = true;

		[KSPField(guiActive=true, guiName="Gravity power", guiFormat="F2", guiUnits="m/s",isPersistant=true)]
        public float
			gee = 0f;

		[KSPField(guiActive=true, guiName="Curr. Thrust", guiFormat="F2", guiUnits="N",isPersistant=true)]
        public float
			thrust = 0;

		[KSPField(guiActive=true, guiName="Avail. Thrust", guiFormat="F2", guiUnits="N",isPersistant=true)]
        public float
			maxThrust = 0;

		[KSPField]
        public string
			coreMeshPath = "model/antigravcore/Sphere001";

		[KSPField(isPersistant = true)]
        public bool
			staged;

		public Vector3 geeVector = Vector3.up;

		public Transform coreMesh;

		public Vector3 thrustVector = Vector3.zero;

		public Vector3 translationVector = Vector3.zero;

		public Vector3 rotationVector = Vector3.zero;

		public override void OnActive ()
		{
//            var agCore = base.transform.FindChild("model").FindChild("antigravcore").FindChild("Sphere001");
//            Debug.Log("agCore: " + agCore);
//            coreMesh = coreMeshPath.Split('/').
//				Aggregate(transform, (mesh, child) => mesh.FindChild(child));
//            Debug.Log("coreMesh: " + coreMesh);
//			
//            coreMesh = agCore;

			staged = true;
		}

		public override void OnStart (StartState state)
		{
			if (state == StartState.Editor)
				gravityEnabled = false;
			else
				gravityEnabled = true;
		}

		[KSPEvent(guiName="Activate Engine", guiActive=true)]
        public void Activate ()
		{
			part.force_activate ();
			gravityEnabled = true;
			Events ["Activate"].active = !gravityEnabled;
			Events ["Deactivate"].active = gravityEnabled;

		}

		[KSPEvent(guiName="Deactivate Engine", guiActive=true)]
        public void Deactivate ()
		{
			gravityEnabled = false;
			Events ["Activate"].active = !gravityEnabled;
			Events ["Deactivate"].active = gravityEnabled;
		}

		[KSPAction("Activate Engine")]
        public void ActivateAction (KSPActionParam param)
		{
			Activate ();
		}

		[KSPAction("Deactivate Engine")]
        public void DeactivateAction (KSPActionParam param)
		{
			Deactivate ();
		}

		[KSPAction("Toggle Engine")]
        public void ToggleAction (KSPActionParam param)
		{
			if (gravityEnabled)
				Deactivate ();
			else
				Activate ();
		}

		public void Update ()
		{
			if (HighLogic.LoadedSceneIsEditor)
				return;

//            if (coreMesh != null && gravityEnabled)
//            {
//                coreMesh.Rotate(Vector3.forward * 8f * Time.time);
//            }			
		}

		public void FixedUpdate ()
		{
			if (HighLogic.LoadedSceneIsEditor)
				return;

			if (!gravityEnabled)
				return;
			
			if (vessel.rigidbody == null)
				return;

			geeVector = FlightGlobals.getGeeForceAtPosition (vessel.findWorldCenterOfMass());  
			
			translationVector = Vector3.zero;
			thrustVector = Vector3.zero;
			rotationVector = Vector3.zero;

			CalculateG ();
			CalculateTranslation ();
			CalculateThrust ();
			CancelG ();
//            CalculateRotation();
            
			ApplyThrust ();
//            ApplyRotation();
		}

		public void CancelG ()
		{
			var antiGeeVector = geeVector * -1;
			foreach (var vesselPart in FlightGlobals.ActiveVessel.parts.Where(p => p.rigidbody != null))
				vesselPart.rigidbody.AddForce (antiGeeVector, ForceMode.Acceleration);

		}

		public void CalculateG ()
		{
			gee = (float)FlightGlobals.getGeeForceAtPosition (vessel.findWorldCenterOfMass()).magnitude;
			maxThrust = gee + 9.81f;
		}

		public Vector3 CalculateTranslation ()
		{
			var fltCtrlState = FlightInputHandler.state;
			translationVector = (new Vector3 (fltCtrlState.X, fltCtrlState.Y, fltCtrlState.Z)).normalized;
            
			translationVector *= maxThrust * 0.1f;
			return translationVector;
		}

		public Vector3 CalculateThrust ()
		{
			var remainingThrust = maxThrust - translationVector.magnitude;

			thrust = FlightInputHandler.state.mainThrottle * maxThrust;
			thrustVector = Vector3.ClampMagnitude (vessel.transform.up * thrust, remainingThrust);

			return thrustVector;
		}
//        public virtual Vector3 CalculateRotation()
//        {
//            var ctrlState = FlightInputHandler.state;
//
//            var localRotation = new Vector3(-ctrlState.pitch, ctrlState.roll, -ctrlState.yaw);
//
//            // 5 degrees per second?
//            var rotationRate = 5f;
//
//            var angle = rotationRate * TimeWarp.fixedDeltaTime;
//
//            rotationVector = vessel.transform.TransformDirection(localRotation) * angle;
//
//            return rotationVector;
//        }

		public void ApplyThrust ()
		{
			var netThrust = translationVector + thrustVector;
			foreach (var vesselPart in FlightGlobals.ActiveVessel.parts.Where(p => p.rigidbody != null))
				vesselPart.rigidbody.AddForce (netThrust, ForceMode.Acceleration);
                
		}
//        public void ApplyRotation()
//        {
////            foreach (var part in FlightGlobals.ActiveVessel.parts.Where(part => part.rigidbody != null))
////                part.rigidbody.AddTorque(rotationVector, ForceMode.Acceleration);
//            var angle = rotationVector.magnitude;
//            var change = Quaternion.AngleAxis(angle, rotationVector);
//            var newRotation = vessel.transform.rotation * change;
//            vessel.SetRotation(newRotation);
//        }

		[KSPEvent(guiActive=false, active=true)]
        public void MechJebVesselStateUpdated ()
		{
//            Debug.Log("MechJebVesselStateUpdated");
			BaseEventData ed = new BaseEventData (BaseEventData.Sender.USER); 


//            vessel.torqueRAvailable = vessel.torqueRAvailable + vessel.mass * 20        
//            vessel.torquePYAvailable = vessel.torquePYAvailable + vessel.mass * 20

			var script = String.Format (@"
                vessel.thrustAvailable = vessel.thrustAvailable + {0} * vessel.mass
                vessel.maxThrustAccel = vessel.maxThrustAccel + {0}", maxThrust);
			ed.Set ("script", script); 
			part.SendEvent ("MechJebLua", ed);
		}
	}
}
//rrslashphish: THANK YOU R4M0N!
//    [9:44pm] careo: and I'd end up recreating part of what you're doing from the sound of it
//        [9:44pm] r4m0n: gratz 
//        [9:45pm] Mu:         ParticleEmitter pe = (ParticleEmitter)newObj.AddComponent("EllipsoidParticleEmitter");
//[9:45pm] Mu:         ParticleAnimator pa = newObj.AddComponent<ParticleAnimator>();
//[9:45pm] rrslashphish: using 1 function from ARUtils and your orbit extensions
//    [9:45pm] rrslashphish:
//        [9:45pm] Mu:         ParticleRenderer pr = newObj.AddComponent<ParticleRenderer>();
//[9:45pm] Hyratel: preset=multi ; preset1=redfire ; preset2=thinsmoke
//    [9:45pm] Mu: sadly the ellipsoid particle emitter is a javascript object 
//        [9:45pm] Taverius:
//        [9:45pm] Hyratel: Mu, ^ make sense for pseudocode?
//            [9:45pm] Hyratel: or
//                [9:45pm] Mu: yes i think i follow you hyra
//                [9:45pm] Hyratel: preset =Draco
//                [9:46pm] Hyratel: (for SpaceX DracoRCS)
//                [9:46pm] KwirkyJ: Hyratel: a library of known effects?
//                [9:46pm] Hyratel: YES
//                [9:46pm] Mu: hehe
//Hyratel: that would streamline it massively
//    [9:46pm] careo: throw that in the OnStart, save a reference to one of the objects and use it to tweak the emitter as needed?
//        [9:46pm] KwirkyJ: fx definitions as separate files, linked to the part by name and path
//        [9:46pm] Mu: aye part/internal/resource library upgrade is well needed
//        [9:46pm] rrslashphish: r4m0n did you write all that ARUtils and OrbitExtensions?
//        [9:47pm] Mu: aye careo
//        [9:47pm] careo: Taverius: well ok. so maybe something can be cooked up easily enough 