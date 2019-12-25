Imports System.Text

Public Class WorkDataGridView
    Inherits DataGridView

    Private wordDictionary As Dictionary(Of String, String) '勤務略名dic

    Protected Overrides Sub InitLayout()
        MyBase.InitLayout()

        DoubleBuffered = True
    End Sub

    'Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As Boolean
    '    Dim inputStr As String = If(Not IsNothing(Me.EditingControl), CType(Me.EditingControl, DataGridViewTextBoxEditingControl).Text, "") '入力文字
    '    Dim columnName As String = Me.Columns(CurrentCell.ColumnIndex).Name '選択列名
    '    If keyData = Keys.Enter Then
    '        If columnName = "Unt" OrElse columnName = "Rdr" OrElse columnName = "Nam" Then
    '            EndEdit()
    '            Return False
    '        ElseIf 7 <= Me.CurrentCell.ColumnIndex AndAlso Me.CurrentCell.ColumnIndex <= 37 Then 'Y1～Y31列
    '            If inputStr = "" OrElse ("A" <= StrConv(inputStr.Substring(0, 1), VbStrConv.Narrow) AndAlso StrConv(inputStr.Substring(0, 1), vbNarrow) <= "T") Then
    '                '入力文字が空またはアルファベット"A"～"T"の場合
    '                Return Me.ProcessTabKey(keyData)
    '            Else
    '                '編集終了時に値の変換処理をする
    '                Try
    '                    '入力文字に対応する勤務略名を選択しているセルに設定
    '                    CType(Me.EditingControl, DataGridViewTextBoxEditingControl).Text = wordDictionary(inputStr)
    '                Catch ex As KeyNotFoundException
    '                    MsgBox("正しく入力して下さい。", MsgBoxStyle.Exclamation, "Work")
    '                    EndEdit()
    '                    Return False
    '                End Try
    '            End If
    '            Return Me.ProcessTabKey(keyData)
    '        Else
    '            Return Me.ProcessTabKey(keyData)
    '        End If
    '    ElseIf keyData = Keys.Back Then
    '        If columnName = "Unt" OrElse columnName = "Rdr" OrElse columnName.Substring(0, 1) = "Y" Then
    '            CurrentCell.Value = ""
    '            BeginEdit(False)
    '        ElseIf columnName = "Nam" Then
    '            BeginEdit(True)
    '        End If
    '        Return MyBase.ProcessDialogKey(keyData)
    '    Else
    '        Return MyBase.ProcessDialogKey(keyData)
    '    End If
    'End Function

    'Protected Overrides Function ProcessDataGridViewKey(e As System.Windows.Forms.KeyEventArgs) As Boolean
    '    Dim inputStr As String = Util.checkDBNullValue(Me.CurrentCell.Value)
    '    If e.KeyCode = Keys.Enter Then
    '        If Me.CurrentCell.RowIndex <= 0 OrElse 51 <= Me.CurrentCell.RowIndex Then
    '            Return Me.ProcessTabKey(e.KeyCode)
    '        End If

    '        Dim columnName As String = Me.Columns(CurrentCell.ColumnIndex).Name
    '        If columnName = "Unt" OrElse columnName = "Rdr" OrElse columnName = "Nam" Then
    '            BeginEdit(True)
    '            Return False
    '        ElseIf 7 <= Me.CurrentCell.ColumnIndex AndAlso Me.CurrentCell.ColumnIndex <= 37 Then 'Y1～Y31列
    '            If inputStr = "" OrElse ("A" <= StrConv(inputStr.Substring(0, 1), VbStrConv.Narrow) AndAlso StrConv(inputStr.Substring(0, 1), VbStrConv.Narrow) <= "T") Then
    '                '入力文字が空またはアルファベット"A"～"T"の場合
    '                Me.ProcessTabKey(e.KeyCode)
    '                BeginEdit(True)
    '                Return False
    '            Else
    '                '編集終了時に値の変換処理をする
    '                Try
    '                    '入力文字に対応する勤務略名を選択しているセルに設定
    '                    Me.CurrentCell.Value = wordDictionary(inputStr)
    '                Catch ex As KeyNotFoundException
    '                    MsgBox("正しく入力して下さい。", MsgBoxStyle.Exclamation, "Work")
    '                    BeginEdit(True)
    '                    Return False
    '                End Try
    '            End If
    '            Me.ProcessTabKey(e.KeyCode)
    '            BeginEdit(True)
    '            Return False
    '        Else
    '            Me.ProcessTabKey(e.KeyCode)
    '            BeginEdit(True)
    '            Return False
    '        End If
    '    End If

    '    Dim tb As DataGridViewTextBoxEditingControl = CType(Me.EditingControl, DataGridViewTextBoxEditingControl)
    '    If Not IsNothing(tb) AndAlso ((e.KeyCode = Keys.Left AndAlso tb.SelectionStart = 0) OrElse (e.KeyCode = Keys.Right AndAlso tb.SelectionStart = tb.TextLength)) Then
    '        Return False
    '    Else
    '        Return MyBase.ProcessDataGridViewKey(e)
    '    End If
    'End Function

    ''' <summary>
    ''' セル編集終了時イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub workDataGridView_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellEndEdit
        Dim inputStr As String = If(IsDBNull(Me(e.ColumnIndex, e.RowIndex).Value), "", Me(e.ColumnIndex, e.RowIndex).Value)
    End Sub

    ''' <summary>
    ''' セルエンターイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub workDataGridView_CellEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellEnter
        Dim columnName As String = Me.Columns(e.ColumnIndex).Name '選択列名

        '選択列によってIMEの設定
        'If columnName = "Unt" Then
        '    Me.ImeMode = Windows.Forms.ImeMode.Hiragana
        'ElseIf columnName = "Rdr" Then
        '    Me.ImeMode = Windows.Forms.ImeMode.Off
        'ElseIf columnName = "Nam" Then
        '    Me.ImeMode = Windows.Forms.ImeMode.Hiragana
        'ElseIf columnName.Substring(0, 1) = "Y" Then
        '    Me.ImeMode = Windows.Forms.ImeMode.Off
        'End If
    End Sub

    ''' <summary>
    ''' CellPaintingイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub workDataGridView_CellPainting(sender As Object, e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles Me.CellPainting
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
    ''' 列ヘッダーマウスクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub workDataGridView_ColumnHeaderMouseClick(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles Me.ColumnHeaderMouseClick
        '選択列を全選択
        For Each row As DataGridViewRow In Me.Rows
            For i As Integer = 0 To Me.Columns.Count - 1
                If i = e.ColumnIndex Then
                    row.Cells(i).Selected = True
                Else
                    row.Cells(i).Selected = False
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' セル編集時に表示されるテキストボックスイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub workDataGridView_EditingControlShowing(sender As Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Me.EditingControlShowing
        Dim tb As DataGridViewTextBoxEditingControl = CType(e.Control, DataGridViewTextBoxEditingControl)
        'tb.CharacterCasing = CharacterCasing.Upper '入力される文字を大文字に
        'RemoveHandler tb.KeyPress, AddressOf dgvTextBox_KeyPress

        'If Me.Columns(Me.CurrentCell.ColumnIndex).Name = "Rdr" Then
        '    tb.MaxLength = 1
        'ElseIf Me.Columns(Me.CurrentCell.ColumnIndex).Name = "Unt" Then
        '    tb.MaxLength = 1
        '    'イベントハンドラを削除、追加
        '    AddHandler tb.KeyPress, AddressOf dgvTextBox_KeyPress
        'End If
    End Sub

    ''' <summary>
    ''' セル編集用keyPress処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvTextBox_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs)
        Dim text As String = CType(sender, DataGridViewTextBoxEditingControl).Text
        Dim lengthByte As Integer = Encoding.GetEncoding("Shift_JIS").GetByteCount(text)
        Dim limitLengthByte As Integer = 4

        If lengthByte >= limitLengthByte Then '設定されているバイト数以上の時
            If e.KeyChar = ChrW(Keys.Back) Then
                'Backspaceは入力可能
                e.Handled = False
            Else
                '入力できなくする
                e.Handled = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' 勤務略名dicセット
    ''' </summary>
    ''' <param name="wordDictionary"></param>
    ''' <remarks></remarks>
    Public Sub setWordDictionary(wordDictionary As Dictionary(Of String, String))
        Me.wordDictionary = wordDictionary
    End Sub
End Class
