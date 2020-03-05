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

    '勤務表示列(H～AL)
    Private workColumn() As String = {"H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL"}

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
    ''' 勤務としてカウントするか判定
    ''' </summary>
    ''' <param name="work">勤務名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function canCountWork(work As String) As Boolean
        Dim result As Boolean = False
        Dim convWork As String = If(shortWorkDic.ContainsKey(work), shortWorkDic(work), work)
        If workTimeDic.ContainsKey(convWork) Then
            Return True
        Else
            If convWork = "有休" Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    ''' <summary>
    ''' 職種カウント用変換
    ''' </summary>
    ''' <param name="syu">職種</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function convSyu(syu As String) As String
        If syu = "介護士" OrElse syu = "介護職" Then
            Return "介護士　介護職"
        ElseIf syu = "介護士ﾊﾟｰﾄ" OrElse syu = "介護職ﾊﾟｰﾄ" Then
            Return "介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ"
        Else
            Return syu
        End If
    End Function

    ''' <summary>
    ''' 勤務カウント用変換
    ''' </summary>
    ''' <param name="work">勤務名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function convWork(work As String) As String
        work = If(shortWorkDic.ContainsKey(work), shortWorkDic(work), work)
        If work = "日勤" OrElse work = "有休" Then
            Return "日勤"
        ElseIf work = "早出" OrElse work = "日早" OrElse work = "遅出" OrElse work = "遅々" OrElse work = "日遅" OrElse work = "特日" Then
            Return "早遅特"
        ElseIf work = "半Ａ" OrElse work = "半Ｂ" OrElse work = "半勤" OrElse work = "半夜" OrElse work = "半行" Then
            Return "半"
        ElseIf work = "Ａ勤" OrElse work = "Ｂ勤" OrElse work = "Ｃ勤" OrElse work = "研修" Then
            Return "ＡＢＣ"
        ElseIf work = "深夜" OrElse work = "夜勤" OrElse work = "宿直" OrElse work = "明け" Then
            Return "夜宿明"
        Else
            Return ""
        End If
    End Function

    Private Overloads Function convNumber(num As Integer) As String
        If num = 0 Then
            Return ""
        Else
            Return num.ToString()
        End If
    End Function

    Private Overloads Function convNumber(num As Decimal) As String
        If num = 0.0 Then
            Return ""
        Else
            Return num.ToString("0.00")
        End If
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
        For i As Integer = 1 To 31
            Dim day As String = youbi(i - 1)
            If day = "日" Then
                Dim column As String = workColumn(i - 1)
                oSheet.Range(column & "7", column & "44").Interior.Pattern = 17
            End If
        Next

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
        '集計用準備
        Dim calcSyuDic As New Dictionary(Of String, Integer(,))
        Dim calcJyoDic As New Dictionary(Of String, Decimal(,))
        For Each nam As String In {"看護師", "介護士　介護職", "介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ", "計", "日勤", "早遅特", "半", "直１２", "ＡＢＣ", "夜宿明"}
            Dim arr(1, 27) As Integer
            For i As Integer = 0 To 1
                For j As Integer = 0 To 27
                    arr(i, j) = 0
                Next
            Next
            calcSyuDic.Add(nam, arr.Clone())
            Dim arr2(1, 0) As Decimal
            For i As Integer = 0 To 1
                arr2(i, 0) = 0.0
            Next
            calcJyoDic.Add(nam, arr2)
        Next

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
            Dim syu As String = Util.checkDBNullValue(rs.Fields("YSyu").Value)
            dataArray(arrayRowIndex, 1) = syu
            '氏名
            dataArray(arrayRowIndex, 2) = Util.checkDBNullValue(rs.Fields("Nam").Value)
            '予定、変更
            dataArray(arrayRowIndex, 5) = "予定"
            dataArray(arrayRowIndex + 1, 5) = "変更"
            '1～31
            For i As Integer = 1 To 31
                Dim yotei As String = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                Dim henko As String = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                If writeType = "予定／実績" Then
                    henko = If(yotei = henko, "", henko)
                End If
                dataArray(arrayRowIndex, 5 + i) = yotei
                dataArray(arrayRowIndex + 1, 5 + i) = henko
                '集計(職種)
                If i <= 28 AndAlso canCountWork(yotei) Then
                    Dim cSyu As String = convSyu(syu) '対応する職種に変換
                    If calcSyuDic.ContainsKey(cSyu) Then
                        calcSyuDic(cSyu)(0, i - 1) += 1
                        calcSyuDic("計")(0, i - 1) += 1
                    End If
                End If
                If i <= 28 AndAlso canCountWork(henko) Then
                    Dim cSyu As String = convSyu(syu) '対応する職種に変換
                    If calcSyuDic.ContainsKey(cSyu) Then
                        calcSyuDic(cSyu)(1, i - 1) += 1
                        calcSyuDic("計")(1, i - 1) += 1
                    End If
                End If
                '集計（勤務）
                If i <= 28 Then
                    Dim cWorkY As String = convWork(yotei) '対応する集計用勤務名に変換
                    If calcSyuDic.ContainsKey(cWorkY) Then
                        calcSyuDic(cWorkY)(0, i - 1) += 1
                    End If
                    Dim cWorkH As String = convWork(henko) '対応する集計用勤務名に変換
                    If calcSyuDic.ContainsKey(cWorkH) Then
                        calcSyuDic(cWorkH)(1, i - 1) += 1
                    End If
                End If
            Next

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
            kansanY = If(CDec(kansanY) > 1.0, "1.00", kansanY)
            kansanY = If(kei = "常勤専従", "1.00", kansanY)
            If kansanY = "0.00" Then
                dataArray(arrayRowIndex, 37) = ""
            Else
                dataArray(arrayRowIndex, 37) = kansanY
            End If
            '変更
            Dim kansanH As String = (Math.Floor((totalH / KANSAN) * 100) / 100).ToString("0.00")
            kansanH = If(CDec(kansanH) > 1.0, "1.00", kansanH)
            kansanH = If(kei = "常勤専従", "1.00", kansanH)
            If kansanH = "0.00" Then
                dataArray(arrayRowIndex + 1, 37) = ""
            Else
                dataArray(arrayRowIndex + 1, 37) = If(kansanY <> kansanH, kansanH, "")
            End If
            '集計用
            Dim calcSyu As String = convSyu(syu) '対応する職種に変換
            Dim jyoY As String = kansanY
            If System.Text.RegularExpressions.Regex.IsMatch(jyoY, "^\d+(\.\d+)?$") AndAlso calcJyoDic.ContainsKey(calcSyu) Then
                calcJyoDic(calcSyu)(0, 0) += CDec(jyoY)
                calcJyoDic("計")(0, 0) += CDec(jyoY)
            End If
            Dim jyoH As String = kansanH
            If System.Text.RegularExpressions.Regex.IsMatch(jyoH, "^\d+(\.\d+)?$") AndAlso calcJyoDic.ContainsKey(calcSyu) Then
                calcJyoDic(calcSyu)(1, 0) += CDec(jyoH)
                calcJyoDic("計")(1, 0) += CDec(jyoH)
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

        '貼り付けデータに集計データを代入
        Dim lastData(,) As String = dataList(dataList.Count - 1)
        For i As Integer = 0 To 27
            '看護師
            Dim y1 As String = convNumber(calcSyuDic("看護師")(0, i))
            Dim h1 As String = convNumber(calcSyuDic("看護師")(1, i))
            lastData(34, 6 + i) = y1
            lastData(35, 6 + i) = If(y1 = h1, "", h1)
            '介護士　介護職
            Dim y2 As String = convNumber(calcSyuDic("介護士　介護職")(0, i))
            Dim h2 As String = convNumber(calcSyuDic("介護士　介護職")(1, i))
            lastData(36, 6 + i) = y2
            lastData(37, 6 + i) = If(y2 = h2, "", h2)
            '介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ
            Dim y3 As String = convNumber(calcSyuDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(0, i))
            Dim h3 As String = convNumber(calcSyuDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(1, i))
            lastData(38, 6 + i) = y3
            lastData(39, 6 + i) = If(y3 = h3, "", h3)
            '計
            Dim y4 As String = convNumber(calcSyuDic("計")(0, i))
            Dim h4 As String = convNumber(calcSyuDic("計")(1, i))
            lastData(40, 6 + i) = y4
            lastData(41, 6 + i) = If(y4 = h4, "", h4)

            '日勤
            Dim y5 As String = convNumber(calcSyuDic("日勤")(0, i))
            Dim h5 As String = convNumber(calcSyuDic("日勤")(1, i))
            lastData(42, 6 + i) = y5
            lastData(43, 6 + i) = If(y5 = h5, "", h5)
            '早遅特
            Dim y6 As String = convNumber(calcSyuDic("早遅特")(0, i))
            Dim h6 As String = convNumber(calcSyuDic("早遅特")(1, i))
            lastData(44, 6 + i) = y6
            lastData(45, 6 + i) = If(y6 = h6, "", h6)
            '半
            Dim y7 As String = convNumber(calcSyuDic("半")(0, i))
            Dim h7 As String = convNumber(calcSyuDic("半")(1, i))
            lastData(46, 6 + i) = y7
            lastData(47, 6 + i) = If(y7 = h7, "", h7)
            '直１２
            Dim y8 As String = convNumber(calcSyuDic("直１２")(0, i))
            Dim h8 As String = convNumber(calcSyuDic("直１２")(1, i))
            lastData(48, 6 + i) = y8
            lastData(49, 6 + i) = If(y8 = h8, "", h8)
            'ＡＢＣ
            Dim y9 As String = convNumber(calcSyuDic("ＡＢＣ")(0, i))
            Dim h9 As String = convNumber(calcSyuDic("ＡＢＣ")(1, i))
            lastData(50, 6 + i) = y9
            lastData(51, 6 + i) = If(y9 = h9, "", h9)
            '夜宿明
            Dim y10 As String = convNumber(calcSyuDic("夜宿明")(0, i))
            Dim h10 As String = convNumber(calcSyuDic("夜宿明")(1, i))
            lastData(52, 6 + i) = y10
            lastData(53, 6 + i) = If(y10 = h10, "", h10)
        Next
        '常勤換算集計
        '看護師
        Dim jy1 As String = convNumber(calcJyoDic("看護師")(0, 0))
        Dim jh1 As String = convNumber(calcJyoDic("看護師")(1, 0))
        lastData(34, 37) = jy1
        lastData(35, 37) = If(jy1 = jh1, "", jh1)
        '介護士　介護職
        Dim jy2 As String = convNumber(calcJyoDic("介護士　介護職")(0, 0))
        Dim jh2 As String = convNumber(calcJyoDic("介護士　介護職")(1, 0))
        lastData(36, 37) = jy2
        lastData(37, 37) = If(jy2 = jh2, "", jh2)
        '介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ
        Dim jy3 As String = convNumber(calcJyoDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(0, 0))
        Dim jh3 As String = convNumber(calcJyoDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(1, 0))
        lastData(38, 37) = jy3
        lastData(39, 37) = If(jy3 = jh3, "", jh3)
        '計
        Dim jy4 As String = convNumber(calcJyoDic("計")(0, 0))
        Dim jh4 As String = convNumber(calcJyoDic("計")(1, 0))
        lastData(40, 37) = jy4
        lastData(41, 37) = If(jy4 = jh4, "", jh4)

        'writeType:予定、実績の場合はそれぞれ不必要のデータ削除
        If writeType = "予定" Then
            '変更データを空白に
            For Each d As String(,) In dataList
                For i As Integer = 1 To 53 Step 2
                    For j As Integer = 6 To 38
                        d(i, j) = ""
                    Next
                Next
            Next
        ElseIf writeType = "実績" Then
            '予定データを空白に
            For Each d As String(,) In dataList
                For i As Integer = 0 To 52 Step 2
                    For j As Integer = 6 To 38
                        d(i, j) = ""
                    Next
                Next
            Next
        End If

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
        For i As Integer = 1 To 31
            Dim day As String = youbi(i - 1)
            If day = "日" Then
                Dim column As String = workColumn(i - 1)
                oSheet.Range(column & "7", column & "62").Interior.Pattern = 17
            End If
        Next

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
        '集計用準備
        Dim calcSyuDic As New Dictionary(Of String, Integer(,))
        Dim calcJyoDic As New Dictionary(Of String, Decimal(,))
        For Each nam As String In {"看護師", "介護士　介護職", "介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ", "計", "日勤", "早遅特", "半", "直１２", "ＡＢＣ", "夜宿明"}
            Dim arr(1, 27) As Integer
            For i As Integer = 0 To 1
                For j As Integer = 0 To 27
                    arr(i, j) = 0
                Next
            Next
            calcSyuDic.Add(nam, arr.Clone())
            Dim arr2(1, 0) As Decimal
            For i As Integer = 0 To 1
                arr2(i, 0) = 0.0
            Next
            calcJyoDic.Add(nam, arr2)
        Next

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
            Dim syu As String = Util.checkDBNullValue(rs.Fields("YSyu").Value)
            dataArray(arrayRowIndex, 1) = syu
            '氏名
            dataArray(arrayRowIndex, 2) = Util.checkDBNullValue(rs.Fields("Nam").Value)
            '予定、変更
            dataArray(arrayRowIndex, 5) = "予定"
            dataArray(arrayRowIndex + 1, 5) = "変更"
            '1～31
            For i As Integer = 1 To 31
                Dim yotei As String = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                Dim henko As String = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                If writeType = "予定／実績" Then
                    henko = If(yotei = henko, "", henko)
                End If
                dataArray(arrayRowIndex, 5 + i) = yotei
                dataArray(arrayRowIndex + 1, 5 + i) = henko
                '集計(職種)
                If i <= 28 AndAlso canCountWork(yotei) Then
                    Dim cSyu As String = convSyu(syu) '対応する職種に変換
                    If calcSyuDic.ContainsKey(cSyu) Then
                        calcSyuDic(cSyu)(0, i - 1) += 1
                        calcSyuDic("計")(0, i - 1) += 1
                    End If
                End If
                If i <= 28 AndAlso canCountWork(henko) Then
                    Dim cSyu As String = convSyu(syu) '対応する職種に変換
                    If calcSyuDic.ContainsKey(cSyu) Then
                        calcSyuDic(cSyu)(1, i - 1) += 1
                        calcSyuDic("計")(1, i - 1) += 1
                    End If
                End If
                '集計（勤務）
                If i <= 28 Then
                    Dim cWorkY As String = convWork(yotei) '対応する集計用勤務名に変換
                    If calcSyuDic.ContainsKey(cWorkY) Then
                        calcSyuDic(cWorkY)(0, i - 1) += 1
                    End If
                    Dim cWorkH As String = convWork(henko) '対応する集計用勤務名に変換
                    If calcSyuDic.ContainsKey(cWorkH) Then
                        calcSyuDic(cWorkH)(1, i - 1) += 1
                    End If
                End If
            Next

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
            kansanY = If(CDec(kansanY) > 1.0, "1.00", kansanY)
            kansanY = If(kei = "常勤専従", "1.00", kansanY)
            If kansanY = "0.00" Then
                dataArray(arrayRowIndex, 37) = ""
            Else
                dataArray(arrayRowIndex, 37) = kansanY
            End If
            '変更
            Dim kansanH As String = (Math.Floor((totalH / KANSAN) * 100) / 100).ToString("0.00")
            kansanH = If(CDec(kansanH) > 1.0, "1.00", kansanH)
            kansanH = If(kei = "常勤専従", "1.00", kansanH)
            If kansanH = "0.00" Then
                dataArray(arrayRowIndex + 1, 37) = ""
            Else
                dataArray(arrayRowIndex + 1, 37) = If(kansanY <> kansanH, kansanH, "")
            End If
            '集計用
            Dim calcSyu As String = convSyu(syu) '対応する職種に変換
            Dim jyoY As String = kansanY
            If System.Text.RegularExpressions.Regex.IsMatch(jyoY, "^\d+(\.\d+)?$") AndAlso calcJyoDic.ContainsKey(calcSyu) Then
                calcJyoDic(calcSyu)(0, 0) += CDec(jyoY)
                calcJyoDic("計")(0, 0) += CDec(jyoY)
            End If
            Dim jyoH As String = kansanH
            If System.Text.RegularExpressions.Regex.IsMatch(jyoH, "^\d+(\.\d+)?$") AndAlso calcJyoDic.ContainsKey(calcSyu) Then
                calcJyoDic(calcSyu)(1, 0) += CDec(jyoH)
                calcJyoDic("計")(1, 0) += CDec(jyoH)
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

        '貼り付けデータに集計データを代入
        Dim lastData(,) As String = dataList(dataList.Count - 1)
        For i As Integer = 0 To 27
            '看護師
            Dim y1 As String = convNumber(calcSyuDic("看護師")(0, i))
            Dim h1 As String = convNumber(calcSyuDic("看護師")(1, i))
            lastData(34, 6 + i) = y1
            lastData(35, 6 + i) = If(y1 = h1, "", h1)
            '介護士　介護職
            Dim y2 As String = convNumber(calcSyuDic("介護士　介護職")(0, i))
            Dim h2 As String = convNumber(calcSyuDic("介護士　介護職")(1, i))
            lastData(36, 6 + i) = y2
            lastData(37, 6 + i) = If(y2 = h2, "", h2)
            '介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ
            Dim y3 As String = convNumber(calcSyuDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(0, i))
            Dim h3 As String = convNumber(calcSyuDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(1, i))
            lastData(38, 6 + i) = y3
            lastData(39, 6 + i) = If(y3 = h3, "", h3)
            '計
            Dim y4 As String = convNumber(calcSyuDic("計")(0, i))
            Dim h4 As String = convNumber(calcSyuDic("計")(1, i))
            lastData(40, 6 + i) = y4
            lastData(41, 6 + i) = If(y4 = h4, "", h4)

            '日勤
            Dim y5 As String = convNumber(calcSyuDic("日勤")(0, i))
            Dim h5 As String = convNumber(calcSyuDic("日勤")(1, i))
            lastData(42, 6 + i) = y5
            lastData(43, 6 + i) = If(y5 = h5, "", h5)
            '早遅特
            Dim y6 As String = convNumber(calcSyuDic("早遅特")(0, i))
            Dim h6 As String = convNumber(calcSyuDic("早遅特")(1, i))
            lastData(44, 6 + i) = y6
            lastData(45, 6 + i) = If(y6 = h6, "", h6)
            '半
            Dim y7 As String = convNumber(calcSyuDic("半")(0, i))
            Dim h7 As String = convNumber(calcSyuDic("半")(1, i))
            lastData(46, 6 + i) = y7
            lastData(47, 6 + i) = If(y7 = h7, "", h7)
            '直１２
            Dim y8 As String = convNumber(calcSyuDic("直１２")(0, i))
            Dim h8 As String = convNumber(calcSyuDic("直１２")(1, i))
            lastData(48, 6 + i) = y8
            lastData(49, 6 + i) = If(y8 = h8, "", h8)
            'ＡＢＣ
            Dim y9 As String = convNumber(calcSyuDic("ＡＢＣ")(0, i))
            Dim h9 As String = convNumber(calcSyuDic("ＡＢＣ")(1, i))
            lastData(50, 6 + i) = y9
            lastData(51, 6 + i) = If(y9 = h9, "", h9)
            '夜宿明
            Dim y10 As String = convNumber(calcSyuDic("夜宿明")(0, i))
            Dim h10 As String = convNumber(calcSyuDic("夜宿明")(1, i))
            lastData(52, 6 + i) = y10
            lastData(53, 6 + i) = If(y10 = h10, "", h10)
        Next
        '常勤換算集計
        '看護師
        Dim jy1 As String = convNumber(calcJyoDic("看護師")(0, 0))
        Dim jh1 As String = convNumber(calcJyoDic("看護師")(1, 0))
        lastData(34, 37) = jy1
        lastData(35, 37) = If(jy1 = jh1, "", jh1)
        '介護士　介護職
        Dim jy2 As String = convNumber(calcJyoDic("介護士　介護職")(0, 0))
        Dim jh2 As String = convNumber(calcJyoDic("介護士　介護職")(1, 0))
        lastData(36, 37) = jy2
        lastData(37, 37) = If(jy2 = jh2, "", jh2)
        '介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ
        Dim jy3 As String = convNumber(calcJyoDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(0, 0))
        Dim jh3 As String = convNumber(calcJyoDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(1, 0))
        lastData(38, 37) = jy3
        lastData(39, 37) = If(jy3 = jh3, "", jh3)
        '計
        Dim jy4 As String = convNumber(calcJyoDic("計")(0, 0))
        Dim jh4 As String = convNumber(calcJyoDic("計")(1, 0))
        lastData(40, 37) = jy4
        lastData(41, 37) = If(jy4 = jh4, "", jh4)

        'writeType:予定、実績の場合はそれぞれ不必要のデータ削除
        If writeType = "予定" Then
            '変更データを空白に
            For Each d As String(,) In dataList
                For i As Integer = 1 To 53 Step 2
                    For j As Integer = 6 To 38
                        d(i, j) = ""
                    Next
                Next
            Next
        ElseIf writeType = "実績" Then
            '予定データを空白に
            For Each d As String(,) In dataList
                For i As Integer = 0 To 52 Step 2
                    For j As Integer = 6 To 38
                        d(i, j) = ""
                    Next
                Next
            Next
        End If

        '月の日数
        Dim year As Integer = CInt(ym.Split("/")(0))
        Dim month As Integer = CInt(ym.Split("/")(1))
        Dim daysInMonth As Integer = DateTime.DaysInMonth(year, month)

        'エクセル準備
        Dim objExcel As Excel.Application = CreateObject("Excel.Application")
        Dim objWorkBooks As Excel.Workbooks = objExcel.Workbooks
        Dim objWorkBook As Excel.Workbook = objWorkBooks.Open(TopForm.excelFilePath)
        Dim oSheet As Excel.Worksheet = objWorkBook.Worksheets("勤務表B4S改")
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
            xlShapes.AddPicture(sign1Path, False, True, cell.Left + 6, cell.Top + 3, 27, 27)
        End If
        If System.IO.File.Exists(sign2Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AF"), Excel.Range)
            xlShapes.AddPicture(sign2Path, False, True, cell.Left + 6, cell.Top + 3, 27, 27)
        End If
        If System.IO.File.Exists(sign3Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AH"), Excel.Range)
            xlShapes.AddPicture(sign3Path, False, True, cell.Left + 6, cell.Top + 3, 27, 27)
        End If
        '週平均就労時間
        oSheet.Range("AN3").Value = WEEKRY_AVERAGE_TIME
        '日数
        oSheet.Range("AL4").Value = daysInMonth
        '曜日行設定
        oSheet.Range("H8", "AL8").Value = youbi
        For i As Integer = 1 To 31
            Dim day As String = youbi(i - 1)
            If day = "日" Then
                Dim column As String = workColumn(i - 1)
                oSheet.Range(column & "7", column & "62").Interior.Pattern = 17
            End If
        Next

        '必要ページ分コピペ
        For i As Integer = 0 To dataList.Count - 2
            Dim xlPasteRange As Excel.Range = oSheet.Range("A" & (64 + (63 * i))) 'ペースト先
            oSheet.Rows("1:63").copy(xlPasteRange)
            oSheet.HPageBreaks.Add(oSheet.Range("A" & (64 + (63 * i)))) '改ページ
        Next

        'データ貼り付け
        For i As Integer = 0 To dataList.Count - 1
            oSheet.Range("B" & (9 + (63 * i)), "AN" & (62 + (63 * i))).Value = dataList(i)
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
    ''' B4→A4(NC)(B4S2)印刷
    ''' </summary>
    ''' <param name="writeType">予定 or 実績 or 予定／実績</param>
    ''' <param name="rs">印刷データレコードセット</param>
    ''' <remarks></remarks>
    Private Sub printB4S2(writeType As String, rs As ADODB.Recordset)
        '集計用準備
        Dim calcSyuDic As New Dictionary(Of String, Integer(,))
        For Each nam As String In {"看護師", "介護士　介護職", "介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ", "計", "日勤", "早遅特", "半", "直１２", "ＡＢＣ", "夜宿明"}
            Dim arr(1, 27) As Integer
            For i As Integer = 0 To 1
                For j As Integer = 0 To 27
                    arr(i, j) = 0
                Next
            Next
            calcSyuDic.Add(nam, arr.Clone())
            Dim arr2(1, 0) As Decimal
            For i As Integer = 0 To 1
                arr2(i, 0) = 0.0
            Next
        Next

        '貼り付けデータ作成
        Dim dataList As New List(Of String(,))
        Dim dataArray(53, 36) As String
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
            Dim syu As String = Util.checkDBNullValue(rs.Fields("YSyu").Value)
            dataArray(arrayRowIndex, 1) = syu
            '氏名
            dataArray(arrayRowIndex, 2) = Util.checkDBNullValue(rs.Fields("Nam").Value)
            '予定、変更
            dataArray(arrayRowIndex, 5) = "予定"
            dataArray(arrayRowIndex + 1, 5) = "変更"
            '1～31
            For i As Integer = 1 To 31
                Dim yotei As String = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                Dim henko As String = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                If writeType = "予定／実績" Then
                    henko = If(yotei = henko, "", henko)
                End If
                dataArray(arrayRowIndex, 5 + i) = yotei
                dataArray(arrayRowIndex + 1, 5 + i) = henko
                '集計(職種)
                If i <= 28 AndAlso canCountWork(yotei) Then
                    Dim cSyu As String = convSyu(syu) '対応する職種に変換
                    If calcSyuDic.ContainsKey(cSyu) Then
                        calcSyuDic(cSyu)(0, i - 1) += 1
                        calcSyuDic("計")(0, i - 1) += 1
                    End If
                End If
                If i <= 28 AndAlso canCountWork(henko) Then
                    Dim cSyu As String = convSyu(syu) '対応する職種に変換
                    If calcSyuDic.ContainsKey(cSyu) Then
                        calcSyuDic(cSyu)(1, i - 1) += 1
                        calcSyuDic("計")(1, i - 1) += 1
                    End If
                End If
                '集計（勤務）
                If i <= 28 Then
                    Dim cWorkY As String = convWork(yotei) '対応する集計用勤務名に変換
                    If calcSyuDic.ContainsKey(cWorkY) Then
                        calcSyuDic(cWorkY)(0, i - 1) += 1
                    End If
                    Dim cWorkH As String = convWork(henko) '対応する集計用勤務名に変換
                    If calcSyuDic.ContainsKey(cWorkH) Then
                        calcSyuDic(cWorkH)(1, i - 1) += 1
                    End If
                End If
            Next

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

        '貼り付けデータに集計データを代入
        Dim lastData(,) As String = dataList(dataList.Count - 1)
        For i As Integer = 0 To 27
            '看護師
            Dim y1 As String = convNumber(calcSyuDic("看護師")(0, i))
            Dim h1 As String = convNumber(calcSyuDic("看護師")(1, i))
            lastData(34, 6 + i) = y1
            lastData(35, 6 + i) = If(y1 = h1, "", h1)
            '介護士　介護職
            Dim y2 As String = convNumber(calcSyuDic("介護士　介護職")(0, i))
            Dim h2 As String = convNumber(calcSyuDic("介護士　介護職")(1, i))
            lastData(36, 6 + i) = y2
            lastData(37, 6 + i) = If(y2 = h2, "", h2)
            '介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ
            Dim y3 As String = convNumber(calcSyuDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(0, i))
            Dim h3 As String = convNumber(calcSyuDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(1, i))
            lastData(38, 6 + i) = y3
            lastData(39, 6 + i) = If(y3 = h3, "", h3)
            '計
            Dim y4 As String = convNumber(calcSyuDic("計")(0, i))
            Dim h4 As String = convNumber(calcSyuDic("計")(1, i))
            lastData(40, 6 + i) = y4
            lastData(41, 6 + i) = If(y4 = h4, "", h4)

            '日勤
            Dim y5 As String = convNumber(calcSyuDic("日勤")(0, i))
            Dim h5 As String = convNumber(calcSyuDic("日勤")(1, i))
            lastData(42, 6 + i) = y5
            lastData(43, 6 + i) = If(y5 = h5, "", h5)
            '早遅特
            Dim y6 As String = convNumber(calcSyuDic("早遅特")(0, i))
            Dim h6 As String = convNumber(calcSyuDic("早遅特")(1, i))
            lastData(44, 6 + i) = y6
            lastData(45, 6 + i) = If(y6 = h6, "", h6)
            '半
            Dim y7 As String = convNumber(calcSyuDic("半")(0, i))
            Dim h7 As String = convNumber(calcSyuDic("半")(1, i))
            lastData(46, 6 + i) = y7
            lastData(47, 6 + i) = If(y7 = h7, "", h7)
            '直１２
            Dim y8 As String = convNumber(calcSyuDic("直１２")(0, i))
            Dim h8 As String = convNumber(calcSyuDic("直１２")(1, i))
            lastData(48, 6 + i) = y8
            lastData(49, 6 + i) = If(y8 = h8, "", h8)
            'ＡＢＣ
            Dim y9 As String = convNumber(calcSyuDic("ＡＢＣ")(0, i))
            Dim h9 As String = convNumber(calcSyuDic("ＡＢＣ")(1, i))
            lastData(50, 6 + i) = y9
            lastData(51, 6 + i) = If(y9 = h9, "", h9)
            '夜宿明
            Dim y10 As String = convNumber(calcSyuDic("夜宿明")(0, i))
            Dim h10 As String = convNumber(calcSyuDic("夜宿明")(1, i))
            lastData(52, 6 + i) = y10
            lastData(53, 6 + i) = If(y10 = h10, "", h10)
        Next

        'writeType:予定、実績の場合はそれぞれ不必要のデータ削除
        If writeType = "予定" Then
            '変更データを空白に
            For Each d As String(,) In dataList
                For i As Integer = 1 To 53 Step 2
                    For j As Integer = 6 To 36
                        d(i, j) = ""
                    Next
                Next
            Next
        ElseIf writeType = "実績" Then
            '予定データを空白に
            For Each d As String(,) In dataList
                For i As Integer = 0 To 52 Step 2
                    For j As Integer = 6 To 36
                        d(i, j) = ""
                    Next
                Next
            Next
        End If

        '月の日数
        Dim year As Integer = CInt(ym.Split("/")(0))
        Dim month As Integer = CInt(ym.Split("/")(1))
        Dim daysInMonth As Integer = DateTime.DaysInMonth(year, month)

        'エクセル準備
        Dim objExcel As Excel.Application = CreateObject("Excel.Application")
        Dim objWorkBooks As Excel.Workbooks = objExcel.Workbooks
        Dim objWorkBook As Excel.Workbook = objWorkBooks.Open(TopForm.excelFilePath)
        Dim oSheet As Excel.Worksheet = objWorkBook.Worksheets("勤務表B4S_2改")
        Dim xlShapes As Excel.Shapes = DirectCast(oSheet.Shapes, Excel.Shapes)
        objExcel.Calculation = Excel.XlCalculation.xlCalculationManual
        objExcel.ScreenUpdating = False

        '年月
        oSheet.Range("C3").Value = "( " & CInt(ym.Split("/")(0)) & "年 " & CInt(ym.Split("/")(1)) & "月度 )"
        '部署?
        oSheet.Range("G3").Value = hyo
        '印影
        If System.IO.File.Exists(sign1Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AB"), Excel.Range)
            xlShapes.AddPicture(sign1Path, False, True, cell.Left + 6, cell.Top + 3, 27, 27)
        End If
        If System.IO.File.Exists(sign2Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AD"), Excel.Range)
            xlShapes.AddPicture(sign2Path, False, True, cell.Left + 6, cell.Top + 3, 27, 27)
        End If
        If System.IO.File.Exists(sign3Path) Then
            Dim cell As Excel.Range = DirectCast(oSheet.Cells(3, "AF"), Excel.Range)
            xlShapes.AddPicture(sign3Path, False, True, cell.Left + 6, cell.Top + 3, 27, 27)
        End If
        '週平均就労時間
        oSheet.Range("AL3").Value = WEEKRY_AVERAGE_TIME
        '日数
        oSheet.Range("AL4").Value = daysInMonth
        '曜日行設定
        oSheet.Range("H8", "AL8").Value = youbi
        For i As Integer = 1 To 31
            Dim day As String = youbi(i - 1)
            If day = "日" Then
                Dim column As String = workColumn(i - 1)
                oSheet.Range(column & "7", column & "62").Interior.Pattern = 17
            End If
        Next

        '必要ページ分コピペ
        For i As Integer = 0 To dataList.Count - 2
            Dim xlPasteRange As Excel.Range = oSheet.Range("A" & (64 + (63 * i))) 'ペースト先
            oSheet.Rows("1:63").copy(xlPasteRange)
            oSheet.HPageBreaks.Add(oSheet.Range("A" & (64 + (63 * i)))) '改ページ
        Next

        'データ貼り付け
        For i As Integer = 0 To dataList.Count - 1
            oSheet.Range("B" & (9 + (63 * i)), "AL" & (62 + (63 * i))).Value = dataList(i)
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
    ''' キャンセルボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class