using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public enum Team
    {
        Plant,
        Zombie
    }

    public enum Type
    {
        Unit,
        Trick
    }

    public enum Class
    {
        MegaGrow,
        Guardian
    }

    public enum Trait
    {
        Pea,
        Nut
    }

    public Team team;
    public Type type;
    public Class _class;
    public Trait[] traits;

    // Start is called before the first frame update
    void Start()
    {
        //play animation
        transform.parent.BroadcastMessage("OnPlay", this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnPlay(Card played)
    {
        
    }

    protected void OnAttack()
    {

    }

    protected void OnDeath()
    {

    }

}
