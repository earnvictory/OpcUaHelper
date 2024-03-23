Imports System.Drawing.Drawing2D

Namespace ControlExt
    Public Class Common
        Enum RoundStyle
            None = 0
            All = 1
            Left = 2
            Right = 3
            Top = 4
            Bottom = 5
            TopLeft = 6
            TopRight = 7
            BottomLeft = 8
            BottomRight = 9
        End Enum
        Public Shared Function DrawRoundRectF(rect As RectangleF, Radius As Integer, Style As RoundStyle) As GraphicsPath
            Dim path As New GraphicsPath()
            If Radius = 0 Then Style = RoundStyle.None
            Select Case Style
                Case RoundStyle.None
                    path.AddRectangle(rect)
                Case RoundStyle.All
                    path.AddArc(rect.X, rect.Y, Radius, Radius, 180, 90)
                    path.AddArc(
                        rect.Right - Radius,
                        rect.Y,
                        Radius,
                        Radius,
                        270,
                        90)
                    path.AddArc(
                        rect.Right - Radius,
                        rect.Bottom - Radius,
                        Radius,
                        Radius, 0, 90)
                    path.AddArc(
                        rect.X,
                        rect.Bottom - Radius,
                        Radius,
                        Radius,
                        90,
                        90)
                Case RoundStyle.Left
                    path.AddArc(rect.X, rect.Y, Radius, Radius, 180, 90)
                    path.AddLine(
                        rect.Right, rect.Y,
                        rect.Right, rect.Bottom)
                    path.AddArc(
                        rect.X,
                        rect.Bottom - Radius,
                        Radius,
                        Radius,
                        90,
                        90)
                Case RoundStyle.Right
                    path.AddArc(
                        rect.Right - Radius,
                        rect.Y,
                        Radius,
                        Radius,
                        270,
                        90)
                    path.AddArc(
                       rect.Right - Radius,
                       rect.Bottom - Radius,
                       Radius,
                       Radius,
                       0,
                       90)
                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y)
                Case RoundStyle.Top
                    path.AddArc(rect.X, rect.Y, Radius, Radius, 180, 90)
                    path.AddArc(
                        rect.Right - Radius,
                        rect.Y,
                        Radius,
                        Radius,
                        270,
                        90)
                    path.AddLine(
                        rect.Right, rect.Bottom,
                        rect.X, rect.Bottom)
                Case RoundStyle.Bottom
                    path.AddArc(
                       rect.Right - Radius,
                       rect.Bottom - Radius,
                       Radius,
                       Radius,
                       0,
                       90)
                    path.AddArc(
                        rect.X,
                        rect.Bottom - Radius,
                        Radius,
                        Radius,
                        90,
                        90)
                    path.AddLine(rect.X, rect.Y, rect.Right, rect.Y)
                Case RoundStyle.TopLeft
                    path.AddArc(rect.X, rect.Y, Radius, Radius, 180, 90)
                    path.AddLine(
                        rect.Right, rect.Y,
                        rect.Right, rect.Bottom)
                    path.AddLine(
                        rect.Right, rect.Bottom,
                        rect.X, rect.Bottom)
                Case RoundStyle.TopRight
                    path.AddArc(
                        rect.Right - Radius,
                        rect.Y,
                        Radius,
                        Radius,
                        270,
                        90)
                    path.AddLine(
                        rect.Right, rect.Bottom,
                        rect.X, rect.Bottom)
                    path.AddLine(
                        rect.X, rect.Bottom,
                        rect.X, rect.Y)

                Case RoundStyle.BottomLeft

                    path.AddArc(
                        rect.X,
                        rect.Bottom - Radius,
                        Radius,
                        Radius,
                        90,
                        90)
                    path.AddLine(rect.X, rect.Y, rect.Right, rect.Y)
                    path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom)
                Case RoundStyle.BottomRight
                    path.AddArc(
                      rect.Right - Radius,
                      rect.Bottom - Radius,
                      Radius,
                      Radius,
                      0,
                      90)

                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y)

                    path.AddLine(rect.X, rect.Y, rect.Right, rect.Y)
            End Select

            path.CloseFigure()
            Return path
        End Function
        ''' <summary>
        ''' 削角矩形
        ''' </summary>
        ''' <param name="rect"></param>
        ''' <param name="Radius"></param>
        ''' <param name="Style"></param>
        ''' <returns></returns>
        Public Shared Function DrawChamferRect(rect As Rectangle, Radius As Integer, Style As RoundStyle) As GraphicsPath
            Dim path As New GraphicsPath()
            If Radius = 0 Then Style = RoundStyle.None
            Dim X As Integer = rect.X
            Dim Y As Integer = rect.Y
            Dim Ra As Integer = Radius
            Dim Ri As Integer = rect.Right
            Dim B As Integer = rect.Bottom
            Select Case Style
                Case RoundStyle.None
                    path.AddRectangle(rect)
                Case RoundStyle.All
                    path.AddLine(X + Ra, Y, Ri - Ra, Y)
                    path.AddLine(Ri, Y + Ra, Ri, B - Ra)
                    path.AddLine(Ri - Ra, B, X + Ra, B)
                    path.AddLine(X, B - Ra, X, Y + Ra)
                Case RoundStyle.Top
                    path.AddLine(X, Y + Ra, X + Ra, Y)
                    path.AddLine(Ri - Ra, Y, Ri, Y + Ra)
                    path.AddLine(Ri, B, X, B)
                Case RoundStyle.Bottom
                    path.AddLine(X, Y, Ri, Y)
                    path.AddLine(Ri, B - Ra, Ri - Ra, B)
                    path.AddLine(X + Ra, B, X, B - Ra)
                Case RoundStyle.TopLeft
                    path.AddLine(X, Y + Ra, X + Ra, Y)
                    path.AddLine(Ri, Y, Ri, B)
                    path.AddLine(Ri, B, X, B)

                Case RoundStyle.TopRight
                    path.AddLine(X, Y, X, B)
                    path.AddLine(X, B, Ri, B)
                    path.AddLine(Ri, Y + Ra, Ri - Ra, Y)

                Case RoundStyle.BottomLeft
                    path.AddLine(X, Y, Ri, Y)
                    path.AddLine(Ri, Y, Ri, B)
                    path.AddLine(X + Ra, B, X, B - Ra)

                Case RoundStyle.BottomRight
                    path.AddLine(X, Y, Ri, Y)
                    path.AddLine(Ri, B - Ra, Ri - Ra, B)
                    path.AddLine(X, B, X, Y)
            End Select

            path.CloseFigure()
            Return path
        End Function
        Public Shared Function DrawRoundRect(rect As Rectangle, Radius As Integer, Style As RoundStyle) As GraphicsPath

            Dim path As New GraphicsPath()
            If Radius = 0 Then Style = RoundStyle.None
            Select Case Style
                Case RoundStyle.None
                    path.AddRectangle(rect)
                Case RoundStyle.All
                    path.AddArc(rect.X, rect.Y, Radius, Radius, 180, 90)
                    path.AddArc(
                        rect.Right - Radius,
                        rect.Y,
                        Radius,
                        Radius,
                        270,
                        90)
                    path.AddArc(
                        rect.Right - Radius,
                        rect.Bottom - Radius,
                        Radius,
                        Radius, 0, 90)
                    path.AddArc(
                        rect.X,
                        rect.Bottom - Radius,
                        Radius,
                        Radius,
                        90,
                        90)
                Case RoundStyle.Left
                    path.AddArc(rect.X, rect.Y, Radius, Radius, 180, 90)
                    path.AddLine(
                        rect.Right, rect.Y,
                        rect.Right, rect.Bottom)
                    path.AddArc(
                        rect.X,
                        rect.Bottom - Radius,
                        Radius,
                        Radius,
                        90,
                        90)
                Case RoundStyle.Right
                    path.AddArc(
                        rect.Right - Radius,
                        rect.Y,
                        Radius,
                        Radius,
                        270,
                        90)
                    path.AddArc(
                       rect.Right - Radius,
                       rect.Bottom - Radius,
                       Radius,
                       Radius,
                       0,
                       90)
                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y)
                Case RoundStyle.Top
                    path.AddArc(rect.X, rect.Y, Radius, Radius, 180, 90)
                    path.AddArc(
                        rect.Right - Radius,
                        rect.Y,
                        Radius,
                        Radius,
                        270,
                        90)
                    path.AddLine(
                        rect.Right, rect.Bottom,
                        rect.X, rect.Bottom)
                Case RoundStyle.Bottom
                    path.AddArc(
                       rect.Right - Radius,
                       rect.Bottom - Radius,
                       Radius,
                       Radius,
                       0,
                       90)
                    path.AddArc(
                        rect.X,
                        rect.Bottom - Radius,
                        Radius,
                        Radius,
                        90,
                        90)
                    path.AddLine(rect.X, rect.Y, rect.Right, rect.Y)
                Case RoundStyle.TopLeft
                    path.AddArc(rect.X, rect.Y, Radius, Radius, 180, 90)
                    path.AddLine(
                        rect.Right, rect.Y,
                        rect.Right, rect.Bottom)
                    path.AddLine(
                        rect.Right, rect.Bottom,
                        rect.X, rect.Bottom)
                Case RoundStyle.TopRight
                    path.AddArc(
                        rect.Right - Radius,
                        rect.Y,
                        Radius,
                        Radius,
                        270,
                        90)
                    path.AddLine(
                        rect.Right, rect.Bottom,
                        rect.X, rect.Bottom)
                    path.AddLine(
                        rect.X, rect.Bottom,
                        rect.X, rect.Y)

                Case RoundStyle.BottomLeft

                    path.AddArc(
                        rect.X,
                        rect.Bottom - Radius,
                        Radius,
                        Radius,
                        90,
                        90)
                    path.AddLine(rect.X, rect.Y, rect.Right, rect.Y)
                    path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom)
                Case RoundStyle.BottomRight
                    path.AddArc(
                      rect.Right - Radius,
                      rect.Bottom - Radius,
                      Radius,
                      Radius,
                      0,
                      90)

                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y)

                    path.AddLine(rect.X, rect.Y, rect.Right, rect.Y)
            End Select

            path.CloseFigure()
            Return path

        End Function
        Public Shared Function GetTextFormatFlags(alignment As ContentAlignment) As TextFormatFlags

            Dim flags As TextFormatFlags = TextFormatFlags.WordBreak

            Select Case alignment
                Case ContentAlignment.MiddleCenter
                    flags = flags Or TextFormatFlags.VerticalCenter Or TextFormatFlags.HorizontalCenter
                Case ContentAlignment.MiddleLeft
                    flags = flags Or TextFormatFlags.VerticalCenter Or TextFormatFlags.Left
                Case ContentAlignment.MiddleRight
                    flags = flags Or TextFormatFlags.VerticalCenter Or TextFormatFlags.Right
                Case ContentAlignment.TopCenter
                    flags = flags Or TextFormatFlags.Top Or TextFormatFlags.HorizontalCenter
                Case ContentAlignment.TopLeft
                    flags = flags Or TextFormatFlags.Top Or TextFormatFlags.Left
                Case ContentAlignment.TopRight
                    flags = flags Or TextFormatFlags.Top Or TextFormatFlags.Right
                Case ContentAlignment.BottomCenter
                    flags = flags Or TextFormatFlags.Bottom Or TextFormatFlags.HorizontalCenter
                Case ContentAlignment.BottomLeft
                    flags = flags Or TextFormatFlags.Bottom Or TextFormatFlags.Left
                Case ContentAlignment.BottomRight
                    flags = flags Or TextFormatFlags.Bottom Or TextFormatFlags.Right
            End Select
            Return flags



        End Function
    End Class

End Namespace
