using UnityEngine;

public class PowerUpCollectible : MonoBehaviour, ICollectible
{
    // Define power-up specific fields

    public void Accept(IPlayerVisitor visitor)
    {
        visitor.Visit(this);
    }


    //add sword powerup using visitor interface
}