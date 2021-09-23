using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    public void Deletus()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
