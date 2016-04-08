using UnityEngine;
using System.Collections;

// An interface to force entities or objects to have a way of identifing when doing damage
public interface IDamageSource {
    Name names { get; }
}
