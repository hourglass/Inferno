# Inferno
Unity 프로젝트  
## Weapon
[WeaponManager](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/WeaponManager.cs)  
- 마우스 입력을 받아 공격과 스킬 함수를 실행합니다.  
- 아래의 무기 컴포넌트에서 함수를 델리게이트로 넘겨받아 공격, 스킬, 패시브 함수를 실행합니다.  
- 선택지의 정보를 저장하는 객체를 관리하고 선택지와 관련된 기능들을 처리합니다.    
---
[SwordManager](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/SwordManager.cs)  
[ArrowManager](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/ArrowManager.cs)  
[TowerManager](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/TowerManager.cs)  
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
- 내용  
---
[PlayerManager](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Player/PlayerManager.cs)  
[PlayerStat](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Player/PlayerStat.cs)  
[Circle](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Player/Circle.cs)  
</br>
## Enemy  
[EnemyController](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyController.cs)  
[EnemyManager](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyManager.cs)  
[EnemyStat](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyStat.cs)  
[EnemyUI](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyUI.cs)  
