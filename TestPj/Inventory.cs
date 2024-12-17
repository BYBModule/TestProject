using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

class Inventory
{
    public static int itemCount = 0;                    // 현재 인벤토리에 저장된 아이템수를 받기위한 변수
    public List<Weapon> weapons = new List<Weapon>();   // 인벤토리에 저장된 무기의 리스트
    public Inventory()
    {

    }
    public int[] GetInventory()                 // 인벤토리에 저장된 아이템의 배열을 받기위한 메소드
    {
        int[] result = new int[weapons.Count];
        for (int i = 0; i < weapons.Count; i++)
        {
            result[i] = weapons[i].Item_Number;
        }
        return result;
    }
    public void InventoryInfo(Player player)   // 인벤토리 정보를 처리하는 메소드
    {
        if (weapons != null)
        {
            itemCount = 0;
            for (int i = 0; i < weapons.Count; i++)
            {
                Console.Write("■");
                if (++itemCount % 5 == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
            if (20 - itemCount > 0)
            {
                Console.WriteLine("\n\n현재 인벤토리는 {0}칸 남았습니다.", 20 - itemCount);
            }
            else
            {
                Console.WriteLine("인벤토리에 빈공간이 없습니다.");
            }
            while (true)
            {
                Console.Write("몇번칸을 확인 하겠습니까?  0. 뒤로가기 : ");
                int itIndex = int.Parse(Console.ReadLine());
                {
                    if (itIndex > 0 && itIndex <= 20)
                    {

                        if (itIndex <= itemCount)
                        {
                            Console.WriteLine("------------------------------------------------------------------------\n");
                            weapons[itIndex - 1].WeaponDataInfo();
                            Console.WriteLine("\n------------------------------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("아이템이 존재하지 않습니다.");
                            continue;
                        }

                    }
                    else if (itIndex == 0)
                    {
                        return;
                    }
                }
                Console.Write("1. 다른 칸 확인하기 2. 착용 3. 판매 4. 나가기 : ");
                int index = int.Parse(Console.ReadLine());
                if (index == 1)
                {
                    continue;
                }
                else if (index == 2)
                {
                    if (player.weapon != null)
                    {
                        if (weapons[itIndex - 1].Damage >= player.weapon.Damage)
                            Console.WriteLine($"\n지금 사용 중인 무기보다 공격력이 {weapons[itIndex - 1].Damage - player.weapon.Damage} 증가합니다.");
                        else
                            Console.WriteLine($"\n지금 사용 중인 무기보다 공격력이 {player.weapon.Damage - weapons[itIndex - 1].Damage} 감소합니다.");
                    }
                    Console.WriteLine("\n정말 착용하시겠습니까? 1. 착용 2. 돌아가기");                        
                    index = int.Parse(Console.ReadLine());
                    if (index == 1)
                    {
                        if (player.weapon != null)
                        {
                            Weapon temp = new Weapon();
                            temp = player.weapon;
                            player.weapon = weapons[itIndex - 1];
                            player.WearingWeapon(weapons[itIndex - 1]);
                            weapons[itIndex - 1] = temp;
                        }
                        else
                        {
                            player.WearingWeapon(weapons[itIndex - 1]);
                            weapons.Remove(weapons[itIndex - 1]);
                        }
                    }
                    else if (index == 2)
                    {
                        return;
                    }
                }
                else if (index == 3)
                {
                    Console.WriteLine("------------------------------------------------------------------------");
                    Console.WriteLine($"{weapons[itIndex-1].Sell_Price} 크레딧을 획득하셨습니다.");
                    player.credit += weapons[itIndex - 1].Sell_Price;
                    weapons.Remove(weapons[itIndex - 1]);
                    break;
                }
                else if (index == 4)
                {
                    return;
                }
            }
        }
        else
        {
            Console.WriteLine("아이템이 없습니다.");
        }
    }

}
