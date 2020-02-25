Imports System.Data.OleDb

Public Class 勤務項目名マスタ

    '勤務
    Private kinArray() As String = {"特養", "事務", "ｼｮｰﾄｽﾃｲ", "ﾃﾞｲｻｰﾋﾞｽ", "ﾍﾙﾊﾟｰｽﾃｰｼｮﾝ", "居宅介護支援", "老人介護支援ｾﾝﾀｰ", "生活支援ﾊｳｽ"}

    '入力時項目名
    Private entArray() As String = {"日勤", "半勤", "早出", "遅出", "Ａ勤", "Ｂ勤", "振替", "夜勤", "宿直", "日直", "Ｃ勤", "研修", "深夜", "明け", "特日", "公休", "有休", "欠勤"}

    'データ表示制御用
    Private canChanged As Boolean = False

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
    ''' keyDownイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub 勤務項目名マスタ_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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
    Private Sub 勤務項目名マスタ_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'データグリッドビュー初期設定
        initDgvKmkM()

        '勤務ボックス初期設定
        initKinBox()
    End Sub

    ''' <summary>
    ''' データグリッドビュー初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initDgvKmkM()
        Util.EnableDoubleBuffering(dgvKmkM)

        With dgvKmkM
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
            .RowHeadersWidth = 30
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
    ''' 内容クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub clearText()
        entBox.Text = ""
        seqBox.Text = ""
        prtBox.Text = ""
    End Sub

    ''' <summary>
    ''' データ表示
    ''' </summary>
    ''' <param name="kin">勤務</param>
    ''' <remarks></remarks>
    Private Sub displayDgvKmkM(kin As String)
        '内容クリア
        dgvKmkM.Columns.Clear()

        'データ取得
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset
        Dim sql As String = "select Kin, Ent, Seq, Prt from KmkM where Kin = '" & kin & "' order by Seq"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        Dim da As OleDbDataAdapter = New OleDbDataAdapter()
        Dim ds As DataSet = New DataSet()
        da.Fill(ds, rs, "KmkM")
        Dim dt As DataTable = ds.Tables("KmkM")

        '表示
        dgvKmkM.DataSource = dt
        dgvKmkM.ClearSelection()
        clearText()
        'フォーカス
        entBox.Focus()

        '幅設定等
        With dgvKmkM
            With .Columns("Kin")
                .HeaderText = "勤務"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 112
            End With
            With .Columns("Ent")
                .HeaderText = "入力時"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 60
            End With
            With .Columns("Seq")
                .HeaderText = "表示順"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 60
            End With
            With .Columns("Prt")
                .HeaderText = "印刷時"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 60
            End With
            If .Rows.Count >= 20 Then
                .Size = New Size(341, 323)
            Else
                .Size = New Size(324, 323)
            End If
        End With

    End Sub

    ''' <summary>
    ''' CellMouseClickイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvKmkM_CellMouseClick(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvKmkM.CellMouseClick
        If e.RowIndex >= 0 Then
            Dim ent As String = Util.checkDBNullValue(dgvKmkM("Ent", e.RowIndex).Value)
            Dim seq As String = Util.checkDBNullValue(dgvKmkM("Seq", e.RowIndex).Value)
            Dim prt As String = Util.checkDBNullValue(dgvKmkM("Prt", e.RowIndex).Value)

            '値反映
            clearText()
            entBox.Text = ent
            seqBox.Text = seq
            prtBox.Text = prt

            'フォーカス
            entBox.Focus()
        End If
    End Sub

    ''' <summary>
    ''' CellPaintingイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvKmkM_CellPainting(sender As Object, e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvKmkM.CellPainting
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
    ''' 勤務ボックス初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initKinBox()
        kinBox.Items.Clear()
        kinBox.Items.AddRange(kinArray)
        canChanged = True
    End Sub

    ''' <summary>
    ''' 入力時項目名ボックス初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initEntBox()
        entBox.Items.Clear()
        entBox.Items.AddRange(entArray)
    End Sub

    ''' <summary>
    ''' 勤務ボックス値変更時イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub kinBox_TextChanged(sender As Object, e As System.EventArgs) Handles kinBox.TextChanged
        If canChanged Then
            Dim kin As String = kinBox.Text
            displayDgvKmkM(kin)
            initEntBox()
        End If
    End Sub

    Private Sub textBox_Enter(sender As Object, e As System.EventArgs) Handles kinBox.Enter, entBox.Enter, seqBox.Enter, prtBox.Enter
        Dim tb As Object = sender
        tb.SelectAll()
        mdFlag = True
    End Sub

    Private Sub textBox_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles kinBox.MouseDown, entBox.MouseDown, seqBox.MouseDown, prtBox.MouseDown
        If mdFlag = True Then
            Dim tb As Object = sender
            tb.SelectAll()
            mdFlag = False
        End If
    End Sub

    ''' <summary>
    ''' 登録ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRegist_Click(sender As System.Object, e As System.EventArgs) Handles btnRegist.Click
        '勤務
        Dim kin As String = kinBox.Text
        If kin = "" Then
            MsgBox("勤務を選択して下さい。", MsgBoxStyle.Exclamation)
            kinBox.Focus()
            Return
        End If
        '入力時項目名
        Dim ent As String = entBox.Text
        If ent = "" Then
            MsgBox("入力時項目名を入力して下さい。", MsgBoxStyle.Exclamation)
            entBox.Focus()
            Return
        End If
        '表示順
        Dim seq As String = seqBox.Text
        If seq <> "" AndAlso Not System.Text.RegularExpressions.Regex.IsMatch(seq, "^\d+$") Then
            MsgBox("表示順は数値を入力して下さい。", MsgBoxStyle.Exclamation)
            seqBox.Focus()
            Return
        End If
        '印刷時項目名
        Dim prt As String = prtBox.Text
        If prt = "" Then
            '空白の場合、入力時項目名の頭文字２文字取得
            Dim entLength As Integer = ent.Length
            If entLength >= 2 Then
                prt = ent.Substring(0, 2)
            Else
                prt = ent.Substring(0, 1)
            End If
        End If

        '表示順が空の場合
        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset()
        Dim sql As String
        If seq = "" Then
            sql = "select top 1 Seq from KmkM where Kin = '" & kin & "' order by Seq Desc"
            rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
            If rs.RecordCount >= 1 Then
                seq = CInt(rs.Fields("Seq").Value) + 1
            Else
                seq = 1
            End If
            rs.Close()
        End If
        
        '登録
        rs = New ADODB.Recordset()
        sql = "select * from KmkM where Kin = '" & kin & "' and Ent = '" & ent & "'"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        If rs.RecordCount <= 0 Then
            rs.AddNew()
        End If
        rs.Fields("Kin").Value = kin
        rs.Fields("Ent").Value = ent
        rs.Fields("Seq").Value = seq
        rs.Fields("Prt").Value = prt
        rs.Update()
        rs.Close()
        cnn.Close()

        '再表示
        displayDgvKmkM(kin)
    End Sub

    ''' <summary>
    ''' 削除ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
        '勤務
        Dim kin As String = kinBox.Text
        If kin = "" Then
            MsgBox("勤務を選択して下さい。", MsgBoxStyle.Exclamation)
            kinBox.Focus()
            Return
        End If
        '入力時項目名
        Dim ent As String = entBox.Text
        If ent = "" Then
            MsgBox("入力時項目名を入力して下さい。", MsgBoxStyle.Exclamation)
            entBox.Focus()
            Return
        End If

        Dim cnn As New ADODB.Connection
        cnn.Open(TopForm.DB_Diary)
        Dim rs As New ADODB.Recordset()
        Dim sql As String = "select * from KmkM where Kin = '" & kin & "' and Ent = '" & ent & "'"
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        If rs.RecordCount <= 0 Then
            MsgBox("登録されていません。", MsgBoxStyle.Exclamation)
            rs.Close()
            cnn.Close()
            Return
        Else
            Dim result As DialogResult = MessageBox.Show("削除してよろしいですか？", "削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                rs.Delete()
                rs.Update()
                rs.Close()
                cnn.Close()
            Else
                Return
            End If
        End If

        '再表示
        displayDgvKmkM(kin)
    End Sub
End Class