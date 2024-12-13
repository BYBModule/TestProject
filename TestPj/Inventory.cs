using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Inventory
{
    public static int itemCount = 0;
    public List<Weapon> weapons = new List<Weapon>();
    public Inventory()
    {
    }
    public int[] GetInventory()
    {
        int[] result = new int[weapons.Count];
        for (int i = 0; i < weapons.Count; i++)
        {
            result[i] = weapons[i].itemNumber;
        }
        return result;
    }                             // 인벤토리번호에 있는 아이템의 정보를 얻기위한 클래스
    public void InventoryInfo(Player player)                    
    {
        if (weapons != null)
        {
            itemCount = 0;
            for (int i = 0; i < weapons.Count; i++)
            {
                Console.Write("■");
                if(++itemCount%5 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine("\n현재 인벤토리는 {0}칸 남았습니다.", 20 - itemCount);
            while (true)
            {
                Console.Write("몇번칸을 확인 하겠습니까? : ");
                int itIndex = int.Parse(Console.ReadLine());
                {
                    if (itIndex > 0 && itIndex < 20)
                    {

                        if (itIndex <= itemCount)
                        {
                            weapons[itIndex - 1].WeaponDataInfo();
                        }
                        else
                        {
                            Console.WriteLine("아이템이 존재하지 않습니다.");
                            continue;
                        }

                    }
                }
                Console.Write("1. 다른 칸 확인하기 2. 착용 3. 판매 4. 나가기 : ");
                int index = int.Parse(Console.ReadLine());
                if(index == 1)
                {
                    continue;
                }
                else if(index == 2)
                {
                    Weapon temp = new Weapon();
                    temp = player.Weapon;

                    player.Weapon = weapons[itIndex - 1];
                    player.WearingWeapon(weapons[itIndex - 1]);
                    weapons[itIndex - 1] = temp;
                    return;
                }
                else if(index == 3)
                {
                    player.credit += weapons[itIndex - 1].sell_Price;
                    weapons.Remove(weapons[itIndex - 1]);
                    break;
                }
            }
        }
        else
        {
            Console.WriteLine("아이템이 없습니다.");
        }
    }   // 인벤토리의 아이템의 행동을 저장한 클래스
}
