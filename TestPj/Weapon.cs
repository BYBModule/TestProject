using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class WeaponData
{
    public int Damage { get; set; }                 // 무기공격력
    public string Weapon_Type { get; set; }         // 아이템 타입
    public string Item_Name { get; set; }           // 아이템 이름
    public int Sell_Price { get; set; }             // 판매가격
    public int ItemNumber { get; set; }             // 아이템 고유번호
}
class Weapon
{
    public int damage;
    public string weapon_Type;
    public string item_Name;
    public int sell_Price;
    public int itemNumber;

    public Weapon()
    {
        this.damage = 0;
        this.weapon_Type = "";
        this.item_Name = "";
        this.sell_Price = 0;
        this.itemNumber = 0;
    }
    public Weapon(int damage, string weapon_Type, string item_Name, int sell_Price, int itemNumber)
    {
        this.damage = damage;
        this.weapon_Type = weapon_Type;
        this.item_Name = item_Name;
        this.sell_Price = sell_Price;
        this.itemNumber = itemNumber;
    }
    public void WeaponDataInfo()
    {
        Console.WriteLine($"공격력 : {damage}\n무기타입 : {weapon_Type}\n아이템 명 : {item_Name}\n판매가격 : {sell_Price}");
    }
}