using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public delegate void ChoiceManagerResumeDel();
    public static ChoiceManagerResumeDel ResumeDel;

    public delegate int WeaponManagerGetSelectedIdDel();
    public static WeaponManagerGetSelectedIdDel GetSelectedIdDel;

    public delegate string WeaponManagerGetExplainTextDel(int id);
    public static WeaponManagerGetExplainTextDel GetExplainTextDel;

    public delegate void WeaponManagerApplyChoiceDel(int id);
    public static WeaponManagerApplyChoiceDel ApplyChoiceDel;


    //WeaponManager weaponManager;

    [SerializeField] Button[] ChoiceBtn = new Button[3];
    [SerializeField] Text[] explainText = new Text[3];

    int[] selectId = new int[3];


    void Start()
    {
        //var player = GameObject.FindGameObjectWithTag("Player");
        //weaponManager = player.GetComponent<WeaponManager>();

        ChoiceManager.SetTextDel = SetText;

        ChoiceBtn[0].onClick.AddListener(delegate { Selected(selectId[0]); });
        ChoiceBtn[1].onClick.AddListener(delegate { Selected(selectId[1]); });
        ChoiceBtn[2].onClick.AddListener(delegate { Selected(selectId[2]); });
    }

    void SetText()
    {
        for (int i = 0; i < selectId.Length; i++)
        {
            selectId[i] = GetSelectedIdDel();
        }

        for (int i = 0; i < explainText.Length; i++)
        {
            explainText[i].text = GetExplainTextDel(selectId[i]);
        }
    }

    void Selected(int id)
    {
        ApplyChoiceDel(id);
        ResumeDel();
    }
}
