Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices

Public Class 勤務表印刷条件

    '年月
    Private ym As String

    '職種
    Private hyo As String

    '曜日配列
    Private youbi() As String

    '印影ファイルパス(Sign1,Sign2,Sign3)
    Private sign1Path As String = ""
    Private sign2Path As String = ""
    Private sign3Path As String = ""

    '印刷 or ﾌﾟﾚﾋﾞｭｰ
    Private printState As Boolean

    '常勤換算用
    '(算出式: 4週合計時間 / 157)
    '4週:28日間とする
    Private Const WEEK4 As Integer = 28
    '分母
    Private Const KANSAN As Decimal = 157.0

    '週平均就労時間
    Private WEEKRY_AVERAGE_TIME As Decimal = 39.25

    Private workArray() As String = {"日勤", "半勤", "早出", "遅出", "Ａ勤", "Ｂ勤", "振替", "夜勤", "宿直", "日直", "Ｃ勤", "明け", "特日", "研修", "深夜", "1/3勤", "1/3半", "日早", "日遅", "遅々", "半Ａ", "半Ｂ", "半夜", "半行"}

    '勤務時間
    Private workTimeDic As New Dictionary(Of String, String)

    '印刷用勤務名Dic
    Private printWorkDic As New Dictionary(Of String, String)

    '短縮勤務名Dic
    Private shortWorkDic As New Dictionary(Of String, String)

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="ym">年月</param>
    ''' <param name="hyo">職種</param>
    ''' <remarks></remarks>
    Public Sub New(ym As String, hyo As String, youbi() As String)
        InitializeComponent()
        Me.ym = ym
        Me.hyo = hyo
        Me.youbi = youbi
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.KeyPreview = True
    End Sub

    Private Sub 勤務表印刷条件_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If e.Control = False Then
                Me.SelectNextControl(Me.ActiveControl, Not e.Shift, True, True, True)
            End If
        End If
    End Sub

    ''' <summary>
    ''' loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub 勤務表印刷条件_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '印刷orﾌﾟﾚﾋﾞｭｰ
        Dim state As String = Util.getIniString("System", "Printer", TopForm.iniFilePath)
        printState = If(state = "Y", True, False)

        '初期選択
        rbtnA4.Checked = True
        rbtnPlan.Checked = True

        'iniファイルから読み込み
        sign1Box.Text = Util.getIniString("System", "Sign1", TopForm.iniFilePath)
        sign2Box.Text = Util.getIniString("System", "Sign2", TopForm.iniFilePath)
        sign3Box.Text = Util.getIniString("System", "Sign3", TopForm.iniFilePath)

        '定数マスタ読み込み
        loadConstM()

        '印刷用勤務名読込
        loadKmkM()
    End Sub

    ''' <summary>
    ''' 実行ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExecute_Click(sender As System.Object, e As System.EventArgs) Handles btnExecute.Click
        '印影ファイル存在チェック
        If sign1Box.Text <> "" Then
            Dim filePath As String = TopForm.sealBoxDirPath & "\" & sign1Box.Text & ".wmf"
            If Not System.IO.File.Exists(filePath) Then
                MsgBox(filePath & " の印影ファイルが存在しません。" & Environment.NewLine & "SealBoxフォルダにファイルを置いて下さい。", MsgBoxStyle.Exclamation)
                Return
            Else
                sign1Path = filePath
            End If
        End If
        If sign2Box.Text <> "" Then
            Dim filePath As String = TopForm.sealBoxDirPath & "\" & sign2Box.Text & ".wmf"
            If Not System.IO.File.Exists(filePath) Then
                MsgBox(filePath & " の印影ファイルが存在しません。" & Environment.NewLine & "SealBoxフォルダにファイルを置いて下さい。", MsgBoxStyle.Exclamation)
                Return
            Else
                sign2Path = filePath
            End If
        End If
        If sign3Box.Text <> "" Then
            Dim filePath As String = TopForm.sealBoxDirPath & "\" & sign3Box.Text & ".wmf"
            If Not System.IO.File.Exists(filePath) Then
                MsgBox(filePath & " の印影ファイルが存在しません。" & Environment.NewLine & "SealBoxフォルダにファイルを置いて下さい。", MsgBoxStyle.Exclamation)
                Return
            Else
                sign3Path = filePath
            End If
        End If

        'iniファイルのSign1,Sign2,Sign3を更新
        Util.putIniString("System", "Sign1", sign1Box.Text, TopForm.iniFilePath)
        Util.putIniString("System", "Sign2", sign2Box.Text, TopForm.iniFilePath)
        Util.putIniString("System", "Sign3", sign3Box.Text, TopForm.iniFilePath)

        'データ取得
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select * from KinD where Ym = '" & ym & "' and Hyo = '" & hyo & "' order by Seq"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
        If rs.RecordCount <= 0 Then
            MsgBox("対象データがありません。", MsgBoxStyle.Exclamation)
            rs.Close()
            cnn.Close()
            Return
        Else
            '印刷処理
            Dim type As String = If(rbtnPlan.Checked, "予定", If(rbtnResult.Checked, "実績", "予定／実績"))
            If rbtnA4.Checked Then
                printA4(type, rs)
            ElseIf rbtnB4.Checked Then
                printB4(type, rs)
            ElseIf rbtnB4S.Checked Then
                printB4S(type, rs)
            ElseIf rbtnB4S2.Checked Then
                printB4S2(type, rs)
            End If
            rs.Close()
            cnn.Close()
            Me.Close()
        End If
    End Sub

    ''' <summary>
    ''' 勤務項目名マスタ読み込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub loadKmkM()
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select Ent, Prt from KmkM where Kin = '" & hyo & "'"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        While Not rs.EOF
            Dim ent As String = Util.checkDBNullValue(rs.Fields("Ent").Value)
            Dim prt As String = Util.checkDBNullValue(rs.Fields("Prt").Value)
            If Not printWorkDic.ContainsKey(ent) Then
                printWorkDic.Add(ent, prt)
            End If
            If Not shortWorkDic.ContainsKey(prt) Then
                shortWorkDic.Add(prt, ent)
            End If
            rs.MoveNext()
        End While
        rs.Close()
        cnn.Close()
    End Sub

    ''' <summary>
    ''' 定数マスタ読み込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub loadConstM()
        Dim hyoNum As String = ""
        If hyo = "特養" Then
            hyoNum = "1"
        ElseIf hyo = "事務" Then
            hyoNum = "2"
        ElseIf hyo = "ｼｮｰﾄｽﾃｲ" Then
            hyoNum = "3"
        ElseIf hyo = "ﾃﾞｲｻｰﾋﾞｽ" Then
            hyoNum = "4"
        ElseIf hyo = "ﾍﾙﾊﾟｰｽﾃｰｼｮﾝ" Then
            hyoNum = "5"
        ElseIf hyo = "居宅介護支援" Then
            hyoNum = "6"
        ElseIf hyo = "老人介護支援ｾﾝﾀｰ" Then
            hyoNum = "7"
        ElseIf hyo = "生活支援ﾊｳｽ" Then
            hyoNum = "8"
        End If

        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim sql As String = "select * from ConstM"
        Dim rs As New ADODB.Recordset
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        While Not rs.EOF
            For i As Integer = 1 To 24
                workTimeDic.Add(workArray(i - 1), rs.Fields("J" & i & hyoNum).Value)
            Next
            rs.MoveNext()
        End While
        rs.Close()
        cnn.Close()
    End Sub

    ''' <summary>
    ''' 勤務時間に変換
    ''' </summary>
    ''' <param name="work">勤務名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function convWorkTime(work As String) As Decimal
        Dim result As Decimal
        If System.Text.RegularExpressions.Regex.IsMatch(work, "^\d+(\.\d)?$") Then
            '数値の場合はそのまま
            result = CDec(work)
        Else
            '数値以外
            work = If(shortWorkDic.ContainsKey(work), shortWorkDic(work), work)
            work = If(work = "有休", "日勤", work)
            work = If(workTimeDic.ContainsKey(work), workTimeDic(work), "0")
            result = CDec(work)
        End If
        Return result
    End Function

    ''' <summary>
    ''' A4印刷
    ''' </summary>
    ''' <param name="writeType">予定 or 実績 or 予定／実績</param>
    ''' <param name="rs">印刷データレコードセット</param>
    ''' <remarks></remarks>
    Private Sub printA4(writeType As String, rs As ADODB.Recordset)
        '貼り付けデータ作成
        Dim dataList As New List(Of String(,))
        Dim dataArray(35, 36) As String
        Dim arrayRowIndex As Integer = 0
        While Not rs.EOF
            If arrayRowIndex = 36 Then
                dataList.Add(dataArray.Clone())
                Array.Clear(dataArray, 0, dataArray.Length)
                arrayRowIndex = 0
            End If

            '勤務形態
            dataArray(arrayRowIndex, 0) = Util.checkDBNullValue(rs.Fields("YKei").Value)
            '職種
            dataArray(arrayRowIndex, 1) = Util.checkDBNullValue(rs.Fields("YSyu").Value)
            '氏名
            dataArray(arrayRowIndex, 2) = Util.checkDBNullValue(rs.Fields("Nam").Value)
            '予定、変更
            dataArray(arrayRowIndex, 5) = "予定"
            dataArray(arrayRowIndex + 1, 5) = "変更"
            '1～31
            If writeType = "予定" Then
                For i As Integer = 1 To 31
                    dataArray(arrayRowIndex, 5 + i) = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                Next
            ElseIf writeType = "実績" Then
                For i As Integer = 1 To 31
                    dataArray(arrayRowIndex + 1, 5 + i) = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                Next
            Else '予定／実績
                For i As Integer = 1 To 31
                    Dim yotei As String = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                    Dim henko As String = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                    dataArray(arrayRowIndex, 5 + i) = yotei
                    If yotei <> henko Then
                        dataArray(arrayRowIndex + 1, 5 + i) = henko
                    End If
                Next
            End If

            arrayRowIndex += 2
            rs.MoveNext()
        End While
        dataList.Add(dataArray.Clone())

        '印刷用に勤務名変換
        For Each d As String(,) In dataList
            For i As Integer = 0 To 35
                For j As Integer = 0 To 36
                    Dim work As String = d(i, j)
                    If Not IsNothing(work) AndAlso printWorkDic.ContainsKey(work) Then
                        d(i, j) = printWorkDic(work)
                    End If
                Next
            Next
        Next

        'エクセル準備
        Dim objExcel As Excel.Application = CreateObject("Excel.Application")
        Dim objWorkBooks As Excel.Workbooks = objExcel.Workbooks
        Dim objWorkBook As Excel.Workbook = objWorkBooks.Open(TopForm.excelFilePath)
        Dim oSheet As Excel.Worksheet = objWorkBook.Worksheets("勤務表A4改")
        Dim xlShapes As Excel.Shapes = DirectCast(oSheet.Shapes, Excel.Shapes)
        objExcel.Calculation = Excel.XlCalculation.xlCalculationManual
        objExcel.ScreenUpdating = False

        '年月
        oSheet.Range("D3").Value = "( " & CInt(ym.Split("/")(0)) & "年 " & CInt(ym.Split("/")(1)) & "月度 )"
        '部署?
        oSheet.Range("J3").Value = hyo
        '印影
        If System.IO.File.Exists(sign1Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AG"), Excel.Range)
            xlShapes.AddPicture(sign1Path, False, True, cell.Left + 6, cell.Top + 3, 30, 30)
        End If
        If System.IO.File.Exists(sign2Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AI"), Excel.Range)
            xlShapes.AddPicture(sign2Path, False, True, cell.Left + 6, cell.Top + 3, 30, 30)
        End If
        If System.IO.File.Exists(sign3Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AK"), Excel.Range)
            xlShapes.AddPicture(sign3Path, False, True, cell.Left + 6, cell.Top + 3, 30, 30)
        End If
        '曜日行設定
        oSheet.Range("H8", "AL8").Value = youbi

        '必要ページ分コピペ
        For i As Integer = 0 To dataList.Count - 2
            Dim xlPasteRange As Excel.Range = oSheet.Range("A" & (48 + (47 * i))) 'ペースト先
            oSheet.Rows("1:47").copy(xlPasteRange)
            oSheet.HPageBreaks.Add(oSheet.Range("A" & (48 + (47 * i)))) '改ページ
        Next

        'データ貼り付け
        For i As Integer = 0 To dataList.Count - 1
            oSheet.Range("B" & (9 + (47 * i)), "AL" & (44 + (47 * i))).Value = dataList(i)
        Next

        objExcel.Calculation = Excel.XlCalculation.xlCalculationAutomatic
        objExcel.ScreenUpdating = True

        '変更保存確認ダイアログ非表示
        objExcel.DisplayAlerts = False

        '印刷
        If printState Then
            oSheet.PrintOut()
        Else
            objExcel.Visible = True
            oSheet.PrintPreview(1)
        End If

        ' EXCEL解放
        objExcel.Quit()
        Marshal.ReleaseComObject(objWorkBook)
        Marshal.ReleaseComObject(objExcel)
        oSheet = Nothing
        objWorkBook = Nothing
        objExcel = Nothing
    End Sub

    ''' <summary>
    ''' B4印刷
    ''' </summary>
    ''' <param name="writeType">予定 or 実績 or 予定／実績</param>
    ''' <param name="rs">印刷データレコードセット</param>
    ''' <remarks></remarks>
    Private Sub printB4(writeType As String, rs As ADODB.Recordset)
        '貼り付けデータ作成
        Dim dataList As New List(Of String(,))
        Dim dataArray(53, 38) As String
        Dim arrayRowIndex As Integer = 0

        While Not rs.EOF
            If arrayRowIndex = 34 Then
                dataList.Add(dataArray.Clone())
                Array.Clear(dataArray, 0, dataArray.Length)
                arrayRowIndex = 0
            End If

            '勤務形態
            Dim kei As String = Util.checkDBNullValue(rs.Fields("YKei").Value)
            dataArray(arrayRowIndex, 0) = kei
            '職種
            dataArray(arrayRowIndex, 1) = Util.checkDBNullValue(rs.Fields("YSyu").Value)
            '氏名
            dataArray(arrayRowIndex, 2) = Util.checkDBNullValue(rs.Fields("Nam").Value)
            '予定、変更
            dataArray(arrayRowIndex, 5) = "予定"
            dataArray(arrayRowIndex + 1, 5) = "変更"
            '1～31
            If writeType = "予定" Then
                For i As Integer = 1 To 31
                    Dim yotei As String = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                    dataArray(arrayRowIndex, 5 + i) = yotei
                Next
            ElseIf writeType = "実績" Then
                For i As Integer = 1 To 31
                    Dim henko As String = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                    dataArray(arrayRowIndex + 1, 5 + i) = henko
                Next
            Else '予定／実績
                For i As Integer = 1 To 31
                    Dim yotei As String = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                    Dim henko As String = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                    dataArray(arrayRowIndex, 5 + i) = yotei
                    If yotei <> henko Then
                        dataArray(arrayRowIndex + 1, 5 + i) = henko
                    End If
                Next
            End If

            '月合計(予定行のみ)
            Dim totalY As Decimal = 0.0
            Dim totalH As Decimal = 0.0
            For i As Integer = 1 To WEEK4
                Dim yotei As String = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                Dim henko As String = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                totalY += convWorkTime(yotei)
                henko = If(henko = "", yotei, henko)
                totalH += convWorkTime(henko)
            Next
            If totalY = 0.0 Then
                dataArray(arrayRowIndex, 38) = ""
            Else
                dataArray(arrayRowIndex, 38) = totalY.ToString()
            End If

            '常勤換算後の人数
            '予定
            Dim kansanY As String = (Math.Floor((totalY / KANSAN) * 100) / 100).ToString("0.00")
            If kansanY = "0.00" Then
                dataArray(arrayRowIndex, 37) = ""
            Else
                If kei = "常勤専従" Then
                    dataArray(arrayRowIndex, 37) = "1.00"
                Else
                    dataArray(arrayRowIndex, 37) = kansanY
                End If
            End If
            '変更
            Dim kansanH As String = (Math.Floor((totalH / KANSAN) * 100) / 100).ToString("0.00")
            If kansanH = "0.00" Then
                dataArray(arrayRowIndex + 1, 37) = ""
            Else
                If kei = "常勤専従" Then
                    dataArray(arrayRowIndex + 1, 37) = ""
                Else
                    dataArray(arrayRowIndex + 1, 37) = If(kansanY <> kansanH, kansanH, "")
                End If
            End If

            arrayRowIndex += 2
            rs.MoveNext()
        End While
        dataList.Add(dataArray.Clone())

        '印刷用に勤務名変換
        For Each d As String(,) In dataList
            For i As Integer = 0 To 33
                For j As Integer = 6 To 36
                    Dim work As String = d(i, j)
                    If Not IsNothing(work) AndAlso printWorkDic.ContainsKey(work) Then
                        d(i, j) = printWorkDic(work)
                    End If
                Next
            Next
        Next

        '左下の勤務時間の表示、定数マスタから取得
        Dim timeList As New List(Of String)
        Dim count As Integer = 1
        For Each kvp As KeyValuePair(Of String, String) In workTimeDic
            If count >= 16 Then
                Exit For
            End If
            Dim work As String = kvp.Key
            Dim time As String = kvp.Value
            If time <> "0" Then
                timeList.Add(work & " " & time)
            End If
            count += 1
        Next

        '固定文字列設定
        For Each d As String(,) In dataList
            d(34, 0) = "看護師"
            d(36, 0) = "介護士　介護職"
            d(38, 0) = "介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ"
            d(40, 0) = "計"
            d(42, 5) = "日勤"
            d(44, 5) = "早遅特"
            d(46, 5) = "半"
            d(48, 5) = "直１２"
            d(50, 5) = "ＡＢＣ"
            d(52, 5) = "夜宿明"
            '左下勤務名 時間
            For i As Integer = 0 To timeList.Count - 1
                If i >= 12 Then
                    Exit For
                End If
                d(42 + i, 0) = timeList(i)
            Next
        Next

        '月の日数
        Dim year As Integer = CInt(ym.Split("/")(0))
        Dim month As Integer = CInt(ym.Split("/")(1))
        Dim daysInMonth As Integer = DateTime.DaysInMonth(year, month)

        'エクセル準備
        Dim objExcel As Excel.Application = CreateObject("Excel.Application")
        Dim objWorkBooks As Excel.Workbooks = objExcel.Workbooks
        Dim objWorkBook As Excel.Workbook = objWorkBooks.Open(TopForm.excelFilePath)
        Dim oSheet As Excel.Worksheet = objWorkBook.Worksheets("勤務表B4改")
        Dim xlShapes As Excel.Shapes = DirectCast(oSheet.Shapes, Excel.Shapes)
        objExcel.Calculation = Excel.XlCalculation.xlCalculationManual
        objExcel.ScreenUpdating = False

        '年月
        oSheet.Range("C3").Value = "( " & CInt(ym.Split("/")(0)) & "年 " & CInt(ym.Split("/")(1)) & "月度 )"
        '部署?
        oSheet.Range("G3").Value = hyo
        '印影
        If System.IO.File.Exists(sign1Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AD"), Excel.Range)
            xlShapes.AddPicture(sign1Path, False, True, cell.Left + 6, cell.Top + 3, 30, 30)
        End If
        If System.IO.File.Exists(sign2Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AF"), Excel.Range)
            xlShapes.AddPicture(sign2Path, False, True, cell.Left + 6, cell.Top + 3, 30, 30)
        End If
        If System.IO.File.Exists(sign3Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AH"), Excel.Range)
            xlShapes.AddPicture(sign3Path, False, True, cell.Left + 6, cell.Top + 3, 30, 30)
        End If
        '週平均就労時間
        oSheet.Range("AN3").Value = WEEKRY_AVERAGE_TIME
        '日数
        oSheet.Range("AL4").Value = daysInMonth
        '曜日行設定
        oSheet.Range("H8", "AL8").Value = youbi

        '必要ページ分コピペ
        For i As Integer = 0 To dataList.Count - 2
            Dim xlPasteRange As Excel.Range = oSheet.Range("A" & (66 + (65 * i))) 'ペースト先
            oSheet.Rows("1:65").copy(xlPasteRange)
            oSheet.HPageBreaks.Add(oSheet.Range("A" & (66 + (65 * i)))) '改ページ
        Next

        'データ貼り付け
        For i As Integer = 0 To dataList.Count - 1
            oSheet.Range("B" & (9 + (65 * i)), "AN" & (62 + (65 * i))).Value = dataList(i)
        Next

        objExcel.Calculation = Excel.XlCalculation.xlCalculationAutomatic
        objExcel.ScreenUpdating = True

        '変更保存確認ダイアログ非表示
        objExcel.DisplayAlerts = False

        '印刷
        If printState Then
            oSheet.PrintOut()
        Else
            objExcel.Visible = True
            oSheet.PrintPreview(1)
        End If

        ' EXCEL解放
        objExcel.Quit()
        Marshal.ReleaseComObject(objWorkBook)
        Marshal.ReleaseComObject(objExcel)
        oSheet = Nothing
        objWorkBook = Nothing
        objExcel = Nothing
    End Sub

    ''' <summary>
    ''' B4→A4(B4S)印刷
    ''' </summary>
    ''' <param name="writeType">予定 or 実績 or 予定／実績</param>
    ''' <param name="rs">印刷データレコードセット</param>
    ''' <remarks></remarks>
    Private Sub printB4S(writeType As String, rs As ADODB.Recordset)

    End Sub

    ''' <summary>
    ''' B4→A4(NC)(B4S2)印刷
    ''' </summary>
    ''' <param name="writeType">予定 or 実績 or 予定／実績</param>
    ''' <param name="rs">印刷データレコードセット</param>
    ''' <remarks></remarks>
    Private Sub printB4S2(writeType As String, rs As ADODB.Recordset)

    End Sub

    ''' <summary>
    ''' キャンセルボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class