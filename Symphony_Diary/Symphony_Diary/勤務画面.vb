Public Class 勤務画面

    'フォームタイプ
    Private formType As String

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="formType">フォームの種類</param>
    ''' <remarks></remarks>
    Public Sub New(formType As String)
        InitializeComponent()
        Me.formType = formType
        Me.Text = "Diary " & formType & " 勤務表"
    End Sub

    Private Sub 勤務画面_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class