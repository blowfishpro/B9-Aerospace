using System;
using UnityEngine;

namespace ExsurgentEngineering
{
	public class HybridSolidRocket : SolidRocket
	{

		[KSPField]
		public float
			originalThrust;
		[KSPField]
		public float
			originalFuelConsumption;
		[KSPField]
		public Vector3
			originalThrustVector;
		
		[KSPField]
		public bool autoDecouple = false;
		
		[KSPField]
		public float ejectionForce = 22f;
		
		public float gimbalAngleV;
		public float gimbalAngleH;
		public bool thrustVectoringCapable = false;
		public float gimbalRange = 1f;

		protected override void onFlightStart ()
		{
			originalThrust = thrust;
			originalFuelConsumption = fuelConsumption;
			originalThrustVector = thrustVector;
			base.onFlightStart ();
		}

		protected override void onActiveFixedUpdate ()
		{
			thrust = originalThrust * FlightInputHandler.state.mainThrottle;
			fuelConsumption = originalFuelConsumption * FlightInputHandler.state.mainThrottle;

			thrustVector = Quaternion.AngleAxis (gimbalAngleV, localRoot.transform.right) * thrustVector;
			thrustVector = Quaternion.AngleAxis (gimbalAngleH, localRoot.transform.forward) * thrustVector;
			
			var estFuelRem = internalFuel - fuelConsumption * Time.fixedDeltaTime;
			if (estFuelRem <= 0) {
				separate ();
			}

			base.onActiveFixedUpdate ();
			updateEffects ();
			
			
			thrust = originalThrust;
			fuelConsumption = originalFuelConsumption;
			thrustVector = originalThrustVector;
		}

		protected override void onCtrlUpd (FlightCtrlState state)
		{
			//base.onCtrlUpd (state);
			this.gimbalAngleH = state.yaw * this.gimbalRange;
			this.gimbalAngleV = state.pitch * this.gimbalRange;

		}
		
		private void separate ()
		{
			
//			var ejectionDirection = -transform.right;
			var ejectionDirection =  (transform.rigidbody.centerOfMass - parent.transform.rigidbody.centerOfMass).normalized;
			
			var ejectionVector = ejectionDirection * ejectionForce * 0.5f * 100f;

			var oldparent = parent;
			decouple(0);
			//var ejectionDirection =  (transform.position - parent.transform.position).normalized;
			
			rigidbody.AddForce(ejectionVector, ForceMode.Force);
			oldparent.rigidbody.AddForce(-ejectionVector, ForceMode.Force);
		}
		
//		protected override void onPartDeactivate ()
//		{
//			base.onPartDeactivate ();
//			
//			separate ();
//			
//			
//		}
		private void updateEffects ()
		{
			foreach (var fxGroup in fxGroups) {
				// TODO: smooth out the updates in this?
				if (thrust == 0)
					fxGroup.Power = 0;
				else
					fxGroup.Power = thrust / originalThrust;
			}
		}

	}

}