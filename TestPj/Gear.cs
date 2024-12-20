using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

class GearData
{
    public string G_Name { get; set; }              // 방어구 데이터
    public int Defense { get; set; }                // 방어력
    public int Sell_Price { get; set; }             // 판매가격
    public string Type { get; set; }                // 방어구의 타입(Head, Defense, Glove, Shoes)
    public string Grade { get; set; }               // 아이템의 세트효과
    public int GItem_Number { get; set; }           // 방어구의 고유번호
}
class Gear
{
    public string g_Name;               // 방어구 이름
    public int defense;                 // 방어력
    public int sell_Price;              // 판매가격         
    public string type;                 // Head, Defense, Glove, Shoes 4종(각각 리스트 0, 1, 2, 3에 할당)
    public string grade;                // 아이템의 등급을받아 처리하는 변수
    public int gItem_Number;            // 아이템 번호

    public Gear()
    {
        this.g_Name = "미착용";
        this.defense = 0;
        this.sell_Price = 0;
        this.type = "";
        this.grade = "";
        this.gItem_Number = 0;
    }
    public Gear(string g_Name, int defense, int sell_Price, string type, string grade, int gItem_Number)
    {
        this.g_Name = g_Name;
        this.defense = defense;
        this.sell_Price = sell_Price;
        this.type = type;
        this.grade = grade;
        this.gItem_Number = gItem_Number;
    }
    // 기어의 정보를 출력하는 메소드
    public void gearDataInfo()
    {
        Console.WriteLine($"아이템 명 : {g_Name}\n\n방어력 : {defense}\t장착부위 : {type}\n\n판매가격 : {sell_Price}");
    }

}

