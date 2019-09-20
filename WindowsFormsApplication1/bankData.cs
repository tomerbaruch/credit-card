using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class bankData
    {
        public string name { get; set; }
        public int startRow { get; set; }
		public int shop { get; set; }
        public int money { get; set; }
        public int date { get; set; }

		public bankData(string name, string startRow, string shop, string money, string date)
		{
			this.name = name;
			this.startRow = Convert.ToInt32(startRow);
			this.shop = Convert.ToInt32(shop);
			this.money = Convert.ToInt32(money);
			this.date = Convert.ToInt32(date);
		}

	}
}
