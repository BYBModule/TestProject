using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

class Enemy
{
    public string f_Name;       // 필드 이름 (마을, 초원, 동굴, 협곡, 바다)
    public int damage;          // 적의 공격력
    public string e_Name;      // 적의 이름
    public int e_MaxHp;         // 적의 최대 Hp
    public int e_Hp;            // 적이 현재 가지고 있는 Hp
    public int e_MaxMp;         // 적의 최대 Mp
    public int e_Mp;            // 적이 가지고 있는 Mp
    public int e_Exp;           // 적이 주는 경험치
    public DropTable droptable; 
    public Enemy()
    {
        this.f_Name = "";
        this.damage = 0;
        this.e_Name = "";
        this.e_MaxHp = 0;
        this.e_Hp = 0;
        this.e_MaxMp = 0;
        this.e_Mp = 0;
        this.e_Exp = 0;
    }
    public Enemy(int damage, string e_Name, int e_MaxHp, int e_MaxMp, int e_Exp, string f_Name)
    {
        this.f_Name = f_Name;
        this.damage = damage;
        this.e_Name = e_Name;
        this.e_MaxHp = e_MaxHp;
        this.e_Hp = e_MaxHp;
        this.e_MaxMp = e_MaxMp;
        this.e_Mp = e_MaxMp;
        this.e_Exp = e_Exp;
    }
    public void showInfo()
    {
        WriteLine("Damage : {0}\nName : {1}\nHp : {2}/{3}\nMp : {4}/{5}\nExp : {6}\n필드 : {7}", this.damage, this.e_Name, this.e_Hp,
                                                                                         this.e_MaxHp, this.e_Mp,
                                                                                         this.e_MaxMp, this.e_Exp, this.f_Name);
    }

}
