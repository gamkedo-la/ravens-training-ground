using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffect {
    void ApplyEffect(Unit unit);
    void RemoveEffect(Unit unit);
}