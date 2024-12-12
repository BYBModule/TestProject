using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Inventory
{
    public static int itemCount = 0;
    public static int itemMaxCount = 20;
    public List<Weapon> weapons;
    public Inventory()
    {
    }

    public void InventoryInfo()
    {
        if (weapons != null)
        {
            Console.WriteLine("현재 인벤토리는 {0}칸 남았습니다.", 20 - weapons.Count);
            while (true)
            {
                Console.Write("몇번칸을 확인 하겠습니까? : ");
                int index = int.Parse(Console.ReadLine());
                {
                    if (index > 0 && index < 20)
                    {

                        if (weapons[index - 1] != null)
                        {
                            weapons[index - 1].WeaponDataInfo();
                        }
                        else
                        {
                            Console.WriteLine("아이템이 존재하지 않습니다.");
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
        }
        else
        {
            Console.WriteLine("아이템이 없습니다.");
        }
    }
}
