using System;
using UnityEngine;
using System.Collections.Generic;

namespace ExsurgentEngineering
{
    public class FuelBalancer : PartModule
    {
        [KSPField(isPersistant=true)]
        public Vector3
            initialCenterofMass;

        [KSPField(isPersistant=false)]
        public Vector3
            imbalance;

        
        public override void OnStart(StartState state)
        {
            if (state == PartModule.StartState.Editor)
                return;

            // save starting CoM... then just blindly try to balance around it.
            initialCenterofMass = vessel.findLocalCenterOfMass();

            foreach (var res in part.Resources)
                Debug.Log("resource: " + res);

            foreach (PartModule mod in part.Modules)
                Debug.Log("module name" + mod.name);


        }

        [KSPEvent(guiName="Set Flow In",guiActive=true, active=true)]
        public void FlowIn()
        {
            foreach (PartResource res in part.Resources)
                res.flowMode = PartResource.FlowMode.In;
        }



        [KSPEvent(guiName="Set Flow Out",guiActive=true, active=true)]
        public void FlowOut()
        {
            foreach (PartResource res in part.Resources)
                res.flowMode = PartResource.FlowMode.Out;
        }
        
        // assume tanks drain symmetrically, meaning the CoM will only shift along 
        // the ... up? ... axis.
        public override void OnFixedUpdate()
        {
            imbalance = vessel.findLocalCenterOfMass() - initialCenterofMass;
        }

        // stolen from Kethane
        public static List<PartResource> GetConnectedResources(Part part, String resourceName)
        {
            var resources = new List<PartResource>();
            part.GetConnectedResources(PartResourceLibrary.Instance.GetDefinition(resourceName).id, resources);
            return resources;
        }
    }
}

