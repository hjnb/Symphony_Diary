Public Class ＤＢ整理

    Public Sub New()
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MinimizeBox = False
        Me.MaximizeBox = False
    End Sub

    Private Sub ＤＢ整理_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    ''' <summary>
    ''' 実行ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDBDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDBDelete.Click
        deleteProgressBar.Minimum = 0
        deleteProgressBar.Maximum = 100
        deleteProgressBar.Value = 0

        '現在年月
        Dim nowYmStr As String = Today.ToString("yyyy/MM")
        '５年前年月
        Dim targetYmStr As String = Today.AddYears(-5).ToString("yyyy/MM")

        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)

        '勤務割データ削除
        Dim cmd As New ADODB.Command()
        cmd.ActiveConnection = cnn
        cmd.CommandText = "delete from KinD where Ym < '" & targetYmStr & "'"
        cmd.Execute()

        deleteProgressBar.Value = 100

        MsgBox("データを削除しました。" & Environment.NewLine & "単独モードでDBCompactを実行して下さい。", MsgBoxStyle.Information)
    End Sub
End Class