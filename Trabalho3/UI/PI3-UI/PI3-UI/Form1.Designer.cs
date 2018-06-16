namespace PI3_UI
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.NumImagem = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LuckMessage = new System.Windows.Forms.Label();
            this.LuckNum = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NumImagem)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(205, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(827, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bem vindo, para saber sua sorte selecione uma imagem do número de 0 a 9";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(340, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "Selecionar Imagem";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NumImagem
            // 
            this.NumImagem.Location = new System.Drawing.Point(43, 254);
            this.NumImagem.Name = "NumImagem";
            this.NumImagem.Size = new System.Drawing.Size(263, 330);
            this.NumImagem.TabIndex = 2;
            this.NumImagem.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(340, 321);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(167, 35);
            this.button2.TabIndex = 3;
            this.button2.Text = "Gerar Sorte de Hoje";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(703, 233);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 32);
            this.label2.TabIndex = 4;
            this.label2.Text = "Frase da Sorte";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(703, 338);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(243, 32);
            this.label3.TabIndex = 5;
            this.label3.Text = "Números da Sorte";
            // 
            // LuckMessage
            // 
            this.LuckMessage.AutoSize = true;
            this.LuckMessage.BackColor = System.Drawing.Color.Transparent;
            this.LuckMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LuckMessage.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LuckMessage.Location = new System.Drawing.Point(705, 285);
            this.LuckMessage.Name = "LuckMessage";
            this.LuckMessage.Size = new System.Drawing.Size(120, 20);
            this.LuckMessage.TabIndex = 6;
            this.LuckMessage.Text = "Frase da Sorte";
            // 
            // LuckNum
            // 
            this.LuckNum.AutoSize = true;
            this.LuckNum.BackColor = System.Drawing.Color.Transparent;
            this.LuckNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LuckNum.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LuckNum.Location = new System.Drawing.Point(705, 384);
            this.LuckNum.Name = "LuckNum";
            this.LuckNum.Size = new System.Drawing.Size(120, 20);
            this.LuckNum.TabIndex = 7;
            this.LuckNum.Text = "Frase da Sorte";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1159, 619);
            this.Controls.Add(this.LuckNum);
            this.Controls.Add(this.LuckMessage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.NumImagem);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Doge Store";
            ((System.ComponentModel.ISupportInitialize)(this.NumImagem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox NumImagem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LuckMessage;
        private System.Windows.Forms.Label LuckNum;
    }
}

