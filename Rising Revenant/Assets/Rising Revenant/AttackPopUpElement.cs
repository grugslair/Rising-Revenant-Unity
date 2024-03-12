
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackPopUpElement : MonoBehaviour
{
    [SerializeField]
    private RawImage attackBackground;

    [SerializeField]
    private TMP_Text attackName;
    [SerializeField]
    private TMP_Text attackDescription;
    [SerializeField]
    private TMP_Text reinforcementName;

    [SerializeField]
    private RawImage reinforcementIcon;

   
    public void SetData(string attackName, string attackDescription, string reinforcementName, Texture2D attackBackground, Texture2D reinforcementIcon)
    {
        Debug.Log("Setting data");
        Debug.Log(attackName);
        this.attackName.text = attackName;
        Debug.Log(this.attackName.text);
        this.attackDescription.text = attackDescription;

        this.reinforcementName.text = reinforcementName;
        this.attackBackground.texture = attackBackground;
        this.reinforcementIcon.texture = reinforcementIcon;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(this.attackName.text);
        }
    }
}
