using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // 선택지 정보를 저장할 객체 클래스
    private class ChoiceData
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

        // 함수의 종류
        private ActionType actionType;
        public ActionType actionTypeP { get { return actionType; } set { actionType = value; } }

        // 함수의 실행타입
        private ExecutionType executionType;
        public ExecutionType executionTypeP { get { return executionType; } set { executionType = value; } }

        // 실행할 함수
        private ActionDelegate actionDel;
        public ActionDelegate actionDelP { get { return actionDel; } set { actionDel = value; } }

        // 선택지 설명 텍스트
        private string explainText;
        public string explainTextP { get { return explainText; } }

        // 패시브 함수 주기
        private float passiveDelay = 0f;
        public float passiveDelayP { get { return passiveDelay; } }
    }

    // 함수의 종류를 구분할 enum
    protected enum ActionType
    {
        Attack,
        Skill,
        Passive
    }

    // 함수의 실행 타입을 구분할 enum
    protected enum ExecutionType
    {
        Immediate,
        AddChain
    }


    // 무기가 생성 된 다음에 호출 Awake -> Start
    void Start()
    {
        // 변수 초기화 함수
        InitVariable();

        // 무기 초기화 함수
        InitWeapon();

        // 선택지 정보 입력 함수
        InitChoiceDatas();

        // id 리스트 셔플 함수
        ShuffleIdList();
    }


    // 변수 초기화 함수
    void InitVariable()
    {
        // 델리게이트 등록
        PlayerManager.CanLookAtDel = CanLookAt;              // 방향 전환이 가능한지 확인하는 함수
        Player.AttackDel = Attack;                           // 키 입력 시 실행할 공격 함수
        Player.SkillDel = Skill;                             // 키 입력 시 실행할 스킬 함수
        ChoiceManager.GetChoiceDel = SelectRandomId;         // 랜덤한 Id를 뽑는 함수
        ChoiceButton.GetSelectedIdDel = GetSelectedId;       // 선택된 Id를 전달하는 함수
        ChoiceButton.GetExplainTextDel = GetExplainTextById; // 선택지의 텍스트를 전달하는 함수
        ChoiceButton.ApplyChoiceDel = ChoiceSelected;        // 선택지를 골랐을 때 처리하는 함수

        // 멤버 변수 초기화
        attackDelay = 0.4f;
        skillDelay = 2f;
        attckEnabled = true;
        skillEnabled = true;
    }


    // 무기 초기화 함수
    protected abstract void InitWeapon();

    // 선택지 정보 입력 함수
    protected abstract void InitChoiceDatas();


    // 공격 함수
    void Attack()
    {
        if (attckEnabled)
        {
            StartCoroutine(AttackRoutine());
        }
    }


    // 스킬 함수
    void Skill()
    {
        if (skillEnabled)
        {
            StartCoroutine(SkillRoutine());
        }
    }


    // 선택지 텍스트를 입력하는 함수
    protected void InputChoiceInfos(string explainText)
    {       
        choiceInfos[currentId] = explainText;
    }


    // 선택지 객체 정보를 입력하는 함수
    protected void InputChoiceDatas(ActionType actionType,
                                    ExecutionType executionType,
                                    ActionDelegate action,
                                    float passiveDelay = 0f)
    {
        if (choiceInfos.Count > 0)
        {
            // 선택지 정보 입력
            choiceDatas[currentId]
                 = new ChoiceData(actionType,             // 함수의 종류
                                  executionType,          // 함수의 실행타입
                                  action,                 // 실행할 함수
                                  choiceInfos[currentId], // 선택지 설명 텍스트
                                  passiveDelay);          // 패시브 함수 주기

            idList.Add(currentId); // Id 리스트에 Id 추가
            currentId++;           // 현재 Id 증가
        }
    }


    // Id 리스트를 셔플하는 함수
    private void ShuffleIdList()
    {
        if (idList.Count > 0)
        {
            // i를 감소시키면서 0~(i-1)의 랜덤 인덱스를 뽑아 i와 교환  
            for (int i = (idList.Count - 1); i > 0; i--)
            {
                int rand = Random.Range(0, i);

                int temp = idList[i];
                idList[i] = idList[rand];
                idList[rand] = temp;
            }
        }
    }


    // 셔플된 리스트에서 3개를 골라 큐에 저장하는 함수
    private void SelectRandomId()
    {
        int choiceCount = 3;

        // 선택지가 3개 이상 남았을 때만 수행
        if (idList.Count >= choiceCount)
        {
            // 셔플된 리스트에서 3개 고르기
            for (int i = 0; i < choiceCount; i++)
            {
                selectedIdQueue.Enqueue(idList[i]);
            }
        }
    }


    // 저장된 큐에서 Id를 넘겨주는 함수
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


    // 저장된 객체에서 선택지 설명을 넘겨주는 함수
    private string GetExplainTextById(int id)
    {
        return choiceDatas[id].explainTextP;
    }


    // 선택지를 골랐을 때 처리하는 함수
    private void ChoiceSelected(int id)
    {
        // 함수의 종류 & 실행 타입 가져오기
        var actionType = choiceDatas[id].actionTypeP;
        var executionType = choiceDatas[id].executionTypeP;

        // 함수의 실행 타입 확인
        if (executionType == ExecutionType.Immediate)
        {
            // Immediate 타입이면 바로 수행
            choiceDatas[id].actionDelP();
        }
        else if (executionType == ExecutionType.AddChain)
        {
            // AddChain 타입이면 기존 델리게이트에 추가
            switch (actionType)
            {
                // 함수의 종류 확인
                case ActionType.Attack:
                    {                   
                        // 공격
                        attackDel += choiceDatas[id].actionDelP;
                    }
                    break;
                case ActionType.Skill:
                    {
                        // 스킬
                        skillDel += choiceDatas[id].actionDelP;
                    }
                    break;
                case ActionType.Passive:
                    {
                        // 패시브
                        var action = choiceDatas[id].actionDelP;    // 수행할 함수
                        var delay = choiceDatas[id].passiveDelayP;  // 패시브 수행 주기

                        // delay가 0보다 클 때만 수행
                        if (delay > 0)
                        {                      
                            // 해당 정보로 코루틴 수행
                            StartCoroutine(PassiveRoutine(action, delay));
                        }
                    }
                    break;
            }
        }
       
        idList.Remove(id);  // 선택된 Id를 리스트에서 제거
        ShuffleIdList();    // Id 리스트 셔플
    }


    // 방향 전환 가능 상태를 넘겨주는 함수
    private bool CanLookAt()
    {
        bool canLookAt = attckEnabled;
        return canLookAt;
    }


    // 특정 위치에 게임오브젝트를 생성하는 함수
    protected void Create(GameObject obj, Transform spawn, float degree, Transform parent = null)
    {
        Instantiate(obj, spawn.position, Quaternion.Euler(0f, 0f, (degree - 90) + GetDirectionDel()), parent);
    }


    // 공격 코루틴
    IEnumerator AttackRoutine()
    {
        attackDel();
        attckEnabled = false;
        yield return new WaitForSeconds(attackDelay);
        attckEnabled = true;
    }


    // 스킬 코루틴
    IEnumerator SkillRoutine()
    {
        skillDel();
        skillEnabled = false;
        yield return new WaitForSeconds(skillDelay);
        skillEnabled = true;
    }


    // 패시브 코루틴
    IEnumerator PassiveRoutine(ActionDelegate action, float delay)
    {
        var passiveDelay = new WaitForSeconds(delay);

        while (true)
        {
            if (action != null)
            {
                action();
                yield return passiveDelay;
            }
            else
            {
                yield break;
            }
        }
    }


    //Delegate//
    //플레이어의 각도를 받아올 델리게이트
    public delegate float PlayerManagerGetDirectionDel();
    public static PlayerManagerGetDirectionDel GetDirectionDel;

    // 공격 & 스킬 & 패시브를 수행할 델리게이트
    public delegate void ActionDelegate();
    protected ActionDelegate attackDel;
    protected ActionDelegate skillDel;
    protected ActionDelegate passiveDel;


    //Member Variable//
    // 선택지의 설명 텍스트를 저장할 Dictionary (Id로 식별)
    private Dictionary<int, string> choiceInfos = new Dictionary<int, string>();

    // 선택지의 객체 정보를 저장할 Dictionary (Id로 식별)
    private Dictionary<int, ChoiceData> choiceDatas = new Dictionary<int, ChoiceData>();

    private int currentId; // 선택지 추가 시 증가 시킬 id 변수
    private List<int> idList = new List<int>(); // id를 저장할 List
    private Queue<int> selectedIdQueue = new Queue<int>(); // 랜덤으로 선택된 id를 저장할 Queue

    // 공격 & 스킬 딜레이
    private float attackDelay;
    private float skillDelay;

    // 공격 & 스킬 사용 가능 여부
    bool attckEnabled;
    bool skillEnabled;
}
