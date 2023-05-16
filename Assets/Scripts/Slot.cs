using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public virtual void OnDrop(PointerEventData eventData) { }

    public virtual void InitSlot() { }
}