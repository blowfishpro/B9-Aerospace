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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[KPartModuleConfigurationDocumentation(
    "\n//Animates based on: The total mass of the current vessel.")]
public class KModuleAnimateMass : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        return (this.vessel.GetTotalMass() - MinValue) / _Denominator;
    }
}
