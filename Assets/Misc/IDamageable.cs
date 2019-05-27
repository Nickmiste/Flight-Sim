using System;

public interface IDamageable
{
    void Damage(float damage);
}

public interface ITargetable
{
    //If null, the object becomes untargetable.
    //To make an object targetable with no extra info, return an empty string.
    string GetTargetInfo();
}

public interface IUnshootable : ITargetable
{

}

public interface ILockable : ITargetable
{

}