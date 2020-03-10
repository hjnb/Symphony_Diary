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
    Private Const INPUT_ROW_COUNT As Integer = 160

    '職種別集計部分
    Private Const CALCULATE_ROW_COUNT As Integer = 8

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
    Private workDic As New Dictionary(Of String, String) From {{"0", ""}, {"1", "日勤"}, {"2", "半勤"}, {"3", "早出"}, {"4", "遅出"}, {"5", "Ａ勤"}, {"6", "Ｂ勤"}, {"7", "振替"}, {"8", "夜勤"}, {"9", "宿直"}, {"10", "日直"}, {"11", "Ｃ勤"}, {"12", "明け"}, {"13", "特日"}, {"14", "研修"}, {"15", "深夜"}, {"16", "1/3勤"}, {"17", "1/3半"}, {"18", "日早"}, {"19", "日遅"}, {"20", "遅々"}, {"21", "半Ａ"}, {"22", "半Ｂ"}, {"23", "半夜"}, {"24", "半行"}, {"25", "公休"}, {"26", "有休"}, {"27", "欠勤"}}

    Private workArray() As String = {"日勤", "半勤", "早出", "遅出", "Ａ勤", "Ｂ勤", "振替", "夜勤", "宿直", "日直", "Ｃ勤", "明け", "特日", "研修", "深夜", "1/3勤", "1/3半", "日早", "日遅", "遅々", "半Ａ", "半Ｂ", "半夜", "半行"}

    '勤務時間
    Private workTimeDic As New Dictionary(Of String, String)

    '短縮勤務名Dic
    Private shortWorkDic As New Dictionary(Of String, String)

    'ラベル表示用
    Private labelList As New List(Of String)

    '勤務入力制御用
    Private workNumList As New List(Of Integer) From {0, 25, 26, 27}

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
        '名前列選択時
        colorDic.Add("SelectedNam", Color.FromKnownColor(KnownColor.Yellow))
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
        '曜日行と勤務入力行部分
        For i = 0 To INPUT_ROW_COUNT
            workDt.Rows.Add(workDt.NewRow())
        Next
        '集計部分
        For i = 1 To CALCULATE_ROW_COUNT
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
            For i As Integer = 2 To INPUT_ROW_COUNT + CALCULATE_ROW_COUNT Step 2
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
        dgvWork.setInputNumList(workNumList)
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
        '集計行の背景色設定
        setCalculateRowBackColor()

        '行番号設定
        setSeqValue()

        'データ取得、表示
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select * from KinD where Ym = '" & ym & "' and Hyo = '" & formType & "' order by Seq"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
        If rs.RecordCount <= 0 Then
            rs.Close()
            '職員マスタに登録されている人を初期表示
            sql = "select * from NamM where Kin = '" & formType & "' order by Id"
            rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
            Dim rowIndex As Integer = 1
            While Not rs.EOF
                Dim kei As String = Util.checkDBNullValue(rs.Fields("Kei").Value)
                addComboBoxItem(kei, "Kei")
                Dim syu As String = Util.checkDBNullValue(rs.Fields("Syu").Value)
                addComboBoxItem(syu, "Syu")
                Dim nam As String = Util.checkDBNullValue(rs.Fields("Nam").Value)

                dgvWork("Kei", rowIndex).Value = kei
                dgvWork("Syu", rowIndex).Value = syu
                dgvWork("Nam", rowIndex).Value = nam

                rowIndex += 2
                rs.MoveNext()
            End While
            rs.Close()
            cnn.Close()
            Return
        End If
        Dim seqCount As Integer = 2
        While Not rs.EOF
            For i As Integer = 1 To 31
                Dim kei As String = Util.checkDBNullValue(rs.Fields("YKei").Value)
                Dim syu As String = Util.checkDBNullValue(rs.Fields("YSyu").Value)
                Dim nam As String = Util.checkDBNullValue(rs.Fields("Nam").Value)
                Dim seq As Integer = seqCount
                Dim yotei As String = Util.checkDBNullValue(rs.Fields("Yotei" & i).Value)
                Dim henko As String = Util.checkDBNullValue(rs.Fields("Henko" & i).Value)
                '勤務
                addComboBoxItem(kei, "Kei")
                dgvWork("Kei", seq - 1).Value = kei
                '職種
                addComboBoxItem(syu, "Syu")
                dgvWork("Syu", seq - 1).Value = syu
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
            If seqCount > INPUT_ROW_COUNT Then
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
    ''' コンボボックスへ追加
    ''' </summary>
    ''' <param name="item">追加文字列</param>
    ''' <param name="columnName">列名</param>
    ''' <remarks></remarks>
    Private Sub addComboBoxItem(item As String, columnName As String)
        Dim cb As DataGridViewComboBoxColumn = DirectCast(dgvWork.Columns(columnName), DataGridViewComboBoxColumn)
        '存在しない場合追加
        If Not cb.Items.Contains(item) Then
            cb.Items.Add(item)
        End If
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
    ''' 集計行の背景色設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub setCalculateRowBackColor()
        For i As Integer = INPUT_ROW_COUNT + 1 To INPUT_ROW_COUNT + CALCULATE_ROW_COUNT
            For Each cell As DataGridViewCell In dgvWork.Rows(i).Cells
                cell.Style.BackColor = colorDic("Disable")
                cell.ReadOnly = True
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
        '早、遅のみ対応する文字列に変換するため
        shortWorkDic.Add("早", "早出")
        shortWorkDic.Add("遅", "遅出")
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
                    workNumList.Add(i)
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
        '勤務形態
        Dim kei As String = Util.checkDBNullValue(rowY.Cells("Kei").Value)
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
        '変更(表示はしない)
        'If totalH = 0.0 Then
        '    rowH.Cells("Tuki").Value = ""
        'Else
        '    rowH.Cells("Tuki").Value = If(totalY <> totalH, totalH, "")
        'End If

        '常勤換算後の人数
        '予定
        Dim kansanY As String = (Math.Floor((totalY / KANSAN) * 100) / 100).ToString("0.00")
        kansanY = If(CDec(kansanY) > 1.0, "1.00", kansanY)
        If kansanY = "0.00" Then
            rowY.Cells("Jyo").Value = kansanY
        Else
            If kei = "常勤専従" Then
                rowY.Cells("Jyo").Value = "1.00"
            Else
                rowY.Cells("Jyo").Value = kansanY
            End If
        End If
        '変更
        Dim kansanH As String = (Math.Floor((totalH / KANSAN) * 100) / 100).ToString("0.00")
        kansanH = If(CDec(kansanH) > 1.0, "1.00", kansanH)
        If kansanH = "0.00" Then
            rowH.Cells("Jyo").Value = kansanH
        Else
            If kei = "常勤専従" Then
                rowH.Cells("Jyo").Value = "1.00"
            Else
                rowH.Cells("Jyo").Value = kansanH
            End If
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
            '数値以外の場合、ConstMに勤務がある、且つ、振替公休有休欠勤以外は該当
            work = If(shortWorkDic.ContainsKey(work), shortWorkDic(work), work)
            If workTimeDic.ContainsKey(work) AndAlso work <> "振替" AndAlso work <> "公休" AndAlso work <> "有休" AndAlso work <> "欠勤" Then
                Return True
            Else
                Return False
            End If
        End If
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
        If workTimeDic.ContainsKey(convWork) AndAlso convWork <> "1/3勤" AndAlso convWork <> "1/3半" Then
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
    ''' 対象行に勤務の入力があるかチェック
    ''' </summary>
    ''' <param name="row">dgv行</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function existsWorkStr(row As DataGridViewRow) As Boolean
        For i As Integer = 1 To 31
            If Util.checkDBNullValue(row.Cells("Y" & i).Value) <> "" Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' 登録ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRegist_Click(sender As System.Object, e As System.EventArgs) Handles btnRegist.Click
        '入力チェック(氏名未入力行は登録不可)
        For i As Integer = 1 To INPUT_ROW_COUNT Step 2
            Dim yKei As String = Util.checkDBNullValue(dgvWork("Kei", i).Value)
            Dim ySyu As String = Util.checkDBNullValue(dgvWork("Syu", i).Value)
            Dim hKei As String = Util.checkDBNullValue(dgvWork("Kei", i + 1).Value)
            Dim hSyu As String = Util.checkDBNullValue(dgvWork("Syu", i + 1).Value)
            Dim nam As String = Util.checkDBNullValue(dgvWork("Nam", i).Value)
            Dim inputFlg As Boolean = False
            If yKei <> "" OrElse ySyu <> "" OrElse hKei <> "" OrElse hSyu <> "" OrElse existsWorkStr(dgvWork.Rows(i)) OrElse existsWorkStr(dgvWork.Rows(i + 1)) Then
                inputFlg = True
            End If
            If inputFlg AndAlso nam = "" Then
                MsgBox("氏名の無い行に入力しています。", MsgBoxStyle.Exclamation)
                Return
            End If
        Next

        '対象年月
        Dim ym As String = adBox.getADymStr()

        '既存データ削除
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim cmd As New ADODB.Command()
        cmd.ActiveConnection = cnn
        cmd.CommandText = "delete from KinD where Ym = '" & ym & "' and Hyo = '" & formType & "'"
        cmd.Execute()

        '登録
        Dim seqCount As Integer = 2
        Dim rs As New ADODB.Recordset
        rs.Open("KinD", cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        For i As Integer = 1 To INPUT_ROW_COUNT Step 2
            Dim nam As String = Util.checkDBNullValue(dgvWork("Nam", i).Value)
            If nam = "" Then
                Continue For
            End If
            rs.AddNew()
            rs.Fields("Seq").Value = seqCount
            rs.Fields("Ym").Value = ym
            rs.Fields("Hyo").Value = formType
            rs.Fields("Id").Value = 0
            rs.Fields("Nam").Value = nam
            Dim yKei As String = Util.checkDBNullValue(dgvWork("Kei", i).Value)
            Dim hKei As String = Util.checkDBNullValue(dgvWork("Kei", i + 1).Value)
            hKei = If(hKei = "", yKei, hKei)
            Dim ySyu As String = Util.checkDBNullValue(dgvWork("Syu", i).Value)
            Dim hSyu As String = Util.checkDBNullValue(dgvWork("Syu", i + 1).Value)
            hSyu = If(hSyu = "", ySyu, hSyu)
            rs.Fields("YKei").Value = yKei
            rs.Fields("YSyu").Value = ySyu
            rs.Fields("HKei").Value = hKei
            rs.Fields("HSyu").Value = hSyu
            For j As Integer = 1 To 31
                Dim yotei As String = Util.checkDBNullValue(dgvWork("Y" & j, i).Value)
                Dim henko As String = Util.checkDBNullValue(dgvWork("Y" & j, i + 1).Value)
                henko = If(henko = "", yotei, henko)
                rs.Fields("Yotei" & j).Value = yotei
                rs.Fields("Henko" & j).Value = henko
            Next
            rs.Fields("Yflg").Value = ""
            rs.Fields("Hflg").Value = ""

            seqCount += 2
        Next
        rs.Update()
        rs.Close()
        cnn.Close()

        MsgBox("登録しました。", MsgBoxStyle.Information)
    End Sub

    ''' <summary>
    ''' 換算ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnConv_Click(sender As System.Object, e As System.EventArgs) Handles btnConv.Click
        '勤務入力行部分の換算
        For i As Integer = 1 To INPUT_ROW_COUNT Step 2
            Dim nam As String = Util.checkDBNullValue(dgvWork("Nam", i).Value)
            If nam = "" Then
                Continue For
            Else
                calcWorkTime(dgvWork.Rows(i), dgvWork.Rows(i + 1))
            End If
        Next

        '集計行部分
        addComboBoxItem("看護師", "Syu")
        addComboBoxItem("介護士 介護職", "Syu")
        addComboBoxItem("介護士ﾊﾟｰﾄ 介護職ﾊﾟｰﾄ", "Syu")
        addComboBoxItem("計", "Syu")
        dgvWork.Rows(161).Cells("Syu").Value = "看護師"
        dgvWork.Rows(163).Cells("Syu").Value = "介護士 介護職"
        dgvWork.Rows(165).Cells("Syu").Value = "介護士ﾊﾟｰﾄ 介護職ﾊﾟｰﾄ"
        dgvWork.Rows(165).Cells("Syu").Style.Font = New Font("MS UI Gothic", 8)
        dgvWork.Rows(167).Cells("Syu").Value = "計"

        '集計処理
        Dim calcSyuDic As New Dictionary(Of String, Integer(,))
        Dim calcJyoDic As New Dictionary(Of String, Decimal(,))
        For Each nam As String In {"看護師", "介護士　介護職", "介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ", "計"}
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
        For i As Integer = 1 To INPUT_ROW_COUNT Step 2
            '対応する職種に変換
            Dim syu As String = convSyu(Util.checkDBNullValue(dgvWork("Syu", i).Value))
            '勤務部分
            For j As Integer = 1 To 28
                '予定勤務
                Dim workY As String = Util.checkDBNullValue(dgvWork("Y" & j, i).Value)
                workY = If(shortWorkDic.ContainsKey(workY), shortWorkDic(workY), workY)
                If canCountWork(workY) AndAlso calcSyuDic.ContainsKey(syu) Then
                    calcSyuDic(syu)(0, j - 1) += 1
                    calcSyuDic("計")(0, j - 1) += 1
                End If
                '変更勤務
                Dim workH As String = Util.checkDBNullValue(dgvWork("Y" & j, i + 1).Value)
                workH = If(workH = "", workY, workH)
                workH = If(shortWorkDic.ContainsKey(workH), shortWorkDic(workH), workH)
                If canCountWork(workH) AndAlso calcSyuDic.ContainsKey(syu) Then
                    calcSyuDic(syu)(1, j - 1) += 1
                    calcSyuDic("計")(1, j - 1) += 1
                End If
            Next
            '常勤換算
            Dim jyoY As String = Util.checkDBNullValue(dgvWork("Jyo", i).Value)
            If System.Text.RegularExpressions.Regex.IsMatch(jyoY, "^\d+(\.\d+)?$") AndAlso calcJyoDic.ContainsKey(syu) Then
                calcJyoDic(syu)(0, 0) += CDec(jyoY)
                calcJyoDic("計")(0, 0) += CDec(jyoY)
            End If
            Dim jyoH As String = Util.checkDBNullValue(dgvWork("Jyo", i + 1).Value)
            jyoH = If(jyoH = "", jyoY, jyoH)
            If System.Text.RegularExpressions.Regex.IsMatch(jyoH, "^\d+(\.\d+)?$") AndAlso calcJyoDic.ContainsKey(syu) Then
                calcJyoDic(syu)(1, 0) += CDec(jyoH)
                calcJyoDic("計")(1, 0) += CDec(jyoH)
            End If
        Next
        '集計結果表示
        If formType = "特養" OrElse formType = "ｼｮｰﾄｽﾃｲ" Then
            For i As Integer = 1 To 28
                '看護師
                Dim y1 As String = convNumber(calcSyuDic("看護師")(0, i - 1))
                Dim h1 As String = convNumber(calcSyuDic("看護師")(1, i - 1))
                dgvWork("Y" & i, 161).Value = y1
                dgvWork("Y" & i, 162).Value = If(h1 = y1, "", h1)
                '介護士　介護職
                Dim y2 As String = convNumber(calcSyuDic("介護士　介護職")(0, i - 1))
                Dim h2 As String = convNumber(calcSyuDic("介護士　介護職")(1, i - 1))
                dgvWork("Y" & i, 163).Value = y2
                dgvWork("Y" & i, 164).Value = If(h2 = y2, "", h2)
                '介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ
                Dim y3 As String = convNumber(calcSyuDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(0, i - 1))
                Dim h3 As String = convNumber(calcSyuDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(1, i - 1))
                dgvWork("Y" & i, 165).Value = y3
                dgvWork("Y" & i, 166).Value = If(h3 = y3, "", h3)
                '計
                Dim y4 As String = convNumber(calcSyuDic("計")(0, i - 1))
                Dim h4 As String = convNumber(calcSyuDic("計")(1, i - 1))
                dgvWork("Y" & i, 167).Value = y4
                dgvWork("Y" & i, 168).Value = If(h4 = y4, "", h4)
            Next
        End If
        
        '常勤換算部分
        If formType = "特養" OrElse formType = "ｼｮｰﾄｽﾃｲ" Then
            '看護師
            Dim jy1 As String = convNumber(calcJyoDic("看護師")(0, 0))
            Dim jh1 As String = convNumber(calcJyoDic("看護師")(1, 0))
            dgvWork("Jyo", 161).Value = jy1
            dgvWork("Jyo", 162).Value = If(jh1 = jy1, "", jh1)
            '介護士　介護職
            Dim jy2 As String = convNumber(calcJyoDic("介護士　介護職")(0, 0))
            Dim jh2 As String = convNumber(calcJyoDic("介護士　介護職")(1, 0))
            dgvWork("Jyo", 163).Value = jy2
            dgvWork("Jyo", 164).Value = If(jh2 = jy2, "", jh2)
            '介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ
            Dim jy3 As String = convNumber(calcJyoDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(0, 0))
            Dim jh3 As String = convNumber(calcJyoDic("介護士ﾊﾟｰﾄ　介護職ﾊﾟｰﾄ")(1, 0))
            dgvWork("Jyo", 165).Value = jy3
            dgvWork("Jyo", 166).Value = If(jh3 = jy3, "", jh3)
            '計
            Dim jy4 As String = convNumber(calcJyoDic("計")(0, 0))
            Dim jh4 As String = convNumber(calcJyoDic("計")(1, 0))
            dgvWork("Jyo", 167).Value = jy4
            dgvWork("Jyo", 168).Value = If(jh4 = jy4, "", jh4)
        End If
    End Sub

    ''' <summary>
    ''' 削除ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
        '対象年月
        Dim ym As String = adBox.getADymStr()

        'データ存在チェック
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select * from KinD where Ym = '" & ym & "' and Hyo = '" & formType & "' order by Seq"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        If rs.RecordCount <= 0 Then
            MsgBox("登録されていません。", MsgBoxStyle.Exclamation)
            rs.Close()
            cnn.Close()
            Return
        End If
        rs.Close()

        '削除
        Dim result As DialogResult = MessageBox.Show("削除してよろしいですか？", "削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = Windows.Forms.DialogResult.Yes Then
            Dim cmd As New ADODB.Command()
            cmd.ActiveConnection = cnn
            cmd.CommandText = "delete from KinD where Ym = '" & ym & "' and Hyo = '" & formType & "'"
            cmd.Execute()
            cnn.Close()
            MsgBox("削除しました。", MsgBoxStyle.Information)
        Else
            cnn.Close()
        End If
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

    ''' <summary>
    ''' CellFormattingイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvWork_CellFormatting(sender As Object, e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvWork.CellFormatting
        If e.RowIndex > 0 AndAlso dgvWork.Columns(e.ColumnIndex).Name = "Jyo" Then
            If e.RowIndex Mod 2 = 1 Then
                '予定行
                If e.Value = "0.00" Then
                    e.Value = ""
                End If
            Else
                '変更行
                If e.Value = "0.00" OrElse e.Value = Util.checkDBNullValue(dgvWork("Jyo", e.RowIndex - 1).Value) Then
                    e.Value = ""
                End If
            End If
            e.FormattingApplied = True
        End If
    End Sub

    ''' <summary>
    ''' CellMouseClickイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvWork_CellMouseClick(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvWork.CellMouseClick
        If e.RowIndex >= 1 AndAlso e.RowIndex <= INPUT_ROW_COUNT Then
            Dim columnName As String = dgvWork.Columns(e.ColumnIndex).Name
            If columnName = "Nam" Then
                '選択行の背景色設定
                setSelectedBackColor(e.RowIndex)
            End If
        End If
    End Sub

    ''' <summary>
    ''' 名前列選択時の行の背景色設定
    ''' </summary>
    ''' <param name="rowIndex">選択行index</param>
    ''' <remarks></remarks>
    Private Sub setSelectedBackColor(rowIndex As Integer)
        Dim setBackColor As Color = If(dgvWork("Nam", rowIndex).Style.BackColor = colorDic("SelectedNam"), colorDic("Default"), colorDic("SelectedNam"))
        If setBackColor = colorDic("Default") Then
            For Each cell As DataGridViewCell In dgvWork.Rows(rowIndex).Cells
                Dim columnName As String = dgvWork.Columns(cell.ColumnIndex).Name
                If columnName = "Type" Then
                    cell.Style.BackColor = colorDic("Disable")
                Else
                    Dim youbi As String = Util.checkDBNullValue(dgvWork(cell.ColumnIndex, 0).Value)
                    If youbi = "日" Then
                        cell.Style.BackColor = colorDic("Holiday")
                    Else
                        cell.Style.BackColor = setBackColor
                    End If
                End If
            Next
        Else
            For Each cell As DataGridViewCell In dgvWork.Rows(rowIndex).Cells
                cell.Style.BackColor = setBackColor
            Next
        End If
    End Sub
End Class