using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
class Player : Character
{
    public int lv;                          // 플레이어의 레벨을 저장할 변수
    public int posion;                      // 플레이어가 소지하고 있는 포션의 개수
    public int maxExp;                      // 다음 레벨로 가기위한 경험치
    public bool wearing_Weapon = false;     // 아이템 착용 유무
    public int weapon_Number;               // 착용중인 무기의 고유번호
    public int[] gear_Number = new int[4];  // 착용중인 방어구의 고유번호
    public Weapon weapon = new Weapon();    // 착용중인 무기의 데이터를 저장할 변수
    public int[] itemInventory;             // 인벤토리에 무기의 고유값을 저장하기 위한 배열
    public int[] gearInventory;             // 인벤토리에 방어구의 고유값을 저장하기 위한 배열
    //public Weapon Weapon { get; set; }    // 플레이어 무기 데이터값을 가져오기 위한 프로퍼티
    public Inventory inventory;             // 플레이어의 인벤토리
    public int credit = 0;                  // 플레이어의 크레딧
    public List<Skill> skill;               // 사용할 스킬의 리스트
    public bool playerTurn = true;          // 플레이어의 턴
    public bool InTower = false;            // 타워 입장 유무
    public string setOption;                // 세트옵션의 이름을 저장합니다.
    private bool setOn;                     // 장비 세트옵션이 켜져있는지 확인합니다.
    public List<Gear> gears = new List<Gear>(new Gear[4]);

    // 외부에서 플레이어데이터를 가져오고 저장하기위한 프로퍼티
    public string Name { get; set; }        
    public int Exp { get; set; }
    public int Lv { get; set; }
    public int Posion { get; set; }
    public int Credit { get; set; }
    public int MaxExp { get; set; }
    public int Weapon_Number { get; set; }
    public int[] Gear_Number { get; set; }
    public int[] ItemInventory { get; set; }
    public int[] GearInventory { get; set; }

    public Player()
    {

    }
    // 새로 생성되는 데이터를 저장하는 생성자
    public Player(string name)
    {
        this.name = name;
        this.damage = 5;
        this.maxHp = 50;
        this.maxMp = 10;
        this.maxExp = 10;
        this.hp = maxHp;
        this.mp = 0;
        this.posion = 5;
        this.exp = 0;
        this.lv = 1;
        this.itemInventory = null;
    } 
    public Player(string Name,int Exp, int Lv, int Posion,
                  int Credit, int Weapon_Number, int[] gear_Number, int[] itemInventory, int[] gearInventory) 
    {
        this.lv = Lv;
        if (Lv > 1)
        {
            this.damage = 5 + (2 * (Lv - 1));
            this.maxHp = 50 + (10 * (Lv - 1));
            this.maxMp = 30 + (5 * (Lv - 1));
            this.maxExp = 10 + (20* (Lv - 1));
        }
        else
        {
            this.damage = 5;
            this.maxHp = 50;
            this.maxMp = 30;
            this.maxExp = 10;
        }
        this.name = Name;
        this.hp = maxHp;
        this.mp = 0;
        this.posion = Posion;
        this.exp = Exp;
        this.credit = Credit;
        this.weapon_Number = Weapon_Number;
        this.gear_Number = gear_Number;
        this.itemInventory = itemInventory;
        this.gearInventory = gearInventory;
    }
    
    public override void ShowInfo()                 // 플레이어 정보 출력
    {
        if (wearing_Weapon == true)
        {
            Console.WriteLine("------------------------------------------------------------------------");
            WriteLine("Lv : {0} Player : {1}\nDamage : {2}\tDefense : {11}\nHp : {3}/{4}\nMp : {5}/{6}\nExp : {7}/{8}\n소지금액 : {9}\n포션 : {10}개", this.lv, this.name, this.damage, this.hp,
                                                                               this.maxHp, this.mp,
                                                                                 this.maxMp, this.exp, this.maxExp, this.credit,
                                                                                 this.posion, this.defense);
            WriteLine($"착용 무기 : {weapon.Item_Name}");
            Console.WriteLine("------------------------------------------------------------------------");
        }
        else
        {
            Console.WriteLine("------------------------------------------------------------------------");
            WriteLine("Lv : {0} Player : {1}\nDamage : {2}\tDefense : {11}\nHp : {3}/{4}\nMp : {5}/{6}\nExp : {7}/{8}\n소지금액 : {9}\n포션 : {10}개", this.lv, this.name, this.damage, this.hp,
                                                                               this.maxHp, this.mp,
                                                                                 this.maxMp, this.exp, this.maxExp, this.credit,
                                                                                 this.posion, this.defense);
            Console.WriteLine("------------------------------------------------------------------------");
        }
    }
    // 무기 착용시 데미지증감을 적용하는 메소드
    public void WearingWeapon(Weapon weapon)
    {
        if (weapon != null)
        {
            if (wearing_Weapon == false)
            {
                this.damage = this.damage + weapon.Damage;
                this.weapon = weapon;
                this.weapon_Number = weapon.Item_Number;

            }
            else
            {
                this.damage = this.damage - this.weapon.Damage + weapon.Damage;
                this.weapon = weapon;
                this.weapon_Number = weapon.Item_Number;
            }
            wearing_Weapon = true;
        }
    }
    // 방어구 착용시 방어력증감을 적용하는 메소드
    public void WearingGear(Gear gear, int typeIndex)
    {
        if (gears[typeIndex].g_Name == "미착용")
        {
            this.defense = this.defense + gear.defense;
            this.gears[typeIndex] = gear;
        }
        else
        {
            this.defense = this.defense - gears[typeIndex].defense + gear.defense;
            this.gears[typeIndex] = gear;
        }

        this.gear_Number[typeIndex] = gear.gItem_Number;
    }
    // 방어구 착용 여부를 확인하는 메소드
    public void IsWearingGear()
    {
        if (this.gears[0].type != "Head")
        {
            if (this.gears[0].type != "")
            {
                WriteLine("머리가 잘못입력됬습니다.");
                this.gears[0] = new Gear();
                WearingGear(gears[0], 0);
                this.gear_Number[0] = 0;
            }
        }
        if(this.gears[1].type != "Armor")
        {
            if (this.gears[1].type != "")
            {
                WriteLine("방어구가 잘못입력됬습니다.");
                this.gears[1] = new Gear();
                WearingGear(gears[1], 1);
                this.gear_Number[1] = 0;
            }
        }
        if (this.gears[2].type != "Glove")
        {
            if (this.gears[2].type != "")
            {
                WriteLine("장갑이 잘못입력됬습니다.");
                this.gears[2] = new Gear();
                WearingGear(gears[2], 2);
                this.gear_Number[2] = 0;
            }
        }
        if (this.gears[3].type != "Shoes")
        {
            if (this.gears[3].type != "")
            {
                WriteLine("신발이 잘못입력됬습니다.");
                this.gears[3] = new Gear();
                WearingGear(gears[3], 3);
                this.gear_Number[3] = 0;
            }
        }
    }
    public void Dead()// 플레이어 사망시 처리되는 메소드
    {
        if (InTower == true)
        {
            hp = maxHp;
            return;
        }
        else
        {
            if (this.exp - (this.maxExp / 10) >= 0)
            {
                this.exp = this.exp - (this.maxExp / 10);
                WriteLine($"{maxExp / 10}의 경험치를 잃었습니다.");
            }
            else
            {
                WriteLine($"{exp}의 경험치를 잃었습니다.");
                this.exp = 0;
            }
            hp = maxHp;
        }
    }
    public void Reward(Enemy enemy, Player player, List<Weapon> weaponList, List<Gear> gearList)
    {
        Random reWard = new Random();
        int lucky = reWard.Next(0, 100);
        int randDrop = reWard.Next(0, enemy.droptable.item_Number.Length);
        int dropWeapon = enemy.droptable.item_Number[reWard.Next(0, enemy.droptable.item_Number.Length)];
        int dropGear = enemy.droptable.GItem_Number[reWard.Next(0, enemy.droptable.GItem_Number.Length)];
        int dropType = reWard.Next(0, 2) % 2;
        if (enemy.type == "보스")
        {
            if (dropType == 0)
            {
                Console.WriteLine("아이템을 획득하셨습니다. : {0}", weaponList[dropWeapon - 1].Item_Name);
                if (player.weapon.Damage < weaponList[dropWeapon - 1].Damage)
                {
                    if (inventory.weapons.Count >= 20)
                    {
                        Write("장착중인 아이템이 자동판매 되었습니다. : ");
                        credit += weapon.Sell_Price;
                        WriteLine($"{weapon.Sell_Price}크레딧을 획득하셨습니다");
                        WearingWeapon(weaponList[dropWeapon - 1]);
                    }
                    else
                    {
                        inventory.weapons.Add(weapon);
                        WearingWeapon(weaponList[dropWeapon - 1]);
                    }
                }
                else
                {
                    if (inventory.weapons.Count >= 20)
                    {
                        Write("획득한 아이템을 판매합니다. : ");
                        credit += weaponList[dropWeapon - 1].Sell_Price;
                        WriteLine($"{weaponList[dropWeapon - 1].Sell_Price} 크레딧을 획득하셨습니다");
                    }
                    else
                    {
                        inventory.weapons.Add(weaponList[dropWeapon-1]);
                    }
                }
            }
            else
            {
                Console.WriteLine("아이템을 획득하셨습니다. : {0}", gearList[dropGear - 1].g_Name);
                for (int i = 0; i < player.gears.Count; i++)
                {
                    if (player.gears[i].type == gearList[dropGear - 1].type)
                    {
                        if (player.gears[i].defense < gearList[dropGear - 1].defense)
                        {
                            if (inventory.gears.Count >= 40)
                            {
                                Write("장착중인 아이템이 자동판매 되었습니다. : ");
                                credit += gears[i].sell_Price;
                                WriteLine($"{gears[i].sell_Price}크레딧을 획득하셨습니다");
                                WearingGear(gearList[dropGear - 1], i);
                                player.Set_Option();
                            }
                            else
                            {
                                inventory.gears.Add(gears[i]);
                                WearingGear(gearList[dropGear - 1], i);
                                player.Set_Option();
                            }
                        }
                        else
                        {
                            if (inventory.gears.Count >= 40)
                            {
                                Write("획득한 아이템을 판매합니다. : ");
                                credit += gearList[dropGear - 1].sell_Price;
                                WriteLine($"{gearList[dropGear - 1].sell_Price} 크레딧을 획득하셨습니다");
                            }
                            else
                            {
                                inventory.gears.Add(gearList[dropGear - 1]);
                            }
                        }
                        player.Set_Option();
                    }
                }
            }
        }
        else 
        {
            if (lucky < 100)
            {
                if (dropType == 0)
                {
                    randDrop = enemy.droptable.item_Number[randDrop];
                    WriteLine("------------------------------------------------------------------------");
                    Console.WriteLine("아이템을 획득하셨습니다. : {0}", weaponList[randDrop - 1].Item_Name);
                    if (player.weapon.Item_Name == "미착용")
                    {
                        WearingWeapon(weaponList[randDrop - 1]);
                    }
                    else if (player.weapon.Item_Name != "미착용")
                    {
                        if (player.weapon.Damage < weaponList[randDrop - 1].Damage)
                        {
                            if (inventory.weapons.Count >= 20)
                            {
                                Write("장착중인 아이템이 자동판매 되었습니다. : ");
                                player.credit += player.weapon.Sell_Price;
                                WriteLine($"{player.weapon.Sell_Price}크레딧을 획득하셨습니다");
                                WearingWeapon(weaponList[randDrop - 1]);
                            }
                            else
                            {
                                WearingWeapon(weaponList[randDrop - 1]);
                                inventory.weapons.Add(weapon);
                            }
                        }
                        else
                        {
                            if (inventory.weapons.Count >= 20)
                            {
                                Write("획득한 아이템을 판매합니다. : ");
                                player.credit += weaponList[randDrop - 1].Sell_Price;
                                WriteLine($"{weaponList[randDrop - 1].Sell_Price} 크레딧을 획득하셨습니다");
                            }
                            else
                            {
                                inventory.weapons.Add(weaponList[randDrop - 1]);
                            }
                        }
                    }
                }
                else
                {
                    randDrop = reWard.Next(0, enemy.droptable.GItem_Number.Length);
                    randDrop = enemy.droptable.GItem_Number[randDrop];
                    Console.WriteLine("아이템을 획득하셨습니다. : {0}", gearList[randDrop - 1].g_Name);
                    for (int i = 0; i < player.gears.Count; i++)
                    {
                        if (player.gears[i].type == gearList[randDrop - 1].type)
                        {
                            if (player.gears[i].defense < gearList[randDrop - 1].defense)
                            {
                                if (inventory.gears.Count >= 40)
                                {
                                    Write("장착중인 아이템이 자동판매 되었습니다. : ");
                                    credit += gears[i].sell_Price;
                                    WriteLine($"{gears[i].sell_Price}크레딧을 획득하셨습니다");
                                    WearingGear(gearList[randDrop - 1], i);
                                    player.Set_Option();
                                }
                                else
                                {
                                    WearingGear(gearList[randDrop - 1], i);
                                    player.Set_Option();
                                    if (gears[i].g_Name != "미착용")
                                    {
                                        inventory.gears.Add(gears[i]);
                                    }
                                }
                            }
                            else
                            {
                                if (inventory.gears.Count >= 40)
                                {
                                    Write("획득한 아이템을 판매합니다. : ");
                                    credit += gearList[randDrop - 1].sell_Price;
                                    WriteLine($"{gearList[randDrop - 1].sell_Price} 크레딧을 획득하셨습니다");
                                }
                                else
                                {
                                    inventory.gears.Add(gearList[randDrop - 1]);
                                }
                            }
                        }
                    }
                }
            }
            player.exp = exp + enemy.exp;
            player.credit = player.credit + (enemy.exp * reWard.Next(2, 5));
            while (player.exp >= player.maxExp)
            {
                exp = exp - maxExp;
                player.lv = player.lv + 1;
                LvUp();
            }
        }
    }
    public void LvUp()                              // 레벨업 처리 메소드                              
    {
        if (wearing_Weapon == true)
        {
            if (lv != 1)
            {
                this.damage = 5 + (2 * (lv - 1) + weapon.Damage);
                this.maxHp = 50 + (10 * (lv - 1));
                this.hp = maxHp;
                this.maxMp = 30 + (5 * (lv - 1));
                this.maxExp = maxExp + 20;
            }
        }
        else if (wearing_Weapon == false)
        {
            if (lv != 1)
            {
                this.damage = 5 + (2 * (lv - 1));
                this.maxHp = 50 + (10 * (lv - 1));
                this.hp = maxHp;
                this.maxMp = 30 + (5 * (lv - 1));
                this.maxExp = maxExp + 20;
            }
        }
    }
    public override float SkillActive()             // 무기에 따라 사용될 스킬을 출력
    {
        int input;
        while (weapon.Item_Name != null && this.mp >= 20)
        {
            Write("사용할 스킬을 선택하세요 ");
            if (weapon.Weapon_Type == "활")
            {
                WriteLine("1. 더블 샷, 2. 마비화살 3. 매그넘 샷");
                input = int.Parse(ReadLine());
                if (input == 1)
                {
                    WriteLine($"{skill[0].S_Name}");
                    SType(skill[0]);
                    this.mp -= 20;
                    return skill[0].Incr_Damage;
                }
                else if (input == 2)
                {
                    if (mp >= 30)
                    {
                        WriteLine($"{skill[1].S_Name}");
                        SType(skill[1]);
                        this.mp -= 30;
                        return skill[1].Incr_Damage;
                    }
                    else
                    {
                        WriteLine("마나가 부족합니다.");
                    }
                }
                else if (input == 3)
                {
                    if (mp >= 40)
                    {
                        WriteLine($"{skill[2].S_Name}");
                        SType(skill[2]);
                        this.mp -= 40;
                        return skill[2].Incr_Damage;
                    }
                    else
                    {
                        WriteLine("마나가 부족합니다.");
                    }
                }
                else
                {
                    WriteLine("잘못 입력했습니다");
                    continue;
                }
            }
            else if (weapon.Weapon_Type == "검")
            {
                WriteLine("1. 회전베기, 2. 더블어택, 3. 신경독, 4. 드레인");
                input = int.Parse(ReadLine());
                if (input == 1)
                {
                    WriteLine($"{skill[3].S_Name}");
                    SType(skill[3]);
                    this.mp -= 20;
                    return skill[3].Incr_Damage;
                }
                else if (input == 2)
                {
                    if (mp >= 30)
                    {
                        WriteLine($"{skill[4].S_Name}");
                        SType(skill[4]);
                        this.mp -= 30;
                        return skill[4].Incr_Damage;
                    }
                    else
                    {
                        WriteLine("마나가 부족합니다.");
                    }
                }
                else if(input == 3)
                {
                    if (mp >= 40)
                    {
                        WriteLine($"{skill[5].S_Name}");
                        SType(skill[5]);
                        this.mp -= 40;
                        return skill[5].Incr_Damage;
                    }
                    else
                    {
                        WriteLine("마나가 부족합니다.");
                    }
                }
                else if(input == 4)
                {
                    if (mp >= 40)
                    {
                        WriteLine($"{skill[6].S_Name}");
                        SType(skill[6]);
                        this.mp -= 40;
                        return skill[6].Incr_Damage;
                    }
                    else
                    {
                        WriteLine("마나가 부족합니다.");
                    }
                }
                else
                {
                    WriteLine("잘못 입력했습니다");
                    continue;
                }
            }
            else if (weapon.Weapon_Type == "스태프")
            {
                WriteLine("1. 블레이즈 2. 아이스애로우  3. 메테오");
                input = int.Parse(ReadLine());
                if (input == 1)
                {
                    if (mp >= 20)
                    {
                        WriteLine($"{skill[7].S_Name}");
                        SType(skill[7]);
                        this.mp -= 20;
                        return skill[7].Incr_Damage;
                    }
                    else 
                    {
                        WriteLine("마나가 부족합니다.");
                    }
                }
                else if (input == 2)
                {
                    if (mp >= 30)
                    {
                        WriteLine($"{skill[8].S_Name}");
                        SType(skill[8]);
                        this.mp -= 30;
                        return skill[8].Incr_Damage;
                    }
                    else
                    {
                        WriteLine("마나가 부족합니다.");
                    }
                }
                else if (input == 3)
                {
                    if (mp >= 60)
                    {
                        WriteLine($"{skill[9].S_Name}");
                        SType(skill[9]);
                        this.mp -= 60;
                        return skill[9].Incr_Damage;
                    }
                    else
                    {
                        WriteLine("잘못 입력했습니다");
                        continue;
                    }
                }
            }

        }
        WriteLine("장착한 무기가 없어서 사용할 수 없습니다.");
        return 0;
    }
    // 장비 4개를 모두 착용했을때 세트옵션을 처리하기위한 메소드(미구현Player.cs)
    public void Set_Option()
    {
        
        if (gears[0].grade != "Normal")
        {
            if (gears[0].grade == "Atlatis" || gears[1].grade == "Atlatis" || gears[2].grade == "Atlatis"
                || gears[3].grade == "Atlatis")
            {
                setOn = false;
                if (gears[0].g_Name == "세이렌의 머리장식" && gears[1].g_Name == "상어의 비늘 갑옷" &&
                    gears[2].g_Name == "세이렌의 손장식" && gears[3].g_Name == "어인의 물갈퀴")
                {
                    setOn = true;
                    // Atlantis 세트 효과
                    Console.WriteLine("아틀란티스 세트를 사용하고 있습니다.");
                    setOption = gears[0].grade;
                }
                IsSetOption();
            }
            else if (gears[0].grade == "Challenge" || gears[1].grade == "Challenge" || gears[2].grade == "Challenge" ||
                     gears[3].grade == "Challenge")
            {
                setOn = false;
                if (gears[0].g_Name == "도전자의 모자 Ⅰ" && gears[1].g_Name == "도전자의 갑옷 Ⅰ" &&
                    gears[2].g_Name == "도전자의 장갑 Ⅰ" && gears[3].g_Name == "도전자의 신발 Ⅰ")
                {
                    setOn = true;
                }
                if (gears[0].g_Name == "도전자의 모자 Ⅱ" && gears[1].g_Name == "도전자의 갑옷 Ⅱ" &&
                    gears[2].g_Name == "도전자의 장갑 Ⅱ" && gears[3].g_Name == "도전자의 신발 Ⅱ")
                {
                    setOn = true;
                }
                if (setOn)
                {
                    // 도전자 세트 효과
                    Console.WriteLine("도전자 세트를 사용하고 있습니다.");
                    setOption = gears[0].grade;
                }
                IsSetOption();
            }
            else if (gears[0].grade == "ForestKeeper" || gears[1].grade == "ForestKeeper" || gears[2].grade == "ForestKeeper" 
                  || gears[3].grade == "ForestKeeper")
            {
                setOn = false;
                if (gears[0].g_Name == "귀족의 상징" && gears[1].g_Name == "고급정장" &&
                    gears[2].g_Name == "새하얀 장갑" && gears[3].g_Name == "화려한 신발")
                {
                    setOn = true;
                }
                else if (gears[0].g_Name == "여행자의 머리깃" && gears[1].g_Name == "저주를 노래하는 자의 옷" &&
                         gears[2].g_Name == "닿지 않는 손바닥" && gears[3].g_Name == "버리지 못한 미련")
                {
                    setOn = true;

                }
                else if (gears[0].g_Name == "자아를 잃은자의 투구" && gears[1].g_Name == "파멸의 갑옷" &&
                         gears[2].g_Name == "부서진 건틀릿" && gears[3].g_Name == "금이간 그리브")
                {
                    setOn = true;
                }
                if (setOn)
                {
                    // ForestKeeper 세트 효과
                    Console.WriteLine("죽은 숲의 숲지기 세트를 사용하고 있습니다.");
                    setOption = gears[0].grade;
                }
                IsSetOption();

            }
            else if (gears[0].grade == "DeathKnight" || gears[1].grade == "DeathKnight" || gears[2].grade == "DeathKnight" 
                  || gears[3].grade == "DeathKnight")
            {
                if (gears[0].g_Name == "망령의 투구" && gears[1].g_Name == "망령의 갑옷" &&
                    gears[2].g_Name == "망령의 장갑" && gears[3].g_Name == "망령의 신발")
                {
                    setOn = true;
                    // DeathKnight 세트 효과
                    Console.WriteLine("죽음의 기사 세트를 사용하고 있습니다.");
                    setOption = gears[0].grade;
                    IsSetOption();

                }
            }
            else if (gears[0].grade == "Sylphid" || gears[1].grade == "Sylphid" || gears[2].grade == "Sylphid" || 
                     gears[3].grade == "Sylphid")
            {
                if (gears[0].g_Name == "망령의 투구" && gears[1].g_Name == "망령의 갑옷" &&
                  gears[2].g_Name == "망령의 장갑" && gears[3].g_Name == "망령의 신발")
                {
                    setOn = true;
                    // Sylphid 세트 효과
                    Console.WriteLine("바람과 함께하는 자 세트를 사용하고 있습니다.");
                    setOption = gears[0].grade;
                    IsSetOption();

                }
            }
            else if (gears[0].grade == "DemonLord" || gears[1].grade == "DemonLord" || gears[2].grade == "DemonLord" || 
                     gears[3].grade == "DemonLord")
            {
                if (gears[0].g_Name == "데몬로드 모자" && gears[1].g_Name == "데몬로드 갑옷" &&
                    gears[2].g_Name == "데몬로드 장갑" && gears[3].g_Name == "데몬로드 신발")
                {
                    setOn = true;
                    // DemonLord 세트효과
                    Console.WriteLine("영원한 지배자 세트를 사용하고 있습니다.");
                    setOption = gears[0].grade;
                    IsSetOption();

                }
           }
        }
        else
        {
            setOn = false;
            IsSetOption();

        }
    }
    public void IsSetOption()
    {
        if (setOn)
        {
            if(setOption == "Atlantis")
            {
                this.defense = this.defense + 10;
                this.damage = this.damage + 10;
            }
            else if (setOption == "Challenge")
            {
                this.defense = this.defense + 30;
                this.damage = this.damage + 30;
            }
            else if (setOption == "ForestKeeper")
            {
                this.defense = this.defense + 40;
                this.damage = this.damage + 40;
            }
            else if (setOption == "DeathKnight")
            {
                this.defense = this.defense - 80;
                this.damage = this.damage + 120;
            }
            else if (setOption == "Sylphid")
            {
                this.defense = this.defense + 100;
                this.damage = this.damage + 70;
            }
            else if (setOption == "DemonLord")
            {
                this.defense = this.defense + 60;
                this.damage = this.damage + 90;
            }
        }
        else if(!setOn)
        {
            if (setOption == "Atlantis")
            {
                this.defense = this.defense - 10;
                this.damage = this.damage - 10;
                setOption = "";
                Console.WriteLine("아틀란티스 세트효과가 해제되었습니다.");
            }
            else if (setOption == "Challenge")
            {
                this.defense = this.defense - 30;
                this.damage = this.damage - 30;
                setOption = "";
                Console.WriteLine("도전자 세트효과가 해제되었습니다.");
            }
            else if (setOption == "ForestKeeper")
            {
                this.defense = this.defense - 40;
                this.damage = this.damage - 40;
                setOption = "";
                Console.WriteLine("죽은숲의 숲지기 세트효과가 해제되었습니다.");
            }
            else if (setOption == "DeathKnight")
            {
                this.defense = this.defense + 80;
                this.damage = this.damage - 120;
                setOption = "";
                Console.WriteLine("죽음의 기사 세트효과가 해제되었습니다.");
            }
            else if (setOption == "Sylphid")
            {
                this.defense = this.defense - 100;
                this.damage = this.damage - 70;
                setOption = "";
                Console.WriteLine("바람과 함께하는 자 세트효과가 해제되었습니다.");
            }
            else if (setOption == "DemonLord")
            {
                this.defense = this.defense - 60;
                this.damage = this.damage - 90;
                setOption = "";
                Console.WriteLine("영원한 지배자 세트효과가 해제되었습니다.");
            }
        }
    }
    public void SetOptionInfo()
    {
        if (setOn)
        {
            if (setOption == "Atlantis")
            {
                Console.Write("\n아틀란티스");
            }
            else if (setOption == "Challenge")
            {
                Console.Write("\n도전자");
            }
            else if (setOption == "ForestKeeper")
            {
                Console.Write("\n죽은숲의 숲지기");
            }
            else if (setOption == "DeathKnight")
            {
                Console.Write("\n죽음의 기사");
            }
            else if (setOption == "Sylphid")
            {
                Console.Write("\n바람과 함께하는 자");
            }
            else if (setOption == "DemonLord")
            {
                Console.Write("\n영원한 지배자");
            }
        }
        else
        {
            Console.Write("미적용");
        }
    }

}