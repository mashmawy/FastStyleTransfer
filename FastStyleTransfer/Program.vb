Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace FastStyleTransfer
    Friend Module Program
        <STAThread>
        Private Sub Main()
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New FastStyleTransferDemo())
        End Sub
    End Module
End Namespace
