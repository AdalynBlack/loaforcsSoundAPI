﻿using loaforcsSoundAPI.API;
using loaforcsSoundAPI.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace loaforcsSoundAPI.LethalCompany.Conditions;
internal class MoonConditionProvider : ConditionProvider {
    public override bool Evaluate(SoundReplaceGroup group, JObject conditionDef) {
        SoundPlugin.logger.LogExtended("LethalCompany:moon_name value: " + StartOfRound.Instance.currentLevel.name);
        return conditionDef["value"].Value<string>() == StartOfRound.Instance.currentLevel.name;
    }
}
