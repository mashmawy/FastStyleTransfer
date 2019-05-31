Imports AlbiruniML
Imports alb = AlbiruniML.Ops
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Threading

Namespace FastStyleTransfer
    Public Partial Class FastStyleTransferDemo
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private tnet As TransformNet

        Public Shared Function ResizeImage(ByVal image As Image, ByVal width As Integer, ByVal height As Integer) As Bitmap
            Dim destRect = New Rectangle(0, 0, width, height)
            Dim destImage = New Bitmap(width, height)
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution)

            Using graphics = Graphics.FromImage(destImage)
                graphics.CompositingMode = CompositingMode.SourceCopy
                graphics.CompositingQuality = CompositingQuality.HighQuality
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear
                graphics.SmoothingMode = SmoothingMode.HighQuality
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality

                Using wrapMode = New ImageAttributes()
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY)
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode)
                End Using
            End Using

            Return destImage
        End Function

        Private imageTensor As Tensor

        Private Function LoadImage(ByVal ofd As OpenFileDialog) As Tensor
            Dim image As Image = ResizeImage(Bitmap.FromFile(ofd.FileName), 500, 500)
            SrcPictureBox.Image = image
            Dim x = alb.buffer(New Integer() {image.Height, image.Width, 3})
            Dim bmp As Bitmap = New Bitmap(image)

            For i As Integer = 0 To bmp.Height - 1

                For j As Integer = 0 To bmp.Width - 1
                    Dim clr As Color = bmp.GetPixel(j, i)
                    Dim red As Single = clr.R / 255.0F
                    Dim green As Single = clr.G / 255.0F
                    Dim blue As Single = clr.B / 255.0F
                    x.[Set](red, i, j, 0)
                    x.[Set](green, i, j, 1)
                    x.[Set](blue, i, j, 2)
                Next
            Next

            Return x.toTensor()
        End Function

        Private Sub LoadTensor(ByVal dataobject As Object)
            Dim data As Tensor = TryCast(dataobject, Tensor)
            Dim bmp As Bitmap = New Bitmap(data.Shape(0), data.Shape(1))

            For i As Integer = 0 To bmp.Height - 1

                For j As Integer = 0 To bmp.Width - 1
                    Dim col = Color.FromArgb(CInt((data.[Get](i, j, 0) * 255.0F)), CInt((data.[Get](i, j, 1) * 255.0F)), CInt((data.[Get](i, j, 2) * 255.0F)))
                    bmp.SetPixel(j, i, col)
                Next
            Next

            DisPictureBox.Image = bmp
            Me.label1.Visible = False
            Me.progressBar1.Visible = False
            Me.SaveButton.Enabled = True
            Me.TransferButton.Enabled = True
            Me.StyleComboBox.Enabled = True
            Me.BrowseButton.Enabled = True
        End Sub

        Private Sub BrowseButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim ofd As OpenFileDialog = New OpenFileDialog()
            Dim resp = ofd.ShowDialog()

            If resp = System.Windows.Forms.DialogResult.OK Then
                Me.imageTensor = LoadImage(ofd)
            End If

            Me.TransferButton.Enabled = True
        End Sub

        Friend Delegate Sub FormInvok()
        Friend Delegate Sub FormInvokWithParam(ByVal param As Object)
        Private t As Thread

        Private Sub TransferButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            Me.TransferButton.Enabled = False
            Me.label1.Visible = True
            Me.progressBar1.Visible = True
            Me.progressBar1.Value = 0
            Me.progressBar1.Maximum = 120
            Me.StyleComboBox.Enabled = False
            Me.BrowseButton.Enabled = False
            t = New Thread(New ThreadStart(AddressOf Transfer))
            t.Start()
        End Sub

        Private Sub Transfer()
            Dim sw As Stopwatch = New Stopwatch()
            sw.Start()
            Me.imageTensor.keep()
            Dim p = tnet.Predict(Me.imageTensor)
            Me(New FormInvokWithParam(AddressOf LoadTensor), p)
            sw.[Stop]()
            MessageBox.Show(sw.ElapsedMilliseconds.ToString())
        End Sub

        Private Sub FastStyleTransferDemo_Load(ByVal sender As Object, ByVal e As EventArgs)
            tnet = New TransformNet()
            AddHandler tnet.ReportProgress, AddressOf tnet_ReportProgress

            For Each item In tnet.variableDictionary
                Me.StyleComboBox.Items.Add(item.Key)
            Next

            Me.StyleComboBox.SelectedItem = Me.StyleComboBox.Items(0)
            Me.StylePictureBox.Image = New Bitmap("styles/" & Me.StyleComboBox.Items(0).ToString() & ".jpg")
        End Sub

        Private Sub tnet_ReportProgress(ByVal progress As Integer)
            Me(New FormInvokWithParam(AddressOf UpdateProgress), progress)
        End Sub

        Private Sub UpdateProgress(ByVal value As Object)
            Me.progressBar1.Value = Convert.ToInt32(value)
        End Sub

        Private Sub StyleComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.StylePictureBox.Image = New Bitmap("styles/" & Me.StyleComboBox.SelectedItem.ToString() & ".jpg")
            Me.tnet.ChangeVariable(Me.StyleComboBox.SelectedItem.ToString())
        End Sub

        Private Sub SaveButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim sfd As SaveFileDialog = New SaveFileDialog()
            Dim sd = sfd.ShowDialog()

            If sd = System.Windows.Forms.DialogResult.OK Then
                DisPictureBox.Image.Save(sfd.FileName)
            End If
        End Sub

        Private Sub FastStyleTransferDemo_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
            If t IsNot Nothing Then

                If t.IsAlive Then
                    t.Abort()
                End If
            End If
        End Sub
    End Class
End Namespace
