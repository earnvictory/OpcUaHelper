Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Namespace ControlExt
    Public Class Common

        '''<summary>
        '''设置窗口大小、位置、顺序
        '''     hwnd:Integer型,欲定位的窗口
        '''     hWndInsertAfter:Integer型,窗口句柄。在窗口列表中,窗口hwnd会置于这个窗口句柄的后面。也可能选用下述值之一：
        '''     HWND_BOTTOM
        '''     将窗口置于窗口列表底部
        '''     HWND_TOP
        '''     将窗口置于Z序列的顶部；Z序列代表在分级结构中,窗口针对一个给定级别的窗口显示的顺序
        '''     HWND_TOPMOST
        '''     将窗口置于列表顶部,并位于任何最顶部窗口的前面
        '''     HWND_NOTOPMOST
        '''     将窗口置于列表顶部,并位于任何最顶部窗口的后面
        '''     x:Integer型,窗口新的x坐标。如hwnd是一个子窗口,则x用父窗口的客户区坐标表示
        '''     y:Integer型,窗口新的y坐标。如hwnd是一个子窗口,则y用父窗口的客户区坐标表示
        '''     cx:Integer型,指定新的窗口宽度
        '''     cy:Integer型,指定新的窗口高度
        '''     wFlags:Integer型,包含了旗标的一个整数
        '''     SWP_DRAWFRAME
        '''     围绕窗口画一个框
        '''     SWP_HIDEWINDOW
        '''     隐藏窗口
        '''     SWP_NOACTIVATE
        '''     不激活窗口
        '''     SWP_NOMOVE
        '''     保持当前位置（x和y设定将被忽略）
        '''     SWP_NOREDRAW
        '''     窗口不自动重画
        '''     SWP_NOSIZE
        '''     保持当前大小（cx和cy会被忽略）
        '''     SWP_NOZORDER
        '''     保持窗口在列表的当前位置（hWndInsertAfter将被忽略）
        '''     SWP_SHOWWINDOW
        '''     显示窗口
        '''     SWP_FRAMECHANGED
        '''     强迫一条WM_NCCALCSIZE消息进入窗口,即使窗口的大小没有改变
        '''</summary>
        Public Declare Auto Function SetWindowPos Lib "user32.dll" Alias "SetWindowPos" (
        ByVal hwnd As Integer,
        ByVal hWndInsertAfter As Integer,
        ByVal x As Integer,
        ByVal y As Integer,
        ByVal cx As Integer,
        ByVal cy As Integer,
        ByVal wFlags As Integer
    ) As Integer
        Public Const SWP_SHOWWINDOW As Integer = &H40
        Public Const HWND_TOP As Integer = &H0
        Public Shared Sub SetWindowPos(Handle As Integer, X As Integer, Y As Integer, Width As Integer, Height As Integer)
            SetWindowPos(Handle, 0, X, Y, Width, Height, &H40)
        End Sub
        Public Shared Function Graphics_SetSmoothHighQuality(g As Graphics) As Graphics
            With g
                .SmoothingMode = SmoothingMode.HighQuality
                .InterpolationMode = InterpolationMode.HighQualityBilinear
                .CompositingQuality = CompositingQuality.HighQuality
            End With
            Return g
        End Function
        Public Shared Sub DrawRectanglef(g As Graphics, pen As Pen, Rect As RectangleF)
            g.DrawRectangle(pen, Rect.X, Rect.Y, Rect.Width, Rect.Height)
        End Sub

        Public Shared Function TransformCircular(ByVal rectf As RectangleF, ByVal Optional leftTopRadius As Single = 0F, ByVal Optional rightTopRadius As Single = 0F, ByVal Optional rightBottomRadius As Single = 0F, ByVal Optional leftBottomRadius As Single = 0F) As GraphicsPath
            leftTopRadius = Math.Abs(leftTopRadius)
            rightTopRadius = Math.Abs(rightTopRadius)
            rightBottomRadius = Math.Abs(rightBottomRadius)
            leftBottomRadius = Math.Abs(leftBottomRadius)
            Dim leftTop_x As New PointF(rectf.Left, rectf.Top)
            Dim leftTop_y As New PointF(rectf.Left, rectf.Top)

            If leftTopRadius > 0 Then
                leftTop_x = New PointF(rectf.Left + leftTopRadius, rectf.Top)
                leftTop_y = New PointF(rectf.Left, rectf.Top + leftTopRadius)
            End If

            Dim rightTop_x As New PointF(rectf.Right, rectf.Top)
            Dim rightTop_y As New PointF(rectf.Right, rectf.Top)

            If rightTopRadius > 0 Then
                rightTop_x = New PointF(rectf.Right - rightTopRadius, rectf.Top)
                rightTop_y = New PointF(rectf.Right, rectf.Top + rightTopRadius)
            End If

            Dim rightBottom_x As New PointF(rectf.Right, rectf.Bottom)
            Dim rightBottom_y As New PointF(rectf.Right, rectf.Bottom)

            If rightBottomRadius > 0 Then
                rightBottom_x = New PointF(rectf.Right - rightBottomRadius, rectf.Bottom)
                rightBottom_y = New PointF(rectf.Right, rectf.Bottom - rightBottomRadius)
            End If

            Dim leftBottom_x As New PointF(rectf.Left, rectf.Bottom)
            Dim leftBottom_y As New PointF(rectf.Left, rectf.Bottom)

            If leftBottomRadius > 0 Then
                leftBottom_x = New PointF(rectf.Left + leftBottomRadius, rectf.Bottom)
                leftBottom_y = New PointF(rectf.Left, rectf.Bottom - leftBottomRadius)
            End If

            Dim gp As New GraphicsPath()

            If leftTopRadius > 0 Then
                Dim lefttop_rect As New RectangleF(rectf.Left, rectf.Top, leftTopRadius * 2, leftTopRadius * 2)
                gp.AddArc(lefttop_rect, 180, 90)
            End If

            gp.AddLine(leftTop_x, rightTop_x)

            If rightTopRadius > 0 Then
                Dim righttop_rect As New RectangleF(rectf.Right - rightTopRadius * 2, rectf.Top, rightTopRadius * 2, rightTopRadius * 2)
                gp.AddArc(righttop_rect, 270, 90)
            End If

            gp.AddLine(rightTop_y, rightBottom_y)

            If rightBottomRadius > 0 Then
                Dim rightbottom_rect As New RectangleF(rectf.Right - rightBottomRadius * 2, rectf.Bottom - rightBottomRadius * 2, rightBottomRadius * 2, rightBottomRadius * 2)
                gp.AddArc(rightbottom_rect, 0, 90)
            End If

            gp.AddLine(rightBottom_x, leftBottom_x)

            If leftBottomRadius > 0 Then
                Dim rightbottom_rect As New RectangleF(rectf.Left, rectf.Bottom - leftBottomRadius * 2, leftBottomRadius * 2, leftBottomRadius * 2)
                gp.AddArc(rightbottom_rect, 90, 90)
            End If

            gp.AddLine(leftBottom_y, leftTop_y)
            gp.CloseAllFigures()
            Return gp
        End Function


        Public Shared Function TransformRectangleF(ByVal rectf As RectangleF, ByVal pen As Single, ByVal Optional alignment As PenAlignment = PenAlignment.Center) As RectangleF
            If pen <= 0 Then
                Return rectf
            End If

            Dim result As New RectangleF()

            If alignment = PenAlignment.Center OrElse alignment = PenAlignment.Left OrElse alignment = PenAlignment.Right OrElse alignment = PenAlignment.Outset Then
                result.Width = rectf.Width - pen
                result.Height = rectf.Height - pen
                result.X = rectf.X '+ (CInt(pen)) / 2
                result.Y = rectf.Y '+ (CInt(pen)) / 2
            ElseIf alignment = PenAlignment.Inset Then

                If pen > 0 AndAlso pen < 2 Then
                    result.Width = rectf.Width - 1
                    result.Height = rectf.Height - 1
                Else
                    result.Width = rectf.Width
                    result.Height = rectf.Height
                End If

                result.X = rectf.X
                result.Y = rectf.Y
            End If

            Return result
        End Function

        Public Shared Function VerifyRGB(ByVal rgb As Integer) As Integer
            If rgb < 0 Then Return 0
            If rgb > 255 Then Return 255
            Return rgb
        End Function
        Public Shared Function CalculatePointForAngle(ByVal center As PointF, ByVal radius As Single, ByVal angle As Single) As PointF
            If radius = 0 Then Return center
            Dim w As Single
            Dim h As Single

            If angle <= 90 Then
                w = radius * CSng(Math.Cos(Math.PI / 180 * angle))
                h = radius * CSng(Math.Sin(Math.PI / 180 * angle))
            ElseIf angle <= 180 Then
                w = -radius * CSng(Math.Sin(Math.PI / 180 * (angle - 90)))
                h = radius * CSng(Math.Cos(Math.PI / 180 * (angle - 90)))
            ElseIf angle <= 270 Then
                w = -radius * CSng(Math.Cos(Math.PI / 180 * (angle - 180)))
                h = -radius * CSng(Math.Sin(Math.PI / 180 * (angle - 180)))
            Else
                w = radius * CSng(Math.Sin(Math.PI / 180 * (angle - 270)))
                h = -radius * CSng(Math.Cos(Math.PI / 180 * (angle - 270)))
            End If

            Return New PointF(center.X + w, center.Y + h)
        End Function
        Enum Shape
            Ellipse
            Rectangle
        End Enum
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
        Enum InterLockType
            On_Lock
            Off_Lock
            No_Lock
        End Enum
        ReadOnly Property InterLockTypes As String()
            Get
                Return [Enum].GetNames(GetType(InterLockType))
            End Get
        End Property
        Enum Operationtype
            AutoReset
            Change
            [Set]
            Reset
        End Enum
        ReadOnly Property Operationtypes As String()
            Get
                Return [Enum].GetNames(GetType(Operationtype))
            End Get
        End Property
        ''' <summary>
        ''' 圆角矩形
        ''' </summary>
        ''' <param name="rect"></param>
        ''' <param name="Radius"></param>
        ''' <param name="Style"></param>
        ''' <returns></returns>
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
        ''' <summary>
        ''' 圆角矩形
        ''' </summary>
        ''' <param name="rect"></param>
        ''' <param name="Radius"></param>
        ''' <param name="Style"></param>
        ''' <returns></returns>
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
        Public Shared Function GetTextFormat(TextAlingment As ContentAlignment) As StringFormat
            Select Case TextAlingment
                Case ContentAlignment.BottomLeft
                    Return New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Far}
                Case ContentAlignment.BottomRight
                    Return New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Far}
                Case ContentAlignment.BottomCenter
                    Return New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Far}
                Case ContentAlignment.MiddleLeft
                    Return New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}
                Case ContentAlignment.MiddleRight
                    Return New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
                Case ContentAlignment.MiddleCenter
                    Return New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                Case ContentAlignment.TopLeft
                    Return New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near}
                Case ContentAlignment.TopRight
                    Return New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Near}
                Case ContentAlignment.TopCenter
                    Return New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Near}
                Case Else
                    Return New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            End Select
        End Function




    End Class

End Namespace
