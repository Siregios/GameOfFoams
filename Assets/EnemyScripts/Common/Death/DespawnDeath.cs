using UnityEngine;
using System.Collections;

public class DespawnDeath : MonoBehaviour, IDeathAction {
    public void Die()
    {
        Destroy(transform.root.gameObject);
    }
}
