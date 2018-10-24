namespace MangaDownloader
{
    partial class MangaDownloaderForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Way_to_save = new System.Windows.Forms.Button();
            this.Found_parts = new System.Windows.Forms.CheckedListBox();
            this.Text_way_to_save = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Link_to_manga = new System.Windows.Forms.TextBox();
            this.MangaNameLbl = new System.Windows.Forms.Label();
            this.Search_parts = new System.Windows.Forms.Button();
            this.Select_all = new System.Windows.Forms.Button();
            this.Select_no = new System.Windows.Forms.Button();
            this.Download_this = new System.Windows.Forms.Button();
            this.About = new System.Windows.Forms.Button();
            this.mangaFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.About_found = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.stop = new System.Windows.Forms.Button();
            this.downloadBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.display_link = new System.Windows.Forms.Label();
            this.pause = new System.Windows.Forms.Button();
            this.down_paret_now = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.convert_cbr = new System.Windows.Forms.CheckBox();
            this.convert_cbz = new System.Windows.Forms.CheckBox();
            this.convert_cbr_big = new System.Windows.Forms.CheckBox();
            this.convert_cbz_big = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Way_to_save
            // 
            this.Way_to_save.Location = new System.Drawing.Point(1097, 71);
            this.Way_to_save.Margin = new System.Windows.Forms.Padding(4);
            this.Way_to_save.Name = "Way_to_save";
            this.Way_to_save.Size = new System.Drawing.Size(211, 25);
            this.Way_to_save.TabIndex = 0;
            this.Way_to_save.Text = "Выбрать путь сохранения";
            this.Way_to_save.UseVisualStyleBackColor = true;
            this.Way_to_save.Click += new System.EventHandler(this.Way_to_save_Click);
            // 
            // Found_parts
            // 
            this.Found_parts.CheckOnClick = true;
            this.Found_parts.FormattingEnabled = true;
            this.Found_parts.Location = new System.Drawing.Point(6, 190);
            this.Found_parts.Margin = new System.Windows.Forms.Padding(4);
            this.Found_parts.Name = "Found_parts";
            this.Found_parts.Size = new System.Drawing.Size(1082, 531);
            this.Found_parts.TabIndex = 0;
            this.Found_parts.TabStop = false;
            this.Found_parts.UseTabStops = false;
            // 
            // Text_way_to_save
            // 
            this.Text_way_to_save.Location = new System.Drawing.Point(7, 74);
            this.Text_way_to_save.Margin = new System.Windows.Forms.Padding(4);
            this.Text_way_to_save.Name = "Text_way_to_save";
            this.Text_way_to_save.Size = new System.Drawing.Size(1082, 22);
            this.Text_way_to_save.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Место сохранения файлов";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 98);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ссылка на мангу";
            // 
            // Link_to_manga
            // 
            this.Link_to_manga.Location = new System.Drawing.Point(7, 118);
            this.Link_to_manga.Margin = new System.Windows.Forms.Padding(4);
            this.Link_to_manga.Name = "Link_to_manga";
            this.Link_to_manga.Size = new System.Drawing.Size(1082, 22);
            this.Link_to_manga.TabIndex = 5;
            this.Link_to_manga.Enter += new System.EventHandler(this.Link_to_manga_Enter);
            this.Link_to_manga.Leave += new System.EventHandler(this.Link_to_manga_Leave);
            // 
            // MangaNameLbl
            // 
            this.MangaNameLbl.AutoSize = true;
            this.MangaNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MangaNameLbl.Location = new System.Drawing.Point(145, 158);
            this.MangaNameLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MangaNameLbl.Name = "MangaNameLbl";
            this.MangaNameLbl.Size = new System.Drawing.Size(128, 17);
            this.MangaNameLbl.TabIndex = 6;
            this.MangaNameLbl.Text = "Название манги";
            // 
            // Search_parts
            // 
            this.Search_parts.Location = new System.Drawing.Point(1097, 117);
            this.Search_parts.Margin = new System.Windows.Forms.Padding(4);
            this.Search_parts.Name = "Search_parts";
            this.Search_parts.Size = new System.Drawing.Size(211, 25);
            this.Search_parts.TabIndex = 7;
            this.Search_parts.Text = "Поиск глав по ссылке";
            this.Search_parts.UseVisualStyleBackColor = true;
            this.Search_parts.Click += new System.EventHandler(this.Search_parts_Click);
            // 
            // Select_all
            // 
            this.Select_all.Location = new System.Drawing.Point(1097, 189);
            this.Select_all.Margin = new System.Windows.Forms.Padding(4);
            this.Select_all.Name = "Select_all";
            this.Select_all.Size = new System.Drawing.Size(211, 25);
            this.Select_all.TabIndex = 8;
            this.Select_all.Text = "Выбрать все";
            this.Select_all.UseVisualStyleBackColor = true;
            this.Select_all.Click += new System.EventHandler(this.Select_all_Click);
            // 
            // Select_no
            // 
            this.Select_no.Location = new System.Drawing.Point(1097, 218);
            this.Select_no.Margin = new System.Windows.Forms.Padding(4);
            this.Select_no.Name = "Select_no";
            this.Select_no.Size = new System.Drawing.Size(211, 25);
            this.Select_no.TabIndex = 9;
            this.Select_no.Text = "Снять выбор со всех";
            this.Select_no.UseVisualStyleBackColor = true;
            this.Select_no.Click += new System.EventHandler(this.Select_no_Click);
            // 
            // Download_this
            // 
            this.Download_this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Download_this.Location = new System.Drawing.Point(1097, 370);
            this.Download_this.Margin = new System.Windows.Forms.Padding(4);
            this.Download_this.Name = "Download_this";
            this.Download_this.Size = new System.Drawing.Size(211, 156);
            this.Download_this.TabIndex = 10;
            this.Download_this.Text = "Начать скачивание выбраных глав";
            this.Download_this.UseVisualStyleBackColor = true;
            this.Download_this.Click += new System.EventHandler(this.Download_this_Click);
            // 
            // About
            // 
            this.About.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.About.Location = new System.Drawing.Point(1097, 660);
            this.About.Margin = new System.Windows.Forms.Padding(4);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(211, 50);
            this.About.TabIndex = 11;
            this.About.Text = "О программе";
            this.About.UseVisualStyleBackColor = true;
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // About_found
            // 
            this.About_found.AutoSize = true;
            this.About_found.Location = new System.Drawing.Point(5, 158);
            this.About_found.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.About_found.Name = "About_found";
            this.About_found.Size = new System.Drawing.Size(98, 17);
            this.About_found.TabIndex = 12;
            this.About_found.Text = "Найдено глав";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.status.ForeColor = System.Drawing.Color.Green;
            this.status.Location = new System.Drawing.Point(461, 817);
            this.status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(115, 17);
            this.status.TabIndex = 14;
            this.status.Text = "Состояние ОК";
            this.status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1066, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "Программа для скачивания манги с сайов http://mintmanga.com, http://readmanga.me/" +
    ", http://selfmanga.ru/, https://manga24.ru/, https://mangalib.me/, https://bato." +
    "to/,";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 798);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1082, 15);
            this.progressBar1.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 775);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(777, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "Прогресс скачивания текущей главы. Во время скачивания в программе невозможно про" +
    "изводить другие действия.";
            // 
            // stop
            // 
            this.stop.Enabled = false;
            this.stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.stop.Location = new System.Drawing.Point(1097, 589);
            this.stop.Margin = new System.Windows.Forms.Padding(4);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(211, 42);
            this.stop.TabIndex = 19;
            this.stop.Text = "Стоп";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // display_link
            // 
            this.display_link.AutoSize = true;
            this.display_link.Location = new System.Drawing.Point(3, 740);
            this.display_link.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.display_link.Name = "display_link";
            this.display_link.Size = new System.Drawing.Size(420, 17);
            this.display_link.TabIndex = 20;
            this.display_link.Text = "Ссылка на закачиваемую страницу: Закачка не производится";
            // 
            // pause
            // 
            this.pause.Enabled = false;
            this.pause.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pause.Location = new System.Drawing.Point(1097, 539);
            this.pause.Margin = new System.Windows.Forms.Padding(4);
            this.pause.Name = "pause";
            this.pause.Size = new System.Drawing.Size(211, 42);
            this.pause.TabIndex = 21;
            this.pause.Text = "Пауза";
            this.pause.UseVisualStyleBackColor = true;
            this.pause.Click += new System.EventHandler(this.pause_Click);
            // 
            // down_paret_now
            // 
            this.down_paret_now.AutoSize = true;
            this.down_paret_now.Location = new System.Drawing.Point(3, 759);
            this.down_paret_now.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.down_paret_now.Name = "down_paret_now";
            this.down_paret_now.Size = new System.Drawing.Size(315, 17);
            this.down_paret_now.TabIndex = 27;
            this.down_paret_now.Text = "Скачиваемая глава: Закачка не производится";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 27);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(427, 17);
            this.label6.TabIndex = 28;
            this.label6.Text = "http://mangachan.me/, http://hentai-chan.me/ и http://yaoichan.me/";
            // 
            // convert_cbr
            // 
            this.convert_cbr.AutoSize = true;
            this.convert_cbr.Location = new System.Drawing.Point(1097, 254);
            this.convert_cbr.Margin = new System.Windows.Forms.Padding(4);
            this.convert_cbr.Name = "convert_cbr";
            this.convert_cbr.Size = new System.Drawing.Size(215, 21);
            this.convert_cbr.TabIndex = 31;
            this.convert_cbr.Text = "Конвертировать главы в cbr";
            this.convert_cbr.UseVisualStyleBackColor = true;
            // 
            // convert_cbz
            // 
            this.convert_cbz.AutoSize = true;
            this.convert_cbz.Location = new System.Drawing.Point(1097, 283);
            this.convert_cbz.Margin = new System.Windows.Forms.Padding(4);
            this.convert_cbz.Name = "convert_cbz";
            this.convert_cbz.Size = new System.Drawing.Size(217, 21);
            this.convert_cbz.TabIndex = 32;
            this.convert_cbz.Text = "Конвертировать главы в cbz";
            this.convert_cbz.UseVisualStyleBackColor = true;
            // 
            // convert_cbr_big
            // 
            this.convert_cbr_big.AutoSize = true;
            this.convert_cbr_big.Location = new System.Drawing.Point(1097, 312);
            this.convert_cbr_big.Margin = new System.Windows.Forms.Padding(4);
            this.convert_cbr_big.Name = "convert_cbr_big";
            this.convert_cbr_big.Size = new System.Drawing.Size(206, 21);
            this.convert_cbr_big.TabIndex = 33;
            this.convert_cbr_big.Text = "Создать один большой cbr";
            this.convert_cbr_big.UseVisualStyleBackColor = true;
            // 
            // convert_cbz_big
            // 
            this.convert_cbz_big.AutoSize = true;
            this.convert_cbz_big.Location = new System.Drawing.Point(1097, 341);
            this.convert_cbz_big.Margin = new System.Windows.Forms.Padding(4);
            this.convert_cbz_big.Name = "convert_cbz_big";
            this.convert_cbz_big.Size = new System.Drawing.Size(208, 21);
            this.convert_cbz_big.TabIndex = 34;
            this.convert_cbz_big.Text = "Создать один большой cbz";
            this.convert_cbz_big.UseVisualStyleBackColor = true;
            // 
            // MangaDownloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 871);
            this.Controls.Add(this.convert_cbz_big);
            this.Controls.Add(this.convert_cbr_big);
            this.Controls.Add(this.convert_cbz);
            this.Controls.Add(this.convert_cbr);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.down_paret_now);
            this.Controls.Add(this.pause);
            this.Controls.Add(this.display_link);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.status);
            this.Controls.Add(this.About_found);
            this.Controls.Add(this.About);
            this.Controls.Add(this.Download_this);
            this.Controls.Add(this.Select_no);
            this.Controls.Add(this.Select_all);
            this.Controls.Add(this.Search_parts);
            this.Controls.Add(this.MangaNameLbl);
            this.Controls.Add(this.Link_to_manga);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Text_way_to_save);
            this.Controls.Add(this.Found_parts);
            this.Controls.Add(this.Way_to_save);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MangaDownloaderForm";
            this.Text = "MangaDownloader";
            this.Load += new System.EventHandler(this.MangaDownloaderForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Way_to_save;
        private System.Windows.Forms.CheckedListBox Found_parts;
        private System.Windows.Forms.TextBox Text_way_to_save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Link_to_manga;
        private System.Windows.Forms.Label MangaNameLbl;
        private System.Windows.Forms.Button Search_parts;
        private System.Windows.Forms.Button Select_all;
        private System.Windows.Forms.Button Select_no;
        private System.Windows.Forms.Button Download_this;
        private System.Windows.Forms.Button About;
        private System.Windows.Forms.FolderBrowserDialog mangaFolderBrowserDialog;
        private System.Windows.Forms.Label About_found;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button stop;
        private System.ComponentModel.BackgroundWorker downloadBackgroundWorker;
        private System.Windows.Forms.Label display_link;
        private System.Windows.Forms.Button pause;
        private System.Windows.Forms.Label down_paret_now;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox convert_cbr;
        private System.Windows.Forms.CheckBox convert_cbz;
        private System.Windows.Forms.CheckBox convert_cbr_big;
        private System.Windows.Forms.CheckBox convert_cbz_big;
    }
}

