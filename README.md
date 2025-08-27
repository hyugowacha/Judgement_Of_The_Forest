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
 
    ### 핵심 기능

     + #### ScriptableObject 기반 무기 데이터 관리
     
