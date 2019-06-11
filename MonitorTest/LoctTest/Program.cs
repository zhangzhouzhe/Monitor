using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoctTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Monster monster = new Monster(1500);

            Player Cike = new Player() { Name = "刺客", Power = 250 };
            Player Mofashi = new Player() { Name = "魔法师", Power = 350 };

            Thread t1 = new Thread(new ParameterizedThreadStart(Cike.LightAttack));


            Thread t2 = new Thread(new ParameterizedThreadStart(Mofashi.MigcAttack));

            t1.Start(monster);
            t2.Start(monster);

            t1.Join();
            t2.Join();
            Console.ReadKey();
        }
    }

    public class Monster
    {
        public Monster(int blood)
        {
            this.Blood = blood;
            Console.WriteLine(string.Format("我是怪物，我有 {0} 滴血!\r\n", blood));
        }
        public int Blood { get; set; }
    }

    public class Player
    {
        //姓名
        public string Name { get; set; }

        //武器
        public string Weapon { get; set; }

        //攻击力
        public int Power { get; set; }

        //物理攻击
        public void PhysAttack(Object monster)
        {
            Monster m = monster as Monster;
            while (m.Blood > 0)
            {
                Monitor.Enter(monster);
                if (m.Blood > 0)
                {
                    Console.WriteLine("当前玩家 【{0}】,使用{1}攻击怪物！", this.Name, this.Weapon);
                    if (m.Blood >= this.Power)
                    {
                        m.Blood -= this.Power;
                    }
                    else
                    {
                        m.Blood = 0;
                    }
                    Console.WriteLine("怪物剩余血量:{0}\r\n", m.Blood);
                }
                Thread.Sleep(500);
                Monitor.Exit(monster);
            }
        }

        public void MigcAttack(Object monster)
        {
            Monster m = monster as Monster;
            Monitor.Enter(monster);
            Console.WriteLine("当前玩家 {0} 进入战斗\r\n", this.Name);
            while (m.Blood > 0)
            {
                Monitor.Pulse(monster);
                Console.WriteLine("当前玩家 {0} 获得攻击权限", this.Name);
                Console.WriteLine("当前玩家 {0},使用 魔法 攻击怪物！", this.Name, this.Weapon);
                m.Blood = (m.Blood >= this.Power) ? m.Blood - this.Power : 0;
                Console.WriteLine("怪物剩余血量:{0}\r\n", m.Blood);
                Thread.Sleep(500);
                Monitor.Wait(monster);
           
            }
            Monitor.Exit(monster);
        }

        //闪电攻击
        public void LightAttack(Object monster)
        {
            Monster m = monster as Monster;
            Monitor.Enter(monster);
            Console.WriteLine("当前玩家 {0} 进入战斗\r\n", this.Name);
            while (m.Blood > 0)
            {
                Monitor.Pulse(monster);
                Console.WriteLine("当前玩家 {0} 获得攻击权限", this.Name);
                Console.WriteLine("当前玩家 {0},使用 闪电 攻击怪物！", this.Name);
                m.Blood = (m.Blood >= this.Power) ? m.Blood - this.Power : 0;
                Console.WriteLine("怪物剩余血量:{0}\r\n", m.Blood);
                Thread.Sleep(500);
                Monitor.Wait(monster);
            }
            Monitor.Exit(monster);
        }

    }
}

