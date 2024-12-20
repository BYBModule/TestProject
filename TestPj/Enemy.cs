using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class Enemy : Character
{
    public string type;                 // 몬스터의 타입
    public DropTable droptable;         // 몬스터의 드랍테이블
    public List<Skill> skill;           // 보스가 사용할 스킬을 저장
    public int Damage { get; set; }     // 외부 데이터에서 데미지값을 받기위한 프로퍼티  
    public int MaxHp { get; set; }      // 외부 데이터에서 최대체력값을 받기위한 프로퍼티
    public int MaxMp { get; set; }      // 외부 데이터에서 최대마나값을 받기위한 프로퍼티
    public int Exp { get; set; }        // 외부 데이터에서 몬스터의 경험치를 받기위한 프로퍼티
    public int Defense {  get; set; }   // 외부 데이터에서 몬스터의 방어력을 받기위한 프로퍼티
    public string Name { get; set; }    // 외부 데이터에서 몬스터의 이름을 받기위한 프로퍼티
    public string Type { get; set; }    // 외부에서 몬스터의 타입을 받기위한 프로퍼티

    public Enemy()
    {
    }
    public Enemy(int Damage, string Name, int Defense, int MaxHp, int MaxMp, int Exp, string Type)
    {
        this.damage = Damage;
        this.name = Name;
        this.defense = Defense;
        this.maxHp = MaxHp;
        this.hp = MaxHp;
        this.maxMp = MaxMp;
        this.mp = 0;
        this.exp = Exp;
        this.type = Type;
    }
    // Character클래스의 ShowInfo를 상속
    public override void ShowInfo()
    {
        base.ShowInfo();
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

