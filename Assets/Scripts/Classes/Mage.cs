using UnityEngine;
using System.Collections;

public class Mage : BaseClass {

	public Mage()
    {
        TheClassType = ClassType.Mage;
        Stamina = 100;
        Strength = 0;
        Intellect = 10;
        Agility = 0;
        Armor = 0;
        OldMovementSpeed = 160;
        MovementSpeed = OldMovementSpeed;
        Health = (Stamina * 1.5f) + 100;
        CurrentHealth = Health;
        MaxEnergy = (Intellect * 1.1f) + 100;
        CurrentEnergy = MaxEnergy;
        
    }
}
