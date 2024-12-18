﻿using System;
using System.Collections.Generic;
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
    public Weapon weapon;                   // 착용중인 무기의 데이터를 저장할 변수
    public int[] itemInventory;             // 인벤토리에 아이템의 고유값을 저장하기위한 배열
    //public Weapon Weapon { get; set; }    // 플레이어 무기 데이터값을 가져오기 위한 프로퍼티
    public Inventory inventory;             // 플레이어의 인벤토리
    public int credit = 0;                  // 플레이어의 크레딧
    public List<Skill> skill;               // 사용할 스킬의 리스트
    public bool playerTurn = true;          // 플레이어의 턴
    public bool InTower = false;            // 타워 입장 유무
    public List<Gear> gears;
    public string Name { get; set; }
    public int Exp { get; set; }
    public int Lv { get; set; }
    public int Posion { get; set; }
    public int Credit { get; set; }
    public int MaxExp { get; set; }
    public int Weapon_Number { get; set; }
    public int[] ItemInventory { get; set; }

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
                  int Credit, int Weapon_Number, int[] itemInventory) 
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
        this.itemInventory = itemInventory;
    }
    
    public override void ShowInfo()                 // 플레이어 정보 출력
    {
        if (wearing_Weapon == true)
        {
            WriteLine("Lv : {0} Player : {1}\nDamage : {2}\nHp : {3}/{4}\nMp : {5}/{6}\nExp : {7}/{8}\n소지금액 : {9}\n포션 : {10}개", this.lv, this.name, this.damage, this.hp,
                                                                               this.maxHp, this.mp,
                                                                                 this.maxMp, this.exp, this.maxExp, this.credit,
                                                                                 this.posion);
            WriteLine($"착용 장비 : {weapon.Item_Name}");
        }
        else
        {
            WriteLine("Lv : {0} Player : {1}\nDamage : {2}\nHp : {3}/{4}\nMp : {5}/{6}\nExp : {7}/{8}\n소지금액 : {9}\n포션 : {10}개", this.lv, this.name, this.damage, this.hp,
                                                                               this.maxHp, this.mp,
                                                                                 this.maxMp, this.exp, this.maxExp, this.credit,
                                                                                 this.posion);
        }
    }
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
    } // 무기 착용여부를 확인하는 메소드
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
    public void Reward(Enemy enemy, Player player, List<Weapon> weaponList)
    {
        Random reWard = new Random();
        int lucky = reWard.Next(0, 100);
        int randDrop = reWard.Next(0, enemy.droptable.item_Number.Length);
        int drop = enemy.droptable.item_Number[reWard.Next(0,2)];
        if (enemy.type == "보스")
        {
            Console.WriteLine("아이템을 획득하셨습니다. : {0}", weaponList[drop].Item_Name);
            if (player.weapon.Damage < weaponList[drop].Damage)
            {
                if (inventory.weapons.Count >= 20)
                {
                    Write("장착중인 아이템이 자동판매 되었습니다. : ");
                    credit += weapon.Sell_Price;
                    WriteLine($"{weapon.Sell_Price}크레딧을 획득하셨습니다");
                    WearingWeapon(weaponList[drop]);
                }
                else
                {
                    inventory.weapons.Add(weapon);
                    WearingWeapon(weaponList[drop]);
                }
            }
            else
            {
                if (inventory.weapons.Count >= 20)
                {
                    Write("획득한 아이템을 판매합니다. : ");
                    credit += weaponList[drop].Sell_Price;
                    WriteLine($"{weaponList[drop].Sell_Price} 크레딧을 획득하셨습니다");
                }
                else
                {
                    inventory.weapons.Add(weaponList[drop]);
                }
            }
        }
        else 
        { 
            if (lucky < 20)
            {
                randDrop = enemy.droptable.item_Number[randDrop];
                WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("아이템을 획득하셨습니다. : {0}", weaponList[randDrop - 1].Item_Name);
                if (player.weapon == null)
                {
                    WearingWeapon(weaponList[randDrop - 1]);
                }
                else if (player.weapon != null)
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

}