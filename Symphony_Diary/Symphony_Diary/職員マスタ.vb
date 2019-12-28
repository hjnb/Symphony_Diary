Imports System.Data.OleDb

Public Class 職員マスタ

    '形態
    Private keiDic As New Dictionary(Of String, Integer) From {{"常勤専従", 1}, {"常勤兼務", 2}, {"常勤以外専従", 3}, {"常勤以外兼務", 4}}

    '勤務
    Private kinDIc As New Dictionary(Of String, Integer) From {{"特養", 1}, {"事務", 2}, {"ｼｮｰﾄｽﾃｲ", 3}, {"ﾃﾞｲｻｰﾋﾞｽ", 4}, {"ﾍﾙﾊﾟｰｽﾃｰｼｮﾝ", 5}, {"居宅介護支援", 6}, {"老人介護支援ｾﾝﾀｰ", 7}, {"生活支援ﾊｳｽ", 8}}

    '職種
    Private syuArray() As String = {"理事長", "施設長", "副施設長", "事務局長", "部長", "課長", "係長", "主任", "管理者", "ｻｰﾋﾞｽ提供責任者", "医師", "正看護師", "准看護師", "看護職", "機能訓練士", "介護支援専門員", "生活相談員", "支援援助員", "介護職", "介護福祉士", "訪問介護員", "管理栄養士", "栄養士", "宿直"}

    'テキストボックスのマウスダウンイベント制御用
    Private mdFlag As Boolean = False

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
        Me.KeyPreview = True
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
    ''' keyDownイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub 職員マスタ_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If e.Control = False Then
                Me.SelectNextControl(Me.ActiveControl, Not e.Shift, True, True, True)
            End If
        End If
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
            .ReadOnly = True
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
        rbtnKei1.Checked = True '形態初期位置
        rbtnKin1.Checked = True '勤務初期位置
        memoBox.Text = ""
        syuListBox.ClearSelected()
    End Sub

    ''' <summary>
    ''' 職種リストボックス初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initSyuListBox()
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
            '形態ラジオボタン
            Dim keiNum As Integer
            keiNum = If(keiDic.ContainsKey(kei), keiDic(kei), 1)
            DirectCast(keiGroupBox.Controls("rbtnKei" & keiNum), RadioButton).Checked = True
            '勤務ラジオボタン
            Dim kinNum As Integer
            kinNum = If(kinDIc.ContainsKey(kin), kinDIc(kin), 1)
            DirectCast(kinGroupBox.Controls("rbtnKin" & kinNum), RadioButton).Checked = True
            'フォーカス
            idBox.Focus()
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

    ''' <summary>
    ''' 職種リスト値変更時イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub syuListBox_SelectedValueChanged(sender As Object, e As System.EventArgs) Handles syuListBox.SelectedValueChanged
        Dim syu As String = If(IsNothing(syuListBox.SelectedItem), "", syuListBox.SelectedItem.ToString())
        syuLabel.Text = syu
    End Sub

    ''' <summary>
    ''' 登録ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRegist_Click(sender As System.Object, e As System.EventArgs) Handles btnRegist.Click
        '入力値チェック
        '職員No.
        Dim id As String = idBox.Text
        If id = "" Then
            MsgBox("職員No.を入力して下さい。", MsgBoxStyle.Exclamation)
            idBox.Focus()
            Return
        End If
        If Not System.Text.RegularExpressions.Regex.IsMatch(id, "^\d+$") Then
            MsgBox("職員No.は数値を入力して下さい。", MsgBoxStyle.Exclamation)
            idBox.Focus()
            Return
        End If
        '氏名
        Dim nam As String = namBox.Text
        If nam = "" Then
            MsgBox("氏名を入力して下さい。", MsgBoxStyle.Exclamation)
            namBox.Focus()
            Return
        End If
        '職種
        Dim syu As String = syuLabel.Text
        If syu = "" Then
            MsgBox("職種を選択して下さい。", MsgBoxStyle.Exclamation)
            Return
        End If
        '形態
        Dim kei As String = ""
        For Each ctrl As Control In keiGroupBox.Controls
            Dim rbtn As RadioButton = TryCast(ctrl, RadioButton)
            If Not IsNothing(rbtn) AndAlso rbtn.Checked Then
                kei = rbtn.Text
                Exit For
            End If
        Next
        '勤務
        Dim kin As String = ""
        For Each ctrl As Control In kinGroupBox.Controls
            Dim rbtn As RadioButton = TryCast(ctrl, RadioButton)
            If Not IsNothing(rbtn) AndAlso rbtn.Checked Then
                kin = rbtn.Text
                Exit For
            End If
        Next
        '特記
        Dim memo As String = memoBox.Text

        '登録
        Dim cn As New ADODB.Connection()
        cn.Open(TopForm.DB_Diary)
        Dim sql As String = "select * from NamM where Id = " & CInt(id)
        Dim rs As New ADODB.Recordset
        rs.Open(sql, cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        If rs.RecordCount <= 0 Then
            rs.AddNew()
        End If
        rs.Fields("Id").Value = id
        rs.Fields("Nam").Value = nam
        rs.Fields("Syu").Value = syu
        rs.Fields("Kei").Value = kei
        rs.Fields("Kin").Value = kin
        rs.Fields("Memo").Value = memo
        rs.Update()
        rs.Close()
        cn.Close()

        '再表示
        clearText()
        displayNamM()
    End Sub

    ''' <summary>
    ''' 削除ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
        '職員No.
        Dim id As String = idBox.Text
        If id = "" Then
            MsgBox("職員No.を入力して下さい。", MsgBoxStyle.Exclamation)
            idBox.Focus()
            Return
        End If
        If Not System.Text.RegularExpressions.Regex.IsMatch(id, "^\d+$") Then
            MsgBox("職員No.は数値を入力して下さい。", MsgBoxStyle.Exclamation)
            idBox.Focus()
            Return
        End If

        '削除
        Dim cn As New ADODB.Connection()
        cn.Open(TopForm.DB_Diary)
        Dim sql As String = "select * from NamM where Id = " & CInt(id)
        Dim rs As New ADODB.Recordset
        rs.Open(sql, cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        If rs.RecordCount <= 0 Then
            MsgBox("登録されていません。", MsgBoxStyle.Exclamation)
            rs.Close()
            cn.Close()
            Return
        Else
            Dim result As DialogResult = MessageBox.Show("削除してよろしいですか？", "削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                rs.Delete()
                rs.Update()
                rs.Close()
                cn.Close()
            Else
                Return
            End If
        End If

        '再表示
        clearText()
        displayNamM()
    End Sub

    Private Sub textBox_Enter(sender As Object, e As System.EventArgs) Handles idBox.Enter, namBox.Enter, memoBox.Enter
        Dim tb As TextBox = CType(sender, TextBox)
        tb.SelectAll()
        mdFlag = True
    End Sub

    Private Sub textBox_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles idBox.MouseDown, namBox.MouseDown, memoBox.MouseDown
        If mdFlag = True Then
            Dim tb As TextBox = CType(sender, TextBox)
            tb.SelectAll()
            mdFlag = False
        End If
    End Sub
End Class