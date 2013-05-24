using System;
using UnityEngine;

namespace ExsurgentEngineering
{
public class ModuleFrictionAdjuster : PartModule
{
	
	[KSPField(guiActive = true, guiName = "friction")]
	public float frictionValue = 0.02f;
	
	private void log (string msg, object thing)
	{
		print (msg + ": " + thing);
	}
	
	public override void OnStart (StartState state)
	{
		log ("friction", frictionValue);
		log("part", part);
		//		log("part.rigidbody", part.rigidbody);
		//		log("part.rigidbody.collider", part.rigidbody.collider);
		//		log("part.rigidbody.collider.material", part.rigidbody.collider.material);
		//
		
		log("part.collider", part.collider);
		log("part.collider.material", part.collider.material);
		
		
		var material = part.collider.material;
		log("material.dynamicFriction",material.dynamicFriction);			
		log("material.staticFriction",material.staticFriction);
		log("material.bounciness", material.bounciness);
		
		log("material.frictionCombine",material.frictionCombine);
		material.frictionCombine = PhysicMaterialCombine.Minimum;
		
		material.staticFriction = frictionValue;
		material.dynamicFriction = frictionValue;
		material.bounciness = 1.0f;
		
		base.OnStart (state);
	}
	
	//	public override void OnFixedUpdate ()
//	{
//		log ("friction", friction);
//		log("part", part);
//		//		log("part.rigidbody", part.rigidbody);
//		//		log("part.rigidbody.collider", part.rigidbody.collider);
//		//		log("part.rigidbody.collider.material", part.rigidbody.collider.material);
//		//
//		
//		log("part.collider", part.collider);
//		log("part.collider.material", part.collider.material);
//		
//		
//		var material = part.collider.material;
//		log("material.dynamicFriction",material.dynamicFriction);			
//		log("material.staticFriction",material.staticFriction);
//		log("material.bounciness", material.bounciness);
//
//		log("material.frictionCombine",material.frictionCombine);
//		
//		material.staticFriction = 0.0f;
//		material.dynamicFriction = 0.0f;
//		material.bounciness = 1.0f;
//		
//		material.frictionCombine = PhysicMaterialCombine.Minimum;
//		
//		base.OnFixedUpdate ();
//	}
}
}