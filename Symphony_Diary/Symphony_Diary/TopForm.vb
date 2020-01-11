Public Class TopForm

    '.iniファイルのパス
    Public iniFilePath As String = My.Application.Info.DirectoryPath & "\Diary.ini"

    'エクセルのパス
    Public excelFilePath As String = My.Application.Info.DirectoryPath & "\Diary.xls"

    'データベースのパス
    Public dbFilePath As String = My.Application.Info.DirectoryPath & "\Diary.mdb"
    Public DB_Diary As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & dbFilePath

    'SealBoxフォルダパス
    Public sealBoxDirPath As String = Util.getIniString("System", "SealBoxDir", iniFilePath)

    '各フォーム
    Private workForm As 勤務画面
    Private syoMForm As 職員マスタ
    Private kinMForm As 勤務項目名マスタ
    Private constMForm As 定数マスタ
    Private createCSVForm As ＣＳＶ書出し
    Private readCSVForm As ＣＳＶ読込み
    Private dbForm As ＤＢ整理

    ''' <summary>
    ''' loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TopForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '構成ファイル、データベース、エクセルの存在チェック
        If Not System.IO.File.Exists(iniFilePath) Then
            MsgBox("構成ファイルが存在しません。ファイルを配置して下さい。", MsgBoxStyle.Exclamation)
            Me.Close()
            Exit Sub
        End If
        If Not System.IO.File.Exists(dbFilePath) Then
            MsgBox("データベースファイルが存在しません。ファイルを配置して下さい。", MsgBoxStyle.Exclamation)
            Me.Close()
            Exit Sub
        End If
        If Not System.IO.File.Exists(excelFilePath) Then
            MsgBox("エクセルファイルが存在しません。ファイルを配置して下さい。", MsgBoxStyle.Exclamation)
            Me.Close()
            Exit Sub
        End If

        'SealBoxDirの存在チェック
        If Not System.IO.Directory.Exists(sealBoxDirPath) Then
            MsgBox("SealBoxDirフォルダが存在しません。" & Environment.NewLine & "iniファイルのSealBoxDirに適切なパスを設定して下さい。", MsgBoxStyle.Exclamation)
            Me.Close()
            Exit Sub
        End If

        '管理者パスワードフォーム表示
        'Dim passForm As Form = New passwordForm(iniFilePath, 1)
        'If passForm.ShowDialog() <> Windows.Forms.DialogResult.OK Then
        '    Me.Close()
        '    Exit Sub
        'End If

        '画面サイズ
        Me.WindowState = FormWindowState.Maximized

        '印刷ラジオボタン初期値設定
        initPrintState()

        '上下ボタン長押し動作不可
        adBox.canHoldDownButton = False
    End Sub

    ''' <summary>
    ''' 印刷ラジオボタン初期値設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initPrintState()
        Dim state As String = Util.getIniString("System", "Printer", iniFilePath)
        If state = "Y" Then
            rbtnPrint.Checked = True
        Else
            rbtnPreview.Checked = True
        End If
    End Sub

    ''' <summary>
    ''' ﾌﾟﾚﾋﾞｭｰラジオボタン値変更イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rbtnPreview_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbtnPreview.CheckedChanged
        If rbtnPreview.Checked = True Then
            Util.putIniString("System", "Printer", "N", iniFilePath)
        End If
    End Sub

    ''' <summary>
    ''' 印刷ラジオボタン値変更イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rbtnPrint_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbtnPrint.CheckedChanged
        If rbtnPrint.Checked = True Then
            Util.putIniString("System", "Printer", "Y", iniFilePath)
        End If
    End Sub

    ''' <summary>
    ''' 各種別ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnTok_Click(sender As System.Object, e As System.EventArgs) Handles btnTok.Click, btnJim.Click, btnSyo.Click, btnDay.Click, btnHel.Click, btnKyo.Click, btnSei.Click
        If IsNothing(workForm) OrElse workForm.IsDisposed Then
            workForm = New 勤務画面(DirectCast(sender, Button).Text, adBox.getADymStr())
            workForm.Show()
        End If
    End Sub

    ''' <summary>
    ''' 職員マスタボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSyoM_Click(sender As System.Object, e As System.EventArgs) Handles btnSyoM.Click
        If IsNothing(syoMForm) OrElse syoMForm.IsDisposed Then
            syoMForm = New 職員マスタ()
            syoMForm.Show()
        End If
    End Sub

    ''' <summary>
    ''' 勤務項目名マスタボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnKinM_Click(sender As System.Object, e As System.EventArgs) Handles btnKinM.Click
        If IsNothing(kinMForm) OrElse kinMForm.IsDisposed Then
            kinMForm = New 勤務項目名マスタ()
            kinMForm.Show()
        End If
    End Sub

    ''' <summary>
    ''' 定数マスタボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnConstM_Click(sender As System.Object, e As System.EventArgs) Handles btnConstM.Click
        If IsNothing(constMForm) OrElse constMForm.IsDisposed Then
            constMForm = New 定数マスタ()
            constMForm.Show()
        End If
    End Sub

    ''' <summary>
    ''' ＣＳＶ書出しボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCreateCSV_Click(sender As System.Object, e As System.EventArgs) Handles btnCreateCSV.Click
        If IsNothing(createCSVForm) OrElse createCSVForm.IsDisposed Then
            createCSVForm = New ＣＳＶ書出し()
            createCSVForm.Show()
        End If
    End Sub

    ''' <summary>
    ''' ＣＳＶ読込みボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnReadCSV_Click(sender As System.Object, e As System.EventArgs) Handles btnReadCSV.Click
        If IsNothing(readCSVForm) OrElse readCSVForm.IsDisposed Then
            readCSVForm = New ＣＳＶ読込み()
            readCSVForm.Show()
        End If
    End Sub

    ''' <summary>
    ''' ＤＢ整理ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDB_Click(sender As System.Object, e As System.EventArgs) Handles btnDB.Click
        If IsNothing(dbForm) OrElse dbForm.IsDisposed Then
            dbForm = New ＤＢ整理()
            dbForm.Show()
        End If
    End Sub
End Class
