
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
        this.attackName.text = attackName;
        this.attackDescription.text = attackDescription;
        this.reinforcementName.text = reinforcementName;
        this.attackBackground.texture = attackBackground;
        this.reinforcementIcon.texture = reinforcementIcon;
    }
}
