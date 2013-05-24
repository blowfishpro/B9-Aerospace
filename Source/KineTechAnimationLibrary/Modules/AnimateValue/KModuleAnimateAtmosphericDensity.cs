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
/// Animates based on: 'this.vessel.atmDensity'. 
/// Value is the density of the atomosphere within
/// the current mainbody of this part's vessel.
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Animates based on: 'this.vessel.atmDensity'. " +
"\n//Value is the density of the atomosphere within" +
"\n//the current mainbody of this part's vessel.")]
public class KModuleAnimateAtmosphericDensity : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        return (float)(this.vessel.atmDensity - MinValue) / _Denominator;
    }
}
