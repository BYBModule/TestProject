using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

class GearData
{
    public string G_Name { get; set; }
    public int Defense { get; set; }
    public int Sell_Price { get; set; }
    public string Type { get; set; }
    public string Grade { get; set; }
    public int GItem_Number { get; set; }
}
class Gear
{
    public string g_Name;           // 방어구 이름
    public int defense;             // 방어력
    public int sell_Price;          // 판매가격         
    public string type;            // Head, Defense, Glove, Shoes 4종(각각 리스트 0, 1, 2, 3에 할당)
    public string grade;            // 아이템의 등급을받아 처리하는 변수
    public int gItem_Number;         // 아이템 번호
    public float damageReduced;     // 피해감소 수치

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

    // 장비 4개를 모두 착용했을때 세트옵션을 처리하기위한 메소드(미구현Player.cs)
    public void Set_Option(List<Gear> gears)
    {
        bool setOn = false;
        if (grade != "Normal")
        {
            if(grade == "Atlatis")
            {
                setOn = false;
                if (gears[0].g_Name == "세이렌의 머리장식" && gears[1].g_Name == "상어의 비늘 갑옷" && 
                    gears[2].g_Name == "세이렌의 손장식" && gears[3].g_Name == "어인의 물갈퀴")
                {
                    // Atlantis 세트 효과
                    Console.WriteLine("아틀란티스 세트를 사용하고 있습니다.");
                }
            }
            else if(grade == "Challenge")
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
                if(setOn)
                {
                    // 도전자 세트 효과
                    Console.WriteLine("도전자 세트를 사용하고 있습니다.");
                }
            }
            else if(grade == "ForestKeeper")
            {
                setOn = false;
                if (gears[0].g_Name == "귀족의 상징" && gears[1].g_Name == "고급정장" &&
                    gears[2].g_Name == "새하얀 장갑" && gears[3].g_Name == "화려한 신발")
                {
                    setOn= true;
                }
                else if (gears[0].g_Name == "여행자의 머리깃" && gears[1].g_Name == "저주를 노래하는 자의 옷" &&
                         gears[2].g_Name == "닿지 않는 손바닥" && gears[3].g_Name == "버리지 못한 미련")
                {
                    setOn = true;

                }
                if (setOn)
                {
                    // ForestKeeper 세트 효과
                    Console.WriteLine("죽은 숲의 숲지기 세트를 사용하고 있습니다.");
                }

            }
            else if(grade == "DeathKnight")
            {
                if (gears[0].g_Name == "망령의 투구" && gears[1].g_Name == "망령의 갑옷" &&
                    gears[2].g_Name == "망령의 장갑" && gears[3].g_Name == "망령의 신발")
                {
                    // DeathKnight 세트 효과
                    Console.WriteLine("죽음의 기사 세트를 사용하고 있습니다.");
                }
            }
            else if(grade == "Sylphid")
            {
                if (gears[0].g_Name == "망령의 투구" && gears[1].g_Name == "망령의 갑옷" &&
                  gears[2].g_Name == "망령의 장갑" && gears[3].g_Name == "망령의 신발")
                {
                    // Sylphid 세트 효과
                    Console.WriteLine("바람과 함께하는 자 세트를 사용하고 있습니다.");

                }
            }
            else if(grade == "DemonLord")
            {
                if (gears[0].g_Name == "데몬로드 모자" && gears[1].g_Name == "데몬로드 갑옷" &&
                    gears[2].g_Name == "데몬로드 장갑" && gears[3].g_Name == "데몬로드 신발")
                {
                    // DemonLord 세트효과
                    Console.WriteLine("영원한 지배자 세트를 사용하고 있습니다.");
                }


            }

        }
    }
    // 기어의 정보를 출력하는 메소드
    public void gearDataInfo()
    {
        Console.WriteLine($"아이템 명 : {g_Name}\n\n방어력 : {defense}\t장착부위 : {type}\n\n판매가격 : {sell_Price}");
    }
}

