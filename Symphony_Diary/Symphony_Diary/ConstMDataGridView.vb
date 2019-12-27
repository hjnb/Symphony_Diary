Public Class ConstMDataGridView
    Inherits DataGridView

    Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As Boolean
        If keyData = Keys.Enter Then
            Return Me.ProcessTabKey(keyData)
        ElseIf keyData = Keys.Back AndAlso CurrentCell.RowIndex >= 1 Then
            CurrentCell.Value = ""
            BeginEdit(False)
            Return MyBase.ProcessDialogKey(keyData)
        Else
            Return MyBase.ProcessDialogKey(keyData)
        End If
    End Function

    Protected Overrides Function ProcessDataGridViewKey(e As System.Windows.Forms.KeyEventArgs) As Boolean
        If e.KeyCode = Keys.Enter Then
            Return Me.ProcessTabKey(e.KeyCode)
        End If

        Dim tb As DataGridViewTextBoxEditingControl = DirectCast(Me.EditingControl, DataGridViewTextBoxEditingControl)
        If Not IsNothing(tb) AndAlso ((e.KeyCode = Keys.Left AndAlso tb.SelectionStart = 0) OrElse (e.KeyCode = Keys.Right AndAlso tb.SelectionStart = tb.TextLength)) Then
            Return False
        Else
            Return MyBase.ProcessDataGridViewKey(e)
        End If
    End Function

End Class
