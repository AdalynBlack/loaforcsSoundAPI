﻿using loaforcsSoundAPI.API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace loaforcsSoundAPI.Formats {
    internal class WavAudioFormat : AudioFileFormat {
        public override AudioClip LoadAudioClip(string path) {
            return LoadFromUWR(path, AudioType.WAV);
        }
    }
}
