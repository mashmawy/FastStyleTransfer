Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports alb = AlbiruniML.Ops
Imports AlbiruniML
Imports Newtonsoft.Json
Imports System.IO
Imports System.Diagnostics
Imports System.Windows.Forms

Namespace FastStyleTransfer
    Public Class TransformNet
        Private variables As Dictionary(Of String, Tensor) = New Dictionary(Of String, Tensor)()
        Public variableDictionary As Dictionary(Of String, Dictionary(Of String, Tensor)) = New Dictionary(Of String, Dictionary(Of String, Tensor))()
        Private timesScalar As Tensor
        Private plusScalar As Tensor
        Private epsilonScalar As Tensor

        Private Shared Function ReadW(ByVal filename As String) As Single()
            Dim d = File.ReadAllBytes(filename)
            Dim s As Single() = New Single(d.Length / 4 - 1) {}
            Dim c As Integer = 0

            For i As Integer = 0 To d.Length - 1 Step 4
                Dim num As Byte() = New Byte(3) {}
                num(0) = d(i)
                num(1) = d(i + 1)
                num(2) = d(i + 2)
                num(3) = d(i + 3)
                s(c) = ToFloat(num)
                c += 1
            Next

            Return s
        End Function

        Private Shared Function ToFloat(ByVal input As Byte()) As Single
            Dim newArray As Byte() = {input(0), input(1), input(2), input(3)}
            Return BitConverter.ToSingle(newArray, 0)
        End Function

        Public Function LoadFolder(ByVal folderName As String) As Dictionary(Of String, Tensor)
            Dim obj As Dictionary(Of String, modelmetainfo) = JsonConvert.DeserializeObject(Of Dictionary(Of String, modelmetainfo))(File.ReadAllText(folderName & "/manifest.json"))
            Dim res As Dictionary(Of String, Tensor) = New Dictionary(Of String, Tensor)()

            For Each item In obj
                Dim s As Single() = ReadW(folderName & "/" & item.Value.filename)
                Dim size As Integer = item.Value.shape(0)

                For i As Integer = 1 To item.Value.shape.Length - 1
                    size *= item.Value.shape(i)
                Next

                If size <> s.Length Then
                    Throw New Exception("asdasD")
                End If

                Dim t = alb.tensor(s, item.Value.shape)
                res.Add(item.Key, t)
            Next

            Return res
        End Function

        Public Sub New()
            Me.timesScalar = alb.scalar(150)
            Me.plusScalar = alb.scalar(255.0F / 2F)
            Me.epsilonScalar = alb.scalar(1e-3F)
            Dim wave = LoadFolder("styles/wave")
            variableDictionary.Add("wave", wave)

            If Directory.Exists("styles/la_muse") Then
                variableDictionary.Add("la_muse", LoadFolder("styles/la_muse"))
            End If

            If Directory.Exists("styles/rain_princess") Then
                variableDictionary.Add("rain_princess", LoadFolder("styles/rain_princess"))
            End If

            If Directory.Exists("styles/scream") Then
                variableDictionary.Add("scream", LoadFolder("styles/scream"))
            End If

            If Directory.Exists("styles/udnie") Then
                variableDictionary.Add("udnie", LoadFolder("styles/udnie"))
            End If

            If Directory.Exists("styles/wreck") Then
                variableDictionary.Add("wreck", LoadFolder("styles/wreck"))
            End If

            Me.variables = wave
        End Sub

        Public Sub ChangeVariable(ByVal name As String)
            Me.variables = Me.variableDictionary(name)
        End Sub

        Public Function varName(ByVal varId As Integer) As String
            If varId = 0 Then
                Return "Variable"
            Else
                Return "Variable_" & varId.ToString()
            End If
        End Function

        Public Function instanceNorm(ByVal input As Tensor, ByVal varId As Integer) As Tensor
            Dim height = input.Shape(0)
            Dim width = input.Shape(1)
            Dim inDepth = input.Shape(2)
            Dim moments = alb.moments(input, New Integer() {0, 1})
            Dim mu = moments.mean
            Dim sigmaSq = moments.variance
            Dim shift = Me.variables(Me.varName(varId))
            Dim scale = Me.variables(Me.varName(varId + 1))
            Dim epsilon = Me.epsilonScalar
            Dim normalized = alb.div(alb.[sub](input, mu), alb.sqrt(alb.add(sigmaSq, epsilon)))
            Dim shifted = alb.add(alb.mul(scale, normalized), shift)
            Return shifted.as3D(height, width, inDepth)
        End Function

        Public Function convLayer(ByVal input As Tensor, ByVal strides As Integer, ByVal relu As Boolean, ByVal varId As Integer) As Tensor
            Dim y = alb.conv2d(input, Me.variables(Me.varName(varId)), New Integer() {strides, strides}, PadType.same)
            Dim y2 = Me.instanceNorm(y, varId + 1)

            If relu Then
                Dim rl = alb.relu(y2)
                Return rl
            End If

            Return y2
        End Function

        Public Function convTransposeLayer(ByVal input As Tensor, ByVal numFilters As Integer, ByVal strides As Integer, ByVal varId As Integer) As Tensor
            Dim height = input.Shape(0)
            Dim width = input.Shape(1)
            Dim newRows = height * strides
            Dim newCols = width * strides
            Dim newShape = New Integer() {newRows, newCols, numFilters}
            Dim y = alb.conv2dTranspose(input, Me.variables(Me.varName(varId)), newShape, New Integer() {strides, strides}, PadType.same)
            Dim y2 = Me.instanceNorm(y, varId + 1)
            Dim y3 = alb.relu(y2)
            Return y3
        End Function

        Public Function residualBlock(ByVal input As Tensor, ByVal varId As Integer) As Tensor
            Dim conv1 = Me.convLayer(input, 1, True, varId)
            Dim conv2 = Me.convLayer(conv1, 1, False, varId + 3)
            Return alb.addStrict(conv2, input)
        End Function

        Public Delegate Sub ReportProgressHandler(ByVal progress As Integer)
        Public Event ReportProgress As ReportProgressHandler

        Private Sub InvokeProgressEvent(ByVal progress As Integer)
            RaiseEvent ReportProgress(progress)
        End Sub

        Public Function Predict(ByVal preprocessedInput As Tensor) As Tensor
            Dim conv1 = Me.convLayer(preprocessedInput, 1, True, 0)
            InvokeProgressEvent(10)
            Dim conv2 = Me.convLayer(conv1, 2, True, 3)
            InvokeProgressEvent(20)
            Dim conv3 = Me.convLayer(conv2, 2, True, 6)
            InvokeProgressEvent(30)
            Dim resid1 = Me.residualBlock(conv3, 9)
            InvokeProgressEvent(40)
            Dim resid2 = Me.residualBlock(resid1, 15)
            InvokeProgressEvent(50)
            Dim resid3 = Me.residualBlock(resid2, 21)
            InvokeProgressEvent(60)
            Dim resid4 = Me.residualBlock(resid3, 27)
            InvokeProgressEvent(70)
            Dim resid5 = Me.residualBlock(resid4, 33)
            InvokeProgressEvent(80)
            Dim convT1 = Me.convTransposeLayer(resid5, 64, 2, 39)
            InvokeProgressEvent(90)
            Dim convT2 = Me.convTransposeLayer(convT1, 32, 2, 42)
            InvokeProgressEvent(100)
            Dim convT3 = Me.convLayer(convT2, 1, False, 45)
            InvokeProgressEvent(110)
            Dim outTanh = alb.tanh(convT3)
            Dim scaled = alb.mul(Me.timesScalar, outTanh)
            Dim shifted = alb.add(Me.plusScalar, scaled)
            Dim clamped = alb.clipByValue(shifted, 0, 255)
            Dim normalized = alb.div(clamped, alb.scalar(255.0F))
            InvokeProgressEvent(120)
            Return normalized
        End Function
    End Class

    Public Class modelmetainfo
        Public Property shape As Integer()
        Public Property filename As String
    End Class
End Namespace
