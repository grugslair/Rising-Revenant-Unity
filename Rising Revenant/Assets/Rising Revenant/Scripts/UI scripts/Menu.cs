using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;// pretty sure this is to delete
    public bool open;

    public virtual void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}
