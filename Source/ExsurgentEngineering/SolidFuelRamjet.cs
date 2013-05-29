using System;
using UnityEngine;

// Isp should be around 625?


using System.Collections;

namespace ExsurgentEngineering
{
public class SolidFuelRamjet : HybridSolidRocket
{
	
	protected override void onPartAwake ()
	{
		base.onPartAwake ();
	}

	protected override void onActiveFixedUpdate ()
	{
		this.calculateAirFlow();
		base.onActiveFixedUpdate ();
	}
	
	private double CalculateDynamicPressure()
	{
		Vector3d orbitalVelocity = vessel.orbit.GetVel();
		Vector3d airVelocity = vessel.mainBody.getRFrmVel (vessel.findWorldCenterOfMass());
		Vector3d relativeVelocity = orbitalVelocity - airVelocity;
		
		//Vector3d velocityNorm = relativeVelocity.normalized;
		
		// only need the speed squared - saves us a square root
		double speedSqr = relativeVelocity.sqrMagnitude;
		
		// dynamic pressure 
		double dynPressure = 0.5f * vessel.atmDensity * speedSqr;
				
		// et voila
		return dynPressure;
	}
	
	
	public double calculateAirFlow() {
		Debug.Log ("\n\n\n");
		var dynamicPressure = CalculateDynamicPressure();
		Debug.Log ("my dynamic pressure: " + dynamicPressure);
		Debug.Log ("\n\n\n");
			
		return dynamicPressureAtm;
		
	}
}

}