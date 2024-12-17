using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Character
{
    public string name;
    public int damage;
    public int maxHp;
    public int hp;
    public int maxMp;
    public int mp;
    public int exp;
    public bool hitStun;
    public int deBurfCount = 0;
    public string deBurfType = "";
    public int dummyDamage = 0;
    public Character()
    {

    }

    public virtual void ShowInfo()
    {
        Console.WriteLine("{0}\nDamage : {1}\nHp : {2}/{3}\nMp : {4}/{5}\nExp : {6}", this.name, this.damage, this.hp,
                                                                   this.maxHp, this.mp,
                                                                     this.maxMp, this.exp);
    }
    public virtual bool IsAlive()
    {
        if(this.hp > 0)
            return true;
        return false;
    }
    public virtual float SkillActive()                      // 사용될 스킬을 출력
    {
        return 0;
    }

    public string SType(Skill skill)
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
            deBurfCount = 5;
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
        if (this.hp > this.hp - (this.maxHp / 10))
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
        if (this.hp + (this.damage / 10) < maxHp)
        {
            this.hp = this.hp + (this.damage / 10);
        }
        else
        {
            this.hp = maxHp;
        }
        Console.WriteLine($"{this.hp - dummy} 만큼 회복되었습니다.");
    }
}