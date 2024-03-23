Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D


Public MustInherit Class RoundedRectangle
        <Flags>
        Public Enum RectangleCorners
            None = 0
            TopLeft = 1
            TopRight = 2
            BottomLeft = 4
            BottomRight = 8
            All = TopLeft Or TopRight Or BottomLeft Or BottomRight
        End Enum

        Public Enum WhichHalf
            TopLeft
            BottomRight
            Both
        End Enum

        Private Shared Sub Corner(ByVal path As GraphicsPath, ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer, ByVal x3 As Integer, ByVal y3 As Integer)
            path.AddLine(x1, y1, x2, y2)
            path.AddLine(x2, y2, x3, y3)
        End Sub

        Public Shared Function Create(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal radius As Integer, ByVal corners As RectangleCorners, ByVal half As WhichHalf) As GraphicsPath
            If radius <= 0 Then
            Dim rectp As New GraphicsPath()
            rectp.AddRectangle(New Rectangle(x, y, width, height))
            Return rectp
        End If

        Dim dia As Integer = radius * 2
        Dim TLarc As New Rectangle(x, y, dia, dia)
        Dim TRarc As New Rectangle(x + width - dia - 1, y, dia, dia)
        Dim BRarc As New Rectangle(x + width - dia - 1, y + height - dia - 1, dia, dia)
        Dim BLarc As New Rectangle(x, y + height - dia - 1, dia, dia)
        Dim TLsquare As New Rectangle(x, y, radius, radius)
        Dim TRsquare As New Rectangle(x + width - radius, y, radius, radius)
        Dim BRsquare As New Rectangle(x + width - radius, y + height - radius, radius, radius)
        Dim BLsquare As New Rectangle(x, y + height - radius, radius, radius)
        Dim p As New GraphicsPath()
        p.StartFigure()

            If half = WhichHalf.Both OrElse half = WhichHalf.TopLeft Then

                If corners.HasFlag(RectangleCorners.BottomLeft) Then
                    p.AddArc(BLarc, 135, 45)
                Else
                    p.AddLine(BLsquare.Left, BLsquare.Bottom, BLsquare.Left, BLsquare.Top)
                End If

                p.AddLine(BLsquare.Left, BLsquare.Top - 1, TLsquare.Left, TLsquare.Bottom + 1)

                If corners.HasFlag(RectangleCorners.TopLeft) Then
                    p.AddArc(TLarc, 180, 90)
                Else
                    Corner(p, TLsquare.Left, TLsquare.Bottom, TLsquare.Left, TLsquare.Top, TLsquare.Right, TLsquare.Top)
                End If

                p.AddLine(TLsquare.Right + 1, TLsquare.Top, TRsquare.Left - 1, TRsquare.Top)
                If corners.HasFlag(RectangleCorners.TopRight) Then p.AddArc(TRarc, -90, 45)
            End If

            If half = WhichHalf.Both OrElse half = WhichHalf.BottomRight Then

                If corners.HasFlag(RectangleCorners.TopRight) Then
                    p.AddArc(TRarc, -45, 45)
                Else
                    p.AddLine(TRsquare.Right, TRsquare.Top, TRsquare.Right, TRsquare.Bottom)
                End If

                p.AddLine(TRsquare.Right, TRsquare.Bottom + 1, BRsquare.Right, BRsquare.Top - 1)

                If corners.HasFlag(RectangleCorners.BottomRight) Then
                    p.AddArc(BRarc, 0, 90)
                Else
                    Corner(p, BRsquare.Right, BRsquare.Top, BRsquare.Right, BRsquare.Bottom, BRsquare.Left, BRsquare.Bottom)
                End If

                p.AddLine(BRsquare.Left - 1, BRsquare.Bottom, BLsquare.Right + 1, BLsquare.Bottom)

                If corners.HasFlag(RectangleCorners.BottomLeft) Then
                    p.AddArc(BLarc, 90, 45)
                Else
                    p.AddLine(BLsquare.Right, BLsquare.Bottom, BLsquare.Left, BLsquare.Bottom)
                End If
            End If

            Return p
        End Function

        Public Shared Function Create(ByVal rect As Rectangle, ByVal radius As Integer, ByVal c As RectangleCorners, ByVal which_half As WhichHalf) As GraphicsPath
            Return Create(rect.X, rect.Y, rect.Width, rect.Height, radius, c, which_half)
        End Function

        Public Shared Function Create(ByVal rect As Rectangle, ByVal radius As Integer, ByVal c As RectangleCorners) As GraphicsPath
            Return Create(rect.X, rect.Y, rect.Width, rect.Height, radius, c, WhichHalf.Both)
        End Function

        Public Shared Function Create(ByVal rect As Rectangle, ByVal radius As Integer) As GraphicsPath
            Return Create(rect.X, rect.Y, rect.Width, rect.Height, radius, RectangleCorners.All, WhichHalf.Both)
        End Function
    End Class

