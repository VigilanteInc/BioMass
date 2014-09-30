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

using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Tac;

namespace KSPBioMass
{
	public class SpaceCenterController : MonoBehaviour, ISavable
    {
        private Settings globalSettings;
        //private SaveGame saveGame;
        private ButtonWrapper button;
        private SavedGameConfigWindow configWindow;

        public SpaceCenterController()
        {
            this.Log("Constructor SCC");
            globalSettings = BioMass.Instance.globalSettings;
            //saveGame = BioMass.Instance.saveGame;
			/*button = new ButtonWrapper(new Rect(Screen.width * 0.75f, 0, 32, 32), Settings.PathTextures+"/herbIcon",
                "BM", "BioMass Configuration Window", OnIconClicked, "HerbIcon");*/
			//button = new ButtonWrapper(new Rect(Screen.width * 0.75f, 0, 32, 32), "BioMass/Textures/HerbIcon","BM", "BioMass Configuration Window", OnIconClicked, "HerbIcon");
			button = new ButtonWrapper(new Rect(Screen.width * 0.75f, 0, 36, 36), "BioMass/Textures/bioIcon","BM", "BioMass Configuration Window", OnIconClicked, "bioIcon");
			configWindow = new SavedGameConfigWindow(globalSettings);
            this.Log(Settings.PathTextures + "/bioIcon");
        }

		void Start()
		{
			this.Log("Start");
			button.Visible = true;
		}

        public void Load(ConfigNode globalNode)
        {
            button.Load(globalNode);
            configWindow.Load(globalNode);
        }

        public void Save(ConfigNode globalNode)
        {
            button.Save(globalNode);
            configWindow.Save(globalNode);
        }

        void OnDestroy()
        {
			this.Log("OnDestroy");
            button.Destroy();
        }

        private void OnIconClicked()
        {
            configWindow.ToggleVisible();
        }
    }
}
