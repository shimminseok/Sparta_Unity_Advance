 # Sparta_Unity_Advance
-내일 배움 캠프 Unity 심화 프로젝트 입니다.

내일배움캠프 Unity 심화 과정에서 진행한 게임 프로젝트입니다. 플레이어와 적의 상태 머신, 아이템과 인벤토리, 스탯 및 버프 시스템 등 다양한 기능 구현을 목표로 합니다.

## 요구 사항
- **Unity 2022.3.17f1** 이상 버전
- Universal Render Pipeline(URP) 패키지와 Cinemachine, TextMeshPro 등을 사용합니다.

## 폴더 구조
- `Assets/1. Scenes` – 게임 씬 파일
- `Assets/2. Scripts` – 게임 로직 스크립트
- `Assets/3. Prefabs` – 프리팹 및 오브젝트
- `Assets/6. Material` – 머티리얼 리소스
- `Assets/10. Tables` – 스크립터블 오브젝트 기반 테이블 데이터

## 시작하기
1. Unity Hub에서 본 프로젝트 폴더를 열어 `MainScene.unity` 혹은 `SampleScene.unity`을 실행합니다.
2. 처음 열 때 필요한 패키지 의존성(URP, Cinemachine 등)을 내려받도록 Unity 패키지 매니저를 실행합니다.

## 주요 기능
- 플레이어와 적의 상태 머신 기반 움직임 및 전투 로직
- 스탯 관리와 버프/디버프, 지속 효과 적용 시스템
- 드래그 앤드 드롭 인벤토리와 아이템 장비·소비·강화 기능
- 스테이지별 웨이브 진행과 네비게이션 메쉬 기반 맵 자동 생성
- 오브젝트 풀링을 이용한 적 및 맵 오브젝트 재사용
- 아이템·몬스터·스테이지 등 스크립터블 오브젝트 테이블 관리
- 인벤토리, 캐릭터 정보, 스테이지 선택 등 다양한 UI 패널 관리
- 골드와 최고 스테이지 기록을 저장하는 계정 시스템

## 구현 사항
<details>
<summary>기본 UI 구현(난이도: ★☆☆☆☆)</summary>
<div markdown="1">
 
 - 게임 화면에 HP, MP, 경험치 바, 현재 스테이지, 골드 및 재화 등의 정보를 표시합니다.
 ![image](https://github.com/user-attachments/assets/f574a9b7-e29c-409b-b0f1-ab2170c8a870)

</div>
</details>

<details>
<summary>플레이어 AI 시스템 (난이도: ★★★☆☆)</summary>
<div markdown="1">

 - 플레이어가 직접 조작하지 않아도 앞을 향해 나아가며, 적을 발견하면 일정 시간마다 자동으로 적을 공격합니다.
  ![Image](https://github.com/user-attachments/assets/51b88004-5cfc-4900-8ef3-c16fd71de17f)
</div>
</details>

<details>
<summary>아이템 및 업그레이드 시스템 (난이도: ★★★☆☆)</summary>
<div markdown="1">
 
 ![Image](https://github.com/user-attachments/assets/44a300d3-ec31-4c8e-9334-528c046091ad)
</div>
</details>

<details>
<summary>게임 내 통화 시스템 (난이도: ★★★☆☆)</summary>
<div markdown="1">

- 게임 내에서 사용할 수 있는 가상의 통화 시스템입니다. 이 통화는 클릭이나 게임 내 활동을 통해 얻을 수 있습니다.
- 스테이지 클리어시 일정량을 게임 재화를 얻을 수 있습니다.

</div>
</details>

<details>
<summary>아이템 및 장비 창 UI 구현 (난이도: ★★★☆☆)</summary>
<div markdown="1">

 - 화면의 버튼을 클릭하면 인벤토리 창이 열리고, 여기서 아이템을 장착하거나 사용할 수 있습니다.
   ![Image](https://github.com/user-attachments/assets/59fce35f-c38b-4cc8-acb7-7b452a5df492)
</div>
</details>

<details>
<summary>스테이지 시스템 (난이도: ★★★★☆)</summary>
<div markdown="1">

- 다양한 스테이지를 구성하고, 플레이어가 원하는 스테이지를 선택하여 입장하는 기능입니다.
  ![Image](https://github.com/user-attachments/assets/594979e8-674a-458e-90f9-c5199a97480d)
</div>
</details>

<details>
<summary>ScriptableObject를 이용한 데이터 관리 (난이도: ★★★★☆)</summary>
<div markdown="1">

![image](https://github.com/user-attachments/assets/99faa0ae-4697-4907-b8ad-191fd7b8da2a)
![image](https://github.com/user-attachments/assets/92ce101d-db32-4219-ac62-eb8556d46a88)
![image](https://github.com/user-attachments/assets/3155b9e1-b7e2-43af-9bb8-9d5bfd0ec7cc)
![image](https://github.com/user-attachments/assets/f0f24774-8f19-4305-95b8-e6414078e2fb)

</div>
</details>

<details>
<summary>파티클 시스템 (난이도: ★★☆☆☆)</summary>
<div markdown="1">

- 클릭이나 탭, 업그레이드 시 파티클이 발생됩니다.
![Image](https://github.com/user-attachments/assets/8a42bd7c-a7f5-4fbd-94b6-62f0f7d99500)
</div>
</details>

<details>
<summary>사운드 이펙트 (난이도: ★★★☆☆)</summary>
<div markdown="1">
토글 안에 넣을 이미지나 글
</div>
</details>

<details>
<summary>아이템 및 업그레이드 시스템</summary>
<div markdown="1">
토글 안에 넣을 이미지나 글
</div>
</details>

<details>
<summary>저장 및 로드 시스템 (난이도: ★★★☆☆)</summary>
<div markdown="1">
토글 안에 넣을 이미지나 글
</div>
</details>

<details>
<summary>다이나믹 카메라 효과 (난이도: ★★★★☆)</summary>
<div markdown="1">
토글 안에 넣을 이미지나 글
</div>
</details>

<details>
<summary>랜덤 맵 생성 기능  (난이도: ★★★★☆)</summary>
<div markdown="1">
토글 안에 넣을 이미지나 글
</div>
</details>

<details>
<summary>BigInteger 기능 (난이도: ★★★★★) (도전)</summary>
<div markdown="1">
토글 안에 넣을 이미지나 글
</div>
</details>

