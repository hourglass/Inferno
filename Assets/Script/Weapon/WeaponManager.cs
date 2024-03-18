using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //플레이어의 각도를 받아올 델리게이트
    public delegate float PlayerManagerGetDirectionDel();
    public static PlayerManagerGetDirectionDel GetDirectionDel;

    //이펙트 생성위치
    [SerializeField] protected Transform spawnPoint = null;

    protected float attackDelay = 0.4f;
    protected float skillDelay = 2f;

    bool attckEnabled = true;
    bool skillEnabled = true;

    public delegate void ActionDelegate();
    protected ActionDelegate attackDel;
    protected ActionDelegate skillDel;
    protected ActionDelegate passiveDel;

    protected Dictionary<int, ChoiceData> choiceDatas = new Dictionary<int, ChoiceData>();
    protected Dictionary<int, string> choiceInfos = new Dictionary<int, string>();

    private Queue<int> selectedIdQueue = new Queue<int>();
    private List<int> idList = new List<int>();

    private int currentId;

    protected enum ActionType
    {
        Attack,
        Skill,
        Passive
    }

    protected enum ExecutionType
    {
        Immediate,
        AddChain
    }

    protected class ChoiceData
    {
        public ChoiceData()
        {
        }

        public ChoiceData(ActionType _actionType,
                          ExecutionType _executionType,
                          ActionDelegate _action,
                          string _explainText,
                          float _passiveDelay = 0f)
        {
            actionType = _actionType;
            executionType = _executionType;
            actionDel = _action;
            explainText = _explainText;
            passiveDelay = _passiveDelay;
        }

        private string explainText;

        private ActionType actionType;
        public ActionType actionTypeP { get { return actionType; } set { actionType = value; } }

        private ExecutionType executionType;
        public ExecutionType executionTypeP { get { return executionType; } set { executionType = value; } }

        private ActionDelegate actionDel;
        public ActionDelegate actionDelP { get { return actionDel; } set { actionDel = value; } }

        private float passiveDelay = 0f;
        public float PassiveDelay { get { return passiveDelay; } }
    }


    void Awake()
    {
        PlayerManager.CanLookAtDel = CanLookAt;
        PlayerController.InputKeyDel = InputKey;
        ChoiceManager.GetChoiceDel = SelectRandomId;
        ChoiceButton.GetSelectedIdDel = GetSelectedId;
        ChoiceButton.GetExplainTextDel = GetExplainTextById;
        ChoiceButton.ApplyChoiceDel = ChoiceSelected;

        InitWeapon();
        InitChoiceDatas();
        ShuffleIdList();
    }

    void InputKey()
    {
        if (Input.GetMouseButtonDown(0) && attckEnabled)
        {
            StartCoroutine(AttackRoutine());
        }

        if (Input.GetMouseButtonDown(2) && skillEnabled)
        {
            StartCoroutine(SkillRoutine());
        }
    }

    protected virtual void InitWeapon()
    {
        //이 함수는 재정의 될 것임.
    }

    protected virtual void InitChoiceDatas()
    {
        //이 함수는 재정의 될 것임.
    }

    protected void InputChoiceDatas(ActionType actionType,
                                    ExecutionType executionType,
                                    ActionDelegate action,
                                    float passiveDelay = 0f)
    {
        if (choiceInfos.Count > 0)
        {
            choiceDatas[currentId]
                 = new ChoiceData(actionType,
                                  executionType,
                                  action,
                                  choiceInfos[currentId],
                                  passiveDelay);

            idList.Add(currentId);
            currentId++;
        }
    }

    protected void InputChoiceInfos(string explainText)
    {
        choiceInfos[currentId] = explainText;
    }

    private int GetSelectedId()
    {
        if (selectedIdQueue.Count > 0)
        {
            return selectedIdQueue.Dequeue();
        }
        else
        {
            return -1;
        }
    }

    private string GetExplainTextById(int id)
    {
        return choiceInfos[id];
    }

    private void ShuffleIdList()
    {
        if (idList.Count > 0)
        {
            // 리스트 셔플
            for (int i = (idList.Count - 1); i > 0; i--)
            {
                int rand = Random.Range(0, i);

                int temp = idList[i];
                idList[i] = idList[rand];
                idList[rand] = temp;
            }
        }
    }

    private void SelectRandomId()
    {
        int choiceCount = 3;

        if (idList.Count >= choiceCount)
        {
            // 셔플된 리스트에서 3개 고르기
            for (int i = 0; i < choiceCount; i++)
            {
                selectedIdQueue.Enqueue(idList[i]);
            }
        }
    }

    private void ChoiceSelected(int id)
    {
        var actionType = choiceDatas[id].actionTypeP;
        var executionType = choiceDatas[id].executionTypeP;

        if (executionType == ExecutionType.Immediate)
        {
            choiceDatas[id].actionDelP();
        }
        else if (executionType == ExecutionType.AddChain)
        {
            switch (actionType)
            {
                case ActionType.Attack:
                    {
                        attackDel += choiceDatas[id].actionDelP;
                    }
                    break;
                case ActionType.Skill:
                    {
                        skillDel += choiceDatas[id].actionDelP;
                    }
                    break;
                case ActionType.Passive:
                    {
                        var action = choiceDatas[id].actionDelP;
                        var delay = choiceDatas[id].PassiveDelay;
                        if (delay > 0)
                        {
                            StartCoroutine(PassiveRoutine(action, delay));
                        }
                    }
                    break;
            }
        }

        idList.Remove(id);
    }


    private bool CanLookAt()
    {
        bool canLookAt = attckEnabled;
        return canLookAt;
    }

    protected void Create(GameObject obj, Transform spawn, float degree)
    {
        Instantiate(obj, spawn.position, Quaternion.Euler(0f, 0f, (degree - 90) + GetDirectionDel()));
    }

    IEnumerator AttackRoutine()
    {
        attackDel();
        attckEnabled = false;
        yield return new WaitForSeconds(attackDelay);
        attckEnabled = true;
    }

    IEnumerator SkillRoutine()
    {
        skillDel();
        skillEnabled = false;
        yield return new WaitForSeconds(skillDelay);
        skillEnabled = true;
    }

    IEnumerator PassiveRoutine(ActionDelegate action, float delay)
    {
        while (true)
        {
            if (action != null)
            {
                action();
                yield return new WaitForSeconds(delay);
            }
            else
            {
                yield break;
            }
        }
    }
}
