using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoleGenerator
{
    public class Player
    {
        public string Name;
        public string Mail;

        public Player(string name,string mail)
        {
            this.Name = name;
            this.Mail = mail;
        }
        public Player()
        {

        }
    }
}
