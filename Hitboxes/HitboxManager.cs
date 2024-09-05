using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HitboxManager : SerializedMonoBehaviour
{
    [DictionaryDrawerSettings(KeyLabel = "Unique Identifier", ValueLabel = "Hitbox Controller")]
    [SerializeField] private Dictionary<int, HitboxController> _hitboxes = new Dictionary<int, HitboxController>();
    public HitboxController GetHitbox(int uid)
    {
        HitboxController hitbox;
        if (!_hitboxes.TryGetValue(uid, out hitbox))
        {
            Debug.LogError("Hitbox with uid " + uid + " not found. \n Did you set it correctly in the attack data or did you forget to add it to the dictionary?");
        }
        return hitbox;
    }
}
