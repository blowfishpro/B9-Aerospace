using System;
using UnityEngine;
using System.Collections.Generic;

namespace ExsurgentEngineering
{
    public class Transformer : PartModule
    {
    
        [KSPField]
        public string
            TransformName;
        [KSPField]
        public string
            CloneTransformName;

        [KSPField]
        public Vector3
            TransformCenter = Vector2.zero;

        void DoIt()
        {
//            Debug.Log("Transformer.OnAwake()");
//            Debug.Log("TransformName: " + TransformName);
            var newGO = new GameObject(TransformName);
            var newTranny = newGO.transform;

            newTranny.position = TransformCenter;
                
            var downThere = -part.transform.up * 1000f;
            newTranny.LookAt(downThere);


            newGO.name = TransformName;
            newTranny.name = TransformName;


            var modelTranny = part.FindModelTransform("model");
            newTranny.parent = modelTranny;
            //newTranny.parent = part.transform;

//            Debug.Log("newGO.name: " + newGO.name);
//            Debug.Log("newTranny.name " + newTranny.name);
            newGO.name = TransformName;
            newTranny.name = TransformName;
//            foreach (var tranny in part.transform.GetComponentsInChildren<Transform>())
//            {
//                Debug.Log("tranny.name: " + tranny.name);
//            }
            foreach (Transform tranny in part.transform)
            {
                Debug.Log("tranny.name: " + tranny.name);
            }
        }

        void AddNew()
        {
            var tranny = part.FindModelTransform(TransformName);
            if (tranny == null)
            {
                Debug.Log("no tranny with name: " + TransformName + " found. adding.");
                DoIt();
                var thrustTransforms = new List<Transform>(base.part.FindModelTransforms(TransformName));
                Debug.Log("thrustTransforms.Count: " + thrustTransforms.Count);
                foreach (var thrustTranny in thrustTransforms)
                {
                    Debug.Log("thrustTranny.name: " + thrustTranny.name);
                }
            } else
            {
                Debug.Log("tranny.localRotation: " + tranny.localRotation);
                Debug.Log("tranny.localPosition: " + tranny.localPosition);
            }
        }

        void AddClone()
        {
            var original = part.FindModelTransform(CloneTransformName);

            var clone = (Transform)Transform.Instantiate(original);
            clone.name = TransformName;

        }

        public override void OnAwake()
        {
            var tranny = part.FindModelTransform(TransformName);
            if (tranny == null)
            {
                if (CloneTransformName == null)
                {
                    AddNew();
                } else
                {
                    AddClone();
                }

            }




        }
        
    }
}

