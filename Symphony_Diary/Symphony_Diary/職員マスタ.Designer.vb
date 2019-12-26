<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class 職員マスタ
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
        Me.syuListBox = New System.Windows.Forms.ListBox()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnRegist = New System.Windows.Forms.Button()
        Me.memoBox = New System.Windows.Forms.TextBox()
        Me.kinGroupBox = New System.Windows.Forms.GroupBox()
        Me.rbtnKin5 = New System.Windows.Forms.RadioButton()
        Me.rbtnKin8 = New System.Windows.Forms.RadioButton()
        Me.rbtnKin7 = New System.Windows.Forms.RadioButton()
        Me.rbtnKin6 = New System.Windows.Forms.RadioButton()
        Me.rbtnKin4 = New System.Windows.Forms.RadioButton()
        Me.rbtnKin3 = New System.Windows.Forms.RadioButton()
        Me.rbtnKin2 = New System.Windows.Forms.RadioButton()
        Me.rbtnKin1 = New System.Windows.Forms.RadioButton()
        Me.keiGroupBox = New System.Windows.Forms.GroupBox()
        Me.rbtnKei4 = New System.Windows.Forms.RadioButton()
        Me.rbtnKei3 = New System.Windows.Forms.RadioButton()
        Me.rbtnKei2 = New System.Windows.Forms.RadioButton()
        Me.rbtnKei1 = New System.Windows.Forms.RadioButton()
        Me.syuLabel = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.namBox = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.idBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvNamM = New System.Windows.Forms.DataGridView()
        Me.GroupBox1.SuspendLayout()
        Me.kinGroupBox.SuspendLayout()
        Me.keiGroupBox.SuspendLayout()
        CType(Me.dgvNamM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.syuListBox)
        Me.GroupBox1.Controls.Add(Me.btnDelete)
        Me.GroupBox1.Controls.Add(Me.btnRegist)
        Me.GroupBox1.Controls.Add(Me.memoBox)
        Me.GroupBox1.Controls.Add(Me.kinGroupBox)
        Me.GroupBox1.Controls.Add(Me.keiGroupBox)
        Me.GroupBox1.Controls.Add(Me.syuLabel)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.namBox)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.idBox)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(19, 15)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(775, 238)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'syuListBox
        '
        Me.syuListBox.BackColor = System.Drawing.SystemColors.Control
        Me.syuListBox.FormattingEnabled = True
        Me.syuListBox.ItemHeight = 12
        Me.syuListBox.Location = New System.Drawing.Point(603, 30)
        Me.syuListBox.Name = "syuListBox"
        Me.syuListBox.Size = New System.Drawing.Size(144, 196)
        Me.syuListBox.TabIndex = 15
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(492, 199)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 33)
        Me.btnDelete.TabIndex = 14
        Me.btnDelete.Text = "削除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnRegist
        '
        Me.btnRegist.Location = New System.Drawing.Point(415, 199)
        Me.btnRegist.Name = "btnRegist"
        Me.btnRegist.Size = New System.Drawing.Size(75, 33)
        Me.btnRegist.TabIndex = 8
        Me.btnRegist.Text = "登録"
        Me.btnRegist.UseVisualStyleBackColor = True
        '
        'memoBox
        '
        Me.memoBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.memoBox.Location = New System.Drawing.Point(87, 207)
        Me.memoBox.Name = "memoBox"
        Me.memoBox.Size = New System.Drawing.Size(288, 19)
        Me.memoBox.TabIndex = 7
        '
        'kinGroupBox
        '
        Me.kinGroupBox.Controls.Add(Me.rbtnKin5)
        Me.kinGroupBox.Controls.Add(Me.rbtnKin8)
        Me.kinGroupBox.Controls.Add(Me.rbtnKin7)
        Me.kinGroupBox.Controls.Add(Me.rbtnKin6)
        Me.kinGroupBox.Controls.Add(Me.rbtnKin4)
        Me.kinGroupBox.Controls.Add(Me.rbtnKin3)
        Me.kinGroupBox.Controls.Add(Me.rbtnKin2)
        Me.kinGroupBox.Controls.Add(Me.rbtnKin1)
        Me.kinGroupBox.Location = New System.Drawing.Point(87, 113)
        Me.kinGroupBox.Name = "kinGroupBox"
        Me.kinGroupBox.Size = New System.Drawing.Size(480, 82)
        Me.kinGroupBox.TabIndex = 11
        Me.kinGroupBox.TabStop = False
        '
        'rbtnKin5
        '
        Me.rbtnKin5.AutoSize = True
        Me.rbtnKin5.Location = New System.Drawing.Point(328, 16)
        Me.rbtnKin5.Name = "rbtnKin5"
        Me.rbtnKin5.Size = New System.Drawing.Size(99, 16)
        Me.rbtnKin5.TabIndex = 11
        Me.rbtnKin5.TabStop = True
        Me.rbtnKin5.Text = "ﾍﾙﾊﾟｰｽﾃｰｼｮﾝ"
        Me.rbtnKin5.UseVisualStyleBackColor = True
        '
        'rbtnKin8
        '
        Me.rbtnKin8.AutoSize = True
        Me.rbtnKin8.Location = New System.Drawing.Point(328, 50)
        Me.rbtnKin8.Name = "rbtnKin8"
        Me.rbtnKin8.Size = New System.Drawing.Size(95, 16)
        Me.rbtnKin8.TabIndex = 10
        Me.rbtnKin8.TabStop = True
        Me.rbtnKin8.Text = "生活支援ﾊｳｽ"
        Me.rbtnKin8.UseVisualStyleBackColor = True
        '
        'rbtnKin7
        '
        Me.rbtnKin7.AutoSize = True
        Me.rbtnKin7.Location = New System.Drawing.Point(165, 50)
        Me.rbtnKin7.Name = "rbtnKin7"
        Me.rbtnKin7.Size = New System.Drawing.Size(124, 16)
        Me.rbtnKin7.TabIndex = 9
        Me.rbtnKin7.TabStop = True
        Me.rbtnKin7.Text = "老人介護支援ｾﾝﾀｰ"
        Me.rbtnKin7.UseVisualStyleBackColor = True
        '
        'rbtnKin6
        '
        Me.rbtnKin6.AutoSize = True
        Me.rbtnKin6.Location = New System.Drawing.Point(16, 50)
        Me.rbtnKin6.Name = "rbtnKin6"
        Me.rbtnKin6.Size = New System.Drawing.Size(95, 16)
        Me.rbtnKin6.TabIndex = 8
        Me.rbtnKin6.TabStop = True
        Me.rbtnKin6.Text = "居宅介護支援"
        Me.rbtnKin6.UseVisualStyleBackColor = True
        '
        'rbtnKin4
        '
        Me.rbtnKin4.AutoSize = True
        Me.rbtnKin4.Location = New System.Drawing.Point(245, 16)
        Me.rbtnKin4.Name = "rbtnKin4"
        Me.rbtnKin4.Size = New System.Drawing.Size(73, 16)
        Me.rbtnKin4.TabIndex = 7
        Me.rbtnKin4.TabStop = True
        Me.rbtnKin4.Text = "ﾃﾞｲｻｰﾋﾞｽ"
        Me.rbtnKin4.UseVisualStyleBackColor = True
        '
        'rbtnKin3
        '
        Me.rbtnKin3.AutoSize = True
        Me.rbtnKin3.Location = New System.Drawing.Point(165, 16)
        Me.rbtnKin3.Name = "rbtnKin3"
        Me.rbtnKin3.Size = New System.Drawing.Size(71, 16)
        Me.rbtnKin3.TabIndex = 6
        Me.rbtnKin3.TabStop = True
        Me.rbtnKin3.Text = "ｼｮｰﾄｽﾃｲ"
        Me.rbtnKin3.UseVisualStyleBackColor = True
        '
        'rbtnKin2
        '
        Me.rbtnKin2.AutoSize = True
        Me.rbtnKin2.Location = New System.Drawing.Point(91, 16)
        Me.rbtnKin2.Name = "rbtnKin2"
        Me.rbtnKin2.Size = New System.Drawing.Size(47, 16)
        Me.rbtnKin2.TabIndex = 5
        Me.rbtnKin2.TabStop = True
        Me.rbtnKin2.Text = "事務"
        Me.rbtnKin2.UseVisualStyleBackColor = True
        '
        'rbtnKin1
        '
        Me.rbtnKin1.AutoSize = True
        Me.rbtnKin1.Location = New System.Drawing.Point(16, 16)
        Me.rbtnKin1.Name = "rbtnKin1"
        Me.rbtnKin1.Size = New System.Drawing.Size(47, 16)
        Me.rbtnKin1.TabIndex = 4
        Me.rbtnKin1.TabStop = True
        Me.rbtnKin1.Text = "特養"
        Me.rbtnKin1.UseVisualStyleBackColor = True
        '
        'keiGroupBox
        '
        Me.keiGroupBox.Controls.Add(Me.rbtnKei4)
        Me.keiGroupBox.Controls.Add(Me.rbtnKei3)
        Me.keiGroupBox.Controls.Add(Me.rbtnKei2)
        Me.keiGroupBox.Controls.Add(Me.rbtnKei1)
        Me.keiGroupBox.Location = New System.Drawing.Point(87, 66)
        Me.keiGroupBox.Name = "keiGroupBox"
        Me.keiGroupBox.Size = New System.Drawing.Size(480, 40)
        Me.keiGroupBox.TabIndex = 10
        Me.keiGroupBox.TabStop = False
        '
        'rbtnKei4
        '
        Me.rbtnKei4.AutoSize = True
        Me.rbtnKei4.Location = New System.Drawing.Point(328, 14)
        Me.rbtnKei4.Name = "rbtnKei4"
        Me.rbtnKei4.Size = New System.Drawing.Size(95, 16)
        Me.rbtnKei4.TabIndex = 3
        Me.rbtnKei4.TabStop = True
        Me.rbtnKei4.Text = "常勤以外兼務"
        Me.rbtnKei4.UseVisualStyleBackColor = True
        '
        'rbtnKei3
        '
        Me.rbtnKei3.AutoSize = True
        Me.rbtnKei3.Location = New System.Drawing.Point(211, 14)
        Me.rbtnKei3.Name = "rbtnKei3"
        Me.rbtnKei3.Size = New System.Drawing.Size(95, 16)
        Me.rbtnKei3.TabIndex = 2
        Me.rbtnKei3.TabStop = True
        Me.rbtnKei3.Text = "常勤以外専従"
        Me.rbtnKei3.UseVisualStyleBackColor = True
        '
        'rbtnKei2
        '
        Me.rbtnKei2.AutoSize = True
        Me.rbtnKei2.Location = New System.Drawing.Point(114, 14)
        Me.rbtnKei2.Name = "rbtnKei2"
        Me.rbtnKei2.Size = New System.Drawing.Size(71, 16)
        Me.rbtnKei2.TabIndex = 1
        Me.rbtnKei2.TabStop = True
        Me.rbtnKei2.Text = "常勤兼務"
        Me.rbtnKei2.UseVisualStyleBackColor = True
        '
        'rbtnKei1
        '
        Me.rbtnKei1.AutoSize = True
        Me.rbtnKei1.Location = New System.Drawing.Point(16, 14)
        Me.rbtnKei1.Name = "rbtnKei1"
        Me.rbtnKei1.Size = New System.Drawing.Size(71, 16)
        Me.rbtnKei1.TabIndex = 0
        Me.rbtnKei1.TabStop = True
        Me.rbtnKei1.Text = "常勤専従"
        Me.rbtnKei1.UseVisualStyleBackColor = True
        '
        'syuLabel
        '
        Me.syuLabel.AutoSize = True
        Me.syuLabel.Location = New System.Drawing.Point(442, 36)
        Me.syuLabel.Name = "syuLabel"
        Me.syuLabel.Size = New System.Drawing.Size(0, 12)
        Me.syuLabel.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(396, 36)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(29, 12)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "職種"
        '
        'namBox
        '
        Me.namBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.namBox.Location = New System.Drawing.Point(214, 33)
        Me.namBox.Name = "namBox"
        Me.namBox.Size = New System.Drawing.Size(97, 19)
        Me.namBox.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(176, 36)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(29, 12)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "氏名"
        '
        'idBox
        '
        Me.idBox.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.idBox.Location = New System.Drawing.Point(87, 33)
        Me.idBox.Name = "idBox"
        Me.idBox.Size = New System.Drawing.Size(69, 19)
        Me.idBox.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(38, 210)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(29, 12)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "特記"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(38, 124)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 12)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "勤務"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(38, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 12)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "形態"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(38, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "職員No."
        '
        'dgvNamM
        '
        Me.dgvNamM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNamM.Location = New System.Drawing.Point(19, 267)
        Me.dgvNamM.Name = "dgvNamM"
        Me.dgvNamM.RowTemplate.Height = 21
        Me.dgvNamM.Size = New System.Drawing.Size(775, 339)
        Me.dgvNamM.TabIndex = 1
        '
        '職員マスタ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(815, 627)
        Me.Controls.Add(Me.dgvNamM)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "職員マスタ"
        Me.Text = "Diary 職員マスタ"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.kinGroupBox.ResumeLayout(False)
        Me.kinGroupBox.PerformLayout()
        Me.keiGroupBox.ResumeLayout(False)
        Me.keiGroupBox.PerformLayout()
        CType(Me.dgvNamM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents idBox As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents namBox As System.Windows.Forms.TextBox
    Friend WithEvents syuLabel As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnRegist As System.Windows.Forms.Button
    Friend WithEvents memoBox As System.Windows.Forms.TextBox
    Friend WithEvents kinGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents keiGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents syuListBox As System.Windows.Forms.ListBox
    Friend WithEvents rbtnKin5 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKin8 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKin7 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKin6 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKin4 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKin3 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKin2 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKin1 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKei4 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKei3 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKei2 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnKei1 As System.Windows.Forms.RadioButton
    Friend WithEvents dgvNamM As System.Windows.Forms.DataGridView
End Class
