# Inferno
Unity 프로젝트  
## Weapon
[Weapon](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/Weapon.cs)  
- 무기와 관련된 기능을 수행하고 관리하는 컴포넌트입니다.  
- 아래의 무기 컴포넌트에서 함수를 delegate로 넘겨받아 공격, 스킬, 패시브 함수를 수행합니다.  
- 선택지의 정보를 저장하는 객체를 관리하고 선택지와 관련된 기능들을 처리합니다.  
---
[WeaponSword](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/WeaponSword.cs)  
[WeaponArrow](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/WeaponArrow.cs)  
[WeaponTower](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/WeaponTower.cs)  
- 무기별 공격, 스킬, 패시브 함수가 작성된 컴포넌트입니다.  
- 부모 클래스인 WeaponManager의 객체에 선택지 정보와 함께 작성된 함수를 넘겨줍니다.  
---
[ChoiceManager](https://github.com/hourglass/Inferno/blob/main/Assets/Script/System/ChoiceManager.cs)
- 선택지 UI를 관리하는 컴포넌트입니다.  
- 플레이어의 레벨 업 시 게임을 일시정지 하고 선택지 UI를 출력합니다.  
---
[ChoiceButton](https://github.com/hourglass/Inferno/blob/main/Assets/Script/System/ChoiceButton.cs)  
- 선택지의 버튼을 관리하는 컴포넌트입니다.  
- WeaponManager로부터 선택지의 설명 텍스트를 받아와 버튼에 적용합니다.  
- onClick 이벤트로 선택된 항목의 Id를 WeaponManager에게 넘겨줍니다.
## Player
[PlayerController](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Player/PlayerController.cs)  
- 플레이어의 조작 담당하는 컴포넌트입니다.  
- 마우스 방향으로 플레이어를 이동 시키고 대시 함수를 수행합니다.  
- WeaponManager에게 공격, 스킬 함수를 넘겨받아 입력 시 함수를 수행합니다.  
---
[PlayerManager](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Player/PlayerManager.cs)  
- 플레이어가 필요한 정보를 관리하는 컴포넌트입니다.  
- 플레이어 스프라이트를 마우스 방향과 적이 있는 방향으로 회전시키는 함수를 수행합니다.  
- 무기 선택창에서 선택된 항목의 인덱스를 받아와 인덱스에 해당하는 무기를 생성합니다.  
---
[PlayerStat](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Player/PlayerStat.cs)  
- 플레이어의 능력치를 관리하는 컴포넌트입니다.
- 피해를 받았을 때의 처리를 수행하고 UI를 갱신 합니다.
- 경험치를 얻었을 때의 처리를 수행하고 UI를 갱신 합니다.
- 플레이어 사망 시의 처리를 수행합니다.
---
## Enemy
[EnemyController](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyController.cs)  
- 적군을 조종하는 컴포넌트입니다.
- 적들의 이동과 공격 함수를 수행합니다.
---
[EnemyManager](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyManager.cs)  
- 적군을 생성하고 관리하는 컴포넌트입니다.  
- 웨이브마다 생성하는 적군의 수를 관리하고 모든 적군을 처치 시 다음 웨이브를 진행합니다.  
---
[EnemyStat](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyStat.cs)  
- 적군의 능력치를 관리하는 컴포넌트입니다.
- 피해를 받았을 때의 처리를 수행하고 UI를 갱신 합니다.
- 적군 사망 시의 처리를 수행합니다.
---
