namespace bitruisseau_RomainSchertenleib
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnModifier = new Button();
            txtPath = new TextBox();
            dataGridView1 = new DataGridView();
            titre = new DataGridViewTextBoxColumn();
            artiste = new DataGridViewTextBoxColumn();
            annee = new DataGridViewTextBoxColumn();
            duree = new DataGridViewTextBoxColumn();
            taille = new DataGridViewTextBoxColumn();
            featuring = new DataGridViewTextBoxColumn();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // btnModifier
            // 
            btnModifier.Location = new Point(623, 121);
            btnModifier.Name = "btnModifier";
            btnModifier.Size = new Size(75, 23);
            btnModifier.TabIndex = 0;
            btnModifier.Text = "modifier";
            btnModifier.UseVisualStyleBackColor = true;
            btnModifier.Click += btnModifier_Click;
            // 
            // txtPath
            // 
            txtPath.Location = new Point(12, 122);
            txtPath.Name = "txtPath";
            txtPath.Size = new Size(590, 23);
            txtPath.TabIndex = 1;
            txtPath.TextChanged += textBox1_TextChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { titre, artiste, annee, duree, taille, featuring });
            dataGridView1.Location = new Point(12, 207);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(643, 408);
            dataGridView1.TabIndex = 2;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // titre
            // 
            titre.HeaderText = "Titre";
            titre.Name = "titre";
            // 
            // artiste
            // 
            artiste.HeaderText = "Artiste";
            artiste.Name = "artiste";
            // 
            // annee
            // 
            annee.HeaderText = "Année";
            annee.Name = "annee";
            // 
            // duree
            // 
            duree.HeaderText = "Durée";
            duree.Name = "duree";
            // 
            // taille
            // 
            taille.HeaderText = "Taille";
            taille.Name = "taille";
            // 
            // featuring
            // 
            featuring.HeaderText = "Featuring";
            featuring.Name = "featuring";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(528, 24);
            label1.Name = "label1";
            label1.Size = new Size(184, 15);
            label1.TabIndex = 3;
            label1.Text = "Bit-Ruisseau Romain Schertenleib";
            label1.Click += label1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1465, 637);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(txtPath);
            Controls.Add(btnModifier);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnModifier;
        private TextBox txtPath;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn titre;
        private DataGridViewTextBoxColumn artiste;
        private DataGridViewTextBoxColumn annee;
        private DataGridViewTextBoxColumn duree;
        private DataGridViewTextBoxColumn taille;
        private DataGridViewTextBoxColumn featuring;
        private Label label1;
    }
}
