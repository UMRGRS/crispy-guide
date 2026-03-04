using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NueGames.NueDeck.Scripts.Energy;
using NueGames.NueDeck.Scripts.Enums;

public class Tests
{
    [Test]
    public void Strengthen_Increases_Value()
    {
        var result = EnergyStrengthHelper.GetNewEnergyStrengthValue(
            EnergyStrength.Weak,
            ModificationType.Strengthen
        );

        Assert.AreEqual(EnergyStrength.Strong, result);
    }
}
