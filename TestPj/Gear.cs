using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Gear
{
    public string g_Name;           // 방어구 이름
    public int defense;             // 방어력
    public int sell_Price;          // 판매가격         
    public string type;             // Head, Defense, Glove, Shoes 4종(각각 리스트 0, 1, 2, 3에 할당)
    public string grade;            // 아이템의 등급을받아 처리하는 변수
    public int item_Number;         // 아이템 번호
    public float damageReduced;     // 피해감소 수치
    public string G_Name { get; set; }
    public int Defense { get; set; }
    public int Sell_Price{  get; set; }
    public string Type {  get; set; }
    public string Grade {  get; set; }
    public int Item_Number { get; set; }
    // 장비 4개를 모두 착용했을때 세트옵션을 처리하기위한 메소드(Player.cs)
    public void Set_Option(List<Gear> gears)
    {
        if(grade != "Normal")
        {
            if(grade == "Atlatis")
            {
                if (gears[0].g_Name == "세이렌의 머리장식" && gears[1].g_Name == "상어의 비늘 갑옷" && 
                    gears[2].g_Name == "세이렌의 손장식" && gears[3].g_Name == "어인의 물갈퀴")
                {
                    // Atlantis 세트 효과
                }
            }
            else if(grade == "Challenge")
            {
                if (gears[0].g_Name == "도전자의 모자 Ⅰ" && gears[1].g_Name == "도전자의 갑옷 Ⅰ" &&
                    gears[2].g_Name == "도전자의 장갑 Ⅰ" && gears[3].g_Name == "도전자의 신발 Ⅰ")
                {
                    // 도전자 세트 효과
                }
                if (gears[0].g_Name == "도전자의 모자 Ⅱ" && gears[1].g_Name == "도전자의 갑옷 Ⅱ" &&
                    gears[2].g_Name == "도전자의 장갑 Ⅱ" && gears[3].g_Name == "도전자의 신발 Ⅱ")
                {
                    // 도전자 세트 효과
                }
            }
            else if(grade == "ForestKeeper")
            {
                // ForestKeeper 세트 효과
            }
            else if(grade == "DeathKnight")
            {
                // DeathKnight 세트 효과
            }
            else if(grade == "Sylphid")
            {
                // Sylphid 세트 효과
            }
            else if(grade == "DemonLord")
            {
                // DemonLord 세트효과

            }

        }
    }
}

