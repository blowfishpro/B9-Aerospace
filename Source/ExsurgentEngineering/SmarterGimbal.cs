using System;
using UnityEngine;
using System.Collections.Generic;

namespace ExsurgentEngineering
{
	public class SmarterGimbal : PartModule
	{

		[KSPField (isPersistant = false)]
		public float pitchRange = 10f;

		[KSPField (isPersistant = false)]
		public float yawRange = 0f;

		[KSPField (isPersistant = false)]
		public String gimbalTransformName;

		[KSPField (isPersistant = false)]
		public float gimbalResponseSpeed = 10f;

		[KSPField (isPersistant = false)]
		public bool useGimbalResponseSpeed;

		[KSPField (isPersistant = true)]
		public bool gimbalLock;

		public Dictionary<Transform,Quaternion> transformsAndRotations = new Dictionary<Transform,Quaternion>();
		public float currentPitchAngle;
		public float currentYawAngle;


		[KSPEvent (guiName = "Free Gimbal", guiActive = true)]
		public void FreeGimbal ()
		{
//			Debug.Log ("FreeGimbal()");

			gimbalLock = false;
			Events ["FreeGimbal"].active = false;
			Events ["LockGimbal"].active = true;
		}


		[KSPEvent (guiName = "Lock Gimbal", guiActive = true)]
		public void LockGimbal ()
		{
//			Debug.Log ("LockGimbal()");

			gimbalLock = true;
			Events ["FreeGimbal"].active = true;
			Events ["LockGimbal"].active = false;

			foreach (var pair in transformsAndRotations)
			{
				var gimbalTransform = pair.Key;
				var initialRotation = pair.Value;

				gimbalTransform.localRotation = initialRotation;
			}
		}


		[KSPAction ("Toggle Gimbal")]
		public void ToggleAction (KSPActionParam param)
		{
//			Debug.Log ("param: " + param);
//			Debug.Log ("param.type: " + param.type);

			if (param.type == KSPActionType.Activate)
				FreeGimbal ();
			else
				LockGimbal ();

		}

		public override string GetInfo ()
		{
			return "Smarter Thrust Vectoring Enabled.";
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
		public override void OnStart (StartState state)
		{
			if (gimbalLock)
				LockGimbal ();
			else
				FreeGimbal ();

			foreach(var gimbalTransform in part.FindModelTransforms(gimbalTransformName))
			{
				transformsAndRotations.Add (gimbalTransform, gimbalTransform.localRotation);
			}


		}


		public override void OnFixedUpdate ()
		{
			if (HighLogic.LoadedSceneIsEditor)
				return;
			if (FlightGlobals.ActiveVessel != vessel)
				return;
			if (gimbalLock)
				return;

			var rollDot = RollAxis ();
			var rollDotSign = Mathf.Sign(rollDot);

			var pitchDot = PitchAxis ();
			var pitchDotSign = Mathf.Sign(pitchDot);

			var yawDot = YawAxis ();
			var yawDotSign = Mathf.Sign(yawDot);

			var pitch = vessel.ctrlState.pitch;
			var roll = vessel.ctrlState.roll;
			var yaw = vessel.ctrlState.yaw;


			var pitchAngle = pitch * pitchRange * pitchDotSign;
			var rollAngle = roll * rollDotSign * pitchRange * -1; // TODO: figure out why the -1 is needed. I didn't think it was'

			var yawAngle = yaw * yawRange * yawDotSign;

			var gimbalAngle = pitchAngle + rollAngle;
			gimbalAngle = Mathf.Clamp (gimbalAngle, -pitchRange, pitchRange);

//			if (useGimbalResponseSpeed)
//			{
//				gimbalAngle = Mathf.Lerp (currentPitchAngle, gimbalAngle, gimbalResponseSpeed * TimeWarp.fixedDeltaTime);
//				yawAngle = Mathf.Lerp (currentYawAngle, yawAngle, gimbalResponseSpeed * TimeWarp.fixedDeltaTime);
//			}



			currentPitchAngle = gimbalAngle;
			currentYawAngle = yawAngle;

			foreach(var pair in transformsAndRotations)
			{
				var gimbalTransform = pair.Key;
				var initialRotation = pair.Value;

				var localUp = gimbalTransform.InverseTransformDirection (vessel.ReferenceTransform.right);
				var localRight = gimbalTransform.InverseTransformDirection (vessel.ReferenceTransform.forward);

				var pitchRotation = Quaternion.AngleAxis (gimbalAngle, localUp);
				var yawRotation = Quaternion.AngleAxis (yawAngle, localRight);

				var targetRotation = initialRotation * pitchRotation * yawRotation;

				if (useGimbalResponseSpeed)
				{
					var angle = gimbalResponseSpeed * TimeWarp.fixedDeltaTime;
					gimbalTransform.localRotation = Quaternion.RotateTowards (gimbalTransform.localRotation, targetRotation, angle);
				} else {
					gimbalTransform.localRotation = targetRotation;

				}


			}

		}



	}
}

