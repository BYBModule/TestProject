using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

class Character
{
    public string name;                 // 이름
    public int damage;                  // 데미지
    public int maxHp;                   // 최대 체력
    public int hp;                      // 체력
    public int maxMp;                   // 최대 마나
    public int mp;                      // 마나
    public int exp;                     // 경험치
    public bool hitStun;                // 스킬 사용 후 기절효과를 적용해야 하는지를 확인하기위한 변수입니다.
    public int deBurfCount = 0;         // 디버프가 지속될 시간을 저장하는 변수입니다.
    public string deBurfType = "";      // 스킬 사용 후 디버프의 타입을 저장할 변수입니다.
    public int dummyDamage;             // 플레이어가 디버프로인해 감소된 공격력을 다시 할당하기위해 선언된 더미변수입니다.
    public int defense = 0;                     // 방어력
    public float damageReduced;         // 피해감소 수치
    public Character()
    {

    }

    public virtual void ShowInfo()      // 정보 출력 메소드
    {
        Console.WriteLine("------------------------------------------------------------------------");
        Console.WriteLine("{0}\nDamage : {1}\t Defense : {7}\nHp : {2}/{3}\nMp : {4}/{5}\nExp : {6}", this.name, this.damage, this.hp,
                                                                   this.maxHp, this.mp,
                                                                     this.maxMp, this.exp, this.defense);
        Console.WriteLine("------------------------------------------------------------------------");
    }
    public virtual bool IsAlive()       // 캐릭터 개체의 생존여부를 확인하기위한 메소드
    {
        if(this.hp > 0)
            return true;
        return false;
    }
    public virtual float SkillActive()  // 사용한 스킬의 데미지를 출력하는 메소드
    {
        return 0;
    }

    public string SType(Skill skill)    // 사용한 스킬의 타입에 따라 처리되는 메소드
    {

        if (skill.Skill_Type == "Normal")
        {
        }
        else if (skill.Skill_Type == "Burf")
        {
            Drain();
        }
        else if(skill.Skill_Type == "Burn")
        {
            deBurfCount = 4;
            deBurfType = "Burn";
        }
        else if (skill.Skill_Type == "Stun")
        {
            hitStun = true;
        }
        else if (skill.Skill_Type == "Curse")
        {
            deBurfCount = 3;
            deBurfType = "Curse";
        }
        return deBurfType;
    }
    public void Burn()          // 일정시간 지속적으로 피해를 입음
    {
        Console.WriteLine("몸이 불타 체력이 서서히 감소합니다.");
        if (this.hp < this.hp - (this.maxHp / 10))
        {
            Console.WriteLine($"{this.hp - (this.maxHp / 10)}의 체력이 감소했습니다.");
        }
        else
        {
            Console.WriteLine($"{this.hp}의 체력이 감소했습니다.");
        }
        this.hp = this.hp - (this.maxHp / 10);
    }
    public void Curse()         // 공격력 감소
    {
        this.dummyDamage = this.damage;
        Console.WriteLine("저주에 걸려 공격력이 감소합니다.");
        Console.WriteLine($"공격력이 {this.damage - this.damage / 10} 감소합니다.");
        this.damage = this.damage - this.damage / 10;
    }
    public void Drain()         // 회복
    {
        int dummy = this.hp;
        if (this.hp + (int)((this.damage * 1.2) / 10) < maxHp)
        {
            this.hp = this.hp + (int)((this.damage * 1.2) / 10);
        }
        else
        {
            this.hp = maxHp;
        }
        Console.WriteLine($"{this.hp - dummy} 만큼 회복되었습니다.");
    }
    public float DamageReduced()
    {
        if(this.defense <= 100)
        {
            damageReduced = this.defense / 2;
        }
        else
        {
            damageReduced = 50 + (this.defense - 100) / 4;
        }
        return damageReduced;
    }
}