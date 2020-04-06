using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections;

namespace MoleGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            allplayers.Clear();
            try
            {
                //读取玩家信息
                JavaScriptSerializer jserializer = new JavaScriptSerializer();
                string filePath = "playerList.json";
                ArrayList objects;
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader sw = new StreamReader(fs, Encoding.Default))
                    {
                        string strJson = sw.ReadToEnd();
                        objects = jserializer.Deserialize<ArrayList>(strJson);
                    }
                }
                foreach (object o in objects)//json里读出来的是object
                {

                    JavaScriptSerializer erializer = new JavaScriptSerializer();
                    var json = erializer.Serialize(o);
                    Player player = erializer.Deserialize<Player>(json);//转回player
                    allplayers.Add(player);
                }
                refreshBoxList();
            }
            catch
            {

            }
           
        }

        public string newName = "";
        public string newMail = "";
        public List<Player> allplayers = new List<Player>();//存储所有玩家信息
        private void buttonGen_Click(object sender, EventArgs e)
        {
            //数据持久化
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            string json = jserializer.Serialize(allplayers);
            string filePath = "playerList.json";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    sw.Write(json);
                }
            }
            int num = PlayerBoxList.CheckedItems.Count;
            if (num % 2 == 1) { MessageBox.Show("所选人数为奇数！"); return; }

            System.Random random = new System.Random();
            List<int> codeList = new List<int>();
            List<int> randomList = new List<int>();
            for (int i = 1; i <= num; i++)
            {
                codeList.Add(i);
            }
            foreach (int item in codeList)//生成乱序数列
            {
                randomList.Insert(random.Next(randomList.Count), item);
            }

            int j = 0;
            for (int i = 0; i < allplayers.Count; i++)
            {
                if (PlayerBoxList.GetItemChecked(i))//给勾选的人发邮件
                {
                    int code = randomList[j];
                    sendMail(allplayers[i], code);
                    //MessageBox.Show(code.ToString());
                    j++;//换下一个code
                }
            }
            MessageBox.Show("Done!");
        }

        private void sendMail(Player p, int code)
        {
            string uid = "gyy9911";//发件人邮箱地址@符号前面的字符tom@dddd.com,则为"tom"  
            string pwd = "RMMWOKVQZIKOLIRG";
            MailAddress from = new MailAddress("gyy9911@yeah.net");

            MailAddress to = new MailAddress(p.Mail);
            MailMessage mailMessage = new MailMessage(from, to);

            string team; string mole;
            if (code % 2 == 1) team = "A"; else team = "B";
            if (code == 0 || code == 1) mole = "你是内鬼！！！"; else mole = "你不是内鬼";

            mailMessage.Subject = "内鬼生成器测试from学生卡";//邮件主题  
            mailMessage.Body = "你属于队伍" + team + "," + mole;//邮件正文 
                                                           //实例化SmtpClient  
            SmtpClient smtpClient = new SmtpClient("smtp.yeah.net", 25);
            smtpClient.UseDefaultCredentials = true;
            //设置验证发件人身份的凭据  
            smtpClient.Credentials = new NetworkCredential(uid, pwd);
            //发送  
            smtpClient.Send(mailMessage);
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            allplayers.RemoveAt(PlayerBoxList.SelectedIndex);
            refreshBoxList();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormAdd f2 = new FormAdd(this);
            f2.ShowDialog();
            allplayers.Add(new Player(newName, newMail));
            refreshBoxList();
        }
        private void refreshBoxList()//刷新列表，与数组同步
        {
            PlayerBoxList.Items.Clear();
            foreach (Player p in allplayers)
            {
                PlayerBoxList.Items.Add(p.Name + "（" + p.Mail + "）", true);
            }
        }
    }
}
