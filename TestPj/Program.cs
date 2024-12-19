using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
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
// 파일 입출력(플레이어 데이터세이브/로드, 무기 데이터)
// 인벤토리
// 플레이어 스킬구현
// 각각의 무기마다 다른 스킬을 사용
// 인벤토리가 꽉찼을 때 아이템 자동획득, 자동판매
// 인벤토리 데이터 확인, 장착 및 판매
// 시련의탑(던전형식 클리어시 보상획득)
// 몬스터 스킬구현
// 데이터시트 추가
// 몬스터 테이블추가
// 무기 데이터 추가
// 스킬 데이터 추가
// 드랍테이블 추가
// - 입출력 부분 한 곳에 묶어서 정리(받아오는 데이터 : 몬스터, 드랍, 무기정보, 스킬, (가능하면 몬스터, 무기 데이터 추가))
// - Player, Enemy클래스 겹치는부분 Character클래스 생성 및 상속
// - Ingame 메서드 내용 분할
// - 각 클래스 접근제한, 프로퍼티
// 스킬 타입에 따라 다른 디메리트를 줌(스턴, 저주(공격력감소), 화상(도트데미지), 흡혈(타격시 체력회복))
// 시련의 탑 보상내용(5층 마다 보상으로 나갈 무기 데이터 추가)
// 현재까지 구현된 내용
// =================================================================================================
//            플레이어와 몬스터에게 방어력 처리, 특정 방어구의 세트옵션을 구현
// (가능하면) 소비템 추가, 잡화상점 추가
// (가능하면) 강화 기능 추가
// (가능하면) 저장된 플레이어 데이터를 들고와 배틀페이즈 실행 (PvP)
// (가능하면) For The King 형식의 선턴(행동주기 설정)추가
// (추가사항) 몬스터리스트 필드클래스를 만들어 분할 
namespace TestCode
{
    internal class Program
    {
        // json 파일로 저장된 데이터시트를 불러오기위한 제너릭클래스
        class DataSheet<T>   
        {
            public List<T> DataIn(string dataSheet)
            {
                StreamReader DataTable = new StreamReader($"./{dataSheet}.json");           //.json 파일의 데이터를 읽습니다.
                string data = DataTable.ReadToEnd();                                        // string형식으로 변환하여 저장합니다.
                DataTable.Close();                                                          // 열려있는 .json파일을 닫습니다.
                List<T> dataList = new List<T>();                                           // string형식의 데이터를 List형식으로 역직렬화합니다.
                List<T> reData = new List<T>();                                             // 반환할 데이터에 데이터리스트를 저장합니다.
                dataList = JsonSerializer.Deserialize<List<T>>(data);
                for ( int i = 0; i < dataList.Count; i++)
                {
                    reData.Add(dataList[i]);
                }
                return reData;
            }
        }
         // 몬스터데이터를 타입에따라 분할하고 드랍테이블을 설정합니다.
        public static List<Enemy> Divide(List<Enemy> list, List<DropTable> drop, string Type)                   
        {
            List<Enemy> reData = new List<Enemy>();
            for (int i = 0; i < list.Count; i++)                // 몬스터데이터를 매개변수의 타입에 맞게 저장합니다.
            {
                if (list[i].Type == Type)
                {
                    reData.Add(new Enemy(list[i].Damage, list[i].Name, list[i].MaxHp, list[i].MaxMp, list[i].Exp, list[i].Type));                  
                }
            }
            for (int i = 0; i < reData.Count; i++)              // 저장된 몬스터데이터의 드랍테이블을 연결합니다.
            {
                for (int j = 0; j < drop.Count; j++)
                {
                    if (reData[i].name == drop[j].enemy_Name)
                    {
                        reData[i].droptable = drop[j];
                    }
                }
            }
            return reData;
        }
        // 플레이어의 로그인을 처리하는 메소드
        public static Player PlayerLogin(List<Player> playerData, List<Weapon> weaponList, List<Gear> gearList, Player player)
        {
            int inputnum;
            while (true)
            {
                Write("플레이어 아이디를 입력해주세요 : ");
                string input = ReadLine();
                for (int i = 0; i < playerData.Count; i++)      // 플레이어 데이터를 순회하며 입력값과 같은 아이디를 가진 세이브 데이터를 찾습니다.
                {
                    if (input == playerData[i].Name)                
                    {
                        WriteLine("저장된 정보를 로드합니다.");
                        player = new Player(playerData[i].Name, playerData[i].Exp, playerData[i].Lv,
                                            playerData[i].Posion, playerData[i].Credit, playerData[i].Weapon_Number, playerData[i].Gear_Number, playerData[i].ItemInventory, playerData[i].GearInventory);
                        if (player.weapon_Number != 0)          // 저장된 플레이어 데이터에 무기착용 여부를 확인합니다.
                        {
                            player.weapon = new Weapon(); 
                            player.weapon = weaponList[playerData[i].Weapon_Number - 1];
                            player.WearingWeapon(weaponList[playerData[i].Weapon_Number - 1]);
                        }
                        if (player.gear_Number.Length != 0)
                        {
                            for (i = 0; i < player.gears.Count; i++)
                            {
                                if (player.gear_Number[i] == 0)
                                {
                                    player.gears[i] = new Gear();
                                }
                                else
                                {
                                    player.gears[i] = (new Gear(gearList[player.gear_Number[i] - 1].g_Name, gearList[player.gear_Number[i] - 1].defense, gearList[player.gear_Number[i] - 1].sell_Price,
                                                              gearList[player.gear_Number[i] - 1].type, gearList[player.gear_Number[i] - 1].grade, gearList[player.gear_Number[i] - 1].gItem_Number));
                                    player.defense += gearList[player.gear_Number[i] - 1].defense;
                                }
                            }
                            player.IsWearingGear();
                        }
                        return player;
                    }
                }
                if (player.name == null)                        // 플레이어 신규 생성
                {
                    Write("해당 이름으로 아이디를 만드시겠습니까? 1. 예 2. 아니오 : ");
                    if (int.TryParse(ReadLine(), out inputnum))
                    {
                        if (inputnum == 1)
                        {
                            player = new Player(input);
                            player.weapon = new Weapon();
                            for (int i = 0; i < 4; i++)
                            {
                                player.gears[i] = new Gear();
                            }
                            player.gears[0].type = "Head";
                            player.gears[1].type = "Armor";
                            player.gears[2].type = "Glove";
                            player.gears[3].type = "Shoes";
                            return player;
                        }
                        else if (inputnum == 2)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        Write("잘못입력했습니다. 처음으로 돌아갑니다.");
                        continue;
                    }
                }
                else if (player.name != null)
                {
                    break;
                }

            }
            return null;
        }   
        // 데이터 저장을 위한 메소드
        public static void PlayerLogout(List<Player> playerData, Player player, string saveFile)
        {
            StreamWriter sw = new StreamWriter($"./{saveFile}.json", false);        
            bool playerSave = false;
            for (int i = 0; i < playerData.Count; i++)
            {
                if (playerData[i].Name == player.name)                              // 플레이어의 기존데이터가 있을경우 해당 데이터
                {
                    playerData[i].Name = player.name;
                    playerData[i].Exp = player.exp;
                    playerData[i].Lv = player.lv;
                    playerData[i].Posion = player.posion;
                    playerData[i].Credit = player.credit;
                    playerData[i].Weapon_Number = player.weapon_Number;
                    playerData[i].Gear_Number[0] = player.gear_Number[0];
                    playerData[i].Gear_Number[1] = player.gear_Number[1];
                    playerData[i].Gear_Number[2] = player.gear_Number[2];
                    playerData[i].Gear_Number[3] = player.gear_Number[3];
                    playerData[i].ItemInventory = player.inventory.GetInventory();
                    playerData[i].GearInventory = player.inventory.GetGearInventory();
                    playerSave = true;
                }
            }
            if (playerSave == false)                                                // 플레이어의 데이터가 새로 저장되어야 할 경우
            {
                playerData.Add(new Player());
                playerData[playerData.Count - 1].Name = player.name;
                playerData[playerData.Count - 1].Exp = player.exp;
                playerData[playerData.Count - 1].Lv = player.lv;
                playerData[playerData.Count - 1].Posion = player.posion;
                playerData[playerData.Count - 1].Credit = player.credit;
                playerData[playerData.Count - 1].Weapon_Number = player.weapon_Number;
                playerData[playerData.Count - 1].Gear_Number = player.gear_Number;
                playerData[playerData.Count - 1].ItemInventory = player.inventory.GetInventory();
                playerData[playerData.Count - 1].GearInventory = player.inventory.GetGearInventory();
                playerSave = true;
            }
            string save = JsonSerializer.Serialize(playerData);
            sw.Write(save);                                                         // 파일 쓰기
            sw.Close();                                                             // 파일 닫기
            return;
        }            
         // 배틀페이즈 처리
        public static void BattlePhase(List<Weapon> weaponList, List<Gear> gearList, Enemy enemy, Player player)
        {
            Random critical = new Random();         // 치명타적용을 위한 변수선언
            int value;
            float skillDamage;
            enemy.ShowInfo();
            while (true)
            {
                if (player.playerTurn == true)
                {
                    player.ShowInfo();
                    WriteLine("------------------------------------------------------------------------");
                    Write("1.공격 2.회복 3.스킬사용 4. 도망 : ");
                    if (int.TryParse(ReadLine(), out value))
                    {
                        WriteLine("\n------------------------------------------------------------------------");
                        if (value == 1)                     // 공격
                        {
                            if (10 > critical.Next(0, 100)) // 10%확률의 치명타
                            {
                                WriteLine("치명타 발생!");
                                enemy.hp = enemy.hp - (player.damage * 2);
                                WriteLine("{0}의 데미지를 입혔습니다.", (player.damage * 2));
                            }
                            else                            // 일반공격                   
                            {
                                enemy.hp = enemy.hp - player.damage;
                                WriteLine("{0}의 데미지를 입혔습니다.", (player.damage));
                            }
                        }
                        else if (value == 2)                // 포션사용
                        {
                            if (player.posion > 0)          // 현재 플레이어가 보유하고 있는 포션의 개수를 체크합니다.
                            {
                                if (player.hp + (player.maxHp / 2) <= player.hp)
                                {
                                    WriteLine("{0}의 체력을 회복시켰습니다.", (player.maxHp / 2));
                                    player.hp = player.hp + (player.maxHp / 2);
                                    player.posion = player.posion - 1;
                                    WriteLine("사용 가능한 포션은 {0}개 입니다", player.posion);
                                }
                                else                        // 회복되는 체력이 플레이어의 최대체력을 넘어갈 때
                                {
                                    WriteLine("{0}의 체력을 회복시켰습니다.", (player.maxHp - player.hp));
                                    player.hp = player.maxHp;
                                    player.posion = player.posion - 1;
                                    WriteLine("사용 가능한 포션은 {0}개 입니다", player.posion);
                                }
                            }
                            else
                            {
                                WriteLine("사용 할 수 있는 포션이 없습니다.");
                                continue;
                            }
                        }
                        else if (value == 3)                // 스킬사용
                        {
                            if (player.weapon.Item_Name != "미착용")      // 플레이어가 무기를 들고있는지 체크합니다.
                            {
                                if (player.mp >= 20)        // 플레이어가 최소한의 스킬을 사용할 수 있는지 체크합니다.
                                {
                                    skillDamage = player.SkillActive();
                                    if (skillDamage > 0)
                                    {
                                        if (10 > critical.Next(0, 100))     // 치명타 로직
                                        {
                                            WriteLine("치명타 발생!");
                                            enemy.hp = enemy.hp - (int)(player.damage * 2 * skillDamage);
                                            WriteLine("{0}의 데미지를 입혔습니다.", (int)(player.damage * 2 * skillDamage));
                                        }
                                        else                                // 일반공격(스킬)
                                        {
                                            enemy.hp = enemy.hp - (int)(player.damage * skillDamage);
                                            WriteLine("{0}의 데미지를 입혔습니다.", (int)(player.damage * skillDamage));
                                        }
                                    }
                                    else
                                    {
                                        WriteLine("스킬사용이 취소되었습니다.");
                                        continue;
                                    }
                                }
                                else
                                {
                                    WriteLine("마나가 부족합니다.");
                                    continue;
                                }

                            }
                            else
                            {
                                WriteLine("무기를 착용하지 않았습니다.");
                            }
                        }
                        else if (value == 4)                                // 도망
                        {
                            enemy.hp = enemy.maxHp;                         // 현재 적개체가 소모한 체력을 회복합니다
                            enemy.mp = 0;                                   // 다음 전투 전 mp를 0으로 고정합니다.
                            WriteLine("전투에서 도망쳤습니다.");
                            WriteLine("------------------------------------------------------------------------");
                            if (enemy.type == "보스")                       // 시련의 탑은 죽거나 클리어하지않으면 나갈 수 없기때문에 사망처리합니다.
                            {
                                player.hp = 0;
                            }
                            return;
                        }
                        else
                        {
                            WriteLine("1 ~ 3 이내의 숫자를 입력해 주세요");
                            continue;
                        }
                        WriteLine("------------------------------------------------------------------------");
                        enemy.ShowInfo();
                    }
                    else
                    {
                        WriteLine("숫자를 입력해주세요");
                        continue;
                    }
                    if (player.deBurfCount > 0)             // 플레이어가 스킬을 사용했을 때 디버프가 적용되는 시간을 체크합니다.
                    {
                        if (player.deBurfType == "Burn")
                        {
                            enemy.Burn();
                            player.deBurfCount -= 1;
                        }
                        if (player.deBurfType == "Curse")
                        {
                            if (player.deBurfCount == 3)
                            {
                                enemy.Curse();
                            }
                            if (player.deBurfCount == 1)
                            {
                                if (enemy.dummyDamage > 0)
                                {
                                    enemy.damage = enemy.dummyDamage;
                                }
                            }
                            player.deBurfCount -= 1;
                        }
                    }
                    if (player.mp <= player.maxMp)          // 플레이어의 턴이 끝날때 10의 마나를 회복합니다.
                    {
                        if (player.maxMp > player.mp + 10)
                        {
                            player.mp += 10;
                        }
                        else
                        {
                            player.mp = player.maxMp;
                        }
                    }
                    if (player.hitStun == true)             // 플레이어가 스킬을 사용하여 몬스터에게 기절효과가 들어갔는지 체크합니다.
                    {
                        WriteLine("------------------------------------------------------------------------");
                        WriteLine("몬스터가 스턴상태입니다 해당 턴을 스킵합니다.");
                        WriteLine("------------------------------------------------------------------------");
                        player.hitStun = false;
                        player.playerTurn = true;
                    }
                    else
                    {
                        player.playerTurn = false;
                    }
                }
                else if (player.playerTurn == false)        // 플레이어의 턴이 아닐때 몬스터는 자동으로 공격합니다.
                {
                    WriteLine("------------------------------------------------------------------------");
                    if (enemy.mp >= 40)                     // 몬스터의 마나가 40이 될 때 자동으로 스킬을 사용합니다.
                    {
                        skillDamage = enemy.SkillActive();  
                        if (10 > critical.Next(0, 100))     // 몬스터의 치명타 로직
                        {
                            WriteLine("적의 치명타 발생!!!");
                            WriteLine("{0}의 데미지를 입었습니다.", (int)((enemy.damage * 2) * skillDamage));
                            player.hp = player.hp - (int)((enemy.damage * 2) * skillDamage);
                            enemy.mp = 0;
                        }
                        else                                
                        {
                            WriteLine("{0}의 데미지를 입었습니다.", (int)(enemy.damage * skillDamage));
                            player.hp = player.hp - (int)(enemy.damage * skillDamage);
                            enemy.mp = 0;
                        }
                    }
                    else
                    {
                        if (10 > critical.Next(0, 100))     // 몬스터의 치명타 로직
                        {
                            WriteLine("적의 치명타 발생!!!");
                            WriteLine("{0}의 데미지를 입었습니다.", (enemy.damage * 2));
                            player.hp = player.hp - (enemy.damage * 2);
                        }
                        else                                // 몬스터의 일반공격
                        {
                            WriteLine("{0}의 데미지를 입었습니다.", (enemy.damage));
                            player.hp = player.hp - enemy.damage;
                        }
                    }
                    if (enemy.mp <= enemy.maxMp)            // 공격을 한 후 10의 마나를 회복합니다.
                    {
                        enemy.mp += 10;
                    }
                    if (enemy.deBurfCount > 0)              // 몬스터가 스킬을 사용한 이후 적용되야하는 디버프가있는지 확인합니다.
                    {
                        if (enemy.deBurfType == "Burn")
                        {
                            player.Burn();
                            enemy.deBurfCount -= 1;
                        }
                        if (enemy.deBurfType == "Curse")
                        {
                            if (enemy.deBurfCount == 3)
                            {
                                player.Curse();
                            }
                            if(enemy.deBurfCount == 1)
                            {
                                if (player.dummyDamage > 0)
                                {
                                    player.damage = player.dummyDamage;
                                }
                            }
                            enemy.deBurfCount -= 1;                         
                        }
                    }
                    if (enemy.hitStun == true)
                    {
                        WriteLine("------------------------------------------------------------------------");
                        WriteLine("플레이어가 스턴상태입니다 해당 턴을 스킵합니다.");
                        enemy.hitStun = false;
                        player.playerTurn = false;
                    }
                    else
                    {
                        player.playerTurn = true;
                        WriteLine("------------------------------------------------------------------------");
                    }

                }
                if (enemy.hp <= 0 || player.IsAlive() == false)
                {
                    player.ShowInfo();
                    WriteLine("\n전투종료");
                    player.deBurfCount = 0;
                    enemy.deBurfCount = 0;
                    if (player.dummyDamage > 0)
                    {
                        player.damage = player.dummyDamage;
                    }
                    if (enemy.dummyDamage > 0)
                    {
                        enemy.damage = enemy.dummyDamage;
                    }
                    if (enemy.hp <= 0)
                    {
                        WriteLine("전투에서 승리했습니다.\n");
                        if (enemy.type != "보스")
                        {

                            player.Reward(enemy, player, weaponList, gearList);
                        }
                        player.mp = 0;
                        enemy.hp = enemy.maxHp;
                        enemy.mp = 0;
                        player.playerTurn = true;
                        return;
                    }
                    else
                    {
                        WriteLine("전투에서 패배하였습니다.\n");
                        enemy.hp = enemy.maxHp;
                        enemy.mp = 0;
                        if (enemy.type != "보스")
                        {
                            player.Dead();
                        }
                        player.playerTurn = true;
                        return;
                    }
                }
            }
        }                 
        // 지역이동을 처리하는 메소드
        public static void FieldMove(List<Enemy> praList, List<Enemy> seaList, List<Enemy> caveList
                                   , List<Enemy> deep_CaveList, List<Enemy> bossList
                                   , Player player, List<Weapon> weaponList, List<Gear> gearList)
        {
            int lotation = 0;
            int killCount = 0;
            int bossChoice = 0;
            while (true)
            {
                player.ShowInfo();
                Write("\n전투를 진행할 지역을 선택해주세요 1. 초원 2. 바다 3. 동굴 4. 시련의 탑 5. 이전 ");
                if (int.TryParse(ReadLine(), out lotation))
                {
                    int respon = new Random().Next(1, 100);   // 지역몹을 랜덤하게 리스폰하기위한 변수
                    if (lotation == 1)                                                                  // 전투지역
                    {
                        BattlePhase(weaponList, gearList, praList[respon % praList.Count], player);
                    }
                    else if (lotation == 2)                                                             // 전투지역
                    {
                        BattlePhase(weaponList, gearList, seaList[respon % seaList.Count], player);
                    }
                    else if (lotation == 3)                                                             // 전투지역
                    {
                        WriteLine("1. 초입 2. 심층");
                        if (int.TryParse(ReadLine(), out lotation))
                        {
                            if (lotation == 1)
                            {
                                BattlePhase(weaponList, gearList, caveList[respon % caveList.Count], player);
                            }
                            else if (lotation == 2)
                            {
                                BattlePhase(weaponList, gearList, deep_CaveList[respon % deep_CaveList.Count], player);
                            }
                            else
                            {
                                WriteLine("다시 입력해주세요. ");
                                continue;
                            }
                        }
                        else
                        {
                            WriteLine("다시 입력해주세요. ");
                            continue;
                        }
                    }else if(lotation == 4)
                    {
                        int reWardCredit = 0;
                        player.InTower = true;
                        Random rand = new Random();
                        Enemy Dummy;
                        while (true)
                        {
                            if (killCount < 15)
                            {
                                int swap = rand.Next(0, 3)%3;
                                Dummy = new Enemy(bossList[swap].damage, bossList[swap].name, bossList[swap].maxHp, 
                                                  bossList[swap].maxMp, bossList[swap].exp, bossList[swap].type);
                                Dummy.skill = bossList[swap].skill;
                                for (int i = 0; i <= killCount; i++)
                                {
                                    Dummy.maxHp += (int)Dummy.maxHp / 20;
                                    Dummy.hp = Dummy.maxHp;
                                    Dummy.damage += (int)Dummy.damage / 20;
                                }
                                BattlePhase(weaponList, gearList, Dummy, player);
                                if (player.IsAlive() == false)
                                {
                                    player.Dead();
                                    player.InTower = false;
                                    break;
                                }
                            }
                            else
                            {
                                int index;
                                WriteLine("도전자격이 갖춰졌습니다. 도전하고자 하는 보스를 선택해주세요");
                                WriteLine("1. 데스나이트, 2. 실피드, 3. 데몬로드");
                                if(int.TryParse(ReadLine(), out index))
                                {
                                    if (index == 1)
                                    {
                                        bossChoice = 3;
                                        Dummy = bossList[3];
                                        BattlePhase(weaponList, gearList, Dummy, player);
                                    }
                                    else if (index == 2)
                                    {
                                        bossChoice = 4;
                                        Dummy = bossList[4];
                                        BattlePhase(weaponList, gearList, Dummy, player);
                                    }
                                    else if (index == 3)
                                    {
                                        bossChoice = 5;
                                        Dummy = bossList[5];
                                        BattlePhase(weaponList, gearList, Dummy, player);
                                    }
                                    else
                                    {
                                        WriteLine("잘못입력했습니다 다시입력해주세요");
                                    }
                                    if (player.IsAlive() == false)
                                    {
                                        player.Dead();
                                        player.InTower = false;
                                        killCount--;
                                        break;
                                    }
                                    else
                                    {
                                        player.InTower = false;
                                        break;
                                    }    
                                }
                                else
                                {
                                    WriteLine("잘못입력했습니다. 다시입력해주세요");
                                    continue;
                                }
                            }
                            reWardCredit += 40 * (killCount + 1);
                            killCount++;
                        }
                        if (killCount > 0)
                        {
                            WriteLine($"총 {killCount} 층 올라가셨습니다.");
                            WriteLine("보상내용");
                            if (killCount > 5 && killCount < 10)
                            {
                                // 도전자무기 Ⅰ
                                player.Reward(bossList[0], player, weaponList, gearList);
                            }
                            else if (killCount >= 10 && killCount < 15)
                            {
                                // 도전자무기 Ⅱ
                                player.Reward(bossList[1], player, weaponList, gearList);
                            }
                            else if( killCount == 15)
                            {
                                WriteLine("최종보상을 획득하셨습니다.");
                                player.Reward(bossList[bossChoice], player, weaponList, gearList);
                            }
                            WriteLine($"{reWardCredit} 크레딧을 획득하셨습니다.");
                            player.credit += reWardCredit;
                        }
                        else
                        {
                            WriteLine("탑을 오르는데 실패했습니다.");
                            killCount = 0;
                            return;
                        }
                        killCount = 0;
                    }
                    else if(lotation == 5)
                    {
                        return;
                    }
                }
            }
        }               
        // 마을에서의 행동을 처리하는 메소드
        public static void Town(Player player)
        {
            int state;
            player.hp = player.maxHp;
            WriteLine("------------------------------------------------------------------------");
            WriteLine("플레이어 체력이 회복되었습니다.");
            WriteLine("------------------------------------------------------------------------");
            while (true)
            {
                WriteLine("하고자 하는 행동을 선택해주세요 1. 포션구입(포션의 가격은 {0}원 입니다.), 2. 이전 ", (10 * (player.lv + 1)));
                if (int.TryParse(ReadLine(), out state))
                {

                    if (state == 1 && player.posion < (10 + player.lv))
                    {
                        // 포션구입
                        if (20 * (player.lv + 1) < player.credit)
                        {
                            WriteLine("포션을 구입했습니다.");
                            player.posion = player.posion + 1;
                            player.credit = player.credit - (10 * (player.lv + 1));
                            WriteLine("포션은 최대 {0}개 가질 수 있습니다.", (10 + player.lv) - 1);
                            player.ShowInfo();
                        }
                        else
                        {
                            WriteLine("돈이 부족합니다..");
                        }
                    }
                    else if (state == 1 && player.posion >= (10 + player.lv))
                    {
                        WriteLine("포션의 개수가 너무 많습니다.");
                    }
                    else if (state == 2)
                    {
                        return;
                    }
                    else
                    {
                        WriteLine("잘못입력했습니다.");
                    }
                }
                else
                {
                    WriteLine("잘못입력했습니다. 다시 입력해주세요");
                    break;
                }
            }
        }
        // 인게임 메소드
        public static void Ingame()
        {
            DataSheet<DropTable> dropTable = new DataSheet<DropTable>();
            DataSheet<Player> p_DataSheet = new DataSheet<Player>();
            DataSheet<Enemy> e_DataSheet = new DataSheet<Enemy>();
            DataSheet<Weapon> w_DataSheet = new DataSheet<Weapon>();
            DataSheet<Skill> s_DataSheet = new DataSheet<Skill>();
            DataSheet<GearData> g_DataSheet = new DataSheet<GearData>();
            List<Skill> skillList = s_DataSheet.DataIn("SkillDataTest");
            List<DropTable> drop = dropTable.DataIn("DropTest");
            List<Player> playerList = p_DataSheet.DataIn("PlayerSave");
            List<Enemy> enemies = e_DataSheet.DataIn("MonsterTest");
            List<Weapon> weapons = w_DataSheet.DataIn("WeaponTypeTest");
            List<GearData> gearData = g_DataSheet.DataIn("GearTestData");
            List<Gear> gearList = new List<Gear>();
            List<Enemy> praList = new List<Enemy>();
            List<Enemy> seaList = new List<Enemy>();
            List<Enemy> caveList = new List<Enemy>();
            List<Enemy> deep_CaveList = new List<Enemy>();
            List<Enemy> bossList = new List<Enemy>();
            List<Enemy> forestList = new List<Enemy>();
            for(int i = 0; i < gearData.Count; i++)
            {
                gearList.Add(new Gear(gearData[i].G_Name, gearData[i].Defense, gearData[i].Sell_Price, gearData[i].Type, gearData[i].Grade, gearData[i].GItem_Number));
            }

            #region 플레이어 생성, 로드
            Player player = new Player();
            player = PlayerLogin(playerList, weapons, gearList, player);
            player.inventory = new Inventory();
            if (player.itemInventory != null)
            {
                for (int i = 0; i < player.itemInventory.Length; i++)
                {
                    player.inventory.weapons.Add(weapons[player.itemInventory[i] - 1]);
                }
                for(int i = 0; i < player.gearInventory.Length; i++)
                {
                    player.inventory.gears.Add(gearList[player.gearInventory[i] - 1]);
                }
            }
            #endregion

            #region 몬스터데이터 필드별로 분할
            praList = Divide(enemies, drop, "초원");
            seaList = Divide(enemies, drop, "바다");
            caveList = Divide(enemies, drop, "동굴(초입)");
            deep_CaveList = Divide(enemies, drop, "동굴(심층)");
            bossList = Divide(enemies, drop, "보스");
            forestList = Divide(enemies, drop, "죽음의숲");
            #endregion

            #region 몬스터, 플레이어 스킬할당
            player.skill = new List<Skill>();
            for (int i = 0; i < skillList.Count; i++)
            {
                if (skillList[i].Char_Name == "Player")
                {
                    player.skill.Add(skillList[i]);
                }
            }
            for (int i = 0; i < bossList.Count; i++)
            {
                bossList[i].skill = new List<Skill>();
                for (int j = 0; j < skillList.Count; j++)
                {

                    if (bossList[i].name == skillList[j].Char_Name)
                    {
                        bossList[i].skill.Add(skillList[j]);
                    }
                }
            }
            #endregion
            int lotation = 0;
            while (true)
            {
                player.ShowInfo();
                WriteLine("하고자 하는 행동을 선택해주세요 1. 인벤토리, 2. 장비창, 3. 지역이동, 4. 마을, 5. 세이브/종료");
                if (int.TryParse(ReadLine(), out lotation))
                {
                    if (lotation == 1)                                                            // 인벤토리
                    {
                        int index;
                        WriteLine("1. 무기창, 2. 방어구");
                        index = int.Parse(ReadLine());
                        if (index == 1)
                        {
                            if (player.inventory != null)
                            {
                                player.inventory.InventoryWeaponInfo(player);
                            }
                            else
                            {
                                Console.WriteLine("아이템이 없습니다.");
                            }
                        }
                        if (index == 2)
                        {
                            if (player.inventory != null)
                            {
                                player.inventory.InventoryGearInfo(player);
                            }
                            else
                            {
                                Console.WriteLine("아이템이 없습니다.");
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if(lotation == 2)
                    {
                        Console.WriteLine($"------------------------------------------------------------------------\n      " +
                            $"착용무기      : {player.weapon.Item_Name}\n------------------------------------------------------------------------\n\t" +
                            $"모자        : {player.gears[0].g_Name}\n------------------------------------------------------------------------\n\t" +
                            $"갑옷        : {player.gears[1].g_Name}\n------------------------------------------------------------------------\n\t" +
                            $"장갑        : {player.gears[2].g_Name}\n------------------------------------------------------------------------\n\t" +
                            $"신발        : {player.gears[3].g_Name}\n------------------------------------------------------------------------\n 추가행동(미구현)");
                    }
                    else if (lotation == 3)
                    {
                        FieldMove(praList, seaList, caveList, deep_CaveList, bossList
                                , player, weapons, gearList);
                    }
                    else if (lotation == 4)
                    {
                        Town(player);
                    }
                    else if (lotation == 5)
                    {
                        PlayerLogout(playerList, player, "PlayerSave");
                        return;
                    }
                }
            }
        }                                                                         

        static void Main(string[] args)
        {
            Ingame();
        }
    }
}
