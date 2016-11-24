using UnityEngine;
using System.Collections;

public class Mage : BaseClass {

	public Mage()
    {
        TheClassType = ClassType.Mage;
        Stamina = 15;
        Strength = 0;
        Intellect = 10;
        Agility = 0;
        Armor = 0;
        MovementSpeed = 160;
        Health = (Stamina * 1.5f) + 10;
        CurrentHealth = Health;
        MaxEnergy = (Intellect * 1.1f) + 5;
        CurrentEnergy = MaxEnergy;
        
    }
}
