# TestProject
(추가사항) 특정 방어구의 세트옵션을 구현

(가능하면) 몬스터리스트 필드클래스를 만들어 분할 

(가능하면) 강화 기능 추가

(가능하면) 잡화상점, 장비상점 구현

(가능하면) 저장된 플레이어 데이터를 들고와 배틀페이즈 실행 (PvP)

(가능하면) For The King 형식의 선턴(행동주기 설정)추가

현재까지 구현된 내용
=================================================================================================
레벨에 따라 최대 Hp, Mp, 공격력을 증가시키고 필요 경험치를 증가시킵니다.

플레이어의 행동은 공격, 포션사용, 도망

무기는 3종류로 설정(검, 활, 스태프)

지역은 마을, 초원, 동굴, 협곡, 바다

플레이어가 사망할 시 가지고 있는 경험치의 10%를 잃습니다.

경험치 획득방식/전투 > 적의 exp만큼 플레이어 exp증가 > 플레이어exp가 Max exp 달성시 플레이어 Lv을 1올리고

초과한 만큼 플레이어 exp를 증가시킴

전투메소드 구현/적 공격 1회 = 플레이어 행동 1회, 플레이어 선턴, 공격1회당 데미지만큼 피해를 입히며

전투가 끝나면 마을로이동

랜덤함수를 사용하여 드랍테이블의 무기와 치명타(공격력의 2배)를 10%확률로 적용 (rand.Next(0, 10) == 10) 

UI Text는 총 6개

1. 기본UI 출력

2.지역이동

3. (지역이동 직후)적조우 및 적 UI 

4. 플레이어 행동(공격, 포션사용, 도망)/적 행동

5. 전투 승리/패배 (드랍테이블 확률에 따라 얻은 무기가 기존무기보다 좋으면 자동획득/교체)

6. 해당 내용을 종료할경우 나오는 텍스트 종료전까진 위의 내용을 반복

파일 입출력(플레이어 데이터세이브/로드, 무기 데이터)

인벤토리

플레이어 스킬구현

각각의 무기마다 다른 스킬을 사용

인벤토리가 꽉찼을 때 아이템 자동획득, 자동판매

인벤토리 데이터 확인, 장착 및 판매

시련의탑(던전형식 클리어시 보상획득)

몬스터 스킬구현

데이터시트 추가

스킬 데이터 추가

몬스터 스킬구현

몬스터 테이블추가

무기 데이터 추가

드랍테이블 추가

시련의 탑(던전형식 클리어시 보상획득)

스킬 데이터 추가

드랍테이블 추가

- 입출력 부분 한 곳에 묶어서 정리(받아오는 데이터 : 몬스터, 드랍, 무기정보, 스킬, (가능하면 몬스터, 무기 데이터 추가))

- Player, Enemy클래스 겹치는부분 Character클래스 생성 및 상속

- Ingame 메서드 내용 분할

- 각 클래스 접근제한, 프로퍼티

스킬 타입에 따라 다른 디메리트를 줌(스턴, 저주(공격력감소), 화상(도트데미지), 흡혈(타격시 체력회복))

시련의 탑 보상내용(5층 마다 보상으로 나갈 무기 데이터 추가)

장비창 구현 및 플레이어와 몬스터에게 방어력 처리
