using System.ComponentModel.Design;
using System.Numerics;
using System.Text.Json;
using System.Threading;
using static System.Console;
// 레벨에 따라 최대 Hp, Mp, 공격력을 증가시키고 필요 경험치를 증가시킵니다.
// 플레이어의 행동은 공격, 포션사용, 도망                (0, 1, 2)  
// 무기는 3종류로 설정(검, 활, 스태프)                   (0, 1, 2)
// 지역은 마을, 초원, 동굴, 협곡, 바다
// 플레이어가 사망할 시 가지고 있는 경험치의 10%를 잃습니다.
// 경험치 획득방식/전투 > 적의 exp만큼 플레이어 exp증가 > 플레이어exp가 Max exp 달성시 플레이어 Lv을 1올리고
// 초과한 만큼 플레이어 exp를 증가시킴
// 전투메소드 구현/적 공격 1회 = 플레이어 행동 1회, 플레이어 선턴, 공격1회당 데미지만큼 피해를 입히며
// 전투가 끝나면 마을로이동
// 랜덤함수를 사용하여 드랍테이블의 무기와 치명타(공격력의 2배)를 10%확률로 적용 (rand.Next(0, 10) == 10) 
// UI Text는 총 6개
// 1. 기본UI 플레이어의 체력({0}/{1}, p_Hp, p_MaxHp)({0}/{1}, p_Mp, p_MaxMp), Lv, Hp, Mp, Exp 4줄 출력 입력 : (지역이동, 마을(회복))
// 2. 지역이동 (초원, 동굴, 협곡, 바다선택)
// 3. (지역이동 직후)적조우 및 적 UI 
// 4. 플레이어 행동(공격, 포션사용, 도망)/적 행동
// 5. 전투 승리/패배 (드랍테이블 확률에 따라 얻은 무기가 기존무기보다 좋으면 자동획득/교체)
// 6. 해당 내용을 종료할경우 나오는 텍스트 종료전까진 위의 내용을 while(true)로 반복
// 아이템 자동획득, 자동판매
// 파일 입출력(플레이어 데이터세이브/로드, 무기 데이터)
// 인벤토리
// 각각의 무기마다 다른 스킬을 사용
// 현재까지 구현된 내용
// 플레이어 스킬구현
// =================================================================================================
// (추가사항) 몬스터 스킬구현
// (추가사항) 몬스터 테이블추가
// (추가사항) 무기 데이터 추가
// (추가사항) 드랍테이블 추가
// (추가사항) 무한의탑(던전형식 클리어시 보상획득
// (추가사항) 코드 최적화
// (추가사항) 턴(행동주기 설정)추가(가능하면)
namespace TestPj
{
    internal class Program
    {

        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("./EnemyList.csv");

            List<Enemy> enemyList = new List<Enemy>();
            bool e_ListLine = false;
            while (sr.Peek() >= 0)
            {
                string[] readData = sr.ReadLine().Split(',');
                if(e_ListLine == false)
                {
                    e_ListLine = true;
                    continue;
                }
                enemyList.Add(new Enemy(int.Parse(readData[0]), readData[1], int.Parse(readData[2]), int.Parse(readData[3]),
                                        int.Parse(readData[4]), readData[5]));
            }
            new Field().Ingame(enemyList);
        }
    }
}

