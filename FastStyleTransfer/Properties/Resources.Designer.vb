Namespace FastStyleTransfer.Properties
    <[global].System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")>
    <[global].System.Diagnostics.DebuggerNonUserCodeAttribute()>
    <[global].System.Runtime.CompilerServices.CompilerGeneratedAttribute()>
    Friend Class Resources
        Private Shared resourceMan As [global].System.Resources.ResourceManager
        Private Shared resourceCulture As [global].System.Globalization.CultureInfo

        <[global].System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
        Friend Sub New()
        End Sub

        <[global].System.ComponentModel.EditorBrowsableAttribute([global].System.ComponentModel.EditorBrowsableState.Advanced)>
        Friend Shared ReadOnly Property ResourceManager As [global].System.Resources.ResourceManager
            Get

                If (resourceMan Is Nothing) Then
                    Dim temp As [global].System.Resources.ResourceManager = New [global].System.Resources.ResourceManager("FastStyleTransfer.Properties.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If

                Return resourceMan
            End Get
        End Property

        <[global].System.ComponentModel.EditorBrowsableAttribute([global].System.ComponentModel.EditorBrowsableState.Advanced)>
        Friend Shared Property Culture As [global].System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set(ByVal value As [global].System.Globalization.CultureInfo)
                resourceCulture = value
            End Set
        End Property
    End Class
End Namespace
