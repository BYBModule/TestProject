﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Console;
class Field : Enemy
{
    private int respon;              // 해당 필드에서 리스폰될 적의 종류
    private bool turn = true;        // 플레이어와 적의 턴
    public Field()
    {
        this.f_Name = "";
    }
    public Field(string f_Name)
    {
        this.f_Name = f_Name;
    }
    public void BattlePhase(Enemy enemy, Player player, List<Weapon> weaponList)
    {
        Random critical = new Random();
        int value;
        float skillDamage;
        WriteLine("------------------------------------------------------------------------");
        enemy.showInfo();
        WriteLine("------------------------------------------------------------------------");
        while (true)
        {

            if (turn == true)
            {
                player.ShowInfo();
                WriteLine("------------------------------------------------------------------------");
                Write("1.공격 2.회복 3.스킬사용 4. 도망 : ");
                if (int.TryParse(ReadLine(), out value))
                {
                    WriteLine("\n------------------------------------------------------------------------");
                    if (value == 1)
                    {
                        if (10 > critical.Next(0, 100))
                        {
                            WriteLine("치명타 발생!");
                            enemy.e_Hp = enemy.e_Hp - (player.attack_damage * 2);
                            WriteLine("{0}의 데미지를 입혔습니다.", (player.attack_damage * 2));
                        }
                        else
                        {
                            enemy.e_Hp = enemy.e_Hp - player.attack_damage;
                            WriteLine("{0}의 데미지를 입혔습니다.", (player.attack_damage));
                        }
                    }
                    else if (value == 2)
                    {
                        if (player.posion > 0)
                        {
                            if (player.p_Hp + (player.p_MaxHp / 2) <= player.p_MaxHp)
                            {
                                WriteLine("{0}의 체력을 회복시켰습니다.", (player.p_MaxHp / 2));
                                player.p_Hp = player.p_Hp + (player.p_MaxHp / 2);
                                player.posion = player.posion - 1;
                                WriteLine("사용 가능한 포션은 {0}개 입니다", player.posion);
                            }
                            else
                            {
                                WriteLine("{0}의 체력을 회복시켰습니다.", (player.p_MaxHp - player.p_Hp));
                                player.p_Hp = player.p_MaxHp;
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
                    else if (value == 3)
                    {
                        if (player.p_Mp > 20)
                        {
                            skillDamage = player.SkillActive(player.Weapon);
                            if (skillDamage > 0)
                            {
                                if (10 > critical.Next(0, 100))
                                {
                                    WriteLine("치명타 발생!");
                                    enemy.e_Hp = enemy.e_Hp - (int)(player.attack_damage * 2 * skillDamage);
                                    WriteLine("{0}의 데미지를 입혔습니다.", (int)(player.attack_damage * 2 * skillDamage));
                                }
                                else
                                {
                                    enemy.e_Hp = enemy.e_Hp - player.attack_damage;
                                    WriteLine("{0}의 데미지를 입혔습니다.",  (int)(player.attack_damage * skillDamage));
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
                    else if (value == 4)
                    {
                        enemy.e_Hp = enemy.e_MaxHp;
                        enemy.e_Mp = 0;
                        WriteLine("전투에서 도망쳤습니다.");
                        WriteLine("------------------------------------------------------------------------");
                        if(enemy.f_Name == "보스")
                        {
                            player.p_Hp = 0;
                        }
                        return;
                    }
                    else
                    {
                        WriteLine("1 ~ 3 이내의 숫자를 입력해 주세요");
                        continue;
                    }
                    WriteLine("------------------------------------------------------------------------");
                    enemy.showInfo();
                }
                else
                {
                    WriteLine("숫자를 입력해주세요");
                    continue;
                }
                if (player.p_Mp <= player.p_MaxMp)
                {
                    player.p_Mp += 10;
                }
                turn = false;
            }
            else if (turn == false)
            {
                WriteLine("------------------------------------------------------------------------");
                if (enemy.e_Mp >= 40)
                {
                    skillDamage = enemy.SkillActive(enemy);
                    if (10 > critical.Next(0, 100))
                    {
                        WriteLine("적의 치명타 발생!!!");
                        WriteLine("{0}의 데미지를 입었습니다.", (int)((enemy.damage * 2) * skillDamage));
                        player.p_Hp = player.p_Hp - (int)((enemy.damage * 2) * skillDamage);
                        enemy.e_Mp = 0;
                    }
                    else
                    {
                        WriteLine("{0}의 데미지를 입었습니다.", (int)(enemy.damage * skillDamage));
                        player.p_Hp = player.p_Hp - (int)(enemy.damage * skillDamage);
                        enemy.e_Mp = 0;
                    }
                }
                else
                {
                    if (10 > critical.Next(0, 100))
                    {
                        WriteLine("적의 치명타 발생!!!");
                        WriteLine("{0}의 데미지를 입었습니다.", (enemy.damage * 2));
                        player.p_Hp = player.p_Hp - (enemy.damage * 2);
                    }
                    else
                    {
                        WriteLine("{0}의 데미지를 입었습니다.", (enemy.damage));
                        player.p_Hp = player.p_Hp - enemy.damage;
                    }
                }
                if (enemy.e_Mp <= enemy.e_MaxMp)
                {
                    enemy.e_Mp += 10;
                }
                turn = true;
                WriteLine("------------------------------------------------------------------------");
            }
            if (enemy.e_Hp <= 0 || player.is_Alive() == false)
            {
                WriteLine("------------------------------------------------------------------------");
                player.ShowInfo();
                WriteLine("------------------------------------------------------------------------");
                WriteLine("\n전투종료");
                if (enemy.e_Hp <= 0)
                {
                    WriteLine("전투에서 승리했습니다.\n");
                    WriteLine("------------------------------------------------------------------------");
                    if (enemy.f_Name != "보스")
                    {
                        player.Reward(enemy, player, weaponList);
                    }
                    player.p_Mp = 0;
                    enemy.e_Hp = enemy.e_MaxHp;
                    enemy.e_Mp = 0;
                    turn = true;
                    WriteLine("------------------------------------------------------------------------");
                    return;
                }
                else
                {
                    WriteLine("전투에서 패배하였습니다.\n");
                    WriteLine("------------------------------------------------------------------------");
                    enemy.e_Hp = enemy.e_MaxHp;
                    enemy.e_Mp = 0;
                    if (enemy.f_Name != "보스")
                    {
                        player.Dead();
                    }
                    turn = true;
                    WriteLine("------------------------------------------------------------------------");
                    return;
                }
            }

        }

    }// BattlePhase
    public void Town(Player player, List<Enemy> enemy, List<Weapon> weaponList)
    {
        int state;
        int killCount = 0;
        bool dungeonA = true;
        player.p_Hp = player.p_MaxHp;
        player.p_Mp = player.p_MaxMp;
        WriteLine("------------------------------------------------------------------------");
        WriteLine("플레이어 체력이 회복되었습니다.");
        WriteLine("------------------------------------------------------------------------");
        while (true)
        {
            WriteLine("하고자 하는 행동을 선택해주세요 1. 지역이동, 2. 포션구입(포션의 가격은 {0}원 입니다.) 3.시련의 탑(입장)", (20 * (player.Lv + 1)));
            if (int.TryParse(ReadLine(), out state))
            {

                if (state == 1)
                {
                    return;
                }
                else if (state == 2 && player.posion < (10 + player.Lv))
                {
                    // 포션구입
                    if (20 * (player.Lv + 1) < player.credit)
                    {
                        WriteLine("포션을 구입했습니다.");
                        player.posion = player.posion + 1;
                        player.credit = player.credit - (20 * (player.Lv + 1));
                        WriteLine("포션은 최대 {0}개 가질 수 있습니다.", (10 + player.Lv) - 1);
                        player.ShowInfo();
                    }
                    else
                    {
                        WriteLine("돈이 부족합니다..");
                    }
                }
                else if (state == 2 && player.posion >= (10 + player.Lv))
                {
                    WriteLine("포션의 개수가 너무 많습니다.");
                }
                else if (state == 3)
                {
                    while (true)
                    {
                        Enemy Dummy = enemy[new Random().Next(0,enemy.Count)];
                        Dummy.e_MaxHp += (int)Dummy.e_MaxHp / 10;
                        Dummy.e_Hp = Dummy.e_MaxHp;
                        Dummy.damage += (int)Dummy.damage / 10;
                        BattlePhase(Dummy, player, weaponList);
                        if (player.is_Alive() == false)
                        {
                            player.Dead();
                            break;
                        }
                        killCount++;
                    }
                    if (killCount > 0)
                    {
                        WriteLine($"총 {killCount} 층 올라가셨습니다.");
                        Write("보상내용");// 미구현
                        WriteLine();
                    }
                    else
                    {
                        WriteLine("탑을 오르는데 실패했습니다.");
                        return;
                    }
                    killCount = 0;
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

    }// 마을
    public void Ingame()   
    {

        StreamReader DropTable = new StreamReader("./DropTable.json");
        StreamReader iD = new StreamReader("./WeaponType.json");
        StreamReader pd = new StreamReader("./PlayerSave.json"); // 플레이어 로드
        StreamReader mD = new StreamReader("./Monster.Json");
        List<Enemy> e_List = new List<Enemy>();
        List<WeaponData> weaponData = new List<WeaponData>();
        List<Weapon> weaponList = new List<Weapon>();
        string readItemData = iD.ReadToEnd();
        weaponData = JsonSerializer.Deserialize<List<WeaponData>>(readItemData);

        for (int i = 0; i < weaponData.Count; i++)
        {
            weaponList.Add(new Weapon(weaponData[i].Damage, weaponData[i].Weapon_Type, weaponData[i].Item_Name, 
                                      weaponData[i].Sell_Price, weaponData[i].Item_Number));
        }
        List<DropTable> drop = new List<DropTable>();
        readItemData = DropTable.ReadToEnd();
        drop = JsonSerializer.Deserialize<List<DropTable>>(readItemData);
        string readMonsterData = mD.ReadToEnd();
        List<MonsterData> monsterData = new List<MonsterData>();
        monsterData = JsonSerializer.Deserialize<List<MonsterData>>(readMonsterData);

        for (int i = 0; i < monsterData.Count; i++)
        {
            e_List.Add(new Enemy(monsterData[i].Damage, monsterData[i].E_Name, monsterData[i].E_MaxHp, monsterData[i].E_MaxMp, monsterData[i].E_Exp, 
                                 monsterData[i].F_Name));
        }

        List<Enemy> praList = new List<Enemy>();
        List<Enemy> seaList = new List<Enemy>();
        List<Enemy> caveList = new List<Enemy>();
        List<Enemy> deep_CaveList = new List<Enemy>();
        List<Enemy> bossList = new List<Enemy>();
        for (int i = 0; i < e_List.Count; i++)
        {
            if (e_List[i].f_Name == "초원")
            {
                praList.Add(e_List[i]);
            }
            else if (e_List[i].f_Name == "바다")
            {
                seaList.Add(e_List[i]);
            }
            else if (e_List[i].f_Name == "동굴(초입)")
            {
                caveList.Add(e_List[i]);
            }            
            else if (e_List[i].f_Name == "동굴(심층)")
            {
                deep_CaveList.Add(e_List[i]);
            }
            else if (e_List[i].f_Name == "보스")
            {
                bossList.Add(e_List[i]);
            }
            else
            {
                WriteLine("알수 없는 값이 존재합니다.");
            }
        }
        for (int i = 0; i < drop.Count; i++)
        {
            for (int j = 0; j < praList.Count; j++)
            {
                if (praList[j].e_Name == drop[i].enemy_Name)
                {
                    praList[j].droptable = drop[i];
                }
                else if (seaList[j].e_Name == drop[i].enemy_Name)
                {
                    seaList[j].droptable = drop[i];
                }
                else if (caveList[j].e_Name == drop[i].enemy_Name)
                {
                    caveList[j].droptable = drop[i];
                }
            }
        }
        List<PlayerData> playerData = new List<PlayerData>();
        string readPData = pd.ReadToEnd();
        pd.Close();
        playerData = JsonSerializer.Deserialize<List<PlayerData>>(readPData);
        Player player = new Player();
        int inputnum;
        while (true) 
        {
            Write("플레이어 아이디를 입력해주세요 : ");
            string input = ReadLine();
            for (int i = 0; i < playerData.Count; i++)
            {
                if (input == playerData[i].Name)
                {
                    WriteLine("저장된 정보를 로드합니다.");
                    player = new Player(playerData[i].Name, playerData[i].MaxExp, playerData[i].Exp, playerData[i].Lv,
                                        playerData[i].Posion, playerData[i].Credit, playerData[i].Weapon_Number, playerData[i].ItemInventory);
                    if (player.weapon_Number != 0)
                    {
                        player.Weapon = new Weapon();
                        player.Weapon = weaponList[playerData[i].Weapon_Number - 1];
                        player.WearingWeapon(weaponList[playerData[i].Weapon_Number - 1]);
                    }
                    break;
                }
            }
            if(player.GetName == null)
            {
                Write("해당 이름으로 아이디를 만드시겠습니까? 1. 예 2. 아니오 : ");
                if(int.TryParse(ReadLine(), out inputnum))
                {
                    if(inputnum == 1)
                    {
                        player = new Player(input);
                        break;
                    }
                    else if(inputnum == 2)
                    {
                        continue;
                    }

                }
                else
                {
                    Write("잘못입력했습니다. 처음으로 돌아갑니다.");
                    continue;
                }
                
            }else if (player.GetName != null)
            {
                break;
            }
        }
        player.inventory = new Inventory();
        if (player.itemInventory != null)
        {
            for (int i = 0; i < player.itemInventory.Length; i++)
            {
                player.inventory.weapons.Add(weaponList[player.itemInventory[i] - 1]);
            }
        }
        StreamReader sD = new StreamReader("SkillData.json");
        List<Skill> skill = new List<Skill>();
        string skillData = sD.ReadToEnd();
        skill = JsonSerializer.Deserialize<List<Skill>>(skillData);
        player.skill = new List<Skill>();
        for (int i  = 0; i < skill.Count; i++)
        {
            if (skill[i].Char_Name == "Player")
            {           
                player.skill.Add(skill[i]);
            }
        }
        for (int i = 0; i < bossList.Count; i++)
        {
            bossList[i].skill = new List<Skill>();
            for (int j = 0; j < skill.Count; j++)
            {

                if (bossList[i].e_Name == skill[j].Char_Name)
                {
                    bossList[i].skill.Add(skill[j]);
                }
            }
        }
        int lotation = 0;
        WriteLine("------------------------------------------------------------------------");
        while (true)
        {
            player.ShowInfo();
            WriteLine("------------------------------------------------------------------------");
            Write("\n전투를 진행할 지역을 선택해주세요 1. 초원 2. 바다 3. 동굴 4. 마을 5. 인벤토리 6. 세이브/종료 : ");
            if (int.TryParse(ReadLine(), out lotation))
            {
                int respon = new Random().Next(1, 100);   // 지역몹을 랜덤하게 리스폰하기위한 변수
                if (lotation == 1)
                {
                    BattlePhase(praList[respon%praList.Count], player, weaponList);
                }
                else if (lotation == 2)
                {
                    BattlePhase(seaList[respon%seaList.Count], player, weaponList);
                }
                else if (lotation == 3)
                {
                    WriteLine("1. 초입 2. 심층");
                    if (int.TryParse(ReadLine(), out lotation))
                    {
                        if (lotation == 1)
                        {
                            BattlePhase(caveList[respon%caveList.Count], player, weaponList);
                        }
                        else if(lotation == 2)
                        {
                            BattlePhase(deep_CaveList[respon%deep_CaveList.Count], player, weaponList);
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
                }
                else if (lotation == 4)
                {
                    Town(player, bossList, weaponList);
                }
                else if (lotation == 5)
                {
                    if (player.inventory != null)
                    {
                        player.inventory.InventoryInfo(player);
                    }
                    else
                    {
                        Console.WriteLine("아이템이 없습니다.");
                    }
                }
                else if (lotation == 6)
                {
                    StreamWriter sw = new StreamWriter("./PlayerSave1.json", false);
                    bool playerSave = false;
                    for (int i = 0; i < playerData.Count; i++)
                    {
                        if (playerData[i].Name == player.GetName)
                        {
                            playerData[i].Name = player.GetName;
                            playerData[i].MaxExp = player.p_MaxExp;
                            playerData[i].Exp = player.p_Exp;
                            playerData[i].Lv = player.Lv;
                            playerData[i].Posion = player.posion;
                            playerData[i].Credit = player.credit;
                            playerData[i].Weapon_Number = player.weapon_Number;
                            playerData[i].ItemInventory = player.inventory.GetInventory();
                            playerSave = true;
                        }
                    }
                    if(playerSave == false)
                    {
                        playerData.Add(new PlayerData());
                        playerData[playerData.Count - 1].Name = player.GetName;
                        playerData[playerData.Count - 1].MaxExp = player.p_MaxExp;
                        playerData[playerData.Count - 1].Exp = player.p_Exp;
                        playerData[playerData.Count - 1].Lv = player.Lv;
                        playerData[playerData.Count - 1].Posion = player.posion;
                        playerData[playerData.Count - 1].Credit = player.credit;
                        playerData[playerData.Count - 1].Weapon_Number = player.weapon_Number;
                        playerData[playerData.Count - 1].ItemInventory = player.inventory.GetInventory();
                        playerSave = true;
                    }    

                    readPData = JsonSerializer.Serialize(playerData);
                    sw.Write(readPData);
                    sw.Close();
                    return;
                }
                else
                {
                    WriteLine("값이 너무 큽니다.");
                    continue;
                }
            }
            else
            {
                WriteLine("숫자를 입력해주세요.");
                continue;
            }
        }
    }   // 인게임 플레이
}