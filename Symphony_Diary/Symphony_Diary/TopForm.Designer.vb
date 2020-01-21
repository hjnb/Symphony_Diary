<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TopForm
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnTok = New System.Windows.Forms.Button()
        Me.btnJim = New System.Windows.Forms.Button()
        Me.btnDay = New System.Windows.Forms.Button()
        Me.btnSyo = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.btnSei = New System.Windows.Forms.Button()
        Me.btnKyo = New System.Windows.Forms.Button()
        Me.btnHel = New System.Windows.Forms.Button()
        Me.btnReadCSV = New System.Windows.Forms.Button()
        Me.btnCreateCSV = New System.Windows.Forms.Button()
        Me.rbtnPreview = New System.Windows.Forms.RadioButton()
        Me.rbtnPrint = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnConstM = New System.Windows.Forms.Button()
        Me.btnSyoM = New System.Windows.Forms.Button()
        Me.btnKinM = New System.Windows.Forms.Button()
        Me.btnDB = New System.Windows.Forms.Button()
        Me.adBox = New ADBox.adBox()
        Me.saveCSVFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(97, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 15)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "対象年月"
        '
        'btnTok
        '
        Me.btnTok.Location = New System.Drawing.Point(169, 79)
        Me.btnTok.Name = "btnTok"
        Me.btnTok.Size = New System.Drawing.Size(174, 45)
        Me.btnTok.TabIndex = 1
        Me.btnTok.Text = "特養"
        Me.btnTok.UseVisualStyleBackColor = True
        '
        'btnJim
        '
        Me.btnJim.Location = New System.Drawing.Point(169, 123)
        Me.btnJim.Name = "btnJim"
        Me.btnJim.Size = New System.Drawing.Size(174, 45)
        Me.btnJim.TabIndex = 2
        Me.btnJim.Text = "事務"
        Me.btnJim.UseVisualStyleBackColor = True
        '
        'btnDay
        '
        Me.btnDay.Location = New System.Drawing.Point(169, 211)
        Me.btnDay.Name = "btnDay"
        Me.btnDay.Size = New System.Drawing.Size(174, 45)
        Me.btnDay.TabIndex = 4
        Me.btnDay.Text = "ﾃﾞｲｻｰﾋﾞｽ"
        Me.btnDay.UseVisualStyleBackColor = True
        '
        'btnSyo
        '
        Me.btnSyo.Location = New System.Drawing.Point(169, 167)
        Me.btnSyo.Name = "btnSyo"
        Me.btnSyo.Size = New System.Drawing.Size(174, 45)
        Me.btnSyo.TabIndex = 3
        Me.btnSyo.Text = "ｼｮｰﾄｽﾃｲ"
        Me.btnSyo.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(169, 387)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(174, 45)
        Me.Button5.TabIndex = 8
        Me.Button5.UseVisualStyleBackColor = True
        '
        'btnSei
        '
        Me.btnSei.Location = New System.Drawing.Point(169, 343)
        Me.btnSei.Name = "btnSei"
        Me.btnSei.Size = New System.Drawing.Size(174, 45)
        Me.btnSei.TabIndex = 7
        Me.btnSei.Text = "生活支援ﾊｳｽ"
        Me.btnSei.UseVisualStyleBackColor = True
        '
        'btnKyo
        '
        Me.btnKyo.Location = New System.Drawing.Point(169, 299)
        Me.btnKyo.Name = "btnKyo"
        Me.btnKyo.Size = New System.Drawing.Size(174, 45)
        Me.btnKyo.TabIndex = 6
        Me.btnKyo.Text = "居宅介護支援"
        Me.btnKyo.UseVisualStyleBackColor = True
        '
        'btnHel
        '
        Me.btnHel.Location = New System.Drawing.Point(169, 255)
        Me.btnHel.Name = "btnHel"
        Me.btnHel.Size = New System.Drawing.Size(174, 45)
        Me.btnHel.TabIndex = 5
        Me.btnHel.Text = "ﾍﾙﾊﾟｰｽﾃｰｼｮﾝ"
        Me.btnHel.UseVisualStyleBackColor = True
        '
        'btnReadCSV
        '
        Me.btnReadCSV.Location = New System.Drawing.Point(169, 494)
        Me.btnReadCSV.Name = "btnReadCSV"
        Me.btnReadCSV.Size = New System.Drawing.Size(174, 45)
        Me.btnReadCSV.TabIndex = 10
        Me.btnReadCSV.Text = "ＣＳＶ読込み"
        Me.btnReadCSV.UseVisualStyleBackColor = True
        '
        'btnCreateCSV
        '
        Me.btnCreateCSV.Location = New System.Drawing.Point(169, 450)
        Me.btnCreateCSV.Name = "btnCreateCSV"
        Me.btnCreateCSV.Size = New System.Drawing.Size(174, 45)
        Me.btnCreateCSV.TabIndex = 9
        Me.btnCreateCSV.Text = "ＣＳＶ書出し"
        Me.btnCreateCSV.UseVisualStyleBackColor = True
        '
        'rbtnPreview
        '
        Me.rbtnPreview.AutoSize = True
        Me.rbtnPreview.Location = New System.Drawing.Point(402, 53)
        Me.rbtnPreview.Name = "rbtnPreview"
        Me.rbtnPreview.Size = New System.Drawing.Size(63, 16)
        Me.rbtnPreview.TabIndex = 11
        Me.rbtnPreview.TabStop = True
        Me.rbtnPreview.Text = "ﾌﾟﾚﾋﾞｭｰ"
        Me.rbtnPreview.UseVisualStyleBackColor = True
        '
        'rbtnPrint
        '
        Me.rbtnPrint.AutoSize = True
        Me.rbtnPrint.Location = New System.Drawing.Point(484, 53)
        Me.rbtnPrint.Name = "rbtnPrint"
        Me.rbtnPrint.Size = New System.Drawing.Size(47, 16)
        Me.rbtnPrint.TabIndex = 12
        Me.rbtnPrint.TabStop = True
        Me.rbtnPrint.Text = "印刷"
        Me.rbtnPrint.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnConstM)
        Me.GroupBox1.Controls.Add(Me.btnSyoM)
        Me.GroupBox1.Controls.Add(Me.btnKinM)
        Me.GroupBox1.Location = New System.Drawing.Point(402, 79)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(209, 153)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "ﾏｽﾀ"
        '
        'btnConstM
        '
        Me.btnConstM.Location = New System.Drawing.Point(26, 97)
        Me.btnConstM.Name = "btnConstM"
        Me.btnConstM.Size = New System.Drawing.Size(160, 38)
        Me.btnConstM.TabIndex = 16
        Me.btnConstM.Text = "定数"
        Me.btnConstM.UseVisualStyleBackColor = True
        '
        'btnSyoM
        '
        Me.btnSyoM.Location = New System.Drawing.Point(26, 23)
        Me.btnSyoM.Name = "btnSyoM"
        Me.btnSyoM.Size = New System.Drawing.Size(160, 38)
        Me.btnSyoM.TabIndex = 14
        Me.btnSyoM.Text = "職員"
        Me.btnSyoM.UseVisualStyleBackColor = True
        '
        'btnKinM
        '
        Me.btnKinM.Location = New System.Drawing.Point(26, 60)
        Me.btnKinM.Name = "btnKinM"
        Me.btnKinM.Size = New System.Drawing.Size(160, 38)
        Me.btnKinM.TabIndex = 15
        Me.btnKinM.Text = "勤務項目名"
        Me.btnKinM.UseVisualStyleBackColor = True
        '
        'btnDB
        '
        Me.btnDB.Location = New System.Drawing.Point(428, 256)
        Me.btnDB.Name = "btnDB"
        Me.btnDB.Size = New System.Drawing.Size(160, 38)
        Me.btnDB.TabIndex = 15
        Me.btnDB.Text = "DB整理"
        Me.btnDB.UseVisualStyleBackColor = True
        '
        'adBox
        '
        Me.adBox.dateText = "21"
        Me.adBox.Location = New System.Drawing.Point(167, 24)
        Me.adBox.Mode = 3
        Me.adBox.monthText = "01"
        Me.adBox.Name = "adBox"
        Me.adBox.Size = New System.Drawing.Size(130, 35)
        Me.adBox.TabIndex = 0
        Me.adBox.yearText = "2020"
        '
        'TopForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(663, 599)
        Me.Controls.Add(Me.adBox)
        Me.Controls.Add(Me.btnDB)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.rbtnPrint)
        Me.Controls.Add(Me.rbtnPreview)
        Me.Controls.Add(Me.btnReadCSV)
        Me.Controls.Add(Me.btnCreateCSV)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.btnSei)
        Me.Controls.Add(Me.btnKyo)
        Me.Controls.Add(Me.btnHel)
        Me.Controls.Add(Me.btnDay)
        Me.Controls.Add(Me.btnSyo)
        Me.Controls.Add(Me.btnJim)
        Me.Controls.Add(Me.btnTok)
        Me.Controls.Add(Me.Label1)
        Me.Name = "TopForm"
        Me.Text = "Diary 勤務表"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnTok As System.Windows.Forms.Button
    Friend WithEvents btnJim As System.Windows.Forms.Button
    Friend WithEvents btnDay As System.Windows.Forms.Button
    Friend WithEvents btnSyo As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents btnSei As System.Windows.Forms.Button
    Friend WithEvents btnKyo As System.Windows.Forms.Button
    Friend WithEvents btnHel As System.Windows.Forms.Button
    Friend WithEvents btnReadCSV As System.Windows.Forms.Button
    Friend WithEvents btnCreateCSV As System.Windows.Forms.Button
    Friend WithEvents rbtnPreview As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnPrint As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnConstM As System.Windows.Forms.Button
    Friend WithEvents btnSyoM As System.Windows.Forms.Button
    Friend WithEvents btnKinM As System.Windows.Forms.Button
    Friend WithEvents btnDB As System.Windows.Forms.Button
    Friend WithEvents adBox As ADBox.adBox
    Friend WithEvents saveCSVFileDialog As System.Windows.Forms.SaveFileDialog

End Class
