/* * * * * * * * * * * * * * * */
/* Author: Dustin 'Kine' Judson
/*
/* This work is licensed under the 
/* Creative Commons Attribution-ShareAlike 3.0 Unported License. 
/*
/* To view a copy of this license, visit: 
/* http://creativecommons.org/licenses/by-sa/3.0/
/* 
/* A the following must be included with 
/* any and all formats using this work or any
/* derivatives of this work:
/* 
/* "Kine-Tech Animation Library by Dustin 'Kine' Judson is viewable at: https://github.com/KineMorto/KineTechAnimationLibrary"
/*
/* * * * * * * * * * * * * * * */
using UnityEngine;

/// <summary>
/// Value is the current distance from the active
/// vessel which is tartting us. When we are not
/// targetted, value is 0. Very effective to use in tandem with
/// KModuleAnimateTargetDistance.
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Value is the current distance from the active"+
"\n//vessel which is tartting us. When we are not" +
"\n//targetted, value is 0. Very effective to use in tandem with" +
"\n//KModuleAnimateTargetDistance.")]
public class KModuleAnimateTargettedDistance : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        if(FlightGlobals.fetch == null)
            return 0f;

        if(this.vessel == FlightGlobals.ActiveVessel)
            return LastNormalTime;


        if(FlightGlobals.fetch.VesselTarget != this.vessel)
            return 0f;

        Vector3 working = FlightGlobals.ActiveVessel.transform.localPosition;
        working -= this.vessel.transform.position;

        return (float)((working.magnitude - MinValue) / _Denominator);
    }

    protected override void UpdateModule()
    {
        base.UpdateModule();

        this.IsLocked = this.vessel == FlightGlobals.ActiveVessel;
    }
}
