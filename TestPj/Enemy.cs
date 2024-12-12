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
    //public void SetDateEnemy(Enemy LocalEnemies ,int value)
    //{
    //    if (f_Name == "초원")
    //    {

    //    }
    //    else if (f_Name == "바다")
    //    {
    //        LocalEnemies[0] = new Enemy(10, "어인", 50, 0, 10, "바다");
    //        LocalEnemies[1] = new Enemy(15, "세이렌", 100, 0, 20, "바다");
    //        LocalEnemies[2] = new Enemy(40, "크라켄", 500, 50, 200, "바다");
    //    }
    //    else if (f_Name == "동굴")
    //    {
    //        LocalEnemies[0] = new Enemy(3, "동굴박쥐", 15, 0, 4, "동굴");
    //        LocalEnemies[1] = new Enemy(17, "웜", 80, 0, 12, "동굴");
    //        LocalEnemies[2] = new Enemy(20, "고블린 무리", 300, 0, 80, "동굴");
    //    }
    //}
    public void showInfo()
    {
        WriteLine("Damage : {0}\nName : {1}\nHp : {2}/{3}\nMp : {4}/{5}\nExp : {6}\n필드 : {7}", this.damage, this.e_Name, this.e_Hp,
                                                                                         this.e_MaxHp, this.e_Mp,
                                                                                         this.e_MaxMp, this.e_Exp, this.f_Name);
    }

}
