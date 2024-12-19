using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

class Inventory
{
    public static int weaponCount = 0;                  // 현재 인벤토리에 저장된 무기를 받기위한 변수
    public static int gearCount = 0;                    // 현재 인벤토리에 저장된 방어구를 받기위한 변수
    public List<Weapon> weapons = new List<Weapon>();   // 인벤토리에 저장된 무기의 리스트
    public List<Gear> gears = new List<Gear>();         // 인벤토리에 저장된 방어구의 리스트
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
    public int[] GetGearInventory()             // 인벤토리에 저장된 방어구 배열을 받기위한 메소드
    {
        int[] result = new int[gears.Count];
        for (int i = 0; i < gears.Count; i++)
        {
            result[i] = gears[i].gItem_Number;
        }
        return result;
    }           
    public void InventoryWeaponInfo(Player player)   // 인벤토리 정보를 처리하는 메소드
    {
        if (weapons.Count != 0)
        {
            weaponCount = 0;
            Console.WriteLine("------------------------------------------------------------------------");
            for (int i = 0; i < weapons.Count; i++)
            {
                //Console.Write("■");
                //if (++weaponCount % 5 == 0)
                //{
                //    Console.WriteLine();
                //    Console.WriteLine();
                //}
                Console.WriteLine($"{i + 1}.\t{weapons[i].Item_Name}");
                weaponCount++;
            }
            Console.WriteLine("------------------------------------------------------------------------");
            if (20 - weaponCount > 0)
            {
                Console.WriteLine("\n\n현재 인벤토리는 {0}칸 남았습니다.", 20 - weaponCount);
            }
            else
            {
                Console.WriteLine("인벤토리에 빈공간이 없습니다.");
            }
            while (true)
            {
                Console.Write("무기의 정보를 확인 하겠습니까?  0. 뒤로가기 : ");
                int itIndex = int.Parse(Console.ReadLine());
                {
                    if (itIndex > 0 && itIndex <= 20)
                    {

                        if (itIndex <= weaponCount)
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
    public void InventoryGearInfo(Player player)   // 인벤토리 정보를 처리하는 메소드
    {
        if (gears.Count != 0)
        {
            gearCount = 0;
            Console.WriteLine("------------------------------------------------------------------------");
            for (int i = 0; i < gears.Count; i++)
            {
                //Console.Write("■");
                //if (++gearCount % 5 == 0)
                //{
                //    Console.WriteLine();
                //    Console.WriteLine();
                //}
                Console.WriteLine($"{i + 1}.\t{gears[i].g_Name}");
                gearCount++;
            }
            Console.WriteLine("------------------------------------------------------------------------");
            if (40 - gearCount > 0)
            {
                Console.WriteLine("\n\n현재 인벤토리는 {0}칸 남았습니다.", 40 - gearCount);
            }
            else
            {
                Console.WriteLine("인벤토리에 빈공간이 없습니다.");
            }
            while (true)
            {
                int itIndex;
                Console.Write("방어구의 정보를 확인 하겠습니까?  0. 뒤로가기 : ");
                if(int.TryParse(Console.ReadLine(), out itIndex))
                {
                    if (itIndex > 0 && itIndex <= 40)
                    {

                        if (itIndex <= gearCount)
                        {
                            Console.WriteLine("------------------------------------------------------------------------\n");
                            gears[itIndex - 1].gearDataInfo();
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
                else 
                {
                    Console.WriteLine("잘못 입력 하셨습니다. 다시입력해주세요");
                    continue;
                }
                Console.Write("1. 다른 칸 확인하기 2. 착용 3. 판매 4. 나가기 : ");
                int index;
                if (int.TryParse(Console.ReadLine(), out index))
                {
                    if (index == 1)
                    {
                        continue;
                    }
                    else if (index == 2)
                    {
                        int type;
                        int index2;
                        for (int i = 0; i < player.gears.Count; i++)
                        {
                            if (player.gears[i].g_Name != "미착용" && gears[itIndex - 1].type == player.gears[i].type)
                            {
                                if (gears[itIndex - 1].defense >= player.gears[i].defense)
                                {
                                    Console.WriteLine($"\n지금 사용 중인 방어구보다 방어력이 {gears[itIndex - 1].defense - player.gears[i].defense} 증가합니다.");
                                }
                                else
                                {
                                    Console.WriteLine($"\n지금 사용 중인 방어구보다 방어력이 {player.gears[i].defense - gears[itIndex - 1].defense} 감소합니다.");
                                }
                                type = i;
                                break;
                            }
                        }
                        Console.WriteLine("\n정말 착용하시겠습니까? 1. 착용 2. 돌아가기");
                        if (int.TryParse(Console.ReadLine(), out index2))
                        {
                            if (index2 == 1)
                            {
                                for (int i = 0; i < player.gears.Count; i++)
                                {
                                    if (gears[itIndex - 1].type == player.gears[i].type)
                                    {
                                        if (player.gears[i].g_Name != "미착용")
                                        {
                                            Gear temp = new Gear();
                                            temp = player.gears[i];
                                            player.WearingGear(gears[itIndex - 1], i);
                                            gears[itIndex - 1] = temp;
                                            break;
                                        }
                                        else
                                        {
                                            player.WearingGear(gears[itIndex - 1], i);
                                            gears.Remove(gears[itIndex - 1]);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (index2 == 2)
                            {
                                return;
                            }
                        }else
                        {
                            Console.WriteLine("잘못입력했습니다");
                            return;
                        }

                    }
                    else if (index == 3)
                    {
                        Console.WriteLine("------------------------------------------------------------------------");
                        Console.WriteLine($"{gears[itIndex - 1].sell_Price} 크레딧을 획득하셨습니다.");
                        player.credit += gears[itIndex - 1].sell_Price;
                        gears.Remove(gears[itIndex - 1]);
                        break;
                    }
                    else if (index == 4)
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("잘못입력했습니다");
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
