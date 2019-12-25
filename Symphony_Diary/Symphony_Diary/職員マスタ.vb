Imports System.Data.OleDb

Public Class 職員マスタ

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
    Private Sub 職員マスタ_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '職種リストボックス初期設定
        initSyuListBox()

        'データグリッドビュー初期設定
        initDgvNamM()

        'マスタデータ表示
        displayNamM()
    End Sub

    ''' <summary>
    ''' データグリッドビュー初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initDgvNamM()
        Util.EnableDoubleBuffering(dgvNamM)

        With dgvNamM
            .AllowUserToAddRows = False '行追加禁止
            .AllowUserToResizeColumns = False '列の幅をユーザーが変更できないようにする
            .AllowUserToResizeRows = False '行の高さをユーザーが変更できないようにする
            .AllowUserToDeleteRows = False '行削除禁止
            .BorderStyle = BorderStyle.FixedSingle
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Control)
            .DefaultCellStyle.ForeColor = Color.Black
            '.DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Control)
            '.DefaultCellStyle.SelectionForeColor = Color.Black
            .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersHeight = 17
            .RowHeadersWidth = 28
            .RowTemplate.Height = 16
            .RowTemplate.HeaderCell = New dgvRowHeaderCell() '行ヘッダの三角マークを非表示に
            .BackgroundColor = Color.FromKnownColor(KnownColor.Control)
            .ShowCellToolTips = False
            .EnableHeadersVisualStyles = False
            .Font = New Font("ＭＳ Ｐゴシック", 8.5)
            .ReadOnly = False
        End With
    End Sub

    ''' <summary>
    ''' マスタデータ表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub displayNamM()
        '内容クリア
        dgvNamM.Columns.Clear()

        'データ取得
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select Id, Nam, Syu, Kei, Kin, Memo from NamM order by Id"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        Dim da As OleDbDataAdapter = New OleDbDataAdapter()
        Dim ds As DataSet = New DataSet()
        da.Fill(ds, rs, "NamM")
        Dim dt As DataTable = ds.Tables("NamM")

        '表示
        dgvNamM.DataSource = dt
        dgvNamM.ClearSelection()

        '幅設定等
        With dgvNamM
            With .Columns("Id")
                .HeaderText = "職員No."
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 60
            End With
            With .Columns("Nam")
                .HeaderText = "氏名"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 100
            End With
            With .Columns("Syu")
                .HeaderText = "職種"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 110
            End With
            With .Columns("Kei")
                .HeaderText = "形態"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 93
            End With
            With .Columns("Kin")
                .HeaderText = "勤務"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 115
            End With
            With .Columns("Memo")
                .HeaderText = "特記"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 250
            End With
        End With

    End Sub

    ''' <summary>
    ''' 内容クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub clearText()
        idBox.Text = ""
        namBox.Text = ""
        syuLabel.Text = ""
        memoBox.Text = ""
    End Sub

    ''' <summary>
    ''' 職種リストボックス初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initSyuListBox()
        Dim syuArray() As String = {"理事長", "施設長", "副施設長", "事務局長", "部長", "課長", "係長", "主任", "管理者", "ｻｰﾋﾞｽ提供責任者", "医師", "正看護師", "准看護師", "看護職", "機能訓練士", "介護支援専門員", "生活相談員", "支援援助員", "介護職", "介護福祉士", "訪問介護員", "管理栄養士", "栄養士", "宿直"}
        syuListBox.Items.Clear()
        syuListBox.Items.AddRange(syuArray)
    End Sub

    ''' <summary>
    ''' CellMouseClickイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvNamM_CellMouseClick(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvNamM.CellMouseClick
        If e.RowIndex >= 0 Then
            Dim id As Integer = dgvNamM("Id", e.RowIndex).Value
            Dim nam As String = Util.checkDBNullValue(dgvNamM("Nam", e.RowIndex).Value)
            Dim syu As String = Util.checkDBNullValue(dgvNamM("Syu", e.RowIndex).Value)
            Dim kei As String = Util.checkDBNullValue(dgvNamM("Kei", e.RowIndex).Value)
            Dim kin As String = Util.checkDBNullValue(dgvNamM("Kin", e.RowIndex).Value)
            Dim memo As String = Util.checkDBNullValue(dgvNamM("Memo", e.RowIndex).Value)

            clearText()

            '画面に値反映
            idBox.Text = id
            namBox.Text = nam
            syuLabel.Text = syu
            memoBox.Text = memo
        End If
    End Sub

    ''' <summary>
    ''' CellPaintingイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvNamM_CellPainting(sender As Object, e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvNamM.CellPainting
        '行ヘッダーかどうか調べる
        If e.ColumnIndex < 0 AndAlso e.RowIndex >= 0 Then
            'セルを描画する
            e.Paint(e.ClipBounds, DataGridViewPaintParts.All)

            '行番号を描画する範囲を決定する
            'e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
            Dim indexRect As Rectangle = e.CellBounds
            indexRect.Inflate(-2, -2)
            '行番号を描画する
            TextRenderer.DrawText(e.Graphics, _
                (e.RowIndex + 1).ToString(), _
                e.CellStyle.Font, _
                indexRect, _
                e.CellStyle.ForeColor, _
                TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            '描画が完了したことを知らせる
            e.Handled = True
        End If
    End Sub
End Class