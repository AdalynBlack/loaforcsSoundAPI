﻿using loaforcsSoundAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using loaforcsSoundAPI.API;
using Unity.Properties;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;

namespace loaforcsSoundAPI.Behaviours {
    public class AudioSourceReplaceHelper : MonoBehaviour {
        internal AudioSource source;
        internal SoundReplacementCollection replacedWith;

        internal static Dictionary<AudioSource, AudioSourceReplaceHelper> helpers = new Dictionary<AudioSource, AudioSourceReplaceHelper>();

        public bool DisableReplacing { get; private set; } = false;

        public bool Loop;
        internal bool _isPlaying;
        
        void Start() {
            
            if(source == null) {
                SoundPlugin.logger.LogWarning($"AudioSource (on gameobject: {gameObject.name}) became null between the OnSceneLoaded callback and Start.");
                return;
            }

            string clipName = (source.clip == null ? "null" : source.clip.name);
            SoundPlugin.logger.LogLosingIt($"AudioSourceReplaceHelper.Start(), gameObject: {gameObject.name}, audioClip.name: " + clipName);
 
            if(source.playOnAwake) {
                if (source.enabled) {
                    source.Play();
                    SoundPlugin.logger.LogLosingIt($"{gameObject.name}:{clipName} calling source.Play() because its playOnAwake and enabled.");
                    _isPlaying = true;
                }
                else {
                    SoundPlugin.logger.LogLosingIt($"{gameObject.name}:{clipName} not calling Play() because its playOnAwake but not enabled.");
                }
            }

            Loop = source.loop;
            source.loop = false;
            SoundPlugin.logger.LogLosingIt($"{gameObject.name}:{clipName}, Looping? {Loop}");

            // this needs to happen after we assign source.loop otherwise things will get cooked
            helpers[source] = this;
        }

        void OnEnable() {
            if(source == null) return;

            helpers[source] = this;
        }

        void OnDestroy() {
            if(source == null) return;
            if(helpers.ContainsKey(source))          
                helpers.Remove(source);
        }

        void LateUpdate() {
	    if (source != null)
	    {
                if (!source.isPlaying && _isPlaying) {
                    if (Loop) {
                        source.Play();
                        SoundPlugin.logger.LogLosingIt($"{gameObject.name}:{(source.clip == null ? "null" : source.clip.name)} succesfully looped!");
                    } else {
                        _isPlaying = false;
                        // this literally handles all other audio clips finishing normally lmao
                    }
                }
	    }
            
            if (replacedWith == null) return;
            if (!replacedWith.group.UpdateEveryFrame) return;
            DisableReplacing = true;

            float currentTime = source.time;

            SoundReplacement replacement = replacedWith.replacements.Where(x => x.TestCondition()).ToList()[0];

            if(replacement.Clip == source.clip) {
                return;
            }

            source.clip = replacement.Clip;
            source.Play();
            source.time = currentTime;

            if (!source.isPlaying) {
                SoundPlugin.logger.LogExtended("Sound ended, resetting update_every_frame");
                DisableReplacing = false;
                replacedWith = null;
            }
        }
    }
}
