using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsQuery;
using CsQuery.ExtensionMethods;
using Ionic.Zip;
using MangaDownloader.Properties;

namespace MangaDownloader
{
    public partial class MangaDownloaderForm : Form
    {
        public MangaDownloaderForm()
        {
            InitializeComponent();
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;//запрет изменения размера
        }
        //глобальные переменные
        //классы и объекты
        Func_saver func_saver = new Func_saver();//класс хранения функций
        List<manga_info> arr_mang_inf = new List<manga_info>();//массив из класса, описание глав + ссылка
        private string global_name;//имя манги
        CancellationTokenSource cancellationTokenSource;

        //переменные потока
        bool pause_download = false;//пауза при загрузке. t-пауза включена, f-выключена
        int err = 0;//проверка правильности пути папки. 0-верно, 1 неверно


        //универсальные переменные
        bool count = false;//проверяет попытку пользователя ввести ссылку на мангу

        private void MangaDownloaderForm_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.ico_3_;           //иконка
            Text_way_to_save.Text = Properties.Settings.Default.Folder_Path;
            Link_to_manga.Text = "http://readmanga.me/****";
            Link_to_manga.ForeColor = Color.Gray;
            status.Text = "Проверка соединения с интернетом";
        }

        private void Way_to_save_Click(object sender, EventArgs e)//кнопка выбора пути к папке
        {
            mangaFolderBrowserDialog.Description = "Выберете папку для сохранения глав";//описание
            mangaFolderBrowserDialog.ShowNewFolderButton = true;
            mangaFolderBrowserDialog.SelectedPath = Settings.Default.Folder_Path;

            if (mangaFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Text_way_to_save.Text = mangaFolderBrowserDialog.SelectedPath;  //путь записывается в text_way_to_save
                Properties.Settings.Default.Folder_Path = mangaFolderBrowserDialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void Link_to_manga_Enter(object sender, EventArgs e)//ввод ссылки на мангу 
        {   //если ссылка на мангу установлена по умолчанию в http://readmanga.me/****, то очистить текстовое поле Link_to_manga 
            if (count == false && Link_to_manga.Text == "http://readmanga.me/****")
            {
                Link_to_manga.Text = null;//очистить текстовое поле Link_to_manga
                Link_to_manga.ForeColor = Color.Black;// изменить цвет текста на черный
                count = true;//была произведена попытка ввести название
            }

        }

        private void Select_all_Click(object sender, EventArgs e)//выбрать все checkbox
        {
            for (int i = 0; i < Found_parts.Items.Count; i++)
            {
                Found_parts.SetItemChecked(i, true);
            }
        }

        private void stop_Click(object sender, EventArgs e)//остановка загрузки
        {   //отключение потока загрузки
            //настройка gui
                err = 0;
                downloadBackgroundWorker.CancelAsync();
                Text_way_to_save.Enabled = true;
                Link_to_manga.Enabled = true;
                Found_parts.Enabled = true;
                Way_to_save.Enabled = true;
                Search_parts.Enabled = true;
                Select_no.Enabled = true;
                Select_all.Enabled = true;
                Download_this.Enabled = true;
                stop.Enabled = false;
                convert_cbr.Enabled = true;
                convert_cbz.Enabled = true;
                convert_cbr_big.Enabled = true;
                convert_cbz_big.Enabled = true;


            display_link.Text = "Ссылка на закачиваемую страницу: Закачка не производится";
                pause_download = false;
                pause.Text = "Пауза";
                pause.Enabled = false;
                down_paret_now.Text = "Скачиваемая глава: Закачка не производится";


        }

        private void Download_this_Click(object sender, EventArgs e)//старт загрузки
        {  
            //проверка выбраных глав для скачивания. процесс начнется только если выбрана хоть 1 глава и выбран путь сохранения
            if (Found_parts.CheckedItems.Count>0 && Text_way_to_save.Text.IndexOf(@":\") != -1)
            {
                if (mangaFolderBrowserDialog.SelectedPath != Text_way_to_save.Text)
                {
                    Properties.Settings.Default.Folder_Path = Text_way_to_save.Text;
                    Properties.Settings.Default.Save();
                }

                cancellationTokenSource = new CancellationTokenSource();
                err = 0;
                //настройка gui
                stop.Enabled = true;
                Text_way_to_save.Enabled = false;
                Link_to_manga.Enabled = false;
                Found_parts.Enabled = false;
                Way_to_save.Enabled = false;
                Search_parts.Enabled = false;
                Select_no.Enabled = false;
                Select_all.Enabled = false;
                Download_this.Enabled = false;
                convert_cbr.Enabled = false;
                convert_cbz.Enabled = false;
                convert_cbr_big.Enabled = false;
                convert_cbz_big.Enabled = false;

                //конфиги паузы
                pause_download = false;
                pause.Text = "Пауза";
                pause.Enabled = true;
                progressBar1.Value = 0;//сброс прогресс бара



                status.Text = "Скачивание началось";
                status.ForeColor = Color.Green;
                //конфиги прогрес бара

                progressBar1.Value = 0;
                //многопоточность настройки

                downloadBackgroundWorker.RunWorkerAsync();
                downloadBackgroundWorker.WorkerSupportsCancellation = true;
                downloadBackgroundWorker.WorkerReportsProgress = true;
                downloadBackgroundWorker.DoWork += DownloadBackgroundWorkerDoWork;
                downloadBackgroundWorker.ProgressChanged += DownloadBackgroundWorkerProgressChanged;
                downloadBackgroundWorker.RunWorkerCompleted += DownloadBackgroundWorkerRunWorkerCompleted;
            }
            else 
            {   
                if(Found_parts.CheckedItems.Count==0)
                {
                    status.Text = "Главы не выбраны";
                    status.ForeColor = Color.Red;
                }
                if (Text_way_to_save.Text.IndexOf(@":\") == -1)
                {
                    status.Text = "Не выбран путь сохранения";
                    status.ForeColor = Color.Red;
                }

            }


        }
           
 
        private void DownloadBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            if (downloadBackgroundWorker.CancellationPending != true)
            {
                manga_info chapter;

                string folder_cbr_name;
                string folder_cbz_name;
                string globalZipTempFolderName;
                string path;


                ZipFile globalZip = new ZipFile(); //создание архива
                globalZip.ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866"); //подключение русского языка


                if (Text_way_to_save.Text.IndexOf(@":\") == -1)
                {
                    err = 1;
                    downloadBackgroundWorker.ReportProgress(err, "err"); //отправление состояния из потока

                }
                else
                {
                    path = $@"{Text_way_to_save.Text}\{global_name}";
                    path = func_saver.filter_disk_folder(path);
                    Console.WriteLine(path);

                    DirectoryInfo dirInfo = new DirectoryInfo(path); //создание главной папки
                    if (!dirInfo.Exists)
                    {
                        dirInfo.Create();
                    }

                    if ((convert_cbr.Checked || convert_cbr_big.Checked) &&
                        dirInfo.GetDirectories().FirstOrDefault(d => d.Name == @"cbr") == null)
                        dirInfo.CreateSubdirectory(@"cbr"); //создана папка для глав cbr
                    folder_cbr_name = $@"{path}\cbr";

                    if ((convert_cbz.Checked || convert_cbz_big.Checked) &&
                        dirInfo.GetDirectories().FirstOrDefault(d => d.Name == @"cbz") == null)
                        dirInfo.CreateSubdirectory(@"cbz"); //создана папка для глав cbz
                    folder_cbz_name = $@"{path}\cbz";

                    globalZipTempFolderName = $@"{path}\GlobalZip";
                    if (convert_cbr_big.Checked || convert_cbz_big.Checked)
                    {
                        if (dirInfo.GetDirectories().FirstOrDefault(d => d.Name == @"GlobalZip") != null)
                            DeleteDirectory(globalZipTempFolderName);

                        dirInfo.CreateSubdirectory(@"GlobalZip");
                    }

                    List<Task> downloadTasksList = new List<Task>();

                    //логика скачивания 
                    foreach (MangaChapterCheckBoxItem selectedChapter in Found_parts.CheckedItems)
                    {
                        if (downloadBackgroundWorker.CancellationPending != true)
                        {
                            chapter = arr_mang_inf.First(x => x.id == selectedChapter.Id);
                            downloadTasksList.Add(ProcessChapterAsync(cancellationTokenSource.Token, chapter, dirInfo,
                                path));
                        }

                    }

                    Task.WaitAll(downloadTasksList.ToArray());

                    //добавление изображения в общий архив
                    if (convert_cbr_big.Checked || convert_cbz_big.Checked)
                    {
                        globalZip.AddDirectory(globalZipTempFolderName, @"");

                        if (convert_cbr_big.Checked)
                            globalZip.Save(
                                $@"{folder_cbr_name}\{func_saver.filter_globalname(global_name)} ({
                                        Guid.NewGuid().ToString().Replace("-", string.Empty)
                                    }).cbr"); //сохранение большого архива cbr

                        if (convert_cbz_big.Checked)
                            globalZip.Save(
                                $@"{folder_cbz_name}\{func_saver.filter_globalname(global_name)} ({
                                        Guid.NewGuid().ToString().Replace("-", string.Empty)
                                    }).cbz"); //сохранение большого архива cbz

                        DeleteDirectory(globalZipTempFolderName);
                    }

                   downloadBackgroundWorker.CancelAsync();
                }
            }
        }


        public async Task ProcessChapterAsync(CancellationToken cancellationToken, manga_info chapter, DirectoryInfo dirInfo, string path)
        {
            var subpath = "";//путь к главам
            string fullpath;
            var page = 1;
            string img_path;

            while (!cancellationToken.IsCancellationRequested)
            {
                ZipFile zip = new ZipFile(); //создание архива
                zip.ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866"); //подключение русского языка

                if (downloadBackgroundWorker.CancellationPending != true)
                {

                    subpath = chapter.sub_name;
                    subpath = func_saver.filter_foldername(subpath);


                    Console.WriteLine(subpath);
                    if (dirInfo.GetDirectories().FirstOrDefault(d => d.Name == subpath) == null) dirInfo.CreateSubdirectory(subpath); //создана папка для главы
                    Console.WriteLine(subpath);
                    fullpath = $@"{path}\{subpath}";



                    downloadBackgroundWorker.ReportProgress(0, chapter); //счетчик страниц в главе

                    foreach (var imInf in chapter.imageInfo)
                    {
                        if (downloadBackgroundWorker.CancellationPending != true)
                        {
                            while (pause_download) System.Threading.Thread.Sleep(1000); //пауза

                            downloadBackgroundWorker.ReportProgress(page, chapter); //передается счетчик страниц

                            Console.WriteLine(imInf.url);
                            Console.WriteLine(fullpath);

                            img_path =
                                $"{fullpath}\\{imInf.name}"; //путь к изображению

                            func_saver.downloadFile(imInf.url, img_path);


                            if (convert_cbr.Checked || convert_cbz.Checked)
                                zip.AddFile(img_path, @"\"); //добавление изображения в архив
                            page++;
                        }
                    }

                    if (convert_cbr_big.Checked || convert_cbz_big.Checked)
                    {
                        var chapterDir = new DirectoryInfo(fullpath);

                        foreach (var item in chapterDir.GetFiles())
                        {
                            item.CopyTo($@"{path}\GlobalZip\{func_saver.convert_number_page(chapter.chapterNumber)}-{item.Name}", true);
                        }
                    }


                    if (convert_cbr.Checked)
                        zip.Save(
                            $@"{path}\cbr\{
                                    func_saver.filter_globalname(global_name)
                                } {subpath}.cbr"); //сохранение архива cbr

                    if (convert_cbz.Checked)
                        zip.Save(
                            $@"{path}\cbz\{
                                    func_saver.filter_globalname(global_name)
                                } {subpath}.cbz"); //сохранение архива cbz

                }

                break;
            }
        }


        public List<img_info> readmanga_ii(string ch_name, string ch_url, Guid ch_id)
        {
           
            List<img_info> img_info_list= new List<img_info>();
            var  HTML_str = func_saver.get_HTML($@"{ch_url}?mtr=1"); //получить html


            Regex pattern = new Regex(@"rm_h\.init.*\]");
            List<string> urlList = pattern.Match(HTML_str)
                .Value.Split(new[] { "],[" }, StringSplitOptions.None).Select(u =>
                      (new Regex(@"http.*\""")).Match(u).Value.Replace("',", string.Empty)
                      .Replace("\"", string.Empty)).ToList();

            var count_page = 1;

            urlList.ForEach(url =>
            {
                var ext = url.Substring(url.LastIndexOf('.'));
                ext = ext.Contains('?') ? ext.Remove(ext.IndexOf('?')) : ext;

                img_info_list.Add(new img_info($"{func_saver.convert_number_page(count_page)}{ext}",
                    url,ch_id,Guid.NewGuid()));

                count_page++;
            });

            return img_info_list;
        }


        public List<img_info> manga24_ii(string ch_name, string ch_url, Guid ch_id)
        {

            List<img_info> img_info_list = new List<img_info>();

            var  HTML_str = func_saver.get_HTML(ch_url); //получить html

            var imageFolderUrl = new Regex(@"("".*"")")
                .Match(new Regex(@"(dir.*/"")").Match(HTML_str).Value).Value
                .Replace("\"", string.Empty);

            Regex pattern = new Regex(@"(images:.*\],\])");
            var imgList = pattern.Match(HTML_str)
                .Value.Split(new[] { "],[" }, StringSplitOptions.None).Select(im =>
                    (new Regex(@"("".*"")")).Match(im).Value.Replace("\"", string.Empty)).ToList();

            var count_page = 1;

            imgList.ForEach(im =>
            {
                img_info_list.Add(new img_info($"{func_saver.convert_number_page(count_page)}{im.Substring(im.LastIndexOf('.'))}",
                    $@"https://manga24.ru/{imageFolderUrl}{im}", ch_id, Guid.NewGuid()));

                count_page++;
            });

            return img_info_list;
        }



        public List<img_info> mangachan_ii(string ch_name, string ch_url, Guid ch_id)
        {

            List<img_info> img_info_list = new List<img_info>();

            var  HTML_str = func_saver.get_HTML(ch_url); //получить html

            Regex pattern = new Regex(@"(""fullimg"":.*"")");

            List<string> urlList = pattern.Match(HTML_str)
                .Value.Split(new[] { "," }, StringSplitOptions.None).Select(u =>
                    (new Regex(@"(""http.*"")")).Match(u).Value.Replace("\"", string.Empty)).ToList();

            var count_page = 1;

            urlList.ForEach(url =>
            {
                img_info_list.Add(new img_info(
                    $"{func_saver.convert_number_page(count_page)}{url.Substring(url.LastIndexOf('.'))}",
                    url, ch_id, Guid.NewGuid()));

                count_page++;
            });

            return img_info_list;
        }


        public List<img_info> hentaichan_ii(string ch_name, string ch_url, Guid ch_id)
        {

            List<img_info> img_info_list = new List<img_info>();

            var  HTML_str = func_saver.get_HTML(ch_url); //получить html

            Regex pattern = new Regex(@"(""fullimg"":.*"")");

            List<string> urlList = pattern.Match(HTML_str)
                .Value.Split(new[] { "," }, StringSplitOptions.None).Select(u =>
                    (new Regex(@"(""http.*"")")).Match(u).Value.Replace("\"", string.Empty)).ToList();

            var count_page = 1;

            urlList.ForEach(url =>
            {
                img_info_list.Add(new img_info(
                    $"{func_saver.convert_number_page(count_page)}{url.Substring(url.LastIndexOf('.'))}",
                    url, ch_id, Guid.NewGuid()));

                count_page++;
            });

            return img_info_list;
        }


        public List<img_info> yaoichan_ii(string ch_name, string ch_url, Guid ch_id)
        {

            List<img_info> img_info_list = new List<img_info>();

            var  HTML_str = func_saver.get_HTML(ch_url); //получить html

            Regex pattern = new Regex(@"(""fullimg"":.*"")");

            List<string> urlList = pattern.Match(HTML_str)
                .Value.Split(new[] { "," }, StringSplitOptions.None).Select(u =>
                    (new Regex(@"(""http.*"")")).Match(u).Value.Replace("\"", string.Empty)).ToList();

            var count_page = 1;

            urlList.ForEach(url =>
            {
                img_info_list.Add(new img_info(
                    $"{func_saver.convert_number_page(count_page)}{url.Substring(url.LastIndexOf('.'))}",
                    url, ch_id, Guid.NewGuid()));

                count_page++;
            });

            return img_info_list;
        }



        public List<img_info> mangalib_ii(string ch_name, string ch_url, Guid ch_id)
        {

            List<img_info> img_info_list = new List<img_info>();

            var  HTML_str = func_saver.get_HTML(ch_url); //получить html

            var imageFolderUrl = new Regex(@"('.*')")
                .Match(new Regex(@"(imgUrl:.*',)").Match(HTML_str).Value).Value
                .Replace("'", string.Empty);
            var imageServer = new Regex(@"('.*')")
                .Match(new Regex(@"(imgServer:.*')").Match(HTML_str).Value).Value
                .Replace("'", string.Empty);

            var imageServerUrl = "https://img1.mangalib.me";

            switch (imageServer)
            {
                case "main":
                {
                    imageServerUrl = "https://img1.mangalib.me";
                    break;
                }
                case "secondary":
                {
                    imageServerUrl = "https://img2.mangalib.me";
                    break;
                }
                default:
                {
                    imageServerUrl = "https://img2.mangalib.me";
                    break;
                }
            }

            Regex pattern = new Regex(@"pages:.*\]");
            var imgList = pattern.Match(HTML_str)
                .Value.Split(new[] { "},{" }, StringSplitOptions.None).Select(im => new Regex(@"("".*"")").Match(im.Remove(im.IndexOf(',')).Replace(@"""page_image"":", string.Empty)).Value.Replace("\"", string.Empty)).ToList();


            var count_page = 1;

            imgList.ForEach(im =>
            {
                img_info_list.Add(new img_info(
                    $"{func_saver.convert_number_page(count_page)}{im.Substring(im.LastIndexOf('.'))}", $@"{imageServerUrl}{imageFolderUrl}{im}", ch_id, Guid.NewGuid()));

                count_page++;
            });

            return img_info_list;
        }


        public List<img_info> bato_ii(string ch_name, string ch_url, Guid ch_id)
        {

            List<img_info> img_info_list = new List<img_info>();

            var  HTML_str = func_saver.get_HTML(ch_url); //получить html

            Regex pattern = new Regex(@"(var images =.*""};)");

            List<string> urlList = pattern.Match(HTML_str)
                .Value.Split(new[] { "," }, StringSplitOptions.None).Select(u =>
                    (new Regex(@"(""http.*"")")).Match(u).Value.Replace("\"", string.Empty)).ToList();

            var count_page = 1;

            urlList.ForEach(url =>
            {
                img_info_list.Add(new img_info(
                    $"{func_saver.convert_number_page(count_page)}{url.Substring(url.LastIndexOf('.'))}",
                    url, ch_id, Guid.NewGuid()));

                count_page++;
            });

            return img_info_list;
        }





        //вывод информации при загрузке
        private void DownloadBackgroundWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {   
            //проверка правильности пути
            if(err==1)
             {
                 status.Text = "Неверно указан путь";
                 status.ForeColor = Color.Red;
             }
           
            //настройка прогресбара progressBar1
            progressBar1.Maximum = ((manga_info)e.UserState).imageInfo.Count;
            progressBar1.Value = e.ProgressPercentage;
            if (e.ProgressPercentage != 0)
            {
                //отображение текущей загрузки: название главы и ссылка на загружаемое изображение
                down_paret_now.Text = "Скачиваемая глава: " +
                                      ((manga_info) e.UserState).sub_name.Remove(
                                          ((manga_info) e.UserState).sub_name.LastIndexOf('('));
                display_link.Text = "Ссылка на закачиваемую страницу: " +
                                    ((manga_info) e.UserState).imageInfo[e.ProgressPercentage-1].url;
            }
        }

        //завершение процесса загрузки
        private void DownloadBackgroundWorkerRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {   
            //конфиги интерфейса и состояния загрузки
            err = 0;
            progressBar1.Value = 0;
            Text_way_to_save.Enabled = true;
            Link_to_manga.Enabled = true;
            Found_parts.Enabled = true;
            Way_to_save.Enabled = true;
            Search_parts.Enabled = true;
            Select_no.Enabled = true;
            Select_all.Enabled = true;
            Download_this.Enabled = true;
            stop.Enabled = false;
            pause_download = false;
            pause.Text = "Пауза";
            pause.Enabled = false;
            convert_cbr.Enabled = true;
            convert_cbz.Enabled = true;
            convert_cbr_big.Enabled = true;
            convert_cbz_big.Enabled = true;
            //сообщения
            down_paret_now.Text = "Скачиваемая глава: Закачка не производится";
           status.Text = "Статус ОК. Закачка не производится";
           display_link.Text = "Ссылка на закачиваемую страницу: Закачка не производится";

        }


        private void Select_no_Click(object sender, EventArgs e)//снять checkbox со всех найденых глав полей  Found_parts
        {
            for (int i = 0; i < Found_parts.Items.Count; i++)
            {
                Found_parts.SetItemChecked(i, false);
            }

        }

        private void Link_to_manga_Leave(object sender, EventArgs e)//при покидании текстового поля Link_to_manga
        {
            if (Link_to_manga.Text == "")//если название введено не было
            {
                Link_to_manga.Text = "http://readmanga.me/****";//ввести маску поля по умолчанию
                Link_to_manga.ForeColor = Color.Gray;//цвет текста серый
                count = false;//ссылка введена не была
            }
        }

        private void Search_parts_Click(object sender, EventArgs e)
        {

            Found_parts.Items.Clear();//очистка поля found_parts
            About_found.Text = "Найдено глав: 0";//количество найденых глав
            MangaNameLbl.Text = String.Empty;
            status.Text = String.Empty;
            try
            {
                var site_link = Link_to_manga.Text;//получение ссылки из поля
                
                //фильтрация адреса
                site_link = func_saver.filter_site_link(site_link);
                //если сайт mintmanga или  readmanga
                if (site_link.Contains("mintmanga.com") || site_link.Contains("readmanga.me") || site_link.Contains("selfmanga.ru"))
                     readmanga_searchparts(site_link); //поиск глав

                if (site_link.Contains("manga24.ru"))
                    manga24_searchparts(site_link);

                if (site_link.Contains("mangachan.me"))
                    mangachan_searchparts(site_link);

                if (site_link.Contains("hentai-chan.me"))
                    hchan_searchparts(site_link);

                if (site_link.Contains("yaoichan.me"))
                    yachan_searchparts(site_link);

                if (site_link.Contains("mangalib.me"))
                    mangalib_searchparts(site_link);

                if (site_link.Contains("bato.to"))
                    bato_searchparts(site_link);

            }
            catch (Exception ex)
            {
                status.Text = "Проверьте правильность написания ссылки";
                status.ForeColor = Color.Red;
            }


        }


      
        private void About_Click(object sender, EventArgs e)//открывает откно с инфой о программе
        {
            About f2 = new About();
            f2.ShowDialog();
        }

        private void pause_Click(object sender, EventArgs e)
        {
            pause_download = !pause_download;
            if (pause_download == false) pause.Text = "Пауза";
            if (pause_download == true) pause.Text = "Продолжить загрузку";
        }


        //функционал формы

        //поиск глав
        public void readmanga_searchparts(string site_link)
        {
            {

                //логика поиска глав mintmanga, readmanga и selfmanga

                string sn;//название для папки главы
                string ch_url;//url главы
                Guid guid;// id главы        
                int urlCount = 1;

                var  HTML_str = func_saver.get_HTML(site_link);//получение кода страницы
                status.Text = "Статус ОК";
                status.ForeColor = Color.Green;


                var dom = CQ.Create(HTML_str);//воссаздаем html страницу из полученного ответа от сервера
                var manga_name = dom[".names .name"].Text().Trim();
                global_name = manga_name;
                var ch = dom[".chapters-link a[href]"];// получаем все элементы с url и названием глав
                ch.Children().Remove();// удаляем ненужные тэги типа "новое" из названия глав

                //настройка прогресбара progressBar1
                progressBar1.Maximum = ch.Length;
                progressBar1.Value = 0;

                status.Text = "Получение списка глав со всеми страницами";
                arr_mang_inf = new List<manga_info>();//создание списка объектов -глав
                var chepterCount = ch.Length;
                ch.Each(x =>
                {
                    progressBar1.Value = urlCount;
                    urlCount++;
                    guid = Guid.NewGuid();
                    sn = func_saver.filter_nowindows_symbols(x.InnerText.Replace(manga_name, string.Empty).Trim());// удаляем лишние пробелы и имя манги из названия главы
                    ch_url = $@"{site_link}{x["href"].Substring(x["href"].IndexOf("/", 1, StringComparison.Ordinal))}";
                    arr_mang_inf.Add(new manga_info(ch_url,
                        func_saver.filter_nowindows_symbols(manga_name),
                        $@"{sn} ({guid.ToString().Replace("-", string.Empty)})",
                        guid,
                        chepterCount,
                        readmanga_ii
                        )); ;//добавление глав в лист arr_mang_inf
                    chepterCount--;
                    Found_parts.Items.Add(new MangaChapterCheckBoxItem($@"{sn}", guid), true);//добавление глав в chekbox found_parts
                });

                About_found.Text = $@"Найдено глав: {ch.Length}";//количество найденых глав
                MangaNameLbl.Text = $@"{manga_name}";
                progressBar1.Maximum = 0;
                progressBar1.Value = 0;
                status.Text = "Статус ОК. Закачка не производится";


            }//конец readmanga, minitmanga, и selfmanga
        }
        public void manga24_searchparts(string site_link)
        {
            {
                string sn;//название для папки главы
                string ch_url;//url главы
                Guid guid;// id главы        
                int urlCount = 1;

                var  HTML_str = func_saver.get_HTML(site_link);//получение кода страницы
                status.Text = "Статус ОК";
                status.ForeColor = Color.Green;


                var dom = CQ.Create(HTML_str);//воссаздаем html страницу из полученного ответа от сервера
                var manga_name = dom["main h2"].Text().Trim();
                global_name = manga_name;//получение название манги для корневой папки
                var ch = dom[".chapter-info a[href]"];// получаем все элементы с url и названием глав
                ch.Children().Remove();// удаляем ненужные тэги типа "новое" из названия глав, если таковые имеются

                //настройка прогресбара progressBar1
                progressBar1.Maximum = ch.Length;
                progressBar1.Value = 0;
                status.Text = "Получение списка глав со всеми страницами";

                arr_mang_inf = new List<manga_info>();//создание списка объектов -глав
                var chepterCount = ch.Length;
                ch.Each(x =>
                {
                    progressBar1.Value = urlCount;
                    urlCount++;
                    guid = Guid.NewGuid();
                    sn = func_saver.filter_nowindows_symbols(x.InnerText.Replace(manga_name, string.Empty).Trim());// удаляем лишние пробелы и имя манги из названия главы, если таковые имеются
                    ch_url = $@"{site_link}{x["href"].Substring(x["href"].IndexOf("/", 1, StringComparison.Ordinal))}";
                    arr_mang_inf.Add(new manga_info(ch_url,
                        func_saver.filter_nowindows_symbols(manga_name),
                        $@"{sn} ({guid.ToString().Replace("-", string.Empty)})",
                        guid,
                        chepterCount,
                        manga24_ii
                    )); ;//добавление глав в лист arr_mang_inf
                    Found_parts.Items.Add(new MangaChapterCheckBoxItem($@"{sn}", guid), true);//добавление глав в chekbox found_parts
                    chepterCount--;
                });

                About_found.Text = $@"Найдено глав: {ch.Length}";//количество найденых глав
                MangaNameLbl.Text = $@"{manga_name}";
                progressBar1.Maximum = 0;
                progressBar1.Value = 0;
                status.Text = "Статус ОК. Закачка не производится";

            }//конец manga24
        }
        public void mangachan_searchparts(string site_link)
        {
            {
                string sn;//название для папки главы
                string ch_url;//url главы
                Guid guid;// id главы        
                int urlCount = 1;

                var  HTML_str = func_saver.get_HTML(site_link);//получение кода страницы
                status.Text = "Статус ОК";
                status.ForeColor = Color.Green;


                var dom = CQ.Create(HTML_str);//воссаздаем html страницу из полученного ответа от сервера
                var manga_name = dom[".name_row h1"].Text().Trim(); 
                global_name = manga_name;//получение название манги для корневой папки
                var ch = dom[".manga a[href]"];// получаем все элементы с url и названием глав
                ch.Children().Remove();// удаляем ненужные тэги типа "новое" из названия глав, если таковые имеются

                //настройка прогресбара progressBar1
                progressBar1.Maximum = ch.Length;
                progressBar1.Value = 0;
                status.Text = "Получение списка глав со всеми страницами";

                arr_mang_inf = new List<manga_info>();//создание списка объектов -глав
                var chepterCount = ch.Length;
                ch.Each(x =>
                {
                    progressBar1.Value = urlCount;
                    urlCount++;
                    guid = Guid.NewGuid();
                    sn = func_saver.filter_nowindows_symbols(x.InnerText.Replace(Regex.Replace(manga_name, @"\(.*\)", string.Empty).Trim(), string.Empty).Trim());// удаляем лишние пробелы и имя манги из названия главы, если таковые имеются
                    ch_url = $@"http://mangachan.me{x["href"].ToString()}";
                    arr_mang_inf.Add(new manga_info(ch_url,
                        func_saver.filter_nowindows_symbols(manga_name),
                        $@"{sn} ({guid.ToString().Replace("-", string.Empty)})",
                        guid,
                        chepterCount,
                        mangachan_ii
                    )); ;//добавление глав в лист arr_mang_inf
                    Found_parts.Items.Add(new MangaChapterCheckBoxItem($@"{sn}", guid), true);//добавление глав в chekbox found_parts
                    chepterCount--;
                });

                About_found.Text = $@"Найдено глав: {ch.Length}";//количество найденых глав
                MangaNameLbl.Text = $@"{manga_name}";
                progressBar1.Maximum = 0;
                progressBar1.Value = 0;
                status.Text = "Статус ОК. Закачка не производится";


            }//конец mangachan
        }

       
        public void hchan_searchparts(string site_link)
        {
            {

                site_link = !site_link.Contains(@"/manga/")
                    ? $@"http://hentai-chan.me/manga{site_link.Substring(site_link.LastIndexOf('/'))}"
                    : site_link;

                string sn;//название для папки главы
                string ch_url;//url главы
                Guid guid;// id главы        
                int urlCount = 1;

                var  HTML_str = func_saver.get_HTML(site_link);//получение кода страницы
                status.Text = "Статус ОК";
                status.ForeColor = Color.Green;

                var dom = CQ.Create(HTML_str);//воссаздаем html страницу из полученного ответа от сервера

                var single = string.IsNullOrWhiteSpace(dom[".extaraNavi :contains('Все главы')"].Text()) && string.IsNullOrWhiteSpace(dom[".extaraNavi :contains('Все части')"].Text());

                var manga_name = Regex.Replace(dom[".name_row h1"].Text(), @"(\- глава.*)|(\- часть.*)|(\- Глава.*)|(\- Часть.*)", string.Empty).Trim();
                global_name = manga_name;//получение название манги для корневой папки

                arr_mang_inf = new List<manga_info>();



                //получение глав
                if (single)
                {
                    //если сингл 
                    guid = Guid.NewGuid();
                    ch_url = Regex.Replace($@"http://hentai-chan.me{dom[".extra_off a[href*='/online/']"][0]["href"]}",
                        @"\.html\?.*", @"\.html");
                    arr_mang_inf.Add(new manga_info(ch_url,
                        func_saver.filter_nowindows_symbols(manga_name),
                        func_saver.filter_nowindows_symbols($@"{manga_name} ({guid.ToString().Replace("-", string.Empty)})"),
                        guid,
                        1,
                        hentaichan_ii
                    ));
                    Found_parts.Items.Add(new MangaChapterCheckBoxItem(manga_name, guid), true);//добавление глав в chekbox found_parts
                }
                else
                {

                    //получение глав
                    HTML_str = func_saver.get_HTML(Regex.Replace($@"http://hentai-chan.me{dom[".extra_off a[href*='/online/']"][0]["href"]}", @"\.html\?.*", @"\.html"));//получение кода страницы
                    dom = CQ.Create(HTML_str);//воссаздаем html страницу из полученного ответа от сервера

                    var ch = dom["#related option"]; // получаем все элементы с url и названием глав
                    ch.Children().Remove(); // удаляем ненужные тэги типа "новое" из названия глав, если таковые имеются
                    
                    //настройка прогресбара progressBar1
                    progressBar1.Maximum = ch.Length;
                    progressBar1.Value = 0;
                    status.Text = "Получение списка глав со всеми страницами";
                    var chepterCount = 1;

                    ch.Each(x =>
                    {
                        progressBar1.Value = urlCount;
                        urlCount++;
                        guid = Guid.NewGuid();
                        sn = func_saver.filter_nowindows_symbols(x.InnerText.Replace($@"{manga_name} -", string.Empty).Trim()); // удаляем лишние пробелы и имя манги из названия главы, если таковые имеются
                        ch_url = $@"http://hentai-chan.me/online/{x.Value.ToString()}";
                        arr_mang_inf.Add(new manga_info(
                          ch_url,
                            func_saver.filter_nowindows_symbols(manga_name),
                            $@"{sn} ({guid.ToString().Replace("-", string.Empty)})",
                            guid,
                            chepterCount,
                          hentaichan_ii
                        )); //добавление глав в лист arr_mang_inf

                        Found_parts.Items.Add(new MangaChapterCheckBoxItem($@"{sn}", guid), true);//добавление глав в chekbox found_parts
                        chepterCount++;
                    });

                    ////сложный способ получения всех глав
                    //var offsetsUrl = $@"{Regex.Replace($@"http://hentai-chan.me{dom[".extra_off a[href*='/related/']"][0]["href"]}", @"\.html\?.*", @"\.html")}?offset=";
                    //var HTML_str = func_saver.get_HTML($@"{offsetsUrl}{0}");//получение кода страницы
                    //dom = CQ.Create(HTML_str);//воссаздаем html страницу из полученного ответа от сервера

                    //var offsets = Convert.ToInt32(Regex.Replace(dom["#pagination_related b:contains('Результ')"].Text(),
                    //    "Результ.*", String.Empty).Trim()) / 10;

                    //for (var i = 0; i <= offsets; i++)
                    //{

                    //    var HTML_str = func_saver.get_HTML($@"{offsetsUrl}{i * 10}");//получение кода страницы
                    //    dom = CQ.Create(HTML_str);//воссаздаем html страницу из полученного ответа от сервера

                    //    var ch = dom[".related_info h2 a[href]"]; // получаем все элементы с url и названием глав
                    //    ch.Children()
                    //        .Remove(); // удаляем ненужные тэги типа "новое" из названия глав, если таковые имеются

                    //    ch.Each(x =>
                    //    {
                    //        guid = Guid.NewGuid();
                    //        sn = x.InnerText.Replace($@"{global_name} -", string.Empty)
                    //            .Trim(); // удаляем лишние пробелы и имя манги из названия главы, если таковые имеются
                    //        arr_mang_inf.Add(new manga_info(
                    //            $@"http://hentai-chan.me{x["href"].ToString().Replace(@"/manga/", @"/online/")}",
                    //            global_name,
                    //            $@"{sn} ({guid.ToString().Replace("-", string.Empty)})",
                    //            guid,
                    //            hentaichan_ii
                    //        ));
                    //        ; //добавление глав в лист arr_mang_inf
                    //        Found_parts.Items.Add(new MangaChapterCheckBoxItem($@"{sn}  |  {global_name}", guid),
                    //            true); //добавление глав в chekbox found_parts
                    //    });
                    //}
                }

                About_found.Text = $@"Найдено глав: {arr_mang_inf.Count}";//количество найденых глав
                MangaNameLbl.Text = $@"{manga_name}";
                progressBar1.Maximum = 0;
                progressBar1.Value = 0;
                status.Text = "Статус ОК. Закачка не производится";

            }//конец hentaichan
        }

        public void yachan_searchparts(string site_link)
        {
            {
                string sn;//название для папки главы
                string ch_url;//url главы
                Guid guid;// id главы        
                int urlCount = 1;

                var  HTML_str = func_saver.get_HTML(site_link);//получение кода страницы
                status.Text = "Статус ОК";
                status.ForeColor = Color.Green;


                var dom = CQ.Create(HTML_str);//воссаздаем html страницу из полученного ответа от сервера
                var manga_name = dom[".name_row h1"].Text().Trim();
                global_name = manga_name;//получение название манги для корневой папки
                var ch = dom[".manga a[href]"];// получаем все элементы с url и названием глав
                ch.Children().Remove();// удаляем ненужные тэги типа "новое" из названия глав, если таковые имеются

                //настройка прогресбара progressBar1
                progressBar1.Maximum = ch.Length;
                progressBar1.Value = 0;
                status.Text = "Получение списка глав со всеми страницами";

                arr_mang_inf = new List<manga_info>();//создание списка объектов -глав
                var chepterCount = ch.Length;
                ch.Each(x =>
                {
                    progressBar1.Value = urlCount;
                    urlCount++;
                    guid = Guid.NewGuid();
                    sn = func_saver.filter_nowindows_symbols(x.InnerText.Replace(Regex.Replace(manga_name, @"\(.*\)", string.Empty).Trim(), string.Empty).Trim());// удаляем лишние пробелы и имя манги из названия главы, если таковые имеются
                    ch_url = $@"http://yaoichan.me{x["href"].ToString()}";
                    arr_mang_inf.Add(new manga_info(ch_url,
                        func_saver.filter_nowindows_symbols(manga_name),
                        $@"{sn} ({guid.ToString().Replace("-", string.Empty)})",
                        guid,
                        chepterCount,
                        yaoichan_ii
                    )); ;//добавление глав в лист arr_mang_inf
                    Found_parts.Items.Add(new MangaChapterCheckBoxItem($@"{sn}", guid), true);//добавление глав в chekbox found_partss
                    chepterCount--;
                });

                About_found.Text = $@"Найдено глав: {ch.Length}";//количество найденых глав
                MangaNameLbl.Text = $@"{manga_name}";
                progressBar1.Maximum = 0;
                progressBar1.Value = 0;
                status.Text = "Статус ОК. Закачка не производится";


            }//конец mangachan
        }

        public void mangalib_searchparts(string site_link)
        {
            {

                //логика поиска глав mangalib

                string sn;//название для папки главы
                string ch_url;//url главы
                Guid guid;// id главы    
                int urlCount = 1;

                var  HTML_str = func_saver.get_HTML(site_link);//получение кода страницы
                status.Text = "Статус ОК";
                status.ForeColor = Color.Green;


                var dom = CQ.Create(HTML_str);//воссаздаем html страницу из полученного ответа от сервера
                var manga_name = dom[".manga__title"].Text().Trim();
                global_name = manga_name;//получение название манги для корневой папки
                manga_name = manga_name.Remove(manga_name.IndexOf("/", StringComparison.Ordinal)).Trim();
                var ch = dom[".chapter-item__name a[href]"];// получаем все элементы с url и названием глав
                                                            //ch.Children().Remove();// удаляем ненужные тэги типа "новое" из названия глав

                //настройка прогресбара progressBar1
                progressBar1.Maximum = ch.Length;
                progressBar1.Value = 0;
                status.Text = "Получение списка глав со всеми страницами";

                arr_mang_inf = new List<manga_info>();//создание списка объектов -глав
                var chepterCount = ch.Length;
                ch.Each(x =>
                {
                    progressBar1.Value = urlCount;
                    urlCount++;
                    guid = Guid.NewGuid();
                    sn = func_saver.filter_nowindows_symbols(x.InnerText.Trim());// удаляем лишние пробелы и имя манги из названия главы
                    ch_url = x["href"];
                    arr_mang_inf.Add(new manga_info(ch_url,
                        func_saver.filter_nowindows_symbols(manga_name),
                        $@"{sn} ({guid.ToString().Replace("-", string.Empty)})",
                        guid,
                        chepterCount,
                      mangalib_ii
                    )); ;//добавление глав в лист arr_mang_inf
                    Found_parts.Items.Add(new MangaChapterCheckBoxItem($@"{sn}", guid), true);//добавление глав в chekbox found_parts
                    chepterCount--;
                });

                About_found.Text = $@"Найдено глав: {ch.Length}";//количество найденых глав
                MangaNameLbl.Text = $@"{manga_name}";

                progressBar1.Maximum = 0;
                progressBar1.Value = 0;
                status.Text = "Статус ОК. Закачка не производится";


            }//конец mangalib
        }

        public void bato_searchparts(string site_link)
        {
            {
                //логика поиска глав bato.to

                string sn;//название для папки главы
                string ch_url;//url главы
                Guid guid;// id главы 
                int urlCount = 1;

                var  HTML_str = func_saver.get_HTML(site_link);//получение кода страницы
                status.Text = "Статус ОК";
                status.ForeColor = Color.Green;


                var dom = CQ.Create(HTML_str);//воссаздаем html страницу из полученного ответа от сервера
                var manga_name = dom[".mt-4.title-set a[href]"].Text().Trim();
                global_name = manga_name;//получение название манги для корневой папки
                var ch = dom[".chapt"];// получаем все элементы с url и названием глав
                ch.Children().Remove();// удаляем ненужные тэги типа "новое" из названия глав

                arr_mang_inf = new List<manga_info>();//создание списка объектов -глав

                //настройка прогресбара progressBar1
                progressBar1.Maximum = ch.Length;
                progressBar1.Value = 0;
                status.Text = "Получение списка глав со всеми страницами";
                var chepterCount = ch.Length;
                ch.Each(x =>
                {
                    progressBar1.Value = urlCount;
                    urlCount++;
                    guid = Guid.NewGuid();
                    sn = func_saver.filter_nowindows_symbols(x.InnerText.Trim());// удаляем лишние пробелы и имя манги из названия главы
                    ch_url =$@"https://bato.to{x["href"]}";
                    arr_mang_inf.Add(new manga_info(ch_url,
                        func_saver.filter_nowindows_symbols(manga_name),
                        $@"{sn} ({guid.ToString().Replace("-", string.Empty)})",
                        guid,
                        chepterCount,
                        bato_ii
                    )); ;//добавление глав в лист arr_mang_inf
                    Found_parts.Items.Add(new MangaChapterCheckBoxItem($@"{sn}", guid), true);//добавление глав в chekbox found_parts
                    chepterCount--;
                });

                About_found.Text = $@"Найдено глав: {ch.Length}";//количество найденых глав
                MangaNameLbl.Text = $@"{manga_name}";
                progressBar1.Maximum = 0;
                progressBar1.Value = 0;
                status.Text = "Статус ОК. Закачка не производится";

            }//конец bato.to
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }


    }

    public class manga_info//класс описания главы + ссылка
    {
        public string link;//ссылка
        public string name;//название корневой папки
        public string sub_name;//название главы для папки
        public Guid id;// id главы
        public int chapterNumber;// номер в списке

        public List<img_info> imageInfo
        {
            get
            {
                if (_imageInfo==null || !_imageInfo.Any())
                {
                    _imageInfo = getImgInfoList(name, link, id);
                }

                return _imageInfo;
            }
            set => throw new NotImplementedException();
        } //лист изо

        private Func<string, string, Guid, List<img_info>> getImgInfoList;
        private List<img_info> _imageInfo;

        public manga_info(string l, string n, string sn, Guid i, int c, Func<string, string, Guid, List<img_info>> g)
        {
            link = l;
            name = n;
            sub_name = sn;
            id = i;
            chapterNumber = c;
            getImgInfoList =g;
        }
    }

    public class img_info//класс описания изображения 
    {
        public string name;//имя
        public string url;//url
        public Guid chId;// id главы
        public Guid imgId;// id изображения

        public img_info(string n, string u, Guid c, Guid i)
        {
            name = n;
            url = u;
            chId = c;
            imgId = i;
        }
    }

    public class global_arch_img_info//класс описания изображения + локальный путь
    {
        public string name;//имя
        public string path;//путь
  
 
        public global_arch_img_info(string n, string p)
        {
            name = n;
            path = p;
        }
    }


    public class MangaChapterCheckBoxItem // кастомный класс для checkBoxItem
    {
        public MangaChapterCheckBoxItem(string chapterName, Guid guid)
        {
            ChapterName = chapterName;
            Id = guid;
        }

        public string ChapterName { get; private set; }
        public Guid Id { get; private set; }


        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(ChapterName)? "" : ChapterName;
        }
    }

    public class Func_saver//сюда будут писаться всякие функции
    {
        //фильтрация строк на символы, запрещеныне windows
        public string filter_nowindows_symbols( string str)
        {
            str = str.Replace(@">", " ");
            str = str.Replace(@"<", " ");
            str = str.Replace(@"?", " ");
            str = str.Replace(@":", " ");
            str = str.Replace(@"|", " ");
            str = str.Replace(@"*", " ");
            str = str.Replace(@"\", " ");
            str = str.Replace(@"/", " ");
            str = str.Replace(@"'", " ");
            str = str.Replace("\n", string.Empty);
            str=new Regex("[ ]{2,}").Replace(str, " ");
            return str;
        }

        //фильтрация пути папок с главами
        public string filter_foldername(string str)
        {   
            str = str.Replace(@">", " ");
            str = str.Replace(@"<", " ");
            str = str.Replace(@"?", " ");
            str = str.Replace(@":", "");
            str = str.Replace(@"|", " ");
            str = str.Replace(@"*", " ");
            str = str.Replace(@"'", " ");
            return str;
        }
        //фильтрация глобального имени
        public string filter_globalname(string str)
        {
            str = str.Replace(@">", " ");
            str = str.Replace(@"<", " ");
            str = str.Replace(@"?", " ");
            str = str.Replace(@":", "");
            str = str.Replace(@"|", " ");
            str = str.Replace(@"*", " ");
            str = str.Replace(@"'", " ");
            str = str.Replace(@"/", " ");
            return str;
        }

        //фильтрация пути  корневой папки 
        public string filter_disk_folder(string str) 
        {
            str = str.Replace(@">", " ");
            str = str.Replace(@"<", " ");
            str = str.Replace(@"?", " ");
            str = str.Replace(@":", "");
            str = str.Replace(@"|", " ");
            str = str.Replace(@"*", " ");
            str = str.Replace(@"/", " ");
            str = str.Replace(@"'", " ");
            str = str.Insert(1, @":");
            return str;

        }

        //получение html кода
        public string get_HTML(string url)
        {
            string html_str;

            using (WebClient wc = new WebClient())
            {
                IWebProxy wp = WebRequest.DefaultWebProxy;
                wp.Credentials = CredentialCache.DefaultCredentials;
                wc.Proxy = wp;
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.2526.73 Safari/537.36");
                html_str = Encoding.UTF8.GetString(wc.DownloadData(url));
                return html_str;
            }

            //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);//запрос

            //var proxy = req.Proxy;
            //if (proxy != null)
            //{
            //    string proxyuri = proxy.GetProxy(req.RequestUri).ToString();
            //    req.UseDefaultCredentials = true;
            //    req.Proxy = new WebProxy(proxyuri, false);
            //    req.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            //}

            //req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.2526.73 Safari/537.36";

            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();//ответ
            //StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);//созданеи потока для получения ответа, перекодтровать в utf8
            //html_str = stream.ReadToEnd();//слушать ответ до конца
            //return html_str;
        }

        public void downloadFile(string chUrl, string img_path)
        {
            using (WebClient wc = new WebClient())
            {
                IWebProxy wp = WebRequest.DefaultWebProxy;
                wp.Credentials = CredentialCache.DefaultCredentials;
                wc.Proxy = wp;
                wc.Headers.Add("user-agent",
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.2526.73 Safari/537.36");
                wc.DownloadFile(chUrl, img_path);
            }
        }

        //фильтрация и корректировка ссылки
            public string filter_site_link(string site_link) 
        {
            if (!site_link.Contains("http://") && !site_link.Contains("https://"))
            {
                site_link = "http://" + "" + site_link;//проверка и правка адреса строки на протокол http
            }
            site_link = site_link.EndsWith(@"/")?site_link.Remove(site_link.Length - 1, 1):site_link;//убирает последний слеш если есть
            return site_link;
        }

        //создание формата изображения типа "abc" т.е 001
        public string convert_number_page(int count)
        {
            string str;
            str = count.ToString();
            if (count < 100) str = "0" + count;
            if (count < 10) str = "00" + count;

            return str;
        }
      
   
    }

}

