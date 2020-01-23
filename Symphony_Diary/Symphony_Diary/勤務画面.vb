Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices

Public Class 勤務画面

    'フォームタイプ
    Private formType As String

    '初期表示年月
    Private initYm As String

    '月の日数
    Private daysInMonth As Integer

    '常勤換算用
    '(算出式: 4週合計時間 / 157)
    '4週:28日間とする
    Private Const WEEK4 As Integer = 28
    '分母
    Private Const KANSAN As Decimal = 157.0

    '入力可能行数（勤務入力部分）
    Private Const INPUT_ROW_COUNT As Integer = 200

    '印刷 or ﾌﾟﾚﾋﾞｭｰ
    Private printState As Boolean

    '勤務割データテーブル
    Private workDt As DataTable

    '背景色
    Private colorDic As Dictionary(Of String, Color)

    '形態
    Private keiArray() As String = {"", "常勤専従", "常勤兼務", "常勤以外専従", "常勤以外兼務"}

    '職種
    Private syuArray() As String = {"", "理事長", "施設長", "副施設長", "事務局長", "部長", "課長", "係長", "主任", "管理者", "ｻｰﾋﾞｽ提供責任者", "医師", "正看護師", "准看護師", "看護職", "機能訓練士", "介護支援専門員", "生活相談員", "支援援助員", "介護職", "介護福祉士", "訪問介護員", "管理栄養士", "栄養士", "宿直"}

    '曜日配列
    Private dayCharArray() As String = {"日", "月", "火", "水", "木", "金", "土"}

    '勤務
    Private workDic As New Dictionary(Of String, String) From {{"1", "日勤"}, {"2", "半勤"}, {"3", "早出"}, {"4", "遅出"}, {"5", "Ａ勤"}, {"6", "Ｂ勤"}, {"7", "振替"}, {"8", "夜勤"}, {"9", "宿直"}, {"10", "日直"}, {"11", "Ｃ勤"}, {"12", "明け"}, {"13", "特日"}, {"14", "研修"}, {"15", "深夜"}, {"16", "1/3勤"}, {"17", "1/3半"}, {"18", "日早"}, {"19", "日遅"}, {"20", "遅々"}, {"21", "半Ａ"}, {"22", "半Ｂ"}, {"23", "半夜"}, {"24", "半行"}, {"25", "公休"}, {"26", "有休"}, {"27", "欠勤"}}

    Private workArray() As String = {"日勤", "半勤", "早出", "遅出", "Ａ勤", "Ｂ勤", "振替", "夜勤", "宿直", "日直", "Ｃ勤", "明け", "特日", "研修", "深夜", "1/3勤", "1/3半", "日早", "日遅", "遅々", "半Ａ", "半Ｂ", "半夜", "半行"}

    '勤務時間
    Private workTimeDic As New Dictionary(Of String, String)

    '短縮勤務名Dic
    Private shortWorkDic As New Dictionary(Of String, String)

    'ラベル表示用
    Private labelList As New List(Of String)

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="formType">フォームの種類</param>
    ''' <remarks></remarks>
    Public Sub New(formType As String, ym As String)
        InitializeComponent()
        Me.formType = formType
        Me.initYm = ym
        Me.Text = "Diary " & formType & " 勤務表"
    End Sub

    ''' <summary>
    ''' loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub 勤務画面_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized

        '印刷orﾌﾟﾚﾋﾞｭｰ
        Dim state As String = Util.getIniString("System", "Printer", TopForm.iniFilePath)
        printState = If(state = "Y", True, False)

        'セル背景色作成
        createCellColor()

        'データグリッドビュー初期設定
        initDgvWork()

        'データ表示
        adBox.setADStr(initYm & "/01")
        displayDgvWork(initYm)

        dgvWork.canCellEnter = True

        '勤務項目名マスタ読み込み
        loadKmkM()

        '定数マスタ読み込み
        loadConstM()
    End Sub

    ''' <summary>
    ''' セル背景色作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub createCellColor()
        colorDic = New Dictionary(Of String, Color)
        'Default
        colorDic.Add("Default", Color.FromKnownColor(KnownColor.Window))
        'Disable
        colorDic.Add("Disable", Color.FromKnownColor(KnownColor.Control))
        '日曜 or 祝日
        colorDic.Add("Holiday", Color.FromArgb(255, 200, 200))
    End Sub

    ''' <summary>
    ''' ラベル作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub createLabel()
        '定数マスタに0以外の数値が登録されている勤務のラベルを作成

        'デフォルト表示　0：ｸﾘｱ、25：公休、26：有休、27：欠勤
        Dim zeroLabel As New Label()
        zeroLabel.ForeColor = System.Drawing.Color.Blue
        zeroLabel.Location = New System.Drawing.Point(7, 10)
        zeroLabel.Name = "label0"
        zeroLabel.Size = New System.Drawing.Size(44, 12)
        zeroLabel.TabIndex = 0
        zeroLabel.Text = "0 ： ｸﾘｱ"
        labelPanel.Controls.Add(zeroLabel)
        Dim label25 As New Label()
        label25.ForeColor = System.Drawing.Color.Blue
        label25.Location = New System.Drawing.Point(722, 33)
        label25.Name = "label25"
        label25.Size = New System.Drawing.Size(55, 12)
        label25.TabIndex = 25
        label25.Text = "25 ： 公休"
        labelPanel.Controls.Add(label25)
        Dim label26 As New Label()
        label26.ForeColor = System.Drawing.Color.Blue
        label26.Location = New System.Drawing.Point(787, 33)
        label26.Name = "label26"
        label26.Size = New System.Drawing.Size(55, 12)
        label26.TabIndex = 26
        label26.Text = "26 ： 有休"
        labelPanel.Controls.Add(label26)
        Dim label27 As New Label()
        label27.ForeColor = System.Drawing.Color.Blue
        label27.Location = New System.Drawing.Point(852, 33)
        label27.Name = "label27"
        label27.Size = New System.Drawing.Size(55, 12)
        label27.TabIndex = 27
        label27.Text = "27 ： 欠勤"
        labelPanel.Controls.Add(label27)

        '定数マスタ読み込み時に作成したラベルリストで作成
        For i As Integer = 1 To labelList.Count
            Dim workLabel As New Label()
            workLabel.TextAlign = ContentAlignment.MiddleLeft
            workLabel.ForeColor = System.Drawing.Color.Blue
            workLabel.Name = "workLabel" & i
            workLabel.Size = New System.Drawing.Size(65, 12)
            workLabel.TabIndex = i * 10
            workLabel.Text = labelList(i - 1)
            If i <= 13 Then
                workLabel.Location = New System.Drawing.Point(7 + (65 * i), 10)
            Else
                workLabel.Location = New System.Drawing.Point(7 + (65 * (i - 13)), 33)
            End If
            labelPanel.Controls.Add(workLabel)
        Next
    End Sub

    ''' <summary>
    ''' データグリッドビュー初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initDgvWork()
        Util.EnableDoubleBuffering(dgvWork)
        With dgvWork
            .AllowUserToAddRows = False '行追加禁止
            .AllowUserToResizeColumns = False '列の幅をユーザーが変更できないようにする
            .AllowUserToResizeRows = False '行の高さをユーザーが変更できないようにする
            .AllowUserToDeleteRows = False '行削除禁止
            .RowHeadersVisible = False '行ヘッダー非表示
            .SelectionMode = DataGridViewSelectionMode.CellSelect
            .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .BackgroundColor = Color.FromKnownColor(KnownColor.Control)
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .RowTemplate.Height = 17
            .ColumnHeadersHeight = 19
            .ShowCellToolTips = False
            .EnableHeadersVisualStyles = False
            .DefaultCellStyle.Font = New Font("MS UI Gothic", 9)
        End With

        dgvWork.Columns.Clear()
        workDt = New DataTable()

        '列定義
        workDt.Columns.Add("Seq", GetType(String))
        workDt.Columns.Add("Ym", GetType(String))
        workDt.Columns.Add("Nam", GetType(String))
        workDt.Columns.Add("Type", GetType(String))
        For i As Integer = 1 To 31
            workDt.Columns.Add("Y" & i, GetType(String))
        Next
        workDt.Columns.Add("Kan", GetType(String))
        workDt.Columns.Add("Jyo", GetType(String))
        workDt.Columns.Add("Tuki", GetType(String))
        workDt.Columns.Add("Sou", GetType(String))

        '空行追加
        For i = 0 To INPUT_ROW_COUNT
            workDt.Rows.Add(workDt.NewRow())
        Next

        '表示
        dgvWork.DataSource = workDt

        'Kei,Syu列追加(コンボボックス列)
        Dim keiColumn As New DataGridViewComboBoxColumn()
        keiColumn.Items.AddRange(keiArray)
        keiColumn.Name = "Kei"
        keiColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
        keiColumn.FlatStyle = FlatStyle.Popup
        dgvWork.Columns.Insert(2, keiColumn)
        Dim syuColumn As New DataGridViewComboBoxColumn()
        syuColumn.Items.AddRange(syuArray)
        syuColumn.Name = "Syu"
        syuColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
        syuColumn.FlatStyle = FlatStyle.Popup
        dgvWork.Columns.Insert(3, syuColumn)

        '幅設定等
        With dgvWork
            '非表示列
            .Columns("Seq").Visible = False
            .Columns("Ym").Visible = False

            '行固定
            .Rows(0).Frozen = True

            '列固定
            .Columns("Type").Frozen = True

            '並び替え禁止
            For Each c As DataGridViewColumn In .Columns
                c.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            '形態
            With .Columns("Kei")
                .Width = 100
                .HeaderText = "形態"
                .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With

            '種別列
            With .Columns("Syu")
                .Width = 140
                .HeaderText = "職種"
                .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With

            '氏名列
            With .Columns("Nam")
                .Width = 92
                .HeaderText = "氏名"
                .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
            End With

            '予定or変更列
            With .Columns("Type")
                .Width = 32
                .HeaderText = ""
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ReadOnly = True
            End With

            'Y1～Y31の列
            For i As Integer = 1 To 31
                With .Columns("Y" & i)
                    .Width = 47
                    .HeaderText = i.ToString()
                    .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End With
            Next
            '変更行の文字を赤に
            For i As Integer = 2 To INPUT_ROW_COUNT Step 2
                For j As Integer = 1 To 31
                    dgvWork("Y" & j, i).Style.ForeColor = Color.Red
                    dgvWork("Y" & j, i).Style.SelectionForeColor = Color.Red
                Next
                dgvWork("Jyo", i).Style.ForeColor = Color.Red
                dgvWork("Jyo", i).Style.SelectionForeColor = Color.Red
                dgvWork("Tuki", i).Style.ForeColor = Color.Red
                dgvWork("Tuki", i).Style.SelectionForeColor = Color.Red
                dgvWork("Sou", i).Style.ForeColor = Color.Red
                dgvWork("Sou", i).Style.SelectionForeColor = Color.Red
            Next

            '換算
            With .Columns("Kan")
                .Width = 35
                .HeaderText = "換算"
                .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ReadOnly = True
            End With

            '常勤換算
            With .Columns("Jyo")
                .Width = 70
                .HeaderText = "常勤換算"
                .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ReadOnly = True
            End With

            '月合計
            With .Columns("Tuki")
                .Width = 70
                .HeaderText = "月合計"
                .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ReadOnly = True
            End With

            '総勤日数
            With .Columns("Sou")
                .Width = 70
                .HeaderText = "総勤日数"
                .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ReadOnly = True
            End With
        End With

        dgvWork.setWordDictionary(workDic)
    End Sub

    ''' <summary>
    ''' 入力内容クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub clearInput()
        For Each row As DataGridViewRow In dgvWork.Rows
            For Each cell As DataGridViewCell In row.Cells
                cell.Value = ""
                If dgvWork.Columns(cell.ColumnIndex).Name = "Type" Then
                    cell.Style.BackColor = colorDic("Disable")
                Else
                    cell.Style.BackColor = colorDic("Default")
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' データ表示
    ''' </summary>
    ''' <param name="ym">年月(yyyy/MM)</param>
    ''' <remarks></remarks>
    Private Sub displayDgvWork(ym As String)
        '入力内容クリア
        clearInput()

        '曜日行設定
        setDayCharRow(ym)
        '日曜日列の背景色設定
        setHolidayColumnColor()

        '行番号設定
        setSeqValue()

        'データ取得、表示
        Dim seqCount As Integer = 2
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select * from KinD where Ym = '" & ym & "' and Hyo = '" & formType & "' order by Seq"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        While Not rs.EOF
            For i As Integer = 1 To 31
                Dim kei As String = Util.checkDBNullValue(rs.Fields("YKei").Value)
                Dim syu As String = Util.checkDBNullValue(rs.Fields("YSyu").Value)
                Dim nam As String = Util.checkDBNullValue(rs.Fields("Nam").Value)
                Dim seq As Integer = seqCount
                Dim yotei As String = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                Dim henko As String = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                '勤務
                'dgvWork("Kei", seq - 1).Value = kei
                '職種
                'dgvWork("Syu", seq - 1).Value = syu
                '氏名
                dgvWork("Nam", seq - 1).Value = nam
                '予定変更列
                dgvWork("Type", seq - 1).Value = "予定"
                dgvWork("Type", seq).Value = "変更"
                '1～31
                dgvWork("Y" & i, seq - 1).Value = yotei
                If yotei <> henko Then
                    dgvWork("Y" & i, seq).Value = henko
                End If
            Next
            seqCount += 2
            If seqCount >= INPUT_ROW_COUNT Then
                Exit While
            End If
            rs.MoveNext()
        End While
        rs.Close()
        cnn.Close()
    End Sub

    ''' <summary>
    ''' 表示ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDisplay_Click(sender As System.Object, e As System.EventArgs) Handles btnDisplay.Click
        Dim ym As String = adBox.getADymStr()
        displayDgvWork(ym)
    End Sub

    ''' <summary>
    ''' 曜日行作成
    ''' </summary>
    ''' <param name="ym">年月(yyyy/MM)</param>
    ''' <remarks></remarks>
    Private Sub setDayCharRow(ym As String)
        If Not System.Text.RegularExpressions.Regex.IsMatch(ym, "\d\d\d\d/\d\d") Then
            Return
        End If
        Dim year As Integer = CInt(ym.Split("/")(0))
        Dim month As Integer = CInt(ym.Split("/")(1))
        daysInMonth = DateTime.DaysInMonth(year, month) '月の日数
        Dim firstDay As DateTime = New DateTime(year, month, 1)
        Dim weekNumber As Integer = CInt(firstDay.DayOfWeek) '月の初日の曜日のindex
        Dim row As DataRow = workDt.Rows(0)
        For i As Integer = 1 To daysInMonth
            row("Y" & i) = dayCharArray((weekNumber + (i - 1)) Mod 7)
        Next
        '曜日行の背景色設定
        For Each cell As DataGridViewCell In dgvWork.Rows(0).Cells
            cell.Style.BackColor = colorDic("Disable")
            cell.ReadOnly = True
        Next
    End Sub

    ''' <summary>
    ''' 日曜日列の背景色設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setHolidayColumnColor()
        Dim targetIndex As New HashSet(Of String)
        For i As Integer = 1 To 31
            Dim youbi As String = Util.checkDBNullValue(dgvWork("Y" & i, 0).Value)
            If youbi = "日" Then
                targetIndex.Add(i.ToString())
            End If
        Next

        For Each row As DataGridViewRow In dgvWork.Rows
            For Each index As String In targetIndex
                row.Cells("Y" & index).Style.BackColor = colorDic("Holiday")
            Next
        Next
    End Sub

    ''' <summary>
    ''' 行挿入ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRowInsert_Click(sender As System.Object, e As System.EventArgs) Handles btnRowInsert.Click
        Dim selectedRowIndex As Integer = If(IsNothing(dgvWork.CurrentRow), -1, dgvWork.CurrentRow.Index) '選択行index
        If selectedRowIndex = -1 OrElse selectedRowIndex = 0 OrElse selectedRowIndex >= INPUT_ROW_COUNT + 1 Then
            Return
        ElseIf Not IsDBNull(workDt.Rows(INPUT_ROW_COUNT - 1).Item("Nam")) AndAlso workDt.Rows(INPUT_ROW_COUNT - 1).Item("Nam") <> "" Then
            '一番下の予定行に既に名前が入力されている場合は行挿入禁止
            MsgBox("行挿入できません。", MsgBoxStyle.Exclamation)
            Return
        Else
            '変更の行を選択してる場合は予定の行を選択しているindexとする
            If selectedRowIndex Mod 2 = 0 Then
                selectedRowIndex -= 1
            End If

            Dim rowJ As DataRow = workDt.NewRow()
            Dim rowY As DataRow = workDt.NewRow()
            rowY("Seq") = selectedRowIndex + 1

            '行追加
            workDt.Rows.InsertAt(rowJ, selectedRowIndex) '変更行
            workDt.Rows.InsertAt(rowY, selectedRowIndex) '予定行

            '追加した行の設定
            dgvWork("Type", selectedRowIndex).Style.BackColor = colorDic("Disable")
            dgvWork("Type", selectedRowIndex + 1).Style.BackColor = colorDic("Disable")
            For i As Integer = 1 To 31
                dgvWork("Y" & i, selectedRowIndex + 1).Style.ForeColor = Color.Red
                dgvWork("Y" & i, selectedRowIndex + 1).Style.SelectionForeColor = Color.Red
            Next
            dgvWork("Jyo", selectedRowIndex + 1).Style.ForeColor = Color.Red
            dgvWork("Jyo", selectedRowIndex + 1).Style.SelectionForeColor = Color.Red
            dgvWork("Tuki", selectedRowIndex + 1).Style.ForeColor = Color.Red
            dgvWork("Tuki", selectedRowIndex + 1).Style.SelectionForeColor = Color.Red
            dgvWork("Sou", selectedRowIndex + 1).Style.ForeColor = Color.Red
            dgvWork("Sou", selectedRowIndex + 1).Style.SelectionForeColor = Color.Red
            setHolidayColumnColor()

            '追加された行以降のSeqの値を更新
            For i As Integer = selectedRowIndex + 2 To INPUT_ROW_COUNT - 1 Step 2
                workDt.Rows(i).Item("Seq") += 2
            Next

            '下から２行削除
            workDt.Rows.RemoveAt(INPUT_ROW_COUNT + 2)
            workDt.Rows.RemoveAt(INPUT_ROW_COUNT + 1)
        End If
    End Sub

    ''' <summary>
    ''' 行削除ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRowDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnRowDelete.Click
        Dim selectedRowIndex As Integer = If(IsNothing(dgvWork.CurrentRow), -1, dgvWork.CurrentRow.Index) '選択行index
        If selectedRowIndex = -1 OrElse selectedRowIndex = 0 OrElse selectedRowIndex >= INPUT_ROW_COUNT + 1 Then
            Return
        Else
            '変更の行を選択してる場合は予定の行を選択しているindexとする
            If selectedRowIndex Mod 2 = 0 Then
                selectedRowIndex -= 1
            End If

            '行削除
            workDt.Rows.RemoveAt(selectedRowIndex)
            workDt.Rows.RemoveAt(selectedRowIndex)

            '削除された行以降のSeqの値を更新
            For i As Integer = selectedRowIndex To INPUT_ROW_COUNT - 3 Step 2
                workDt.Rows(i).Item("Seq") -= 2
            Next

            '下に２行追加
            Dim row As DataRow = workDt.NewRow()
            row("Seq") = INPUT_ROW_COUNT
            workDt.Rows.InsertAt(workDt.NewRow(), INPUT_ROW_COUNT - 1)
            workDt.Rows.InsertAt(row, INPUT_ROW_COUNT - 1)

            '追加した行の設定
            dgvWork("Type", INPUT_ROW_COUNT - 1).Style.BackColor = colorDic("Disable")
            dgvWork("Type", INPUT_ROW_COUNT).Style.BackColor = colorDic("Disable")
            For i As Integer = 1 To 31
                dgvWork("Y" & i, INPUT_ROW_COUNT).Style.ForeColor = Color.Red
                dgvWork("Y" & i, INPUT_ROW_COUNT).Style.SelectionForeColor = Color.Red
            Next
            dgvWork("Jyo", INPUT_ROW_COUNT).Style.ForeColor = Color.Red
            dgvWork("Jyo", INPUT_ROW_COUNT).Style.SelectionForeColor = Color.Red
            dgvWork("Tuki", INPUT_ROW_COUNT).Style.ForeColor = Color.Red
            dgvWork("Tuki", INPUT_ROW_COUNT).Style.SelectionForeColor = Color.Red
            dgvWork("Sou", INPUT_ROW_COUNT).Style.ForeColor = Color.Red
            dgvWork("Sou", INPUT_ROW_COUNT).Style.SelectionForeColor = Color.Red
            setHolidayColumnColor()
        End If
    End Sub

    ''' <summary>
    ''' 行番号(seq)セット
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setSeqValue()
        For i As Integer = 1 To INPUT_ROW_COUNT Step 2
            workDt.Rows(i).Item("Seq") = i + 1
        Next
    End Sub

    ''' <summary>
    ''' 勤務項目名マスタ読み込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub loadKmkM()
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select Ent, Prt from KmkM where Kin = '" & formType & "'"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        While Not rs.EOF
            Dim ent As String = Util.checkDBNullValue(rs.Fields("Ent").Value)
            Dim prt As String = Util.checkDBNullValue(rs.Fields("Prt").Value)
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
        If formType = "特養" Then
            hyoNum = "1"
        ElseIf formType = "事務" Then
            hyoNum = "2"
        ElseIf formType = "ｼｮｰﾄｽﾃｲ" Then
            hyoNum = "3"
        ElseIf formType = "ﾃﾞｲｻｰﾋﾞｽ" Then
            hyoNum = "4"
        ElseIf formType = "ﾍﾙﾊﾟｰｽﾃｰｼｮﾝ" Then
            hyoNum = "5"
        ElseIf formType = "居宅介護支援" Then
            hyoNum = "6"
        ElseIf formType = "老人介護支援ｾﾝﾀｰ" Then
            hyoNum = "7"
        ElseIf formType = "生活支援ﾊｳｽ" Then
            hyoNum = "8"
        End If

        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim sql As String = "select * from ConstM"
        Dim rs As New ADODB.Recordset
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        While Not rs.EOF
            For i As Integer = 1 To 24
                Dim time As String = rs.Fields("J" & i & hyoNum).Value.ToString()
                workTimeDic.Add(workArray(i - 1), time)
                If time <> "0" Then
                    labelList.Add(i & " ： " & workArray(i - 1))
                End If
            Next
            rs.MoveNext()
        End While
        rs.Close()
        cnn.Close()

        'ラベル作成
        createLabel()
    End Sub

    ''' <summary>
    ''' 換算処理(常勤換算後人数、月合計、総勤日数)
    ''' </summary>
    ''' <param name="rowY">予定行</param>
    ''' <param name="rowH">変更行</param>
    ''' <remarks></remarks>
    Private Sub calcWorkTime(rowY As DataGridViewRow, rowH As DataGridViewRow)
        '月合計
        Dim totalY As Decimal = 0.0
        Dim totalH As Decimal = 0.0
        For i As Integer = 1 To WEEK4
            '予定
            Dim workY As String = Util.checkDBNullValue(rowY.Cells("Y" & i).Value)
            totalY += convWorkTime(workY)
            '変更
            Dim workH As String = Util.checkDBNullValue(rowH.Cells("Y" & i).Value)
            workH = If(workH = "", workY, workH)
            totalH += convWorkTime(workH)
        Next
        '予定
        If totalY = 0.0 Then
            rowY.Cells("Tuki").Value = ""
        Else
            rowY.Cells("Tuki").Value = totalY
        End If
        '変更
        If totalH = 0.0 Then
            rowH.Cells("Tuki").Value = ""
        Else
            rowH.Cells("Tuki").Value = If(totalY <> totalH, totalH, "")
        End If

        '常勤換算後の人数
        '予定
        Dim kansanY As String = (Math.Floor((totalY / KANSAN) * 100) / 100).ToString("0.00")
        If kansanY = "0.00" Then
            rowY.Cells("Jyo").Value = ""
        Else
            rowY.Cells("Jyo").Value = kansanY
        End If
        '変更
        Dim kansanH As String = (Math.Floor((totalH / KANSAN) * 100) / 100).ToString("0.00")
        If kansanH = "0.00" Then
            rowH.Cells("Jyo").Value = ""
        Else
            rowH.Cells("Jyo").Value = If(kansanY <> kansanH, kansanH, "")
        End If

        '総勤日数
        Dim totalWorkY As Integer = 0
        Dim totalWorkH As Integer = 0
        For i As Integer = 1 To 31
            '予定
            Dim workY As String = Util.checkDBNullValue(rowY.Cells("Y" & i).Value)
            totalWorkY = If(isWorked(workY), totalWorkY + 1, totalWorkY)
            '変更
            Dim workH As String = Util.checkDBNullValue(rowH.Cells("Y" & i).Value)
            workH = If(workH = "", workY, workH)
            totalWorkH = If(isWorked(workH), totalWorkH + 1, totalWorkH)
        Next
        If totalWorkY = 0 Then
            rowY.Cells("Sou").Value = ""
        Else
            rowY.Cells("Sou").Value = totalWorkY
        End If
        If totalWorkH = 0 Then
            rowH.Cells("Sou").Value = ""
        Else
            If totalWorkY <> totalWorkH Then
                rowH.Cells("Sou").Value = totalWorkH
            End If
        End If
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
    ''' 勤務日数に該当するか
    ''' </summary>
    ''' <param name="work">勤務名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function isWorked(work As String) As Boolean
        If System.Text.RegularExpressions.Regex.IsMatch(work, "^\d+(\.\d)?$") Then
            '数値の場合、該当
            Return True
        Else
            '数値以外の場合、ConstMに勤務がある、且つ、公休有休欠勤以外は該当
            work = If(shortWorkDic.ContainsKey(work), shortWorkDic(work), work)
            If workTimeDic.ContainsKey(work) AndAlso work <> "公休" AndAlso work <> "有休" AndAlso work <> "欠勤" Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    ''' <summary>
    ''' 登録ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRegist_Click(sender As System.Object, e As System.EventArgs) Handles btnRegist.Click

    End Sub

    ''' <summary>
    ''' 換算ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnConv_Click(sender As System.Object, e As System.EventArgs) Handles btnConv.Click
        For i As Integer = 1 To dgvWork.Rows.Count - 1 Step 2
            Dim nam As String = Util.checkDBNullValue(dgvWork("Nam", i).Value)
            If nam = "" Then
                Continue For
            Else
                calcWorkTime(dgvWork.Rows(i), dgvWork.Rows(i + 1))
            End If
        Next
    End Sub

    ''' <summary>
    ''' 削除ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click

    End Sub

    ''' <summary>
    ''' 印刷ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrint_Click(sender As System.Object, e As System.EventArgs) Handles btnPrint.Click
        '印刷条件フォーム表示
        Dim ym As String = adBox.getADymStr()
        Dim youbi(30) As String
        For i As Integer = 1 To 31
            youbi(i - 1) = Util.checkDBNullValue(dgvWork("Y" & i, 0).Value)
        Next
        Dim printForm As New 勤務表印刷条件(ym, formType, youbi)
        printForm.ShowDialog()
    End Sub

    ''' <summary>
    ''' 個人別ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPersonal_Click(sender As System.Object, e As System.EventArgs) Handles btnPersonal.Click
        '管理者パスワードフォーム表示
        Dim passForm As Form = New passwordForm(TopForm.iniFilePath, 1)
        If passForm.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            Return
        End If

        '勤務データ取得
        Dim ym As String = adBox.getADymStr() '選択年月
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select * from KinD where Ym = '" & ym & "' and Hyo = '" & formType & "' order by Seq"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
        Dim personCount As Integer = rs.RecordCount '人数
        If personCount <= 0 Then
            MsgBox("該当がありません。", MsgBoxStyle.Exclamation)
            rs.Close()
            cnn.Close()
            Return
        End If

        '貼り付け用データ作成
        Dim year As Integer = CInt(ym.Split("/")(0))
        Dim month As Integer = CInt(ym.Split("/")(1))
        Dim daysInMonth As Integer = DateTime.DaysInMonth(year, month) '月の日数
        Dim firstDay As DateTime = New DateTime(year, month, 1)
        Dim weekNumber As Integer = CInt(firstDay.DayOfWeek) '初日の曜日のindex
        Dim namList As New List(Of String)
        Dim dataList As New List(Of String(,))
        Dim dataDic As New Dictionary(Of String, String(,))
        While Not rs.EOF
            Dim yVal, hVal As String
            Dim numIndex As Integer = weekNumber
            Dim workData(17, 6) As String
            For i As Integer = 1 To daysInMonth
                workData((numIndex \ 7) * 3, numIndex Mod 7) = i '日にち
                yVal = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                hVal = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                workData((numIndex \ 7) * 3 + 1, numIndex Mod 7) = yVal '予定
                If yVal <> hVal Then
                    workData((numIndex \ 7) * 3 + 2, numIndex Mod 7) = hVal '変更
                End If
                numIndex += 1
            Next
            Dim nam As String = Util.checkDBNullValue(rs.Fields("Nam").Value)
            namList.Add(nam)
            dataList.Add(workData.Clone())
            rs.MoveNext()
        End While
        rs.Close()
        cnn.Close()

        'エクセル準備
        Dim objExcel As Excel.Application = CreateObject("Excel.Application")
        Dim objWorkBooks As Excel.Workbooks = objExcel.Workbooks
        Dim objWorkBook As Excel.Workbook = objWorkBooks.Open(TopForm.excelFilePath)
        Dim oSheet As Excel.Worksheet = objWorkBook.Worksheets("ｶﾚﾝﾀﾞｰ改")
        objExcel.Calculation = Excel.XlCalculation.xlCalculationManual
        objExcel.ScreenUpdating = False

        '年月
        oSheet.Range("C1").Value = year & "年" & month & "月"
        oSheet.Range("C31").Value = year & "年" & month & "月"

        '人数分の枠準備
        Dim forCount As Integer
        For forCount = 1 To ((personCount - 1) \ 2)
            'コピペ処理
            Dim xlPasteRange As Excel.Range = oSheet.Range("A" & ((forCount * 53) + 1)) 'ペースト先
            oSheet.Rows("1:53").copy(xlPasteRange)
        Next

        'データ貼り付け
        Dim count As Integer = 1
        For i As Integer = 0 To namList.Count - 1
            Dim nam As String = namList(i) '氏名
            Dim workData(,) As String = dataList(i) '勤務データ
            If (count Mod 2) = 1 Then
                'ページ上部
                oSheet.Range("E" & (53 * (count \ 2) + 1)).Value = nam
                oSheet.Range("B" & (53 * (count \ 2) + 4), "H" & (53 * (count \ 2) + 21)).Value = workData
            Else
                'ページ下部
                oSheet.Range("E" & (53 * ((count - 1) \ 2) + 31)).Value = nam
                oSheet.Range("B" & (53 * ((count - 1) \ 2) + 34), "H" & (53 * ((count - 1) \ 2) + 51)).Value = workData
            End If
            count += 1
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
End Class