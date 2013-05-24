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
/// Animates based on: 'this.vessel.orbit.GetRelativeVel().magnitude'
/// Value is the current speed at which the part's
/// vessel is orbitting it's current mainbody.
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Animates based on: 'this.vessel.orbit.GetRelativeVel().magnitude'" +
"\n//Value is the current speed at which the part's" +
"\n//vessel is orbitting it's current mainbody.")]
public class KModuleAnimateOrbitalVelocity : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        return (float)(this.vessel.orbit.GetRelativeVel().magnitude - MinValue) / _Denominator;
    }
}
