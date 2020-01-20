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
    Private dbForm As ＤＢ整理

    'CSV用ヘッダー文字列
    Private columnCaption() As String = {"表示順", "対象年月", "勤務表", "職員№", "氏名", "予形態", "予職種", "予1", "予2", "予3", "予4", "予5", "予6", "予7", "予8", "予9", "予10", "予11", "予12", "予13", "予14", "予15", "予16", "予17", "予18", "予19", "予20", "予21", "予22", "予23", "予24", "予25", "予26", "予27", "予28", "予29", "予30", "予31", "予換算", "実形態", "実職種", "実1", "実2", "実3", "実4", "実5", "実6", "実7", "実8", "実9", "実10", "実11", "実12", "実13", "実14", "実15", "実16", "実17", "実18", "実19", "実20", "実21", "実22", "実23", "実24", "実25", "実26", "実27", "実28", "実29", "実30", "実31", "実換算"}

    '勤務名対応dic
    Private workDictionary As Dictionary(Of String, String)

    '保存ファイル名定型部
    Private Const DEFAULT_SAVE_NAME As String = "勤務表"

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

        '勤務対応Dic作成
        createDictionary()
    End Sub

    ''' <summary>
    ''' 勤務対応dic作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub createDictionary()
        workDictionary = New Dictionary(Of String, String)
        'workDictionary.Add("日", "日勤")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
        'workDictionary.Add("", "")
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
        '保存ファイル名等初期値
        Dim ymStr As String = adBox.getADymStr() '年月
        Me.saveCsvFileDialog.FileName = DEFAULT_SAVE_NAME & ymStr.Replace("/", "") & ".csv"
        Me.saveCSVFileDialog.Filter = "Csv|"

        '保存ダイアログでファイル名を設定した場合に処理を実行します。
        If Me.saveCSVFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim strResult As New System.Text.StringBuilder

            'ヘッダー部分作成
            Dim columnCount As Integer = columnCaption.Length - 1
            For i As Integer = 0 To columnCount
                Dim s As String = EncloseDoubleQuotes(columnCaption(i)) '"で囲む
                strResult.Append(s)
                'カンマ追加
                If i < columnCount Then
                    strResult.Append(",")
                End If
            Next
            strResult.Append(vbCrLf) '改行

            'レコード作成
            Dim cnn As New ADODB.Connection
            cnn.Open(DB_Diary)
            Dim rs As New ADODB.Recordset
            Dim sql As String = "select * from KinD where Ym = '" & ymStr & "' order by Hyo, Seq"
            rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockPessimistic)
            While Not rs.EOF
                writeWorkData(rs, strResult)
                rs.MoveNext()
            End While
            rs.Close()
            cnn.Close()

            '保存処理等
            Dim fileName As String = If(Me.saveCSVFileDialog.FileName.EndsWith(".csv"), Me.saveCSVFileDialog.FileName, Me.saveCSVFileDialog.FileName & ".csv") 'ファイル名
            Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS") 'エンコードをShift_JISに
            Dim sw As New System.IO.StreamWriter(fileName, False, enc)
            sw.Write(strResult.ToString)
            sw.Close()
            MsgBox("CSV書き出しが終了しました。", MsgBoxStyle.Information, "Diary")
        End If
    End Sub

    ''' <summary>
    ''' ＣＳＶ読込みボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnReadCSV_Click(sender As System.Object, e As System.EventArgs) Handles btnReadCSV.Click
        
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

    ''' <summary>
    ''' レコードセットのデータを整形してStringBuilderに追加
    ''' </summary>
    ''' <param name="rs">追加するレコードセット</param>
    ''' <param name="sb">追加されるStringBuilder</param>
    ''' <remarks></remarks>
    Private Sub writeWorkData(rs As ADODB.Recordset, sb As System.Text.StringBuilder)
        '表示順
        sb.Append(EncloseDoubleQuotes(rs.Fields("Seq").Value.ToString()) & ",")
        '対象年月
        sb.Append(EncloseDoubleQuotes(Util.checkDBNullValue(rs.Fields("Ym").Value)) & ",")
        '勤務表
        sb.Append(EncloseDoubleQuotes(Util.checkDBNullValue(rs.Fields("Hyo").Value)) & ",")
        '職員№
        sb.Append(EncloseDoubleQuotes("0") & ",")
        '氏名
        sb.Append(EncloseDoubleQuotes(Util.checkDBNullValue(rs.Fields("Nam").Value)) & ",")
        '予形態,予職種
        sb.Append(EncloseDoubleQuotes(Util.checkDBNullValue(rs.Fields("YKei").Value)) & ",")
        sb.Append(EncloseDoubleQuotes(Util.checkDBNullValue(rs.Fields("YSyu").Value)) & ",")
        '予1～予31
        For i As Integer = 1 To 31
            Dim yVal As String = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
            sb.Append(EncloseDoubleQuotes(yVal) & ",")
        Next
        '予換算
        sb.Append(EncloseDoubleQuotes("") & ",")
        '実形態,実職種
        sb.Append(EncloseDoubleQuotes(Util.checkDBNullValue(rs.Fields("HKei").Value)) & ",")
        sb.Append(EncloseDoubleQuotes(Util.checkDBNullValue(rs.Fields("HSyu").Value)) & ",")
        '実1～実31
        For i As Integer = 1 To 31
            Dim hVal As String = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
            sb.Append(EncloseDoubleQuotes(hVal) & ",")
        Next
        '実換算
        sb.Append(EncloseDoubleQuotes(""))
        '改行
        sb.Append(vbCrLf)
    End Sub

    ''' <summary>
    ''' 文字列をダブルクォートで囲む
    ''' </summary>
    Private Function EncloseDoubleQuotes(field As String) As String
        Return """" & field & """"
    End Function

End Class
