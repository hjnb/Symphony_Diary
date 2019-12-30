Public Class 勤務画面

    'フォームタイプ
    Private formType As String

    '入力可能行数（勤務入力部分）
    Private Const INPUT_ROW_COUNT As Integer = 100

    '勤務割データテーブル
    Private workDt As DataTable

    '背景色
    Private colorDic As Dictionary(Of String, Color)

    '形態
    Private keiArray() As String = {"常勤専従", "常勤兼務", "常勤以外専従", "常勤以外兼務"}

    '職種
    Private syuArray() As String = {"理事長", "施設長", "副施設長", "事務局長", "部長", "課長", "係長", "主任", "管理者", "ｻｰﾋﾞｽ提供責任者", "医師", "正看護師", "准看護師", "看護職", "機能訓練士", "介護支援専門員", "生活相談員", "支援援助員", "介護職", "介護福祉士", "訪問介護員", "管理栄養士", "栄養士", "宿直"}

    '曜日配列
    Private dayCharArray() As String = {"日", "月", "火", "水", "木", "金", "土"}

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

    ''' <summary>
    ''' loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub 勤務画面_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized

        'セル背景色作成
        createCellColor()

        'データグリッドビュー初期設定
        initDgvWork()

        '当月のデータ表示
        displayDgvWork(Today.ToString("yyyy/MM"))
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
                    .Width = 43
                    .HeaderText = i.ToString()
                    .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End With
            Next
        End With
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
        Dim daysInMonth As Integer = DateTime.DaysInMonth(year, month) '月の日数
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
End Class