# Inferno
Unity 프로젝트
</br>
## WeaponManager
[WeaponManager.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/WeaponManager.cs)  
- 마우스 입력을 받아 공격과 스킬 함수를 실행합니다.  
- 아래의 무기 컴포넌트에서 함수를 델리게이트로 넘겨받아 공격, 스킬, 패시브 함수를 실행합니다.  
- 선택지의 정보를 저장하는 객체를 관리하고 선택지와 관련된 기능들을 처리합니다.
## Weapon
[SwordManager.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/SwordManager.cs)  
[ArrowManager.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/ArrowManager.cs)  
[TowerManager.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Weapon/TowerManager.cs)  
- 무기별 공격, 스킬, 패시브 함수가 작성된 컴포넌트 입니다.  
- 부모 클래스인 WeaponManager의 객체에 선택지 정보와 함께 작성된 함수를 넘겨줍니다.  
</br>
[ChoiceManager.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/System/ChoiceManager.cs)  
[ChoiceButton.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/System/ChoiceButton.cs)  
</br>
## Player
[PlayerController.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Player/PlayerController.cs)  
[PlayerManager.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Player/PlayerManager.cs)  
[PlayerStat.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Player/PlayerStat.cs)  
[Circle.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Player/Circle.cs)  
</br>
## Enemy  
[EnemyController.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyController.cs)  
[EnemyManager.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyManager.cs)  
[EnemyStat.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyStat.cs)  
[EnemyUI.cs](https://github.com/hourglass/Inferno/blob/main/Assets/Script/Enemy/EnemyUI.cs)  
