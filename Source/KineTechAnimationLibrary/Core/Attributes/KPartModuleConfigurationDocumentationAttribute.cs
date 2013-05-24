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
/// Used for providing a description of the 
/// PartModule within generated documentation cache.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
public class KPartModuleConfigurationDocumentationAttribute : System.Attribute
{
    public readonly string ModuleDocumentation;

    public KPartModuleConfigurationDocumentationAttribute(string moduleDocumentation)
    {
        if(string.IsNullOrEmpty(moduleDocumentation))
            throw new ArgumentException("moduleDocumentation");
        
        this.ModuleDocumentation = moduleDocumentation;
    }
}
