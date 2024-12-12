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
    }
    public void SetInventory()
    {

    }
    public void InventoryInfo()
    {
        if (weapons != null)
        {
            for(int i = 0; i < weapons.Count; i++)
            {
                Console.Write("■");
                if(i != 0&&i%5 == 0)
                {
                    Console.WriteLine();
                }
                itemCount++;
            }
            Console.WriteLine("현재 인벤토리는 {0}칸 남았습니다.", 20 - itemCount);
            while (true)
            {
                Console.Write("몇번칸을 확인 하겠습니까? : ");
                int index = int.Parse(Console.ReadLine());
                {
                    if (index > 0 && index < 20)
                    {

                        if (index <= itemCount)
                        {
                            weapons[index - 1].WeaponDataInfo();
                        }
                        else
                        {
                            Console.WriteLine("아이템이 존재하지 않습니다.");
                            continue;
                        }

                    }
                }
                Console.Write("1. 다른 칸 확인하기 2. 나가기 : ");
                index = int.Parse(Console.ReadLine());
                if(index == 1)
                {
                    continue;
                }
                else if(index == 2)
                {
                    break;
                }
                else
                {
                }
            }
            itemCount = 0;
        }
        else
        {
            Console.WriteLine("아이템이 없습니다.");
        }
    }
}
