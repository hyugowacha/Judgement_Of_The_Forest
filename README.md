# JUDGEMENT_OF_THE_FOREST
## Unity 3D 개인 프로젝트

------

### 프로젝트 소개📌

장르: 3D 액션 RPG

개발 기간: 25.02.19. ~ 25.03.03 (14일)

개발 인원: 1명

주요 파트: New Input System, 상태 패턴, Cinemachine

------

### 기술 상세💡

+ ### New Input System

  <img width="550" height="190" alt="image" src="https://github.com/user-attachments/assets/ed8954a1-8f18-46ad-9dad-ab2703e8e7bc" />

   + 모든 상태들은 공통적으로 updatestate(update문에 들어갈 행동을 실행)에서 인풋매니저의 입력값을 바탕으로 상태를 변환함
   
   + C# 이벤트 호출 방식(Invoke C Sharp Events) 사용
 
  ------------

+ ### 플레이어 상태 패턴

  <img width="4408" height="2532" alt="image" src="https://github.com/user-attachments/assets/c2738240-52ed-4a01-8bd4-7a4458f6de6c" />

  인터페이스 IPlayerState로 공통 인터페이스 정의 및 분기 관리

  + #### void EnterState
    
    상태에 진입할 때 수행하는 기능

  + #### void UpdateState
    
    Update문에서 행할 기능
 
  + #### void FixedUpdateState
    
    FixedUpdate문에서 행할 기능
        
  + #### void CheckNowState
    
    현재 무슨 상태인지 갱신하는 기능
 
<img width="490" height="226" alt="image" src="https://github.com/user-attachments/assets/4c54fc88-3fcd-4217-9915-aa92e56d416d" />
     
  ### 핵심 기능

  + ### PlayerStates.cs
       
  + 플레이어의 기본 상태를 관리함 (Idle, Running, Jumping, Falling)
     
  + ### PlayerAttackStates.cs
       
  + 플레이어의 공격에 관련된 상태를 관리함 (ComboAttack1~4, AttackOn)
     
  + ### PlayerSkillStates.cs
       
  + 플레이어의 스킬에 관련된 상태를 관리함 (ESkill, QSkill)
   
  + ### PlayerController.cs
       
  + 플레이어 자체를 움직이는 메인 컨트롤러

    InputSystem을 통해 받은 입력을 기반으로 상태를 전환함
   
-------------------
       
+ ### Virtual Camera를 이용한 궁극기 카메라 컷신

<img width="825" height="458" alt="image" src="https://github.com/user-attachments/assets/cab26202-2ca9-4bd5-a61c-016873cab2d3" />

궁극기 시전 시 시점이 고정되며, 투사체를 발사함에 따라 Dolly Track을 따라 카메라가 이동함. 스킬 시전이 끝나면 기존 시점 카메라로 변경됨.

  + ### IEnumerator changeCamera

  + 스킬 시전 시 카메라 우선도를 바꾸는 코루틴 메서드

    <img width="468" height="256" alt="image" src="https://github.com/user-attachments/assets/8ee56e9d-cd23-4c26-847f-089a132d1baa" />

------------------

### 기술 스택 🛠
 
+ #### Engine: Unity (C#)

+ #### Physics : Raycast / Rigidbody

+ #### VFX: ParticleSystem 

+ #### UI/UX : Unity UI (Canvas, Sprite)

+ #### 카메라 관련 효과: Cinemachine
