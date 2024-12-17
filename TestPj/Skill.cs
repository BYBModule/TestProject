using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Skill
{
    public string S_Name { get; set; }          // 스킬이름
    public int S_Mp { get; set; }               // 소모마나
    public float Incr_Damage { get; set; }      // 스킬계수
    public string Char_Name { get; set; }       // 스킬을 사용하는 대상
    public string Skill_Type { get; set; }      // 스킬의 공격타입
 
}
