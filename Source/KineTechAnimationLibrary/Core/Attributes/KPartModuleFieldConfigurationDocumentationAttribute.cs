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

/// <summary>
/// Used for documenting usage of a field 
/// that will be serialized with a 'KSP.ConfigNode';
/// </summary>
[AttributeUsage(AttributeTargets.Field , AllowMultiple=false, Inherited=true)]
public class KPartModuleFieldConfigurationDocumentationAttribute : Attribute
{
    public readonly string DefaultValue;
    public readonly string Description;

    public KPartModuleFieldConfigurationDocumentationAttribute(string defaultValue, string description)
    {
        if (string.IsNullOrEmpty(defaultValue))
            throw new ArgumentNullException("defaultValue");

        if (string.IsNullOrEmpty(description))
            throw new ArgumentNullException("description");

        this.DefaultValue = defaultValue;
        this.Description = description;
    }
}
