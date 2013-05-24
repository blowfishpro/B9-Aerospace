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

/// <summary>
/// Animates based on: 'KSP.FlightGlobals.ship_tgtSpeed'. 
/// If we are not the target, or the target is 
/// lost/deselected, and interpolation is not active the 
/// value will jump back to 0.0f. Suggested to use in tandem
/// with KModuleAnimateTargetSpeed.
/// </summary>
[KPartModuleConfigurationDocumentation(
"Animates based on: 'KSP.FlightGlobals.ship_tgtSpeed'. " +
"\n//If we are not the target, or the target is " +
"\n//lost/deselected, and interpolation is not active the " +
"\n//value will jump back to 0.0f. Suggested to use in tandem" +
"\n//with KModuleAnimateTargetSpeed.")]

public class KModuleAnimateTargettedSpeed : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        if(FlightGlobals.fetch == null)
            return 0f;

        if(this.vessel == FlightGlobals.ActiveVessel)
            return LastNormalTime;

        if((FlightGlobals.fetch.VesselTarget as Vessel) != this.vessel)
            return 0f;

        return (float)((FlightGlobals.ship_tgtSpeed - MinValue) / _Denominator);
    }

    protected override void UpdateModule()
    {
        base.UpdateModule();

        this.IsLocked = this.vessel == FlightGlobals.ActiveVessel;
    }
}
