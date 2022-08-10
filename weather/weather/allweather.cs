using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;

namespace weather
{
    public partial class allweather : Form
    {
        Bitmap sun = new Bitmap("image/맑음.png");
        Bitmap cloudy = new Bitmap("image/흐림.png");
        Bitmap verycloudy = new Bitmap("image/구름많음.png");
        const string targetURL = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst"; // 주소
        const string serviceKey = ""; //자신의 인증키
        const string numOfRows = "6"; // 한 페이지 결과 수
        const string pageNo = "1"; // 페이지 번호
        public static string getResults(string nx, string ny)
        {
            string result = string.Empty;
            string base_date = DateTime.Now.ToString("yyyyMMdd"); // 발표 일자
            string base_time = "";
            int time = int.Parse(DateTime.Now.ToString("hh"));
            if (2 <= time && time < 5)
            {
                base_time = "0200";
            }
            else if (5 <= time && time < 8)
            {
                base_time = "0500";
            }
            else if (8 <= time && time < 11)
            {
                base_time = "0800";
            }
            else if (11 <= time && time < 14)
            {
                base_time = "1100";
            }
            else if (14 <= time && time < 17)
            {
                base_time = "1400";
            }
            else if (17 <= time && time < 20)
            {
                base_time = "1700";
            }
            else if (20 <= time && time < 23)
            {
                base_time = "2000";
            }
            else if (23 <= time && time < 2)
            {
                base_time = "2300";
            }
            try
            {
                WebClient client = new WebClient();
                string url = string.Format(@"{0}?serviceKey={1}&numOfRows={2}&pageNo={3}&base_date={4}&base_time={5}&nx={6}&ny={7}",
                    targetURL, serviceKey, numOfRows, pageNo, base_date, base_time, nx, ny);
                using (Stream data = client.OpenRead(url))
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        string s = reader.ReadToEnd();
                        result = s;

                        reader.Close();
                        data.Close();
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            return result;
        }
        public static string stringBetween(string Source, string Start, string End)
        {
            string result = "";
            if (Source.Contains(Start) && Source.Contains(End))
            {
                int StartIndex = Source.IndexOf(Start, 0) + Start.Length;
                int EndIndex = Source.IndexOf(End, StartIndex);
                result = Source.Substring(StartIndex, EndIndex - StartIndex);
                return result;
            }

            return result;
        }

        public allweather()
        {
            InitializeComponent();
            List<string> temperature = new List<string>();
            PictureBox[] weather = { seoul_pic, chuncheon_pic, gangneung_pic, daejeon_pic, cjeongju_pic, daegu_pic, dokdo_pic, gwangju_pic, jeonju_pic, busan_pic, jeju_pic };
            string[,] array = new string[11, 2]
            {
                { "60", "127" }, // 서울
                { "73", "134"}, // 춘천
                { "92", "131" }, // 강릉
                { "67", "100" }, // 대전
                { "69", "107" }, // 청주
                { "89", "90"}, //대구
                { "127", "127" }, //울릉 / 독도
                { "58", "74" }, // 광주
                { "63", "89" }, //전주
                { "98", "76"}, //부산
                { "52", "38" } //제주
            };
            for (int i = 0; i < 11; i++)
            {
                string result = getResults(array[i, 0], array[i, 0]);
                string temp = stringBetween(result, "<fcstValue>", "</fcstValue>");
                temperature.Add(temp);
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);   // Stream결과를 XML형식으로 읽어오기
                XmlNodeList xmResponse = xml.GetElementsByTagName("response");  // <response></response> 기준으로 node 생성
                XmlNodeList xmlList = xml.GetElementsByTagName("item"); // <item></item> 기준으로 node 생성

                foreach (XmlNode node in xmResponse)    // xml의 <response> 값 읽어 들이기
                {
                    if (node["header"]["resultMsg"].InnerText.Equals("NORMAL_SERVICE")) // 정상 응답일 경우
                    {
                        foreach (XmlNode node1 in xmlList)  // <item> 값 읽어 들이기
                        {
                            if (node1["category"].InnerText.Equals("SKY"))  // 하늘상태일 경우
                            {
                                switch (node1["fcstValue"].InnerText)
                                {
                                    case "1":
                                        weather[i].Image = sun;
                                        break;
                                    case "3":
                                        weather[i].Image = verycloudy;
                                        break;
                                    case "4":
                                        weather[i].Image = cloudy;
                                        break;
                                    default:
                                        Console.WriteLine("해당 자료가 없습니다.");
                                        break;
                                }
                            }                        
                        }
                    }
                }
            }
            this.seoul_temperature.Text = temperature[0] + " ℃";
            this.chuncheon_temperature.Text = temperature[1] + " ℃";
            this.gangneung_temperature.Text = temperature[2] + " ℃";
            this.daejeon_temperature.Text = temperature[3] + " ℃";
            this.cjeongju_temperature.Text = temperature[4] + " ℃";
            this.daegu_temperature.Text = temperature[5] + " ℃";
            this.dokdo_temperature.Text = temperature[6] + " ℃";
            this.gwangju_temperature.Text = temperature[7] + " ℃";
            this.jeonju_temperature.Text = temperature[8] + " ℃";
            this.busan_temperature.Text = temperature[9] + " ℃";
            this.jeju_temperature.Text = temperature[10] + " ℃";
            this.time.Text = "기준:" + DateTime.Now.ToString("D") + DateTime.Now.ToString("HH:mm");
        }
    }
}

