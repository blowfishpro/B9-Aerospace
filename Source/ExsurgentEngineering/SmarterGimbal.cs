using System;
using UnityEngine;
using System.Collections.Generic;

namespace ExsurgentEngineering
{
	public class SmarterGimbal : ModuleGimbal
	{
		[KSPField (isPersistant = false)]
		public float pitchRange = 10f;

		[KSPField (isPersistant = false)]
		public float yawRange;

		[KSPField (isPersistant = false)]
		new public String gimbalTransformName;

		[KSPField (isPersistant = false)]
		new public float gimbalResponseSpeed = 10f;

		[KSPField (isPersistant = false)]
		new public bool useGimbalResponseSpeed;

		public Dictionary<Transform,Quaternion> transformsAndRotations = new Dictionary<Transform,Quaternion> ();

		public float currentPitchAngle;

		public float currentYawAngle;

		public override string GetInfo ()
		{
			return String.Format ("Smarter Thrust Vectoring Enabled\n - Pitch: {0}\n - Yaw: {1}", pitchRange, yawRange);
		}

		float RollAxis ()
		{
			if (part.rigidbody != null) {
				var vesselCoM = vessel.findWorldCenterOfMass ();          
				var partCoM = part.rigidbody.worldCenterOfMass;
				var dot = Vector3.Dot (vesselCoM - partCoM, vessel.transform.right);
				return dot;
			}
			return 0f;
		}

		float PitchAxis ()
		{
			if (part.rigidbody != null) {
				var vesselCoM = vessel.findWorldCenterOfMass ();          
				var partCoM = part.rigidbody.worldCenterOfMass;
				var dot = Vector3.Dot (vesselCoM - partCoM, vessel.transform.up);
				return dot;
			}
			return 0f;
		}

		float YawAxis ()
		{
			if (part.rigidbody != null) {
				var vesselCoM = vessel.findWorldCenterOfMass ();          
				var partCoM = part.rigidbody.worldCenterOfMass;
				// FIXME: now I'm just guessing at the axis to use
				var dot = Vector3.Dot (vesselCoM - partCoM, vessel.transform.up);
				return dot;
			}
			return 0f;
		}

		public override void OnLoad (ConfigNode node)
		{
			// use gimbalRange for pitch and yaw ranges if present
			if (node.HasValue ("gimbalRange")) {
				var configRange = float.Parse (node.GetValue ("gimbalRange"));
				pitchRange = configRange;
				yawRange = configRange;
			}
		}

		public override void OnStart (StartState state)
		{
			if (gimbalLock)
				LockGimbal ();
			else
				FreeGimbal ();

			foreach (var gimbalTransform in part.FindModelTransforms(gimbalTransformName)) {
				transformsAndRotations.Add (gimbalTransform, gimbalTransform.localRotation);
			}
		}

		public override void OnFixedUpdate ()
		{
			// FIXME: really, what should the guard conditions be? 
			if (HighLogic.LoadedSceneIsEditor)
				return;
			if (FlightGlobals.ActiveVessel != vessel)
				return;
			if (gimbalLock)
				return;

			var rollAngle = 0f;
			if (part.symmetryMode > 0) {
				var rollDot = RollAxis ();
				var rollDotSign = Mathf.Sign (rollDot);

				var roll = vessel.ctrlState.roll;
				rollAngle = roll * rollDotSign * pitchRange * -1; // TODO: figure out why the -1 is needed. I didn't think it was
			}

			var pitchDot = PitchAxis ();
			var pitchDotSign = Mathf.Sign (pitchDot);

			var yawDot = YawAxis ();
			var yawDotSign = Mathf.Sign (yawDot);

			var yaw = vessel.ctrlState.yaw;
			var yawAngle = yaw * yawRange * yawDotSign;

			var pitch = vessel.ctrlState.pitch;
			var pitchAngle = pitch * pitchRange * pitchDotSign;

			var gimbalAngle = pitchAngle + rollAngle;
			gimbalAngle = Mathf.Clamp (gimbalAngle, -pitchRange, pitchRange);

			currentPitchAngle = gimbalAngle;
			currentYawAngle = yawAngle;

			foreach (var pair in transformsAndRotations) {
				var gimbalTransform = pair.Key;
				var initialRotation = pair.Value;

				var localUp = gimbalTransform.InverseTransformDirection (vessel.ReferenceTransform.right);
				var localRight = gimbalTransform.InverseTransformDirection (vessel.ReferenceTransform.forward);

				var pitchRotation = Quaternion.AngleAxis (gimbalAngle, localUp);
				var yawRotation = Quaternion.AngleAxis (yawAngle, localRight);

				var targetRotation = initialRotation * pitchRotation * yawRotation;

				if (useGimbalResponseSpeed) {
					var angle = gimbalResponseSpeed * TimeWarp.fixedDeltaTime;
					gimbalTransform.localRotation = Quaternion.RotateTowards (gimbalTransform.localRotation, targetRotation, angle);
				} else {
					gimbalTransform.localRotation = targetRotation;
				}
			}
		}
	}
}

