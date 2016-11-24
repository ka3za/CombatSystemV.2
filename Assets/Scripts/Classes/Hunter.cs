using UnityEngine;
using System.Collections;

public class Hunter : BaseClass {

	public Hunter()
    {
        TheClassType = ClassType.Hunter;
        Stamina = 20;
        Strength = 0;
        Intellect = 0;
        Agility = 10;
        Armor = 5;
        MovementSpeed = 170;
        Health = (Stamina * 1.5f) + 10;
        CurrentHealth = Health;
        MaxEnergy = 100;
        CurrentEnergy = MaxEnergy;
    }
}
