using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragBeh1 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public enum Card { DEFAULT, ITEM_SWORD, ITEM_SKILLS, MAGIC, SWORD };
    public Card TypeOfItem = Card.DEFAULT;

    private enum CardState { EQUIPMENT, SLOT };
    [SerializeField]
    private CardState itemState = CardState.EQUIPMENT;

    private GameObject draggedCardCopy = null;
    public bool dropped = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        switch (itemState)
        {
            case CardState.EQUIPMENT:

                draggedCardCopy = Instantiate(gameObject, transform.parent.parent.parent);


                break;

            case CardState.SLOT:

                this.transform.SetParent(this.transform.parent.parent);

                break;                
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggedCardCopy.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dropped == false) 
        {
            Destroy(draggedCardCopy);
        }
    }
}
