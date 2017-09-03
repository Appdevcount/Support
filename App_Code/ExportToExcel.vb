
Imports System
Imports System.Data
Imports System.Configuration
Imports System.IO
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Public Class ExportToExcel

    Public Shared Sub Export(ByVal fileName As String, ByVal gv As GridView)
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", fileName))
        HttpContext.Current.Response.ContentType = "application/ms-excel"
        Dim sw As StringWriter = New StringWriter
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        '  Create a form to contain the grid
        Dim table As Table = New Table
        table.GridLines = gv.GridLines
        '  add the header row to the table
        If (Not (gv.HeaderRow) Is Nothing) Then
            ExportToExcel.PrepareControlForExport(gv.HeaderRow)
            table.Rows.Add(gv.HeaderRow)
        End If
        '  add each of the data rows to the table
        For Each row As GridViewRow In gv.Rows
            ExportToExcel.PrepareControlForExport(row)
            table.Rows.Add(row)
        Next
        '  add the footer row to the table
        If (Not (gv.FooterRow) Is Nothing) Then
            ExportToExcel.PrepareControlForExport(gv.FooterRow)
            table.Rows.Add(gv.FooterRow)
        End If
        '  render the table into the htmlwriter
        table.RenderControl(htw)
        '  render the htmlwriter into the response
        HttpContext.Current.Response.Write(sw.ToString)
        HttpContext.Current.Response.End()
    End Sub

    ' Replace any of the contained controls with literals
    Private Shared Sub PrepareControlForExport(ByVal control As Control)
        Dim i As Integer = 0
        Do While (i < control.Controls.Count)
            Dim current As Control = control.Controls(i)
            If (TypeOf current Is LinkButton) Then
                control.Controls.Remove(current)
                control.Controls.AddAt(i, New LiteralControl(CType(current, LinkButton).Text))
            ElseIf (TypeOf current Is ImageButton) Then
                control.Controls.Remove(current)
                control.Controls.AddAt(i, New LiteralControl(CType(current, ImageButton).AlternateText))
            ElseIf (TypeOf current Is HyperLink) Then
                control.Controls.Remove(current)
                control.Controls.AddAt(i, New LiteralControl(CType(current, HyperLink).Text))
            ElseIf (TypeOf current Is DropDownList) Then
                control.Controls.Remove(current)
                control.Controls.AddAt(i, New LiteralControl(CType(current, DropDownList).SelectedItem.Text))
            ElseIf (TypeOf current Is CheckBox) Then
                control.Controls.Remove(current)
                control.Controls.AddAt(i, New LiteralControl(CType(current, CheckBox).Checked))
                'TODO: Warning!!!, inline IF is not supported ?
            End If
            If current.HasControls Then
                ExportToExcel.PrepareControlForExport(current)
            End If
            i = (i + 1)
        Loop
    End Sub

    Public Shared Function get_equiv_charb(ByVal hexval As String) As String

        Dim charval As String = String.Empty

        ' Arabic Letters
        If hexval = "0627" Then
            charval = "ا"
        ElseIf hexval = "0628" Then
            charval = "ب"
        ElseIf hexval = "0629" Then
            charval = "ة"
        ElseIf hexval = "062A" Then
            charval = "ت"
        ElseIf hexval = "062B" Then
            charval = "ث"
        ElseIf hexval = "062C" Then
            charval = "ج"
        ElseIf hexval = "062D" Then
            charval = "ح"
        ElseIf hexval = "062E" Then
            charval = "خ"
        ElseIf hexval = "062F" Then
            charval = "د"
        ElseIf hexval = "0630" Then
            charval = "ذ"
        ElseIf hexval = "0631" Then
            charval = "ر"
        ElseIf hexval = "0632" Then
            charval = "ز"
        ElseIf hexval = "0633" Then
            charval = "س"
        ElseIf hexval = "0634" Then
            charval = "ش"
        ElseIf hexval = "0635" Then
            charval = "ص"
        ElseIf hexval = "0636" Then
            charval = "ض"
        ElseIf hexval = "0637" Then
            charval = "ط"
        ElseIf hexval = "0638" Then
            charval = "ظ"
        ElseIf hexval = "0639" Then
            charval = "ع"
        ElseIf hexval = "063A" Then
            charval = "غ"
        ElseIf hexval = "0641" Then
            charval = "ف"
        ElseIf hexval = "0642" Then
            charval = "ق"
        ElseIf hexval = "0643" Then
            charval = "ك"
        ElseIf hexval = "0644" Then
            charval = "ل"
        ElseIf hexval = "0645" Then
            charval = "م"
        ElseIf hexval = "0646" Then
            charval = "ن"
        ElseIf hexval = "0647" Then
            charval = "ه"
        ElseIf hexval = "0648" Then
            charval = "و"
        ElseIf hexval = "0649" Then
            charval = "ى"
        ElseIf hexval = "064A" Then
            charval = "ي"
            '*****Persian Alphabets
        ElseIf hexval = "0F01" Then
            charval = "پ"
        ElseIf hexval = "0F02" Then
            charval = "ک"
        ElseIf hexval = "0F03" Then
            charval = "گ"
        ElseIf hexval = "0F04" Then
            charval = "ژ"
        ElseIf hexval = "0F05" Then
            charval = "چ"
            '******************************
        ElseIf hexval = "0621" Then
            charval = "ء"
        ElseIf hexval = "0622" Then
            charval = "آ"
        ElseIf hexval = "0623" Then
            charval = "أ"
        ElseIf hexval = "0624" Then
            charval = "ؤ"
        ElseIf hexval = "0625" Then
            charval = "إ"
        ElseIf hexval = "0626" Then
            charval = "ئ"
            ' Arabic (Hex)
        ElseIf hexval = "00C7" Then
            charval = "ا"
        ElseIf hexval = "00C8" Then
            charval = "ب"
        ElseIf hexval = "00C9" Then
            charval = "ة"
        ElseIf hexval = "00CA" Then
            charval = "ت"
        ElseIf hexval = "00CB" Then
            charval = "ث"
        ElseIf hexval = "00CC" Then
            charval = "ج"
        ElseIf hexval = "00CD" Then
            charval = "ح"
        ElseIf hexval = "00CE" Then
            charval = "خ"
        ElseIf hexval = "00CF" Then
            charval = "د"
        ElseIf hexval = "00D0" Then
            charval = "ذ"
        ElseIf hexval = "00D1" Then
            charval = "ر"
        ElseIf hexval = "00D2" Then
            charval = "ز"
        ElseIf hexval = "00D3" Then
            charval = "س"
        ElseIf hexval = "00D4" Then
            charval = "ش"
        ElseIf hexval = "00D5" Then
            charval = "ص"
        ElseIf hexval = "00D6" Then
            charval = "ض"
        ElseIf hexval = "00D8" Then
            charval = "ط"
        ElseIf hexval = "00D9" Then
            charval = "ظ"
        ElseIf hexval = "00DA" Then
            charval = "ع"
        ElseIf hexval = "00DB" Then
            charval = "غ"
        ElseIf hexval = "00DC" Then
            charval = "ف"
        ElseIf hexval = "00DE" Then
            charval = "ق"
        ElseIf hexval = "00DF" Then
            charval = "ك"
        ElseIf hexval = "00E1" Then
            charval = "ل"
        ElseIf hexval = "00E3" Then
            charval = "م"
        ElseIf hexval = "00E4" Then
            charval = "ن"
        ElseIf hexval = "00E5" Then
            charval = "ه"
        ElseIf hexval = "00E6" Then
            charval = "و"
        ElseIf hexval = "00EC" Then
            charval = "ى"
        ElseIf hexval = "00ED" Then
            charval = "ي"
        ElseIf hexval = "00C1" Then
            charval = "ء"
        ElseIf hexval = "00C2" Then
            charval = "آ"
        ElseIf hexval = "00C3" Then
            charval = "أ"
        ElseIf hexval = "00C4" Then
            charval = "ؤ"
        ElseIf hexval = "00C5" Then
            charval = "إ"
        ElseIf hexval = "00C6" Then
            charval = "ئ"

            ' English Letters (Capital)
        ElseIf hexval = "0041" Then
            charval = "A"
        ElseIf hexval = "0042" Then
            charval = "B"
        ElseIf hexval = "0043" Then
            charval = "C"
        ElseIf hexval = "0044" Then
            charval = "D"
        ElseIf hexval = "0045" Then
            charval = "E"
        ElseIf hexval = "0046" Then
            charval = "F"
        ElseIf hexval = "0047" Then
            charval = "G"
        ElseIf hexval = "0048" Then
            charval = "H"
        ElseIf hexval = "0049" Then
            charval = "I"
        ElseIf hexval = "004A" Then
            charval = "J"
        ElseIf hexval = "004B" Then
            charval = "K"
        ElseIf hexval = "004C" Then
            charval = "L"
        ElseIf hexval = "004D" Then
            charval = "M"
        ElseIf hexval = "004E" Then
            charval = "N"
        ElseIf hexval = "004F" Then
            charval = "O"
        ElseIf hexval = "0050" Then
            charval = "P"
        ElseIf hexval = "0051" Then
            charval = "Q"
        ElseIf hexval = "0052" Then
            charval = "R"
        ElseIf hexval = "0053" Then
            charval = "S"
        ElseIf hexval = "0054" Then
            charval = "T"
        ElseIf hexval = "0055" Then
            charval = "U"
        ElseIf hexval = "0056" Then
            charval = "V"
        ElseIf hexval = "0057" Then
            charval = "W"
        ElseIf hexval = "0058" Then
            charval = "X"
        ElseIf hexval = "0059" Then
            charval = "Y"
        ElseIf hexval = "005A" Then
            charval = "Z"
            ' (Small)
        ElseIf hexval = "0061" Then
            charval = "a"
        ElseIf hexval = "0062" Then
            charval = "b"
        ElseIf hexval = "0063" Then
            charval = "c"
        ElseIf hexval = "0064" Then
            charval = "d"
        ElseIf hexval = "0065" Then
            charval = "e"
        ElseIf hexval = "0066" Then
            charval = "f"
        ElseIf hexval = "0067" Then
            charval = "g"
        ElseIf hexval = "0068" Then
            charval = "h"
        ElseIf hexval = "0069" Then
            charval = "i"
        ElseIf hexval = "006A" Then
            charval = "j"
        ElseIf hexval = "006B" Then
            charval = "k"
        ElseIf hexval = "006C" Then
            charval = "l"
        ElseIf hexval = "006D" Then
            charval = "m"
        ElseIf hexval = "006E" Then
            charval = "n"
        ElseIf hexval = "006F" Then
            charval = "o"
        ElseIf hexval = "0070" Then
            charval = "p"
        ElseIf hexval = "0071" Then
            charval = "q"
        ElseIf hexval = "0072" Then
            charval = "r"
        ElseIf hexval = "0073" Then
            charval = "s"
        ElseIf hexval = "0074" Then
            charval = "t"
        ElseIf hexval = "0075" Then
            charval = "u"
        ElseIf hexval = "0076" Then
            charval = "v"
        ElseIf hexval = "0077" Then
            charval = "w"
        ElseIf hexval = "0078" Then
            charval = "x"
        ElseIf hexval = "0079" Then
            charval = "y"
        ElseIf hexval = "007A" Then
            charval = "z"

            ' Arabic Numbers
        ElseIf hexval = "0660" Then
            charval = "0"
        ElseIf hexval = "0661" Then
            charval = "1"
        ElseIf hexval = "0662" Then
            charval = "2"
        ElseIf hexval = "0663" Then
            charval = "3"
        ElseIf hexval = "0664" Then
            charval = "4"
        ElseIf hexval = "0665" Then
            charval = "5"
        ElseIf hexval = "0666" Then
            charval = "6"
        ElseIf hexval = "0667" Then
            charval = "7"
        ElseIf hexval = "0668" Then
            charval = "8"
        ElseIf hexval = "0669" Then
            charval = "9"

            ' English Numbers
        ElseIf hexval = "0030" Then
            charval = "0"
        ElseIf hexval = "0031" Then
            charval = "1"
        ElseIf hexval = "0032" Then
            charval = "2"
        ElseIf hexval = "0033" Then
            charval = "3"
        ElseIf hexval = "0034" Then
            charval = "4"
        ElseIf hexval = "0035" Then
            charval = "5"
        ElseIf hexval = "0036" Then
            charval = "6"
        ElseIf hexval = "0037" Then
            charval = "7"
        ElseIf hexval = "0038" Then
            charval = "8"
        ElseIf hexval = "0039" Then
            charval = "9"

            ' Special Characters
        ElseIf hexval = "0020" Then
            charval = " " 'space
        ElseIf hexval = "061F" Then
            charval = "؟"
        ElseIf hexval = "0640" Then
            charval = "ـ"
        ElseIf hexval = "066A" Then
            charval = "%"
        ElseIf hexval = "0021" Then
            charval = "!"
        ElseIf hexval = "003B" Then
            charval = ";"
        ElseIf hexval = "003A" Then
            charval = ":"
        ElseIf hexval = "005F" Then
            charval = "_"
        ElseIf hexval = "002F" Then
            charval = "/"
        ElseIf hexval = "005C" Then
            charval = "\\"
            '//elseif hexval= "000A" Then charval= chr(13) . chr(10);
        ElseIf hexval = "000D" Then
            charval = Chr(13)
        ElseIf hexval = "000A" Then
            charval = Chr(10)
        ElseIf hexval = "0040" Then
            charval = "@"
        ElseIf hexval = "002E" Then
            charval = "."
        ElseIf hexval = "060C" Then
            charval = "،"
        ElseIf hexval = "061B" Then
            charval = "؛"
        ElseIf hexval = "002C" Then
            charval = ","
        ElseIf hexval = "003F" Then
            charval = "?"
        ElseIf hexval = "0027" Then
            charval = "\'"
        ElseIf hexval = "0028" Then
            charval = "("
        ElseIf hexval = "0029" Then
            charval = ")"
        ElseIf hexval = "0023" Then
            charval = "#"
        ElseIf hexval = "007E" Then
            charval = "~"
        ElseIf hexval = "003C" Then
            charval = "<"
        ElseIf hexval = "003E" Then
            charval = ">"
        ElseIf hexval = "0026" Then
            charval = "&"
        ElseIf hexval = "0024" Then
            charval = "$"
        ElseIf hexval = "00A3" Then
            charval = "£"
        ElseIf hexval = "00A5" Then
            charval = "¥"
        ElseIf hexval = "00A4" Then
            charval = "¤"
        ElseIf hexval = "00A7" Then
            charval = "§"
        ElseIf hexval = "00A1" Then
            charval = "" '// inverted exclamation mark
        ElseIf hexval = "00BF" Then
            charval = "" '// inverted question mark
        ElseIf hexval = "002A" Then
            charval = "*"
        ElseIf hexval = "002D" Then
            charval = "-"
        ElseIf hexval = "002B" Then
            charval = "+"
        ElseIf hexval = "003D" Then
            charval = "="
        End If
        get_equiv_charb = charval
    End Function

    Public Shared Function ucs2arabic(ByVal s As String) As String

        Dim ucs2, ucs As String
        ucs2 = String.Empty

        For i = 1 To s.Length Step 4
            ucs = get_equiv_charb(Microsoft.VisualBasic.Mid(s, i, 4))
            ucs2 = ucs2 & ucs
        Next

        ucs2arabic = ucs2
    End Function
End Class




