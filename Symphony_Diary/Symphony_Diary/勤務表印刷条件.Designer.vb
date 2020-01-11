<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class 勤務表印刷条件
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbtnB4S2 = New System.Windows.Forms.RadioButton()
        Me.rbtnB4S = New System.Windows.Forms.RadioButton()
        Me.rbtnB4 = New System.Windows.Forms.RadioButton()
        Me.rbtnA4 = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.rbtnPR = New System.Windows.Forms.RadioButton()
        Me.rbtnResult = New System.Windows.Forms.RadioButton()
        Me.rbtnPlan = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.sign1Box = New System.Windows.Forms.TextBox()
        Me.sign2Box = New System.Windows.Forms.TextBox()
        Me.sign3Box = New System.Windows.Forms.TextBox()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtnB4S2)
        Me.GroupBox1.Controls.Add(Me.rbtnB4S)
        Me.GroupBox1.Controls.Add(Me.rbtnB4)
        Me.GroupBox1.Controls.Add(Me.rbtnA4)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(323, 46)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'rbtnB4S2
        '
        Me.rbtnB4S2.AutoSize = True
        Me.rbtnB4S2.Location = New System.Drawing.Point(231, 18)
        Me.rbtnB4S2.Name = "rbtnB4S2"
        Me.rbtnB4S2.Size = New System.Drawing.Size(87, 16)
        Me.rbtnB4S2.TabIndex = 4
        Me.rbtnB4S2.TabStop = True
        Me.rbtnB4S2.Text = "B4→A4(NC)"
        Me.rbtnB4S2.UseVisualStyleBackColor = True
        '
        'rbtnB4S
        '
        Me.rbtnB4S.AutoSize = True
        Me.rbtnB4S.Location = New System.Drawing.Point(147, 18)
        Me.rbtnB4S.Name = "rbtnB4S"
        Me.rbtnB4S.Size = New System.Drawing.Size(63, 16)
        Me.rbtnB4S.TabIndex = 3
        Me.rbtnB4S.TabStop = True
        Me.rbtnB4S.Text = "B4→A4"
        Me.rbtnB4S.UseVisualStyleBackColor = True
        '
        'rbtnB4
        '
        Me.rbtnB4.AutoSize = True
        Me.rbtnB4.Location = New System.Drawing.Point(83, 18)
        Me.rbtnB4.Name = "rbtnB4"
        Me.rbtnB4.Size = New System.Drawing.Size(37, 16)
        Me.rbtnB4.TabIndex = 2
        Me.rbtnB4.TabStop = True
        Me.rbtnB4.Text = "B4"
        Me.rbtnB4.UseVisualStyleBackColor = True
        '
        'rbtnA4
        '
        Me.rbtnA4.AutoSize = True
        Me.rbtnA4.Location = New System.Drawing.Point(20, 18)
        Me.rbtnA4.Name = "rbtnA4"
        Me.rbtnA4.Size = New System.Drawing.Size(37, 16)
        Me.rbtnA4.TabIndex = 0
        Me.rbtnA4.TabStop = True
        Me.rbtnA4.Text = "A4"
        Me.rbtnA4.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbtnPR)
        Me.GroupBox2.Controls.Add(Me.rbtnResult)
        Me.GroupBox2.Controls.Add(Me.rbtnPlan)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 68)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(254, 46)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        '
        'rbtnPR
        '
        Me.rbtnPR.AutoSize = True
        Me.rbtnPR.Location = New System.Drawing.Point(147, 18)
        Me.rbtnPR.Name = "rbtnPR"
        Me.rbtnPR.Size = New System.Drawing.Size(83, 16)
        Me.rbtnPR.TabIndex = 4
        Me.rbtnPR.TabStop = True
        Me.rbtnPR.Text = "予定／実績"
        Me.rbtnPR.UseVisualStyleBackColor = True
        '
        'rbtnResult
        '
        Me.rbtnResult.AutoSize = True
        Me.rbtnResult.Location = New System.Drawing.Point(83, 18)
        Me.rbtnResult.Name = "rbtnResult"
        Me.rbtnResult.Size = New System.Drawing.Size(47, 16)
        Me.rbtnResult.TabIndex = 3
        Me.rbtnResult.TabStop = True
        Me.rbtnResult.Text = "実績"
        Me.rbtnResult.UseVisualStyleBackColor = True
        '
        'rbtnPlan
        '
        Me.rbtnPlan.AutoSize = True
        Me.rbtnPlan.Location = New System.Drawing.Point(20, 18)
        Me.rbtnPlan.Name = "rbtnPlan"
        Me.rbtnPlan.Size = New System.Drawing.Size(47, 16)
        Me.rbtnPlan.TabIndex = 2
        Me.rbtnPlan.TabStop = True
        Me.rbtnPlan.Text = "予定"
        Me.rbtnPlan.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(35, 154)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "施設長"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(35, 185)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 12)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "合議"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(35, 214)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 12)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "合議"
        '
        'sign1Box
        '
        Me.sign1Box.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.sign1Box.Location = New System.Drawing.Point(89, 151)
        Me.sign1Box.Name = "sign1Box"
        Me.sign1Box.Size = New System.Drawing.Size(100, 19)
        Me.sign1Box.TabIndex = 5
        '
        'sign2Box
        '
        Me.sign2Box.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.sign2Box.Location = New System.Drawing.Point(89, 181)
        Me.sign2Box.Name = "sign2Box"
        Me.sign2Box.Size = New System.Drawing.Size(100, 19)
        Me.sign2Box.TabIndex = 6
        '
        'sign3Box
        '
        Me.sign3Box.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.sign3Box.Location = New System.Drawing.Point(89, 211)
        Me.sign3Box.Name = "sign3Box"
        Me.sign3Box.Size = New System.Drawing.Size(100, 19)
        Me.sign3Box.TabIndex = 7
        '
        'btnExecute
        '
        Me.btnExecute.Location = New System.Drawing.Point(197, 250)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(63, 32)
        Me.btnExecute.TabIndex = 8
        Me.btnExecute.Text = "実行"
        Me.btnExecute.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(272, 250)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(63, 32)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "ｷｬﾝｾﾙ"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        '勤務表印刷条件
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(347, 299)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnExecute)
        Me.Controls.Add(Me.sign3Box)
        Me.Controls.Add(Me.sign2Box)
        Me.Controls.Add(Me.sign1Box)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "勤務表印刷条件"
        Me.Text = "勤務表印刷条件"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtnB4S2 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnB4S As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnB4 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnA4 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtnPR As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnResult As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnPlan As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents sign1Box As System.Windows.Forms.TextBox
    Friend WithEvents sign2Box As System.Windows.Forms.TextBox
    Friend WithEvents sign3Box As System.Windows.Forms.TextBox
    Friend WithEvents btnExecute As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
