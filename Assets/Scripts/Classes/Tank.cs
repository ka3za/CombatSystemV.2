using UnityEngine;
using System.Collections;

public class Tank : BaseClass {

	public Tank()
    {
        TheClassType = ClassType.Tank;
        Stamina = 200;
        Strength = 3;
        Intellect = 0;
        Agility = 0;
        Armor = 10;
        OldMovementSpeed = 120;
        MovementSpeed = OldMovementSpeed;
        Health = (Stamina * 1.5f) + 10;
        CurrentHealth = Health;
        MaxEnergy = 100;
        CurrentEnergy = MaxEnergy;

    }
}
