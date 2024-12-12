using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        WriteLine("------------------------------------------------------------------------");
        enemy.showInfo();
        WriteLine("------------------------------------------------------------------------");
        while (true)
        {

            if (turn == true)
            {
                player.ShowInfo();
                WriteLine("------------------------------------------------------------------------");
                Write("1.공격 2.회복 3.후퇴 : ");
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
                        WriteLine("전투에서 도망쳤습니다.");
                        WriteLine("------------------------------------------------------------------------");
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
                turn = false;
            }
            else if (turn == false)
            {
                WriteLine("------------------------------------------------------------------------");
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
                turn = true;
                WriteLine("------------------------------------------------------------------------");
            }
            if (enemy.e_Hp <= 0 || player.p_Hp <= 0)
            {
                WriteLine("------------------------------------------------------------------------");
                player.ShowInfo();
                WriteLine("------------------------------------------------------------------------");
                WriteLine("\n전투종료");
                if (enemy.e_Hp <= 0)
                {
                    WriteLine("전투에서 승리했습니다.\n");
                    WriteLine("------------------------------------------------------------------------");
                    player.Reward(enemy, player, weaponList);
                    enemy.e_Hp = enemy.e_MaxHp;
                    enemy.e_Mp = enemy.e_MaxMp;
                    turn = true;
                    return;
                }
                else
                {
                    WriteLine("전투에서 패배하였습니다.\n");
                    WriteLine("------------------------------------------------------------------------");
                    enemy.e_Hp = enemy.e_MaxHp;
                    enemy.e_Mp = enemy.e_MaxMp;
                    player.Dead();
                    turn = true;
                    return;
                }
            }
        }

    }// BattlePhase 종료
    public void Home(Player player)
    {
        int state;
        player.p_Hp = player.p_MaxHp;
        player.p_Mp = player.p_MaxMp;
        Write("플레이어 체력이 회복되었습니다.");
        while (true)
        {
            WriteLine("하고자 하는 행동을 선택해주세요 1. 지역이동, 2.포션구입(포션의 가격은 {0}원 입니다.)", (5 * (player.Lv + 1)));
            if (int.TryParse(ReadLine(), out state))
            {

                if (state == 1)
                {
                    return;
                }
                else if (state == 2 && player.posion < (10 + player.Lv))
                {
                    // 포션구입
                    if (5 * (player.Lv + 1) < player.credit)
                    {
                        WriteLine("포션을 구입했습니다.");
                        player.posion = player.posion + 1;
                        player.credit = player.credit - (5 * (player.Lv + 1));
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
    public void Ingame(List<Enemy> e_List)
    {
        StreamReader DropTable = new StreamReader("./DropTable.json");
        StreamReader iD = new StreamReader("./WeaponType.json");
        StreamReader pd = new StreamReader("./PlayerData.json"); // 플레이어 로드(미구현)treamReader pd
        List<PlayerData> playerData = new List<PlayerData>();
        string readPData = pd.ReadToEnd();
        playerData = JsonSerializer.Deserialize<List<PlayerData>>(readPData);

        Player player;
        player = new Player(playerData[0].Name, playerData[0].MaxExp, playerData[0].Exp, playerData[0].Lv,
                            playerData[0].Posion, playerData[0].Creadit);
        List<WeaponData> weaponData = new List<WeaponData>();
        List<Weapon> weaponList = new List<Weapon>();
        string readItemData = iD.ReadToEnd();
        weaponData = JsonSerializer.Deserialize<List<WeaponData>>(readItemData);

        for (int i = 0; i < weaponData.Count; i++)
        {
            weaponList.Add(new Weapon(weaponData[i].Damage, weaponData[i].Weapon_Type, weaponData[i].Item_Name, weaponData[i].Sell_Price, weaponData[i].ItemNumber));
        }
        List<DropTable> drop = new List<DropTable>();
        readItemData = DropTable.ReadToEnd();
        drop = JsonSerializer.Deserialize<List<DropTable>>(readItemData);
        List<Enemy> praList = new List<Enemy>();
        List<Enemy> seaList = new List<Enemy>();
        List<Enemy> caveList = new List<Enemy>();
        Field local = new Field();
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
            else if (e_List[i].f_Name == "동굴")
            {
                caveList.Add(e_List[i]);
            }
            else
            {
                WriteLine("알수 없는 값이 존재합니다.");
            }
        }
        for (int i = 0; i < drop.Count; i++)
        {
            for (int j = 0; j < seaList.Count; j++)
            {
                if (praList[j].e_Name == drop[i].enemy_Name)
                {
                    praList[j].droptable = drop[i];
                }
                else if (seaList[j].e_Name == drop[i].enemy_Name)
                {
                    seaList[j].droptable = drop[i];
                }else if (caveList[j].e_Name == drop[i].enemy_Name)
                {
                    caveList[j].droptable = drop[i];
                }
            }
        }
        int lotation = 0;
        while (true)
        {
            player.ShowInfo();
            Write("\n전투를 진행할 지역을 선택해주세요 1. 초원 2. 바다 3. 동굴 4. 마을 5. 인벤토리 6. 세이브/종료 : ");
            if (int.TryParse(ReadLine(), out lotation))
            {
                int respon = new Random().Next(1, 99) % 3;   // 지역몹을 랜덤하게 리스폰하기위한 변수
                if (lotation == 1)
                {
                    local.f_Name = praList[respon].f_Name;
                    local.BattlePhase(praList[2], player, weaponList);
                }
                else if (lotation == 2)
                {
                    local.f_Name = seaList[respon].f_Name;
                    local.BattlePhase(seaList[respon], player, weaponList);
                }
                else if (lotation == 3)
                {
                    local.f_Name = caveList[respon].f_Name;
                    local.BattlePhase(caveList[respon], player, weaponList);
                }
                else if (lotation == 4)
                {
                    local.f_Name = "마을";
                    Home(player);
                }
                else if (lotation == 5)
                {
                    if (player.inventory != null)
                    {
                        player.inventory.InventoryInfo();
                    }
                    else
                    {
                        Console.WriteLine("해당기능은 미구현입니다.");
                    }
                }    
                else if (lotation == 6)
                {
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
    }
}