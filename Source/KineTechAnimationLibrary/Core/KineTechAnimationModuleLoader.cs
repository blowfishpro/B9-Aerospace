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
using KSP.IO;
using UnityEngine;

public class KineTechAnimationModuleLoader : KSP.Testing.UnitTest
{
    private const string SETTINGS_FILE_NAME = "Settings.KineTechAnimation";


    public KineTechAnimationModuleLoader()
        : base()
    {
        HandleSettings();
    }

    public void HandleSettings()
    {
        if (!File.Exists<KineTechAnimationModuleLoader>(SETTINGS_FILE_NAME))
        {
            ConfigNode node = new ConfigNode();
            node.AddValue("DumpDocumentationOnStartup", false);
            node.Save(IOUtils.GetFilePathFor(typeof(KineTechAnimationModuleLoader), SETTINGS_FILE_NAME));
        }

        if (File.Exists<KineTechAnimationModuleLoader>(SETTINGS_FILE_NAME))
        {
            ConfigNode node = ConfigNode.Load(
                IOUtils.GetFilePathFor(typeof(KineTechAnimationModuleLoader), SETTINGS_FILE_NAME));

            if(node == null)
                return;

            //Dump Documentation
            if(node.HasValue("DumpDocumentationOnStartup"))
            {
                bool working = false;
                if(bool.TryParse(node.GetValue("DumpDocumentationOnStartup"), out working))
                {
                    GameObject dumperObject = new GameObject("Kine-Tech Animation - ConfigDocumentationGenerator");
                    dumperObject.AddComponent<KConfigDocumentationGenerator>();
                    GameObject.DontDestroyOnLoad(dumperObject);
                }
            }
                
        }
    }
}
