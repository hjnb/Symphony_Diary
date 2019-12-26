<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class 勤務項目名マスタ
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
        Me.prtBox = New System.Windows.Forms.TextBox()
        Me.seqBox = New System.Windows.Forms.TextBox()
        Me.entBox = New System.Windows.Forms.ComboBox()
        Me.kinBox = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnRegist = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.dgvKmkM = New System.Windows.Forms.DataGridView()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvKmkM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.prtBox)
        Me.GroupBox1.Controls.Add(Me.seqBox)
        Me.GroupBox1.Controls.Add(Me.entBox)
        Me.GroupBox1.Controls.Add(Me.kinBox)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(21, 19)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(324, 154)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'prtBox
        '
        Me.prtBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.prtBox.Location = New System.Drawing.Point(121, 116)
        Me.prtBox.Name = "prtBox"
        Me.prtBox.Size = New System.Drawing.Size(49, 19)
        Me.prtBox.TabIndex = 8
        Me.prtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'seqBox
        '
        Me.seqBox.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.seqBox.Location = New System.Drawing.Point(121, 85)
        Me.seqBox.Name = "seqBox"
        Me.seqBox.Size = New System.Drawing.Size(49, 19)
        Me.seqBox.TabIndex = 7
        Me.seqBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'entBox
        '
        Me.entBox.FormattingEnabled = True
        Me.entBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.entBox.IntegralHeight = False
        Me.entBox.Location = New System.Drawing.Point(121, 52)
        Me.entBox.Name = "entBox"
        Me.entBox.Size = New System.Drawing.Size(136, 20)
        Me.entBox.TabIndex = 6
        '
        'kinBox
        '
        Me.kinBox.FormattingEnabled = True
        Me.kinBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.kinBox.Location = New System.Drawing.Point(121, 19)
        Me.kinBox.Name = "kinBox"
        Me.kinBox.Size = New System.Drawing.Size(136, 20)
        Me.kinBox.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(29, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(29, 12)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "勤務"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(29, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(77, 12)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "入力時項目名"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(29, 120)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 12)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "印刷時項目名"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(29, 89)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "表示順"
        '
        'btnRegist
        '
        Me.btnRegist.Location = New System.Drawing.Point(212, 182)
        Me.btnRegist.Name = "btnRegist"
        Me.btnRegist.Size = New System.Drawing.Size(67, 30)
        Me.btnRegist.TabIndex = 1
        Me.btnRegist.Text = "登録"
        Me.btnRegist.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(278, 182)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(67, 30)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "削除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'dgvKmkM
        '
        Me.dgvKmkM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvKmkM.Location = New System.Drawing.Point(21, 220)
        Me.dgvKmkM.Name = "dgvKmkM"
        Me.dgvKmkM.RowTemplate.Height = 21
        Me.dgvKmkM.Size = New System.Drawing.Size(324, 323)
        Me.dgvKmkM.TabIndex = 3
        '
        '勤務項目名マスタ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(367, 561)
        Me.Controls.Add(Me.dgvKmkM)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnRegist)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "勤務項目名マスタ"
        Me.Text = "Diary 勤務項目名"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.dgvKmkM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents seqBox As System.Windows.Forms.TextBox
    Friend WithEvents entBox As System.Windows.Forms.ComboBox
    Friend WithEvents kinBox As System.Windows.Forms.ComboBox
    Friend WithEvents prtBox As System.Windows.Forms.TextBox
    Friend WithEvents btnRegist As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents dgvKmkM As System.Windows.Forms.DataGridView
End Class
