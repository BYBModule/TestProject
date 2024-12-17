using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class Enemy : Character
{
    public string type;        // 몬스터의 타입
    public DropTable droptable; // 몬스터의 드랍테이블
    public List<Skill> skill;   // 보스가 사용할 스킬을 저장
    public int Damage { get; set; }
    public int MaxHp { get; set; }
    public int MaxMp { get; set; }
    public string Name { get; set; }
    public int Exp { get; set; }
    public string Type { get; set; }

    public Enemy()
    {
    }
    public Enemy(int Damage, string Name, int MaxHp, int MaxMp, int Exp, string Type)
    {
        this.damage = Damage;
        this.name = Name;
        this.maxHp = MaxHp;
        this.hp = MaxHp;
        this.maxMp = MaxMp;
        this.mp = 0;
        this.exp = Exp;
        this.type = Type;
    }
    public override void ShowInfo()
    {
        Console.WriteLine("Damage : {0}\nName : {1}\nHp : {2}/{3}\nMp : {4}/{5}\nExp : {6}", this.damage, this.name, this.hp,
                                                                                 this.maxHp, this.mp,
                                                                                 this.maxMp, this.exp);
    }
    public override float SkillActive()                      // 랜덤으로 사용될 스킬을 출력
    {
        Random rand = new Random();
        int use_Skill = rand.Next(0, this.skill.Count);
        Console.WriteLine($"{this.skill[use_Skill].S_Name} 사용 ");
        SType(this.skill[use_Skill]);
        return this.skill[use_Skill].Incr_Damage;
    }

}

