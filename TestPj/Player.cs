using System;
using System.Text.Json;
using static System.Console;
class PlayerData
{
    public string Name {  get; set; }
    public int MaxExp { get; set; }
    public int Exp { get; set; }
    public int Lv{ get; set; }
    public int Posion{ get; set; }
    public int Creadit {  get; set; }
}   // json데이터파일을 저장하기위한 클래스
class Player
{
    public int attack_damage;           // 플레이어의 공격력을 저장할 변수
    private string p_Name;              // 플레이어의 이름을 저장할 변수
    public int p_MaxHp;                 // 플레이어의 최대 체력
    public int p_Hp;                    // 플레이어의 현재 Hp를 저장할 변수
    public int p_MaxMp;                 // 플레이어의 최대 Mp
    public int p_Mp;                    // 플레이어의 현재 Mp를 저장할 변수
    protected int _Lv;                  // 플레이어의 레벨을 저장할 변수
    public int posion;                  // 플레이어의 Hp를 채워주는 포션의 개수              
    public int p_MaxExp = 10;           // 다음 레벨로 가기위한 최대 경험치
    public int p_Exp;                  // 플레이어의 현재 가지고 있는 경험치를 저장할 변수
    private bool wearing_Weapon = false;
    private Weapon weapon;              // 현재 무기종류
    public Inventory inventory;
    public int credit = 0;
    public string GetName { get { return p_Name; } }    // 이름을 보기위한 프로퍼티
    public int Lv
    {
        get
        {
            return _Lv;
        }
    }   // 레벨을 보기위한 프로퍼티
    public Player()
    {
        this.p_Name = null;
    } // 기본생성자
    public Player(string name)
    {
        this.p_Name = name;
        this.attack_damage = 5;
        this.p_MaxHp = 50;
        this.p_MaxMp = 10;
        this.p_MaxExp = 10;
        this.p_Hp = p_MaxHp;
        this.p_Mp = p_MaxMp;
        this.posion = 5;
        this.p_Exp = 0;
        this._Lv = 1;
    } // 새로 생성되는 데이터를 저장하는 생성자
    public Player(string name, int p_MaxExp, int p_Exp, 
                     int Lv, int posion, int creadit)
    {
        if (Lv != 1)
        {
            this.attack_damage = 5 + (2 * (Lv - 1));
            this.p_MaxHp = 50 + (10 * (Lv - 1));
            this.p_MaxMp = 10 + (5 * (Lv - 1));
        }
        else
        {
            this.attack_damage = 5;
            this.p_MaxHp = 50;
            this.p_MaxMp = 10;
        }
        this.p_Name = name;
        this.p_Hp = p_MaxHp;
        this.p_Mp = p_MaxMp;
        this.posion = posion;
        this.p_MaxExp = p_MaxExp;
        this.p_Exp = p_Exp;
        this._Lv = Lv;
        this.credit = creadit;
    } // 세이브된 데이터를 로드하기위한 생성자
    public void ShowInfo()
    {

        if (wearing_Weapon == true)
        {
            WriteLine("Lv : {0} Player : {1}\nDamage : {2}\nHp : {3}/{4}\nMp : {5}/{6}\nExp : {7}/{8}\n소지금액 : {9}\n포션 : {10}개", this._Lv, this.p_Name, this.attack_damage, this.p_Hp,
                                                                               this.p_MaxHp, this.p_Mp,
                                                                                 this.p_MaxMp, this.p_Exp, this.p_MaxExp, this.credit,
                                                                                 this.posion);
            WriteLine($"착용 장비 : {weapon.item_Name}");
        }
        else
        {
            WriteLine("Lv : {0} Player : {1}\nDamage : {2}\nHp : {3}/{4}\nMp : {5}/{6}\nExp : {7}/{8}\n소지금액 : {9}\n포션 : {10}개", this._Lv, this.p_Name, this.attack_damage, this.p_Hp,
                                                                               this.p_MaxHp, this.p_Mp,
                                                                                 this.p_MaxMp, this.p_Exp, this.p_MaxExp, this.credit,
                                                                                 this.posion);
        }
    }   // 플레이어의 정보를 출력하는 메소드
    public void WearingWeapon(Weapon weapon)
    {
        if (wearing_Weapon == false)
        {
            this.attack_damage = this.attack_damage + weapon.damage;
            this.weapon = weapon;
        }
        else
        {
            this.attack_damage = this.attack_damage -this.weapon.damage + weapon.damage;
            this.weapon = weapon;
        }
        wearing_Weapon = true;
    } // 무기 착용여부를 확인하는 메소드
    public void Dead()
    {
        if (this.p_Exp - (this.p_MaxExp / 10) >= 0)
        {
            this.p_Exp = this.p_Exp - (this.p_MaxExp / 10);
        }
        else
        {
            this.p_Exp = 0;
        }
        p_Hp = p_MaxHp;
    }   // 플레이어 사망시 처리되는 메소드
    public void Reward(Enemy enemy, Player player, List<Weapon> weaponList)
    {
        Random reWard = new Random();
        int lucky = reWard.Next(0, 100);
        int randDrop = reWard.Next(0, enemy.droptable.item_Number.Length);
        if(lucky < 20)
        {

            randDrop = enemy.droptable.item_Number[randDrop];
            Console.WriteLine("아이템을 획득하셨습니다. : {0}", weaponList[randDrop - 1].item_Name);
            if (player.weapon == null)
            {
                WearingWeapon(weaponList[randDrop - 1]);
            }
            else if (player.weapon != null)
            {
                if (player.weapon.damage < weaponList[randDrop - 1].damage)
                {
                    Write("장착중인 아이템이 자동판매 되었습니다. : ");
                    player.credit += player.weapon.sell_Price;
                    WriteLine($"{player.weapon.sell_Price}크레딧을 획득하셨습니다");
                    WearingWeapon(weaponList[randDrop - 1]);
                }
                else 
                {
                    Write("획득한 아이템을 판매합니다. : ");
                    player.credit += weaponList[randDrop - 1].sell_Price;
                    WriteLine($"{weaponList[randDrop - 1].sell_Price} 크레딧을 획득하셨습니다");
                }
            }

        }
        player.p_Exp = p_Exp + enemy.e_Exp;
        player.credit = player.credit + (enemy.e_Exp * reWard.Next(2, 5));
        while (player.p_Exp >= player.p_MaxExp)
        {
            p_Exp = p_Exp - p_MaxExp;
            player._Lv = player._Lv + 1;
            LvUp();
        }
    } // 전투승리 시 보상
    public void LvUp()  // 레벨업을 할때 처리되는 메소드
    {
        if (wearing_Weapon == true)
        {
            if (_Lv != 1)
            {
                this.attack_damage = 5 + (2 * (_Lv - 1) + weapon.damage);
                this.p_MaxHp = 50 + (10 * (_Lv - 1));
                this.p_Hp = p_MaxHp;
                this.p_MaxMp = 10 + (5 * (_Lv - 1));
                this.p_Mp = p_MaxMp;
                this.p_MaxExp = p_MaxExp * 2;
            }
        }
        else if(wearing_Weapon == false)
        {
            if (_Lv != 1)
            {
                this.attack_damage = 5 + (2 * (_Lv - 1));
                this.p_MaxHp = 50 + (10 * (_Lv - 1));
                this.p_Hp = p_MaxHp;
                this.p_MaxMp = 10 + (5 * (_Lv - 1));
                this.p_Mp = p_MaxMp;
                this.p_MaxExp = p_MaxExp * 2;
            }
        }
        
    }
    public void SkillActive(Weapon weapon)
    {
        if(weapon == null)
        {
            WriteLine("장착한 무기가 없어서 사용할 수 없습니다.");
            return;
        }
        if(weapon.weapon_Type == "검")
        {

        }
        else if (weapon.weapon_Type == "스태프")
        {
            
        }else if(weapon.weapon_Type == "활")
        {

        }
    }// 무기에 따라 사용될 다양한 스킬을 저장하는 메소드
}
