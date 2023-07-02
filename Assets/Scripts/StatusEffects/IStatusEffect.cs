using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffect {
    public void ApplyEffect(Unit unit);
    public void RemoveEffect(Unit unit);
}