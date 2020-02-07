﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IGatherEffect
{
    void StartGathering();
    void OnGather(float newPercent);
    void OnRegen(float newPercent);
    void StopGathering();
}
