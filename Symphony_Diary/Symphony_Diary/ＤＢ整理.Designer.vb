<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ＤＢ整理
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
        Me.btnDBDelete = New System.Windows.Forms.Button()
        Me.deleteProgressBar = New System.Windows.Forms.ProgressBar()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnDBDelete
        '
        Me.btnDBDelete.Location = New System.Drawing.Point(299, 104)
        Me.btnDBDelete.Name = "btnDBDelete"
        Me.btnDBDelete.Size = New System.Drawing.Size(66, 27)
        Me.btnDBDelete.TabIndex = 9
        Me.btnDBDelete.Text = "実行"
        Me.btnDBDelete.UseVisualStyleBackColor = True
        '
        'deleteProgressBar
        '
        Me.deleteProgressBar.Location = New System.Drawing.Point(154, 109)
        Me.deleteProgressBar.Name = "deleteProgressBar"
        Me.deleteProgressBar.Size = New System.Drawing.Size(119, 16)
        Me.deleteProgressBar.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(46, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 14)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "勤務表"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(23, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(347, 15)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "処理効率向上のため、次のデータの５年以前を整理します"
        '
        'ＤＢ整理
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(405, 155)
        Me.Controls.Add(Me.btnDBDelete)
        Me.Controls.Add(Me.deleteProgressBar)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "ＤＢ整理"
        Me.Text = "ＤＢ整理"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnDBDelete As System.Windows.Forms.Button
    Friend WithEvents deleteProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
