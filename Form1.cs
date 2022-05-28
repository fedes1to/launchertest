using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;

public class Form1 : Form
{
    static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
    string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
    private bool isDownloaded;
    private BackgroundWorker backgroundWorker1;
    private Button downloadButton;
    private ProgressBar progressBar1;
    //  private TextBox logo;
    private TextBox changelog;
    private bool needUpdate;
    public string newversion;
    public string oldversion;
    public string[] change;
    private PictureBox logo;
    private Button discord1;
    private Button discord2;
    private Button youtube1;
    private Button youtube2;
    private Button github1;


    public Form1()
    {
        try {
        oldversion = File.ReadAllText(strWorkPath + "/Game/version.txt");
        newversion = File.ReadAllText(strWorkPath + "/version.txt");
        if (oldversion != newversion) {
            needUpdate = true;
        }
        }
        catch {}
        using (WebClient client2 = new WebClient()) {
           client2.DownloadFile(new Uri("https://cdn.discordapp.com/attachments/950079803749445762/967431928716361728/Loading_Day_D.png"), strWorkPath + "/background.png");
           client2.DownloadFile(new Uri("https://cdn.discordapp.com/attachments/950079803749445762/977952193179156500/1653232363670.png"), strWorkPath + "/logo.png");
           client2.DownloadFile(new Uri("https://cdn.discordapp.com/attachments/950079803749445762/978365195766333450/version.txt"), strWorkPath + "/version.txt");
           client2.DownloadFile(new Uri("https://cdn.discordapp.com/attachments/886309159795060827/977847309687726080/icon.ico"), strWorkPath + "/icon.ico");
        }
        if (File.Exists(strWorkPath + "/Game/version.txt")) {
            oldversion = File.ReadAllText(strWorkPath + "/Game/version.txt");
            newversion = File.ReadAllText(strWorkPath + "/version.txt");
        }
        if (newversion != oldversion) {
            needUpdate = true;
        }
        isDownloaded = Directory.Exists(strWorkPath + "/Game");
        InitializeComponent();
        
        // Instantiate BackgroundWorker and attach handlers to its
        // DoWork and RunWorkerCompleted events.
        backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
        backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
        backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
    }

    private void downloadButton_Click(object sender, EventArgs e)
    {
        if (!needUpdate && isDownloaded) {
            var gameProcess = new Process();
            gameProcess = Process.Start(strWorkPath + "/Game/dnSpy.exe");
            Hide();
            gameProcess.WaitForExit();
            Show();
            return;
        }

        // Start the download operation in the background.
        this.backgroundWorker1.RunWorkerAsync();
        this.progressBar1.Show();

        // Disable the button for the duration of the logo
        this.downloadButton.Enabled = false;

        // Once you have started the background thread you
        // can exit the handler and the application will
        // wait until the RunWorkerCompleted event is raised.

        // Or if you want to do something else in the main thread,
        // such as update a progress bar, you can do so in a loop
        // while checking IsBusy to see if the background task is
        // still running.

        while (this.backgroundWorker1.IsBusy)
        {
            // Keep UI messages moving, so the form remains
            // responsive during the asynchronous operation.
            Application.DoEvents();
        }
    }

    private void discord1Button_Click(object sender, EventArgs e)
    {
        ProcessStartInfo psInfo = new ProcessStartInfo {
        FileName = "https://discord.gg/58dK3u65PE",
        UseShellExecute = true
        };
        Process.Start(psInfo);
    }

    private void discord2Button_Click(object sender, EventArgs e)
    {
        ProcessStartInfo psInfo = new ProcessStartInfo {
        FileName = "https://discord.gg/trZM2XRCQU",
        UseShellExecute = true
        };
        Process.Start(psInfo);
    }

    private void youtube1Button_Click(object sender, EventArgs e)
    {
        ProcessStartInfo psInfo = new ProcessStartInfo {
        FileName = "https://www.youtube.com/c/RobDEV",
        UseShellExecute = true
        };
        Process.Start(psInfo);
    }

    private void youtube2Button_Click(object sender, EventArgs e)
    {
        ProcessStartInfo psInfo = new ProcessStartInfo {
        FileName = "https://www.youtube.com/user/nobody7123",
        UseShellExecute = true
        };
        Process.Start(psInfo);
    }

    private void github1Button_Click(object sender, EventArgs e)
    {
        ProcessStartInfo psInfo = new ProcessStartInfo {
        FileName = "https://github.com/lbertitoyt",
        UseShellExecute = true
        };
        Process.Start(psInfo);
    }

    public void client_DownloadProgressChanged(Object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

    private void backgroundWorker1_DoWork(
        object sender,
        DoWorkEventArgs e)
    {
        // here download code
        using (WebClient client = new WebClient()) {
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            client.DownloadFileAsync(new Uri("https://download963.mediafire.com/flfbl5h5b8gg/ie0qt2b0g1ad7xq/dnSpyex-netframework.zip"), strWorkPath + "/game.zip");
            if (Directory.Exists(strWorkPath + "/Game")) {
                System.IO.Directory.Delete(strWorkPath + "/Game", true);
            }
        }
    }

    private void backgroundWorker1_RunWorkerCompleted(
        object sender,
        System.ComponentModel.AsyncCompletedEventArgs e)
    {
        // Set progress bar to 100% in case it's not already there.
        if (progressBar1.Value == 100) {
        if (e.Error == null)
        {
            this.isDownloaded = true;
            MessageBox.Show("Download Complete, wait a minute for files to extract");
            ZipFile.ExtractToDirectory(strWorkPath + "/game.zip", strWorkPath + "/Game");
            File.Delete(strWorkPath + "/game.zip");
            this.progressBar1.Hide();
            this.downloadButton.Text = "Launch Game";

        }
        else
        {
            MessageBox.Show(
                "Failed to download file" + e.Error.ToString(),
                "Download failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        // Enable the download button and reset the progress bar.
        this.downloadButton.Enabled = true;
        progressBar1.Value = 0;
        }
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// Required method for Designer support
    /// </summary>
    private void InitializeComponent()
    {
        this.downloadButton = new System.Windows.Forms.Button();
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.logo = new PictureBox();
        this.SuspendLayout();
        //
        // downloadButton
        //
        this.downloadButton.Location = new System.Drawing.Point(765, 400);
        this.downloadButton.Name = "downloadButton";
        this.downloadButton.Size = new System.Drawing.Size(100, 23);
        this.downloadButton.TabIndex = 0;
        this.downloadButton.UseVisualStyleBackColor = true;
        this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
        if (!isDownloaded && !needUpdate) {
            this.downloadButton.Text = "Download Game";
        } else if (needUpdate && isDownloaded) {
            this.downloadButton.Text = "Update Game";
        } else {
            this.downloadButton.Text = "Launch Game";
        }
        //
        // progressBar1
        //
        this.progressBar1.Location = new System.Drawing.Point(50, 402);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(666, 20);
        this.progressBar1.TabIndex = 1;
        this.progressBar1.Hide();
        //
        // Form1
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(900, 450);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.Controls.Add(this.progressBar1);
        this.Controls.Add(this.downloadButton);
        this.Name = "PG Launcher";
        this.Text = "PG Launcher";
        this.Icon = Icon.ExtractAssociatedIcon(strWorkPath + "/icon.ico");
        this.ResumeLayout(false);
        if (File.Exists(strWorkPath + "/background.png")) {
            this.BackgroundImage = Image.FromFile(strWorkPath + "/background.png");
        }
        //
        // LOGO IMAGE CODE
        //
        this.logo.Location = new Point(300, 20);
        this.logo.Name = "picLogo";
        this.logo.Size = new Size(450, 60);
        this.logo.BackgroundImage = Image.FromFile(strWorkPath + "/logo.png"); // TBD, ADD THE IMAGE
        this.logo.BorderStyle = BorderStyle.FixedSingle;
        this.Controls.Add(this.logo);
        //
        // textbox (name of the game)
        //
    /*  USE THIS IF YOU AREN'T GOING TO USE A PREDEFINED LOGO
        this.logo = new System.Windows.Forms.TextBox();
        this.logo.Text = "Pixel Gun THINGY Launcher";
        this.logo.ReadOnly = true;
        this.logo.AcceptsReturn = false;
        this.logo.AcceptsTab = false;
        this.logo.Enabled = false;
        this.logo.Location = new Point(290, 10);
        this.logo.Font = new Font("Arial", 30, FontStyle.Regular);
        this.logo.Size = new Size(500, 100);
        this.logo.BorderStyle = BorderStyle.None;
        this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        this.logo.BackColor = this.BackColor;
        this.Controls.Add(this.logo);
    */
        //
        // textbox (changelog)
        //
        using (WebClient client = new WebClient()) {
            client.DownloadFile("https://cdn.discordapp.com/attachments/950079803749445762/978366667014946846/changelog.txt", strWorkPath + "/changelog.txt");
        }
        if (File.Exists(strWorkPath + "/changelog.txt")){
            change = File.ReadAllLines(strWorkPath + "/changelog.txt");
        }
        this.changelog = new System.Windows.Forms.TextBox();
        this.changelog.ReadOnly = true;
        this.changelog.Multiline = true;
        this.changelog.Size = new Size(600, 350);
        this.changelog.Location = new Point(60, 100);
        this.changelog.ScrollBars = ScrollBars.Vertical;
        if (change.Length != 0) 
        {
            for (int i = 0; i < change.Length; i++) {
            this.changelog.AppendText(change[i] + "\r\n");
        } 
        }
        this.Controls.Add(this.changelog);
        //
        // social buttons
        //
        this.discord1 = new Button();
        this.discord1.Location = new System.Drawing.Point(775, 150);
        this.discord1.Name = "downloadButton";
        this.discord1.Size = new System.Drawing.Size(150, 25);
        this.discord1.TabIndex = 0;
        this.discord1.UseVisualStyleBackColor = true;
        this.discord1.Click += new System.EventHandler(this.discord1Button_Click);
        this.discord1.BackColor = Color.DarkBlue;
        this.discord1.TabStop = false;
        this.discord1.FlatStyle = FlatStyle.Flat;
        this.discord1.FlatAppearance.BorderSize = 1;
        this.discord1.Text = "Join Old PG3D";
        this.Controls.Add(discord1);

        this.discord2 = new Button();
        this.discord2.Location = new System.Drawing.Point(775, 200);
        this.discord2.Name = "downloadButton";
        this.discord2.Size = new System.Drawing.Size(150, 25);
        this.discord2.TabIndex = 0;
        this.discord2.UseVisualStyleBackColor = true;
        this.discord2.Click += new System.EventHandler(this.discord2Button_Click);
        this.discord2.BackColor = Color.DarkBlue;
        this.discord2.TabStop = false;
        this.discord2.FlatStyle = FlatStyle.Flat;
        this.discord2.FlatAppearance.BorderSize = 1;
        this.discord2.Text = "Join PC PG3D";
        this.Controls.Add(discord2);

        this.youtube1 = new Button();
        this.youtube1.Location = new System.Drawing.Point(775, 250);
        this.youtube1.Name = "downloadButton";
        this.youtube1.Size = new System.Drawing.Size(150, 25);
        this.youtube1.TabIndex = 0;
        this.youtube1.UseVisualStyleBackColor = true;
        this.youtube1.Click += new System.EventHandler(this.youtube1Button_Click);
        this.youtube1.BackColor = Color.DarkRed;
        this.youtube1.TabStop = false;
        this.youtube1.FlatStyle = FlatStyle.Flat;
        this.youtube1.FlatAppearance.BorderSize = 1;
        this.youtube1.Text = "Sub to RobDEV!";
        this.Controls.Add(youtube1);

        this.youtube2 = new Button();
        this.youtube2.Location = new System.Drawing.Point(775, 300);
        this.youtube2.Name = "downloadButton";
        this.youtube2.Size = new System.Drawing.Size(150, 25);
        this.youtube2.TabIndex = 0;
        this.youtube2.UseVisualStyleBackColor = true;
        this.youtube2.Click += new System.EventHandler(this.youtube2Button_Click);
        this.youtube2.BackColor = Color.DarkRed;
        this.youtube2.TabStop = false;
        this.youtube2.FlatStyle = FlatStyle.Flat;
        this.youtube2.FlatAppearance.BorderSize = 1;
        this.youtube2.Text = "Sub to DashCat!";
        this.Controls.Add(youtube2);

        this.github1 = new Button();
        this.github1.Location = new System.Drawing.Point(775, 350);
        this.github1.Name = "downloadButton";
        this.github1.Size = new System.Drawing.Size(150, 25);
        this.github1.TabIndex = 0;
        this.github1.UseVisualStyleBackColor = true;
        this.github1.Click += new System.EventHandler(this.github1Button_Click);
//      this.github1.BackColor = Color.Blue;
        this.github1.TabStop = false;
        this.github1.FlatStyle = FlatStyle.Flat;
        this.github1.FlatAppearance.BorderSize = 1;
        this.github1.Text = "Follow me on GitHub";
        this.Controls.Add(github1);
    }

    #endregion
}

static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new Form1());
    }
}