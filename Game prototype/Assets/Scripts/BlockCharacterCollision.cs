using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCharacterCollision : MonoBehaviour
{
    public BoxCollider2D characterCollider;
    public BoxCollider2D characterBlockerCollider;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(characterCollider, characterBlockerCollider, true);
    }
}
