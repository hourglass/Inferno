using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // 선택지 정보를 저장할 객체 클래스
    private class ChoiceData
    {
        public ChoiceData(ActionType actionType,
                          ExecutionType executionType,
                          ActionDelegate action,
                          string explainText,
                          float passiveDelay = 0f)
        {
            _actionType = actionType;
            _executionType = executionType;
            _actionDel = action;
            _explainText = explainText;
            _passiveDelay = passiveDelay;
        }

        // 함수의 종류
        private ActionType _actionType;
        public ActionType actionType { get { return _actionType; } }

        // 함수의 실행타입
        private ExecutionType _executionType;
        public ExecutionType executionType { get { return _executionType; } }

        // 실행할 함수
        private ActionDelegate _actionDel;
        public ActionDelegate actionDel { get { return _actionDel; } }

        // 선택지 설명 텍스트
        private string _explainText;
        public string explainText { get { return _explainText; } }

        // 패시브 함수 주기
        private float _passiveDelay = 0f;
        public float passiveDelay { get { return _passiveDelay; } }
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
                = new ChoiceData(
                     actionType,             // 함수의 종류
                     executionType,          // 함수의 실행타입
                     action,                 // 실행할 함수
                     choiceInfos[currentId], // 선택지 설명 텍스트
                     passiveDelay            // 패시브 함수 주기
                     );

            idList.Add(currentId); // Id 리스트에 Id 추가
            currentId++;           // 현재 Id 증가
        }
    }


    // 선택지 텍스트를 입력하는 함수
    protected void InputChoiceInfos(string explainText)
    {
        choiceInfos[currentId] = explainText;
    }


    // 오브젝트를 생성하는 함수
    protected void Create(string key, Transform spawn, float degree = 0f, Transform parent = null)
    {
        Quaternion direction = Quaternion.Euler(0f, 0f, GetDirectionDel() + (degree - 90f));
        ObjectPoolManager.instance.Spawn(key, spawn.position, direction, parent); ;
    }
    

    // 무기 초기화 함수
    protected abstract void InitWeapon();


    // 선택지 정보 입력 함수
    protected abstract void InitChoiceDatas();


    // 무기가 생성 된 다음에 호출 Awake -> Start
    private void Start()
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


    private void OnDestroy()
    {
        // 델리게이트 해제
        PlayerManager.CanLookAtDel -= CanLookAt;
        Player.AttackDel -= Attack;
        Player.SkillDel -= Skill;
        ChoiceManager.GetChoiceDel -= SelectRandomId;
        ChoiceButton.GetSelectedIdDel -= GetSelectedId;
        ChoiceButton.GetExplainTextDel -= GetExplainTextById;
        ChoiceButton.ApplyChoiceDel -= ChoiceSelected;
    }


    // 변수 초기화 함수
    private void InitVariable()
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


    // 공격 함수
    private void Attack()
    {
        if (attckEnabled)
        {
            StartCoroutine(AttackRoutine());
        }
    }


    // 스킬 함수
    private void Skill()
    {
        if (skillEnabled)
        {
            StartCoroutine(SkillRoutine());
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
        return choiceDatas[id].explainText;
    }


    // 선택지를 골랐을 때 처리하는 함수
    private void ChoiceSelected(int id)
    {
        // 함수의 종류 & 실행 타입 가져오기
        var actionType = choiceDatas[id].actionType;
        var executionType = choiceDatas[id].executionType;

        // 함수의 실행 타입 확인
        if (executionType == ExecutionType.Immediate)
        {
            // Immediate 타입이면 바로 수행
            choiceDatas[id].actionDel();
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
                        attackDel += choiceDatas[id].actionDel;
                    }
                    break;
                case ActionType.Skill:
                    {
                        // 스킬
                        skillDel += choiceDatas[id].actionDel;
                    }
                    break;
                case ActionType.Passive:
                    {
                        // 패시브
                        var action = choiceDatas[id].actionDel;    // 수행할 함수
                        var delay = choiceDatas[id].passiveDelay;  // 패시브 수행 주기

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


    // 공격 코루틴
    private IEnumerator AttackRoutine()
    {
        attackDel();
        attckEnabled = false;
        yield return new WaitForSeconds(attackDelay);
        attckEnabled = true;
    }


    // 스킬 코루틴
    private IEnumerator SkillRoutine()
    {
        skillDel();
        skillEnabled = false;
        yield return new WaitForSeconds(skillDelay);
        skillEnabled = true;
    }


    // 패시브 코루틴
    private IEnumerator PassiveRoutine(ActionDelegate action, float delay)
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


    // Delegate //
    // 플레이어의 각도를 받아올 델리게이트
    public delegate float PlayerManagerGetDirectionDel();
    public static PlayerManagerGetDirectionDel GetDirectionDel;

    // 공격 & 스킬 & 패시브를 수행할 델리게이트
    public delegate void ActionDelegate();
    protected ActionDelegate attackDel;
    protected ActionDelegate skillDel;
    protected ActionDelegate passiveDel;

    protected string currentAttackKey;
    protected string currentSkillKey;

    // Member Variable //
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
    private bool attckEnabled;
    private bool skillEnabled;
}
