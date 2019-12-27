Public Class 定数マスタ

    Private kinArray() As String = {"特養", "事務", "ｼｮｰﾄｽﾃｲ", "ﾃﾞｲｻｰﾋﾞｽ", "ﾍﾙﾊﾟｰｽﾃｰｼｮﾝ", "居宅介護支援", "老人介護支援ｾﾝﾀｰ", "生活支援ﾊｳｽ"}
    Private workArray() As String = {"日勤", "半勤", "早出", "遅出", "Ａ勤", "Ｂ勤", "振替", "夜勤", "宿直", "日直", "Ｃ勤", "明け", "特日", "研修", "深夜", "1/3勤", "1/3半", "日早", "日遅", "遅々", "半Ａ", "半Ｂ", "半夜", "半行"}

    Private disableCellColor As Color = Color.FromKnownColor(KnownColor.Control)

    Private constMDt As DataTable

    ''' <summary>
    ''' 行ヘッダーのカレントセルを表す三角マークを非表示に設定する為のクラス。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class dgvRowHeaderCell

        'DataGridViewRowHeaderCell を継承
        Inherits DataGridViewRowHeaderCell

        'DataGridViewHeaderCell.Paint をオーバーライドして行ヘッダーを描画
        Protected Overrides Sub Paint(ByVal graphics As Graphics, ByVal clipBounds As Rectangle, _
           ByVal cellBounds As Rectangle, ByVal rowIndex As Integer, ByVal cellState As DataGridViewElementStates, _
           ByVal value As Object, ByVal formattedValue As Object, ByVal errorText As String, _
           ByVal cellStyle As DataGridViewCellStyle, ByVal advancedBorderStyle As DataGridViewAdvancedBorderStyle, _
           ByVal paintParts As DataGridViewPaintParts)
            '標準セルの描画からセル内容の背景だけ除いた物を描画(-5)
            MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, _
                     formattedValue, errorText, cellStyle, advancedBorderStyle, _
                     Not DataGridViewPaintParts.ContentBackground)
        End Sub

    End Class

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
    End Sub

    ''' <summary>
    ''' loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub 定数マスタ_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'データグリッドビュー初期設定
        initDgvConstM()

        'データ表示
        displayDgvConstM()
    End Sub

    ''' <summary>
    ''' データグリッドビュー初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initDgvConstM()
        Util.EnableDoubleBuffering(dgvConstM)
        With dgvConstM
            .AllowUserToAddRows = False '行追加禁止
            .AllowUserToResizeColumns = False '列の幅をユーザーが変更できないようにする
            .AllowUserToResizeRows = False '行の高さをユーザーが変更できないようにする
            .AllowUserToDeleteRows = False '行削除禁止
            .RowHeadersWidth = 115
            .SelectionMode = DataGridViewSelectionMode.CellSelect
            .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .BackgroundColor = Color.FromKnownColor(KnownColor.Control)
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .RowTemplate.Height = 18
            .ColumnHeadersHeight = 19
            .ShowCellToolTips = False
            .EnableHeadersVisualStyles = False
            .RowTemplate.HeaderCell = New dgvRowHeaderCell() '行ヘッダの三角マークを非表示に
            .DefaultCellStyle.Font = New Font("MS UI Gothic", 9)
            .ImeMode = Windows.Forms.ImeMode.Disable
        End With

        dgvConstM.Columns.Clear()
        constMDt = New DataTable()

        '列定義
        For i As Integer = 1 To 24
            constMDt.Columns.Add("J" & i, GetType(String))
        Next

        '空行追加
        For i = 0 To 8
            constMDt.Rows.Add(constMDt.NewRow())
        Next

        '表示
        dgvConstM.DataSource = constMDt

        '幅設定等
        With dgvConstM
            '並び替え禁止
            For Each c As DataGridViewColumn In .Columns
                c.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            '1～24の列
            For i As Integer = 1 To 24
                With .Columns("J" & i)
                    .Width = 38
                    .HeaderText = i.ToString()
                    .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End With
            Next
        End With

        '勤務名行
        For i As Integer = 1 To 24
            dgvConstM("J" & i, 0).Value = workArray(i - 1)
            dgvConstM("J" & i, 0).Style.BackColor = disableCellColor
            dgvConstM("J" & i, 0).ReadOnly = True
        Next

        '左上セル
        dgvConstM.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub

    ''' <summary>
    ''' 内容クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub clearInput()
        For i As Integer = 1 To dgvConstM.Rows.Count - 1
            For Each cell As DataGridViewCell In dgvConstM.Rows(i).Cells
                cell.Value = ""
            Next
        Next
    End Sub

    ''' <summary>
    ''' データ表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub displayDgvConstM()
        '内容クリア
        clearInput()

        'データ取得、表示
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select top 1 * from ConstM"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        While Not rs.EOF
            Dim ymd As String = Util.checkDBNullValue(rs.Fields("Ymd").Value)
            dgvConstM.TopLeftHeaderCell.Value = ymd
            For i As Integer = 1 To 8
                For j As Integer = 1 To 24
                    Dim time As String = rs.Fields("J" & j & i).Value
                    dgvConstM("J" & j, i).Value = time
                Next
            Next
            rs.MoveNext()
        End While
        rs.Close()
        cnn.Close()
    End Sub

    ''' <summary>
    ''' CellPaintingイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvConstM_CellPainting(sender As Object, e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvConstM.CellPainting
        '行ヘッダーかどうか調べる
        If e.ColumnIndex < 0 AndAlso e.RowIndex >= 1 Then
            'セルを描画する
            e.Paint(e.ClipBounds, DataGridViewPaintParts.All)

            '行番号を描画する範囲を決定する
            'e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
            Dim indexRect As Rectangle = e.CellBounds
            indexRect.Inflate(-2, -2)
            '行番号を描画する
            TextRenderer.DrawText(e.Graphics, _
                kinArray(e.RowIndex - 1), _
                e.CellStyle.Font, _
                indexRect, _
                e.CellStyle.ForeColor, _
                TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            '描画が完了したことを知らせる
            e.Handled = True
        End If
        '選択したセルに枠を付ける
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 AndAlso (e.PaintParts And DataGridViewPaintParts.Background) = DataGridViewPaintParts.Background Then
            e.Graphics.FillRectangle(New SolidBrush(e.CellStyle.BackColor), e.CellBounds)

            If (e.PaintParts And DataGridViewPaintParts.SelectionBackground) = DataGridViewPaintParts.SelectionBackground AndAlso (e.State And DataGridViewElementStates.Selected) = DataGridViewElementStates.Selected Then
                e.Graphics.DrawRectangle(New Pen(Color.Black, 2I), e.CellBounds.X + 1I, e.CellBounds.Y + 1I, e.CellBounds.Width - 3I, e.CellBounds.Height - 3I)
            End If

            Dim pParts As DataGridViewPaintParts = e.PaintParts And Not DataGridViewPaintParts.Background
            e.Paint(e.ClipBounds, pParts)
            e.Handled = True
        End If
    End Sub

    ''' <summary>
    ''' 列ヘッダークリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvConstM_ColumnHeaderMouseClick(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvConstM.ColumnHeaderMouseClick
        '選択列を全選択
        For Each row As DataGridViewRow In dgvConstM.Rows
            For i As Integer = 0 To dgvConstM.Columns.Count - 1
                If i = e.ColumnIndex Then
                    row.Cells(i).Selected = True
                Else
                    row.Cells(i).Selected = False
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' 行ヘッダークリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvConstM_RowHeaderMouseClick(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvConstM.RowHeaderMouseClick
        '選択行を全選択
        For Each row As DataGridViewRow In dgvConstM.Rows
            If row.Index = e.RowIndex Then
                For Each cell As DataGridViewCell In row.Cells
                    cell.Selected = True
                Next
            Else
                For Each cell As DataGridViewCell In row.Cells
                    cell.Selected = False
                Next
            End If
        Next
    End Sub

    ''' <summary>
    ''' 登録ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRegist_Click(sender As System.Object, e As System.EventArgs) Handles btnRegist.Click
        '入力値のチェック、空の場合は0をセット
        For i As Integer = 1 To 8
            For j As Integer = 1 To 24
                Dim input As String = Util.checkDBNullValue(dgvConstM("J" & j, i).Value)
                If input = "" Then
                    input = "0"
                    dgvConstM("J" & j, i).Value = "0"
                End If
                If Not System.Text.RegularExpressions.Regex.IsMatch(input, "^\d+(\.\d+)?$") Then
                    MsgBox(i & "行" & j & "列のセルの数値を正しく入力して下さい。", MsgBoxStyle.Exclamation)
                    Return
                End If
            Next
        Next

        '登録
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select * from ConstM"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        If rs.RecordCount <= 0 Then
            rs.AddNew()
        End If
        Dim ymd As String = Today.ToString("yyyy/MM/dd") '現在日付
        rs.Fields("Ymd").Value = ymd
        For i As Integer = 1 To 8
            For j As Integer = 1 To 24
                rs.Fields("J" & j & i).Value = dgvConstM("J" & j, i).Value
            Next
        Next
        rs.Update()
        rs.Close()
        cnn.Close()

        '再表示
        displayDgvConstM()
    End Sub
End Class