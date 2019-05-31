Namespace FastStyleTransfer
    Partial Public Class FastStyleTransferDemo
        Private components As System.ComponentModel.IContainer = Nothing

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

        Private Sub InitializeComponent()
            Me.SaveButton = New System.Windows.Forms.Button()
            Me.TransferButton = New System.Windows.Forms.Button()
            Me.StyleLabel = New System.Windows.Forms.Label()
            Me.StyleComboBox = New System.Windows.Forms.ComboBox()
            Me.BrowseButton = New System.Windows.Forms.Button()
            Me.StylePictureBox = New System.Windows.Forms.PictureBox()
            Me.DisPictureBox = New System.Windows.Forms.PictureBox()
            Me.SrcPictureBox = New System.Windows.Forms.PictureBox()
            Me.progressBar1 = New System.Windows.Forms.ProgressBar()
            Me.label1 = New System.Windows.Forms.Label()
            CType(Me.StylePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DisPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.SrcPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            Me.SaveButton.Enabled = False
            Me.SaveButton.Location = New System.Drawing.Point(672, 295)
            Me.SaveButton.Name = "SaveButton"
            Me.SaveButton.Size = New System.Drawing.Size(75, 23)
            Me.SaveButton.TabIndex = 17
            Me.SaveButton.Text = "Save"
            Me.SaveButton.UseVisualStyleBackColor = True
            AddHandler Me.SaveButton.Click, New System.EventHandler(AddressOf Me.SaveButton_Click)
            Me.TransferButton.Enabled = False
            Me.TransferButton.Location = New System.Drawing.Point(554, 295)
            Me.TransferButton.Name = "TransferButton"
            Me.TransferButton.Size = New System.Drawing.Size(75, 23)
            Me.TransferButton.TabIndex = 16
            Me.TransferButton.Text = "Transfer"
            Me.TransferButton.UseVisualStyleBackColor = True
            AddHandler Me.TransferButton.Click, New System.EventHandler(AddressOf Me.TransferButton_Click)
            Me.StyleLabel.AutoSize = True
            Me.StyleLabel.Location = New System.Drawing.Point(57, 295)
            Me.StyleLabel.Name = "StyleLabel"
            Me.StyleLabel.Size = New System.Drawing.Size(63, 13)
            Me.StyleLabel.TabIndex = 15
            Me.StyleLabel.Text = "Select Style"
            Me.StyleComboBox.FormattingEnabled = True
            Me.StyleComboBox.Location = New System.Drawing.Point(140, 290)
            Me.StyleComboBox.Name = "StyleComboBox"
            Me.StyleComboBox.Size = New System.Drawing.Size(121, 21)
            Me.StyleComboBox.TabIndex = 14
            AddHandler Me.StyleComboBox.SelectedIndexChanged, New System.EventHandler(AddressOf Me.StyleComboBox_SelectedIndexChanged)
            Me.BrowseButton.Location = New System.Drawing.Point(303, 290)
            Me.BrowseButton.Name = "BrowseButton"
            Me.BrowseButton.Size = New System.Drawing.Size(75, 23)
            Me.BrowseButton.TabIndex = 13
            Me.BrowseButton.Text = "Browse.."
            Me.BrowseButton.UseVisualStyleBackColor = True
            AddHandler Me.BrowseButton.Click, New System.EventHandler(AddressOf Me.BrowseButton_Click)
            Me.StylePictureBox.Location = New System.Drawing.Point(50, 36)
            Me.StylePictureBox.Name = "StylePictureBox"
            Me.StylePictureBox.Size = New System.Drawing.Size(223, 248)
            Me.StylePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.StylePictureBox.TabIndex = 12
            Me.StylePictureBox.TabStop = False
            Me.DisPictureBox.Location = New System.Drawing.Point(554, 36)
            Me.DisPictureBox.Name = "DisPictureBox"
            Me.DisPictureBox.Size = New System.Drawing.Size(223, 248)
            Me.DisPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.DisPictureBox.TabIndex = 11
            Me.DisPictureBox.TabStop = False
            Me.SrcPictureBox.Location = New System.Drawing.Point(303, 36)
            Me.SrcPictureBox.Name = "SrcPictureBox"
            Me.SrcPictureBox.Size = New System.Drawing.Size(223, 248)
            Me.SrcPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.SrcPictureBox.TabIndex = 10
            Me.SrcPictureBox.TabStop = False
            Me.progressBar1.Location = New System.Drawing.Point(170, 319)
            Me.progressBar1.Name = "progressBar1"
            Me.progressBar1.Size = New System.Drawing.Size(295, 23)
            Me.progressBar1.TabIndex = 18
            Me.progressBar1.Visible = False
            Me.label1.AutoSize = True
            Me.label1.Location = New System.Drawing.Point(98, 321)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(60, 13)
            Me.label1.TabIndex = 19
            Me.label1.Text = "Transfering"
            Me.label1.Visible = False
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(826, 354)
            Me.Controls.Add(Me.label1)
            Me.Controls.Add(Me.progressBar1)
            Me.Controls.Add(Me.SaveButton)
            Me.Controls.Add(Me.TransferButton)
            Me.Controls.Add(Me.StyleLabel)
            Me.Controls.Add(Me.StyleComboBox)
            Me.Controls.Add(Me.BrowseButton)
            Me.Controls.Add(Me.StylePictureBox)
            Me.Controls.Add(Me.DisPictureBox)
            Me.Controls.Add(Me.SrcPictureBox)
            Me.MaximizeBox = False
            Me.Name = "FastStyleTransferDemo"
            Me.Text = "Fast Style Transfer"
            AddHandler Me.FormClosing, New System.Windows.Forms.FormClosingEventHandler(AddressOf Me.FastStyleTransferDemo_FormClosing)
            AddHandler Me.Load, New System.EventHandler(AddressOf Me.FastStyleTransferDemo_Load)
            CType((Me.StylePictureBox), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.DisPictureBox), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.SrcPictureBox), System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

        Private SaveButton As System.Windows.Forms.Button
        Private TransferButton As System.Windows.Forms.Button
        Private StyleLabel As System.Windows.Forms.Label
        Private StyleComboBox As System.Windows.Forms.ComboBox
        Private BrowseButton As System.Windows.Forms.Button
        Private StylePictureBox As System.Windows.Forms.PictureBox
        Private DisPictureBox As System.Windows.Forms.PictureBox
        Private SrcPictureBox As System.Windows.Forms.PictureBox
        Private progressBar1 As System.Windows.Forms.ProgressBar
        Private label1 As System.Windows.Forms.Label
    End Class
End Namespace
