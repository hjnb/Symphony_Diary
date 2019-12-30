<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class 勤務画面
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.adBox = New ADBox.adBox()
        Me.btnDisplay = New System.Windows.Forms.Button()
        Me.btnRowInsert = New System.Windows.Forms.Button()
        Me.btnRowDelete = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.dgvWork = New Symphony_Diary.WorkDataGridView(Me.components)
        CType(Me.dgvWork, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'adBox
        '
        Me.adBox.dateText = "30"
        Me.adBox.Location = New System.Drawing.Point(30, 25)
        Me.adBox.Mode = 3
        Me.adBox.monthText = "12"
        Me.adBox.Name = "adBox"
        Me.adBox.Size = New System.Drawing.Size(130, 35)
        Me.adBox.TabIndex = 0
        Me.adBox.yearText = "2019"
        '
        'btnDisplay
        '
        Me.btnDisplay.Location = New System.Drawing.Point(168, 30)
        Me.btnDisplay.Name = "btnDisplay"
        Me.btnDisplay.Size = New System.Drawing.Size(50, 25)
        Me.btnDisplay.TabIndex = 1
        Me.btnDisplay.Text = "表示"
        Me.btnDisplay.UseVisualStyleBackColor = True
        '
        'btnRowInsert
        '
        Me.btnRowInsert.Location = New System.Drawing.Point(393, 30)
        Me.btnRowInsert.Name = "btnRowInsert"
        Me.btnRowInsert.Size = New System.Drawing.Size(50, 25)
        Me.btnRowInsert.TabIndex = 3
        Me.btnRowInsert.Text = "行挿入"
        Me.btnRowInsert.UseVisualStyleBackColor = True
        '
        'btnRowDelete
        '
        Me.btnRowDelete.Location = New System.Drawing.Point(444, 30)
        Me.btnRowDelete.Name = "btnRowDelete"
        Me.btnRowDelete.Size = New System.Drawing.Size(50, 25)
        Me.btnRowDelete.TabIndex = 4
        Me.btnRowDelete.Text = "行削除"
        Me.btnRowDelete.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(632, 26)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(69, 33)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "登録"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(700, 26)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(69, 33)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "換算"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(836, 26)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(69, 33)
        Me.Button3.TabIndex = 8
        Me.Button3.Text = "印刷"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(768, 26)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(69, 33)
        Me.Button4.TabIndex = 7
        Me.Button4.Text = "削除"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(904, 26)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(69, 33)
        Me.Button5.TabIndex = 9
        Me.Button5.Text = "個人別"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'dgvWork
        '
        Me.dgvWork.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWork.Location = New System.Drawing.Point(30, 68)
        Me.dgvWork.Name = "dgvWork"
        Me.dgvWork.RowTemplate.Height = 21
        Me.dgvWork.Size = New System.Drawing.Size(943, 582)
        Me.dgvWork.TabIndex = 2
        '
        '勤務画面
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(999, 694)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnRowDelete)
        Me.Controls.Add(Me.btnRowInsert)
        Me.Controls.Add(Me.dgvWork)
        Me.Controls.Add(Me.btnDisplay)
        Me.Controls.Add(Me.adBox)
        Me.Name = "勤務画面"
        Me.Text = "Diary "
        CType(Me.dgvWork, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents adBox As ADBox.adBox
    Friend WithEvents btnDisplay As System.Windows.Forms.Button
    Friend WithEvents dgvWork As Symphony_Diary.WorkDataGridView
    Friend WithEvents btnRowInsert As System.Windows.Forms.Button
    Friend WithEvents btnRowDelete As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
End Class
