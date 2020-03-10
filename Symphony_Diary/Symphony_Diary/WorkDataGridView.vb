Imports System.Text

Public Class WorkDataGridView
    Inherits DataGridView

    Public canCellEnter As Boolean = False

    Private wordDictionary As Dictionary(Of String, String) '勤務略名dic

    Private inputNumList As New List(Of Integer)

    Protected Overrides Sub InitLayout()
        MyBase.InitLayout()

        DoubleBuffered = True
    End Sub

    Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As Boolean
        Dim columnName As String = Me.Columns(CurrentCell.ColumnIndex).Name '選択列名
        If keyData = Keys.Enter Then
            If (columnName = "Kei" OrElse columnName = "Syu") AndAlso Not IsNothing(Me.EditingControl) Then
                Dim cb As DataGridViewComboBoxColumn = DirectCast(Me.Columns(CurrentCell.ColumnIndex), DataGridViewComboBoxColumn)
                Dim inputStr As String = DirectCast(Me.EditingControl, DataGridViewComboBoxEditingControl).Text '入力文字
                'コンボボックス項目に追加
                If Not cb.Items.Contains(inputStr) Then
                    cb.Items.Add(inputStr)
                End If
                Me(CurrentCell.ColumnIndex, CurrentCell.RowIndex).Value = inputStr

                EndEdit()
                Return False
            ElseIf columnName = "Nam" Then
                EndEdit()
                Return False
            ElseIf 6 <= Me.CurrentCell.ColumnIndex AndAlso Me.CurrentCell.ColumnIndex <= 36 Then 'Y1～Y31列
                Dim inputStr As String = If(Not IsNothing(Me.EditingControl), CType(Me.EditingControl, DataGridViewTextBoxEditingControl).Text, "") '入力文字
                If inputStr = "" Then
                    '入力文字が空
                    Return Me.ProcessTabKey(keyData)
                Else
                    '編集終了時に値の変換処理をする
                    Try
                        '入力文字に対応する勤務略名を選択しているセルに設定
                        If wordDictionary.ContainsKey(inputStr) AndAlso Not inputNumList.Contains(inputStr) Then
                            MsgBox("定数登録されていません。", MsgBoxStyle.Exclamation, "Diary")
                        End If
                        CType(Me.EditingControl, DataGridViewTextBoxEditingControl).Text = wordDictionary(inputStr)
                    Catch ex As KeyNotFoundException
                        MsgBox("正しく入力して下さい。", MsgBoxStyle.Exclamation, "Diary")
                        EndEdit()
                        Return False
                    End Try
                End If
                Return Me.ProcessTabKey(keyData)
            Else
                Return Me.ProcessTabKey(keyData)
            End If
        ElseIf keyData = Keys.Back Then
            If columnName = "Kei" OrElse columnName = "Syu" OrElse columnName.Substring(0, 1) = "Y" Then
                CurrentCell.Value = ""
                BeginEdit(False)
            ElseIf columnName = "Nam" Then
                BeginEdit(True)
            End If
            Return MyBase.ProcessDialogKey(keyData)
        Else
            Return MyBase.ProcessDialogKey(keyData)
        End If
    End Function

    Protected Overrides Function ProcessDataGridViewKey(e As System.Windows.Forms.KeyEventArgs) As Boolean
        Dim inputStr As String = Util.checkDBNullValue(Me.CurrentCell.Value)
        If e.KeyCode = Keys.Enter Then
            Dim columnName As String = Me.Columns(CurrentCell.ColumnIndex).Name
            If columnName = "Kei" OrElse columnName = "Syu" OrElse columnName = "Nam" Then
                BeginEdit(True)
                Return False
            ElseIf 6 <= Me.CurrentCell.ColumnIndex AndAlso Me.CurrentCell.ColumnIndex <= 36 Then 'Y1～Y31列
                If inputStr = "" Then
                    '入力文字が空
                    Me.ProcessTabKey(e.KeyCode)
                    BeginEdit(True)
                    Return False
                Else
                    '編集終了時に値の変換処理をする
                    Try
                        '入力文字に対応する勤務略名を選択しているセルに設定
                        If wordDictionary.ContainsKey(inputStr) AndAlso Not inputNumList.Contains(inputStr) Then
                            MsgBox("定数登録されていません。", MsgBoxStyle.Exclamation, "Diary")
                        End If
                        Me.CurrentCell.Value = wordDictionary(inputStr)
                    Catch ex As KeyNotFoundException
                        MsgBox("正しく入力して下さい。", MsgBoxStyle.Exclamation, "Diary")
                        BeginEdit(True)
                        Return False
                    End Try
                End If
                Me.ProcessTabKey(e.KeyCode)
                BeginEdit(True)
                Return False
            Else
                Me.ProcessTabKey(e.KeyCode)
                BeginEdit(True)
                Return False
            End If
        End If

        If TypeOf Me.EditingControl Is DataGridViewTextBoxEditingControl Then
            Dim tb = DirectCast(Me.EditingControl, DataGridViewTextBoxEditingControl)
            If Not IsNothing(tb) AndAlso ((e.KeyCode = Keys.Left AndAlso tb.SelectionStart = 0) OrElse (e.KeyCode = Keys.Right AndAlso tb.SelectionStart = tb.TextLength)) Then
                Return False
            Else
                Return MyBase.ProcessDataGridViewKey(e)
            End If
        Else
            Return MyBase.ProcessDataGridViewKey(e)
        End If
        
    End Function

    ''' <summary>
    ''' セルエンターイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub workDataGridView_CellEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellEnter
        If canCellEnter Then
            If Me(e.ColumnIndex, e.RowIndex).GetType().Equals(GetType(DataGridViewComboBoxCell)) Then
                Me.BeginEdit(False)
                If Not IsNothing(Me.EditingControl) Then
                    Dim cb As ComboBox = DirectCast(Me.EditingControl, ComboBox)
                    'cb.DroppedDown = True
                End If
            End If

            '選択列によってIMEの設定
            Dim columnName As String = Me.Columns(e.ColumnIndex).Name '選択列名
            If columnName = "Kei" OrElse columnName = "Syu" OrElse columnName = "Nam" Then
                Me.ImeMode = Windows.Forms.ImeMode.Hiragana
            ElseIf columnName.Substring(0, 1) = "Y" Then
                Me.ImeMode = Windows.Forms.ImeMode.Off
            End If
        End If
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
        '該当する列か調べる
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        Dim columnName As String = dgv.CurrentCell.OwningColumn.Name
        If columnName = "Kei" OrElse columnName = "Syu" Then
            Dim cb As DataGridViewComboBoxEditingControl = DirectCast(e.Control, DataGridViewComboBoxEditingControl)
            cb.IntegralHeight = False
            cb.MaxDropDownItems = 6
            cb.DropDownStyle = ComboBoxStyle.DropDown
        End If
    End Sub

    Private Sub WorkDataGridView_CellValidating(sender As Object, e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles Me.CellValidating
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        Dim columnName As String = dgv.Columns(e.ColumnIndex).Name
        If (columnName = "Kei" OrElse columnName = "Syu") AndAlso TypeOf dgv.Columns(e.ColumnIndex) Is DataGridViewComboBoxColumn Then
            Dim cb As DataGridViewComboBoxColumn = DirectCast(dgv.Columns(e.ColumnIndex), DataGridViewComboBoxColumn)
            'コンボボックスの項目に追加する
            If Not cb.Items.Contains(e.FormattedValue) Then
                cb.Items.Add(e.FormattedValue)
            End If
            'セルの値を設定しないと、元に戻ってしまう
            dgv(e.ColumnIndex, e.RowIndex).Value = e.FormattedValue
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

    ''' <summary>
    ''' 入力可能な勤務数字リストを設定
    ''' </summary>
    ''' <param name="numList"></param>
    ''' <remarks></remarks>
    Public Sub setInputNumList(numList As List(Of Integer))
        Me.inputNumList = numList
    End Sub
End Class
