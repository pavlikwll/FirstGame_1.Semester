using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;
    public UnityEvent onSelected;
    public UnityEvent onDeselected;

    public bool reusable;
    public bool destroyAfterUse;

    private bool alreadyInteracted;
    
    public void Interact()
    {
        onInteract?.Invoke();
        alreadyInteracted = true;
        
        if(destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
