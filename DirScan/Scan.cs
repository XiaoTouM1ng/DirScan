using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DirScan
{
    public class Scan
    {
        Queue queue;
        HttpClientHelper httpClientHelper;
        List<KeyValuePair<string, string>> heads;
        string url;
        string method;
        string code;
        int count = 0;

        public Scan(string url, string method,string code,string cookie,string agent,string refe) {
            queue = new Queue();
            heads = new List<KeyValuePair<string, string>>();

            if (cookie.Trim().Length != 0)
            {
                heads.Add(new KeyValuePair<string, string>("cookie", cookie.Trim()));
            }

            if (agent.Trim().Length != 0)
            {
                heads.Add(new KeyValuePair<string, string>("user-agent", agent.Trim()));
            }

            if (refe.Trim().Length != 0)
            {
                heads.Add(new KeyValuePair<string, string>("referer", refe.Trim()));
            }
            this.url = url;
            this.method = method;
            this.code = code;
            httpClientHelper = HttpClientHelper.GetInstance();
        }


        public void addFile(string file) {

            try
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string line;
                    // 逐行读取文件内容，直到文件末尾
                    while ((line = reader.ReadLine()) != null)
                    {
                        queue.Enqueue(line);
                    }
                }
                Form1.form.progressBar1.Maximum = queue.Count;
                count = queue.Count;


            }
            catch (Exception)
            {
                return;
            }

        }

        public async Task actionAsync()
        {

            while (true) {

                if (queue.Count == 0) {
                    break;
                }
                string tUrl = url + queue.Dequeue();
                try
                {

                    HttpResponseMessage responseMessage = null;
                    

                    if (method == "GET")
                    {
                        responseMessage = httpClientHelper.Get(tUrl, heads);
                    }

                    if (method == "HEAD")
                    {
                        responseMessage = httpClientHelper.Head(tUrl, heads);
                    }

                    if (method == "POST")
                    {
                        responseMessage = httpClientHelper.Post(tUrl, "", heads);
                    }
                    
                    Form1.form.Invoke(new Action(() => {
                        Form1.form.UrlLab.Text = tUrl;
                    }));

                    foreach (var item in this.code.Split('|'))
                    {
                        if ((int)responseMessage.StatusCode == int.Parse(item))
                        {
                            string htmlContent = await responseMessage.Content.ReadAsStringAsync();
                            string title = getTitle(htmlContent);

                            if (tUrl == url) {

                                break;
                            }

                            Form1.form.Invoke(new Action(()=>{
                                Form1.form.addDirList(tUrl, title, item);
                            }));

                            break;
                        }
                    }

                    Form1.form.progressBar1.Invoke(new Action(() => {

                        Form1.form.progressBar1.Value = count - queue.Count; //设置进度条

                        if (Form1.form.progressBar1.Value == Form1.form.progressBar1.Maximum) {

                            Form1.form.button1.Text = "开始";
                        }

                    }));

                }
                catch (Exception ex)
                {
                    Form1.form.Invoke(new Action(() => {
                        Form1.form.progressBar1.Value = count - queue.Count; //设置进度条

                        if (Form1.form.progressBar1.Value == Form1.form.progressBar1.Maximum)
                        {

                            Form1.form.button1.Text = "开始";
                        }

                        Form1.form.addMsg(tUrl + " " + ex.Message);

                    }));
                    continue;
                }

            }

        }

        public void stop() {
            queue.Clear();
        }

        public string getTitle(string content) {
            string titlePattern = @"<title>(.*?)<\/title>";
            Match match = Regex.Match(content, titlePattern, RegexOptions.IgnoreCase);
            if (match.Success) {

                return match.Groups[1].Value.Trim();

            }

            return "   ";

        }

    }
}
