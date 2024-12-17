using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Weapon
{
    public int Damage { get; set; }                 // 무기공격력(프로퍼티)
    public string Weapon_Type { get; set; }         // 아이템 타입(프로퍼티)
    public string Item_Name { get; set; }           // 아이템 이름(프로퍼티)
    public int Sell_Price { get; set; }             // 판매가격(프로퍼티)
    public int Item_Number { get; set; }            // 아이템 고유번호(프로퍼티)
    //private int damage;                              // 무기공격력
    //private string weapon_Type;                      // 아이템 타입
    //private string item_Name;                        // 아이템 이름
    //private int sell_Price;                          // 판매가격
    //private int itemNumber;                          // 아이템 고유번호

    public Weapon()
    {
        this.Damage = 0;
        this.Weapon_Type = "";
        this.Item_Name = null;
        this.Sell_Price = 0;
        this.Item_Number = 0;
    }
    public Weapon(int damage, string weapon_Type, string item_Name, int sell_Price, int itemNumber)
    {
        this.Damage = damage;
        this.Weapon_Type = weapon_Type;
        this.Item_Name = item_Name;
        this.Sell_Price = sell_Price;
        this.Item_Number = itemNumber;
    }
    public void WeaponDataInfo()
    {
        Console.WriteLine($"아이템 명 : {Item_Name}\n\n공격력 : {Damage}\t무기타입 : {Weapon_Type}\n\n판매가격 : {Sell_Price}");
    }           // 무기정보를 출력합니다
}