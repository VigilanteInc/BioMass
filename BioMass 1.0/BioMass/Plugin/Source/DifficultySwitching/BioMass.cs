/*Based on TAC-Lifesupport TacLifeSupport.cs*/
/**
 * Thunder Aerospace Corporation's Life Support for Kerbal Space Program.
 * Written by Taranis Elsu.
 * 
 * (C) Copyright 2013, Taranis Elsu
 * 
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 * 
 * This code is licensed under the Attribution-NonCommercial-ShareAlike 3.0 (CC BY-NC-SA 3.0)
 * creative commons license. See <http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode>
 * for full details.
 * 
 * Attribution — You are free to modify this code, so long as you mention that the resulting
 * work is based upon or adapted from this code.
 * 
 * Non-commercial - You may not use this work for commercial purposes.
 * 
 * Share Alike — If you alter, transform, or build upon this work, you may distribute the
 * resulting work only under the same or similar license to the CC BY-NC-SA 3.0 license.
 * 
 * Note that Thunder Aerospace Corporation is a ficticious entity created for entertainment
 * purposes. It is in no way meant to represent a real entity. Any similarity to a real entity
 * is purely coincidental.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;
using KSP.IO;

namespace KSPBioMass
{
    /*
     * HookMethod to get BioMass Scenario load at Spacecenter
     * Credits to TAC Life Support
    */
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class AddBioMassToScenarioModules : MonoBehaviour
    {
        void Start()
        {
            var game = HighLogic.CurrentGame;

            ProtoScenarioModule psm = game.scenarios.Find(s => s.moduleName == typeof(BioMass).Name);
            if (psm == null)
            {
                this.Log_DebugOnly("Adding BioMass to Scenarios");
                psm = game.AddProtoScenarioModule(typeof(BioMass), GameScenes.SPACECENTER,GameScenes.FLIGHT);
            }
            else
            {
                if (!psm.targetScenes.Any(s => s == GameScenes.SPACECENTER))
                {
                    psm.targetScenes.Add(GameScenes.SPACECENTER);
                }
                if (!psm.targetScenes.Any(s => s == GameScenes.FLIGHT))
                {
                    psm.targetScenes.Add(GameScenes.FLIGHT);
                } 
            }
        }
    }

    public class BioMass : ScenarioModule
    {
        public static BioMass Instance { get; private set; }
        public Settings globalSettings { get; private set; }
        //public SaveGame saveGame { get; private set; }
        private readonly List<Component> controller = new List<Component>();

        public BioMass()
        {
            this.Log_DebugOnly("Constructor BioMass");
            Instance = this;
            globalSettings = new Settings();
            //saveGame = new SaveGame();
        }

        public override void OnAwake()
        {

            this.Log_DebugOnly("Wakeup in " + HighLogic.LoadedScene);
            base.OnAwake();

            globalSettings.Load();

            if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
            {
                this.Log_DebugOnly("Adding SpaceCenter Controller");
                var c = gameObject.AddComponent<SpaceCenterController>();
                controller.Add(c);
                globalSettings.controller.Add(c);
            }
        }

        public override void OnLoad(ConfigNode gameNode)
        {
            base.OnLoad(gameNode);

            //saveGame.Load(gameNode);
            globalSettings.Load();
        }

        public override void OnSave(ConfigNode gameNode)
        {
            base.OnSave(gameNode);
            //saveGame.Save(gameNode);

            globalSettings.Save();
        }

        void OnDestroy()
        {
            this.Log("OnDestroy");
            foreach (Component c in controller)
            {
                Destroy(c);
            }
            controller.Clear();
        }
    }
}
