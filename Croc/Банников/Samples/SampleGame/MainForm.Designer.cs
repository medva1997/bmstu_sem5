namespace SampleGame
{
    partial class MainForm
    {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.text = new System.Windows.Forms.TextBox();
            this.letters = new System.Windows.Forms.Panel();
            this.reset = new System.Windows.Forms.Button();
            this.back = new System.Windows.Forms.Button();
            this.listWords = new System.Windows.Forms.ListBox();
            this.newGame = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.Button();
            this.labelFound = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // text
            // 
            this.text.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text.Enabled = false;
            this.text.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.text.Location = new System.Drawing.Point(11, 10);
            this.text.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(459, 53);
            this.text.TabIndex = 0;
            // 
            // letters
            // 
            this.letters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.letters.AutoSize = true;
            this.letters.Location = new System.Drawing.Point(11, 64);
            this.letters.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.letters.Name = "letters";
            this.letters.Size = new System.Drawing.Size(459, 91);
            this.letters.TabIndex = 1;
            // 
            // reset
            // 
            this.reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.reset.ForeColor = System.Drawing.Color.Red;
            this.reset.Location = new System.Drawing.Point(12, 161);
            this.reset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(85, 77);
            this.reset.TabIndex = 2;
            this.reset.Text = "X";
            this.reset.UseVisualStyleBackColor = false;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // back
            // 
            this.back.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.back.ForeColor = System.Drawing.Color.SaddleBrown;
            this.back.Location = new System.Drawing.Point(12, 242);
            this.back.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(85, 77);
            this.back.TabIndex = 2;
            this.back.Text = "<";
            this.back.UseVisualStyleBackColor = false;
            this.back.Click += new System.EventHandler(this.back_Click);
            // 
            // listWords
            // 
            this.listWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listWords.FormattingEnabled = true;
            this.listWords.ItemHeight = 16;
            this.listWords.Location = new System.Drawing.Point(104, 161);
            this.listWords.Name = "listWords";
            this.listWords.Size = new System.Drawing.Size(191, 324);
            this.listWords.Sorted = true;
            this.listWords.TabIndex = 3;
            // 
            // newGame
            // 
            this.newGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.newGame.ForeColor = System.Drawing.Color.ForestGreen;
            this.newGame.Location = new System.Drawing.Point(13, 323);
            this.newGame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.newGame.Name = "newGame";
            this.newGame.Size = new System.Drawing.Size(85, 77);
            this.newGame.TabIndex = 2;
            this.newGame.Text = "*";
            this.newGame.UseVisualStyleBackColor = false;
            this.newGame.Click += new System.EventHandler(this.new_Click);
            // 
            // add
            // 
            this.add.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.add.ForeColor = System.Drawing.Color.Navy;
            this.add.Location = new System.Drawing.Point(13, 404);
            this.add.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(85, 77);
            this.add.TabIndex = 2;
            this.add.Text = "+";
            this.add.UseVisualStyleBackColor = false;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // labelFound
            // 
            this.labelFound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFound.AutoSize = true;
            this.labelFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelFound.Location = new System.Drawing.Point(316, 161);
            this.labelFound.Name = "labelFound";
            this.labelFound.Size = new System.Drawing.Size(152, 55);
            this.labelFound.TabIndex = 4;
            this.labelFound.Text = "label1";
            // 
            // labelTotal
            // 
            this.labelTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotal.AutoSize = true;
            this.labelTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTotal.Location = new System.Drawing.Point(318, 430);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(152, 55);
            this.labelTotal.TabIndex = 5;
            this.labelTotal.Text = "label2";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(480, 508);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.labelFound);
            this.Controls.Add(this.listWords);
            this.Controls.Add(this.add);
            this.Controls.Add(this.newGame);
            this.Controls.Add(this.back);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.letters);
            this.Controls.Add(this.text);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Игрушечка";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text;
        private System.Windows.Forms.Panel letters;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button back;
        private System.Windows.Forms.ListBox listWords;
        private System.Windows.Forms.Button newGame;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Label labelFound;
        private System.Windows.Forms.Label labelTotal;
    }
}

